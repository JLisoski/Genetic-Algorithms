using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS489HW5
{
    public class DNA<T>
    {
        public T[] Genes { get; private set; }
        public float Fitness { get; private set; }
        private Random rand;
        private Func<T> getRandomGene;
        private Func<float, int> fitnessFunction;

        public DNA(int size, Random random, Func<T> getRandomGene, Func<float, int> fitnessFunction, bool shouldInitGenes = true)
        {
            Genes = new T[size];
            this.rand = random;
            this.getRandomGene = getRandomGene;
            this.fitnessFunction = fitnessFunction;

            if (shouldInitGenes)
            {
                for (int i = 0; i < Genes.Length; i++)
                {
                    Genes[i] = getRandomGene();
                }
            }

        }

        public float CalculateFitness(int index)
        {
            Fitness = fitnessFunction(index);
            return Fitness;
        }

        public DNA<T> Crossover(DNA<T> otherParent)
        {
            DNA<T> child = new DNA<T>(Genes.Length, rand, getRandomGene, fitnessFunction, shouldInitGenes: false);

            for (int i = 0; i < Genes.Length; i++)
            {
                child.Genes[i] = rand.NextDouble() < 0.5 ? Genes[i] : otherParent.Genes[i];
            }

            return child;
        }

        public void Mutate(float mutationRate)
        {
            for (int i = 0; i < Genes.Length; i++)
            {
                if(rand.NextDouble() < mutationRate)
                {
                    Genes[i] = getRandomGene();
                }
            }
        }
    }
}
