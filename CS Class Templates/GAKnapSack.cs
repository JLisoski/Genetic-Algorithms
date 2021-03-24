using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS489HW5
{
    public class GAKnapSack
    {
        //KnapSack Variables 
        public int noItems;
        public double[] values;
        public double[] weights;
        public double knapsackMaxSize;

        //GA Variables
        public int chromosomeLength;
        public static int noCandidates;
        public static int maxGen;
        public static double crossoverProb;
        public static double mutationProb;
        public List<String> CandidateSolutions;
        public double bestFitness;

        //Constructor
        public GAKnapSack(int noItems, double[] values, double[] weights, double knapsackSize, int population, int generations, double crossProb, double mutProb)
        {
            //Knapsack initializations
            this.noItems = noItems;
            this.values = values;
            this.weights = weights;
            this.knapsackMaxSize = knapsackSize;

            //GA initializations
            chromosomeLength = noItems;
            GAKnapSack.noCandidates = population;
            maxGen = generations;
            crossoverProb = crossProb;
            mutationProb = mutProb;

            CandidateSolutions = new List<String>();

            //Initializae first generation
            generateSolutions(GAKnapSack.noCandidates);

            //Run the genetic algorithm until required fitness acheived
            for(int i=0; i < GAKnapSack.maxGen && (bestFitness < (0.9*knapsackSize)); i++)
            {
                Console.WriteLine("Generation: " + (i + 1));
                Console.WriteLine(" " + "Best Solution: " + getBestSolution());
                Console.WriteLine(" " + "Best value: " + bestFitness);
                newGen();

            }
            
        }

        public void generateSolutions(int candidateSize)
        {
            Random rand = new Random();

            String cand;

            for(int i=0; i < candidateSize; i++)
            {
                cand = "";

                for(int j=0; j < noItems; j++)
                {
                    int letter = rand.Next();
                    cand += letter;
                }

                CandidateSolutions.Add(cand);
            }
        }

        public void setFitness(double fitness)
        {
            this.bestFitness = fitness;
        }
        public double calcFitness(String solution)
        {
            double fit = 0;
            double weight = 0;

            for(int i=0; i < solution.Length; i++)
            {
                if(solution.ElementAt(i) == 49)
                {
                    weight += weights[i];
                    fit += values[i];
                }
            }

            if(weight <= knapsackMaxSize)
            {
                return fit;
            }
            else
            {
                return -1;
            }
        }

        public String crossOver(String candidate1, String candidate2)
        {
            String crossedOver = "";
            Random rand = new Random();

            for (int i = 0; i < candidate1.Length; i++)
            {
                if (rand.Next() >= crossoverProb)
                {
                    crossedOver += candidate1.ElementAt(i);
                }
                else
                {
                    crossedOver += candidate2.ElementAt(i);
                }
            }

            return crossedOver;
        }

        public String mutate(String candidate)
        {
            Random rand = new Random();

            for (int i=0; i < candidate.Length; i++)
            {
                if(rand.Next() <= mutationProb)
                {
                    candidate = changeBit(i, candidate);
                }
            }

            return candidate;
        }

        public String changeBit(int idx, String candidate)
        {
            String returnStr = "";
            for(int i=0; i < candidate.Length; i++)
            {
                if(i == idx)
                {
                    if(candidate.ElementAt(i) == 49)
                    {
                        returnStr += 0;
                    }
                    else
                    {
                        returnStr += 1;
                    }
                }
                else
                {
                    returnStr += candidate.ElementAt(i);
                }
            }

            return returnStr;
        }

        public String getBestSolution()
        {
            double bestFit = -1;
            String bestSol = null;

            foreach (String CandidateSolution in CandidateSolutions)
            {
                double newFit = calcFitness(CandidateSolution);
                if(newFit != -1)
                {
                    if(newFit >= bestFit)
                    {
                        bestSol = CandidateSolution;
                        bestFit = newFit;
                    }
                }
            }

            setFitness(bestFit);

            return bestSol;
        }

        public void newGen()
        {
            String can1 = getBestSolution();

            CandidateSolutions.Remove(can1);

            String can2 = getBestSolution();

            CandidateSolutions.Add(can1);

            for(int i=1; i < noCandidates; i++)
            {
                CandidateSolutions.Add(mutate(crossOver(can1, can2)));
            }
        }

    }
}
