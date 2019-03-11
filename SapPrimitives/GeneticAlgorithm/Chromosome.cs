namespace SapPrimitives.FactorySetting
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Chromosome<T>
    {
        private double fitness;
        private Random randomNumber;
        private Func<int, double> calculateFitness;
        private Func<T[]> getRandomGene;
        private Func<Chromosome<T>, Chromosome<T>, Chromosome<T>> crossOverFunction;
        private double mutationRate;
        private T[] genes;
        private int size;


        public double Fitness => fitness;
        public double MutationRate { get => mutationRate; set => mutationRate = value; }
        public T[] Genes => genes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gene"></param>
        /// <param name="mutationRate"></param>
        /// <param name="fitnessFunction"></param>
        /// <param name="randomGeneFunction"></param>
        /// <param name="randomNumber"></param>
        public Chromosome(int decisionVariableNumber, double mutationRate, Func<int, double> fitnessFunction, Func<T[]> randomGeneFunction, Func<Chromosome<T>, Chromosome<T>, Chromosome<T>> crossOverFunction, Random randomNumber, bool initateGene)
        {
            this.mutationRate = mutationRate;
            this.calculateFitness = fitnessFunction;
            this.getRandomGene = randomGeneFunction;
            this.randomNumber = randomNumber;
            size = decisionVariableNumber;
            this.crossOverFunction = crossOverFunction;
            genes = new T[decisionVariableNumber];

            if (initateGene)
            {
                genes = getRandomGene();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gene"></param>
        /// <param name="fitnessFunction"></param>
        /// <param name="randomGeneFunction"></param>
        /// <param name="randomNumber"></param>
        public Chromosome(int decisionVariableNumber, Func<int, double> fitnessFunction, Func<T[]> randomGeneFunction, Func<Chromosome<T>, Chromosome<T>, Chromosome<T>> crossOverFunction, Random randomNumber)
            : this(decisionVariableNumber, 0.01, fitnessFunction, randomGeneFunction, crossOverFunction, randomNumber, false)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gene"></param>
        /// <param name="mutationRate"></param>
        /// <param name="fitnessFunction"></param>
        /// <param name="randomGeneFunction"></param>
        /// <param name="randomNumber"></param>
        public Chromosome(T[] genes, double mutationRate, Func<int, double> fitnessFunction, Func<T[]> randomGeneFunction, Func<Chromosome<T>, Chromosome<T>, Chromosome<T>> crossOverFunction, Random randomNumber)
        {
            this.genes = genes;
            this.mutationRate = mutationRate;
            this.calculateFitness = fitnessFunction;
            this.getRandomGene = randomGeneFunction;
            this.crossOverFunction = crossOverFunction;
            this.randomNumber = randomNumber;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double GetFitness(int index)
        {
            fitness = calculateFitness(index);
            return fitness;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromosomePartner"></param>
        /// <returns></returns>
        public Chromosome<T> CrossOver(Chromosome<T> chromosomePartner)
        {
            return crossOverFunction(this, chromosomePartner);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Mutate()
        {
            if (randomNumber.NextDouble() < mutationRate)
            {
                genes = getRandomGene();
            }
        }






    }
}
