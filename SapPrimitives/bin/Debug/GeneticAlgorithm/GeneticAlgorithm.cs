namespace SapPrimitives.FactorySetting
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class GeneticAlgorithm<T>
    {
        private double generationCount;
        private int elitism;
        private double mutationRate;
        private Random randomNumber;
        private Func<T[]> getRandomGene;
        private Func<int, double> fitnessFunction;
        private int decisionVariables;
        private T[] fittestGenes;
        private List<Chromosome<T>> electedChromosomes;
        private List<Chromosome<T>> chromosomes;
        private double totalFitness;
        private double highestFitness;
        private Chromosome<T> bestChromosome;
        Func<Chromosome<T>, Chromosome<T>, Chromosome<T>> crossOverFunction;


        public double GenerationCount { get => generationCount; }
        public int Elitism { get => elitism; set => elitism = value; }
        public double MutationRate { get => mutationRate; set => mutationRate = value; }
        public Random Random { get => randomNumber; set => randomNumber = value; }
        public double HighestFitness => highestFitness;
        public double TotalFitness => totalFitness;

        public List<Chromosome<T>> ElectedChromosomes => electedChromosomes;
        public List<Chromosome<T>> Chromosomes => chromosomes;

        public T[] FittestGenes => fittestGenes;

        public Chromosome<T> BestChromosome { get => bestChromosome; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="populationSize"></param>
        /// <param name="dnaSize"></param>
        /// <param name="random"></param>
        /// <param name="getRandomGene"></param>
        /// <param name="fitnessFunction"></param>
        /// <param name="elitism"></param>
        /// <param name="mutationRate"></param>
        public GeneticAlgorithm(int decisionVariables, int populationSize, Random random, Func<T[]> getRandomGene, Func<int, double> fitnessFunction, Func<Chromosome<T>, Chromosome<T>, Chromosome<T>> crossOverFunction,
            int elitism, double mutationRate = 0.0)
        {
            generationCount = 1;
            this.elitism = elitism;
            this.mutationRate = mutationRate;
            this.randomNumber = random;
            this.getRandomGene = getRandomGene;
            this.fitnessFunction = fitnessFunction;
            this.crossOverFunction = crossOverFunction;
            this.decisionVariables = decisionVariables;
            chromosomes = new List<Chromosome<T>>(populationSize);
            electedChromosomes = new List<Chromosome<T>>(populationSize);
            fittestGenes = new T[decisionVariables];
            for (int i = 0; i < populationSize; i++)
            {
                chromosomes.Add(new Chromosome<T>(decisionVariables, mutationRate, fitnessFunction, getRandomGene, crossOverFunction, random, true));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="crossedOver"></param>
        /// <param name="eliteCounts"></param>
        public void NewGenerationElitism(bool crossedOver, int eliteCounts)
        {
            if (chromosomes.Count > 0)
            {
                CalculateFitness();
                chromosomes.Sort(CompareChromosome);

            }
            electedChromosomes.Clear();

            for (int i = 0; i < chromosomes.Count; i++)
            {
                if (i < eliteCounts && i < chromosomes.Count)
                {
                    electedChromosomes.Add(chromosomes[i]);
                }
                else if (i < chromosomes.Count || crossedOver)
                {
                    Chromosome<T> parent1 = ChooseParent();
                    Chromosome<T> parent2 = ChooseParent();
                    Chromosome<T> child = parent1.CrossOver(parent2);
                    child.Mutate();
                    electedChromosomes.Add(child);
                }
                else
                {
                    electedChromosomes.Add(new Chromosome<T>(decisionVariables, mutationRate, fitnessFunction, getRandomGene, crossOverFunction, randomNumber, true));
                }
            }

            var tmpList = chromosomes;
            chromosomes = electedChromosomes;
            electedChromosomes = tmpList;
            generationCount++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private int CompareChromosome(Chromosome<T> a, Chromosome<T> b)
        {
            if (a.Fitness > b.Fitness)
            {
                return -1;
            }
            else if (a.Fitness < b.Fitness)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalculateFitness()
        {
            totalFitness = 0;
            Chromosome<T> fittest = chromosomes[0];
            for (int i = 0; i < chromosomes.Count; i++)
            {
                totalFitness += chromosomes[i].GetFitness(i);
                if (chromosomes[i].Fitness > fittest.Fitness)
                {
                    fittest = chromosomes[i];
                }
            }
            fittest.Genes.CopyTo(fittestGenes, 0);
            highestFitness = fittest.Fitness;
            bestChromosome = fittest;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Chromosome<T> ChooseParent()
        {
            double random = randomNumber.NextDouble() * totalFitness;

            for (int i = 0; i < chromosomes.Count; i++)
            {
                if (random < chromosomes[i].Fitness)
                {
                    return chromosomes[i];
                }
                random -= chromosomes[i].Fitness;
            }
            return chromosomes[0];
        }


    }



















}

