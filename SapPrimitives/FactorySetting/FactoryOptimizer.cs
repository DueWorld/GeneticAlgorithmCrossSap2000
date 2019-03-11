namespace SapPrimitives.FactorySetting
{
    using SapPrimitives.FramePrimitives;
    using SapPrimitives.GeneralizedPrimitives;
    using System;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// 
    /// </summary>
    public class FactoryOptimizer
    {
        private GeneticAlgorithm<double> geneticAlgorithm;
        private Factory sapSteelFactory;
        private Random geneticRandom;
        private SapModel sapModel;
        private SapSteelSection section1;
        private SapSteelSection section2;
        private RichTextBox reportingControl;
        private int populationSize;
        private double currentWeight;
        private double bestFitness;
        private double currentGeneration;
        private int numberOfIterations;
        private int eliteCount;

        public double CurrentWeight => currentWeight;
        public double BestFittnes => bestFitness;
        public double CurrentGeneration => currentGeneration;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sapModel"></param>
        /// <param name="section1"></param>
        /// <param name="section2"></param>
        /// <param name="populationSize"></param>
        public FactoryOptimizer(SapModel sapModel, SapSteelSection section1, SapSteelSection section2, int numberOfIterations, int populationSize, int eliteCount, RichTextBox reportingControl)
        {
            geneticRandom = new Random();
            this.sapModel = sapModel;
            this.section1 = section1;
            this.section2 = section2;
            this.reportingControl = reportingControl;
            this.numberOfIterations = numberOfIterations;
            this.populationSize = populationSize;
            this.eliteCount = eliteCount;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            StringBuilder sb = new StringBuilder();

            geneticAlgorithm = new GeneticAlgorithm<double>(2, populationSize, geneticRandom, GetRandomGenes, CalculateFitness, CrossOver, eliteCount);
            while (geneticAlgorithm.GenerationCount <= numberOfIterations)
            {
                geneticAlgorithm.NewGenerationElitism(true, eliteCount);
                currentGeneration = geneticAlgorithm.GenerationCount;
                bestFitness = geneticAlgorithm.HighestFitness;
            }

            sapSteelFactory = new Factory(sapModel, section1, section2, geneticAlgorithm.FittestGenes[0], geneticAlgorithm.FittestGenes[1], 100);
            currentWeight = sapSteelFactory.SteelWeight;
            bestFitness = geneticAlgorithm.HighestFitness;
            currentGeneration = geneticAlgorithm.GenerationCount;
            sb.AppendLine($"BEST FITNESS {bestFitness}");
            sb.AppendLine();
            sb.AppendLine($"CURRENT SPACING {geneticAlgorithm.BestChromosome.Genes[0]}");
            sb.AppendLine();
            sb.AppendLine($"CURRENT HEIGHT {geneticAlgorithm.BestChromosome.Genes[1]}");
            sb.AppendLine();
            sb.AppendLine($"CURRENT TOTAL WEIGHT {currentWeight}");
            reportingControl.Text = sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double[] GetRandomGenes()
        {
            double spacing = 0;
            double height = 0;
            do
            {
                spacing = GetRandomDoubleStep(true);
                height = GetRandomDoubleStep(false);
                sapSteelFactory = new Factory(sapModel, section1, section2, spacing, height, 100);
                sapSteelFactory.DeleteAll();

            } while (!sapSteelFactory.IsSafe);
            double[] genes = { spacing, height };
            return genes;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private double CalculateFitness(int index)
        {
            return 0.025 * geneticAlgorithm.Chromosomes[index].Genes[0] - 0.0183 * geneticAlgorithm.Chromosomes[index].Genes[1];
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private double GetRandomDoubleStep(bool spacing)
        {
            double randomNumber = 0;
            if (!spacing)
            {
                randomNumber = geneticRandom.Next(0, 17);
                randomNumber = (randomNumber / 4) + 8;
            }
            else
            {
                randomNumber = geneticRandom.Next(0, 17);
                randomNumber = (randomNumber / 4) + 4;
            }
            return randomNumber;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromosome1"></param>
        /// <param name="chromsome2"></param>
        /// <returns></returns>
        private Chromosome<double> CrossOver(Chromosome<double> chromosome1, Chromosome<double> chromsome2)
        {
            Chromosome<double> child = new Chromosome<double>(2, geneticAlgorithm.MutationRate, CalculateFitness, GetRandomGenes, CrossOver, geneticRandom, false);
            do
            {
                for (int i = 0; i < chromosome1.Genes.Length; i++)
                {
                    child.Genes[i] = geneticRandom.NextDouble() < 0.5 ? chromosome1.Genes[i] : chromsome2.Genes[i];
                }
                sapSteelFactory = new Factory(sapModel, section1, section2, child.Genes[0], child.Genes[1], 100);
                sapSteelFactory.DeleteAll();

            }
            while (!sapSteelFactory.IsSafe);
            sapSteelFactory.DeleteAll();
            return child;
        }
    }
}
