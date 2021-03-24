using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS489HW5
{
    public partial class Form1 : Form
    {

        //GA Knapsack Variables
        static int numOfItems = 10;
        static double[] items = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        static double[] values = new double[] { 18, 60, 47, 55, 53, 72, 90, 83, 21, 16 };
        static double[] weights = new double[] { 6, 14, 13, 9, 11, 16, 20, 17, 3, 5 };
        static double[] totalValues = new double[population];
        static double[] totalWeights = new double[population];
        static int[,] chromosomes;
        static double weightCap = 35;
        static double mutationRate = 0.5;
        static double fitness;
        static double[] fitnessArray;
        static double bestFitness;
        static int population;
        static int generations;

        //Random Variable
        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("System is go...");
        }

        //Genetic Algorithm Knapsack 
        public void geneticKnapSack()
        {
            //Clear output textBox3 initially
            textBox3.Clear();

            //Check if user has entered population, if not deafault to a set population 
            if(textBox5.Text == "")
            {
                //Default to population of 5
                population = 5;
            }
            else
            {
                //If not empty, convert to Int 32
                population = Convert.ToInt32(textBox5.Text);
            }

            //Check if user has entered generations, if not deafault to a set generation 
            if (textBox7.Text == "")
            {
                //Default to population of 100
                generations = 100;
            }
            else
            {
                //If not empty, convert to Int 32
                generations = Convert.ToInt32(textBox7.Text);
            }

            //Initiliaze chromosomes and fittnessArray variables
            chromosomes = new int[population, numOfItems];
            fitnessArray = new double[population];


            //For Loop with check to the population size
            for (int i = 0; i < generations; i++)
            {
                //For Debugging Purposes
                //Console.WriteLine("Inisde for loop! Generation is ..." + generations.ToString());
                textBox3.Text += "****************************\r\n";

                //Declare temporary variabe to hold i+1
                int temp = i + 1;
                textBox3.Text += "Generation = " + temp.ToString() + "\r\n";

                //Create random 10 digit binary array
                randomizePopulation();
                //Call fitness function 
                fitnessFunc();

                //For Debugging Print Out BestFitnes
                //Console.WriteLine("BestFitness is... " + bestFitness.ToString());
                textBox3.Text += "Fitness = " + printSingleMatrix(fitnessArray) + "\r\n";

                //For Debugging Print out BinaryArray
                //Console.WriteLine(printSingleMatrix(binaryArray));
                textBox3.Text += printMatrix(chromosomes) + "\r\n";

                //Find the sum of values and weights for those with a 1
                textBox3.Text += "Selected Individuals: ";
                totalValues = sumValuesForPop();
                totalWeights = sumWeightsForPop();

                //For Debugging, Print out current generation total Value
                //Console.WriteLine("Total Value is ... " + result.ToString());
                textBox3.Text += "Value f(x) = " + printSingleMatrix(totalValues) + "\r\n";
                textBox3.Text += "Weight = " + printSingleMatrix(totalWeights) + "\r\n";

                //For astheitcs
                textBox3.Text += "****************************\r\n";

                //For threading problem...
                Console.WriteLine("Still working...");
            }

            //For threading problem...
            Console.WriteLine("Finished!");
        }

        public void fitnessFunc()
        {
            for (int i = 0; i < population; i++)
            {
                //Update variables
                fitness = 0;
                double vfitness = 0;
                double wFitness = 0;

                for (int j = 0; j < numOfItems; j++)
                {
                    if (chromosomes[i,j] == 1)
                    {
                        vfitness += values[i];
                        wFitness += weights[i];
                    }
                }

                //For Debugging Purposes 
                //Console.WriteLine("wFitness is ... " + wFitness.ToString());

                //If fitness greater then weight cap set to 0, otherwise keep
                if (wFitness <= weightCap)
                {
                    fitness = vfitness;

                    if (fitness >= bestFitness)
                    {
                        bestFitness = fitness;
                    }
                }
                else
                {
                    fitness = 0;
                }

                fitnessArray[i] = fitness;
            }
        }

        public void randomizePopulation()
        {

            for (int i = 0; i < population; i++)
            {
                for (int j = 0; j < numOfItems; j++)
                {
                    if (random.NextDouble() <= mutationRate)
                    {
                        chromosomes[i,j] = 1;
                    }
                    else
                    {
                        chromosomes[i,j] = 0;
                    }
                }
            }
        }

        public double[] sumValuesForPop()
        {
            double[] totalValue = new double[population];

            for (int i = 0; i < population; i++)
            {
                textBox3.Text += "[";

                for (int j = 0; j < numOfItems; j++)
                {
                    if (chromosomes[i,j] == 1)
                    {
                        totalValue[i] += values[i];

                        //Place holder for i + 1 value
                        int temporary = j + 1;
                        textBox3.Text += temporary.ToString() + " ";
                    }
                }

                if(i == (population - 1))
                {
                    textBox3.Text += "]";
                }
                else
                {
                    textBox3.Text += "],";
                }
            }

            //New Line Character
            textBox3.Text += "\r\n";

            return totalValue;
        }

        public double[] sumWeightsForPop()
        {
            double[] totalWeight = new double[population];

            for (int i = 0; i < population; i++)
            {
                for (int j = 0; j < numOfItems; j++)
                {
                    if (chromosomes[i,j] == 1)
                    {
                        totalWeight[i] += weights[i];
                    }
                }

            }
            return totalWeight;
        }

        //Initialize KnapSack Button 
        private void button1_Click(object sender, EventArgs e)
        {
            //Display possible items for the KnapSack 
            textBox1.Text = "Item#\tWeight\tValue\r\n1\t6\t18\r\n2\t14\t60\r\n3\t13\t47\r\n4\t9\t55\r\n5\t11\t53\r\n6\t16\t72\r\n7\t20\t90\r\n8\t17\t83\r\n9\t3\t21\r\n10\t5\t16\r\n";
            //Display max weight in textbox2
            textBox2.Text = weightCap.ToString();
        }

        //Calculate Max Value of given KnapSack
        private void button2_Click(object sender, EventArgs e)
        {
            //Give GA output textbox a scrollbar, vertical
            textBox3.ScrollBars = ScrollBars.Vertical;
            //Call geneticKnapSack()
            geneticKnapSack();

            //Output Population
            textBox6.Text = population.ToString();
            //Output Generations
            textBox8.Text = generations.ToString();
            //Output found bestFitness to Max Value
            textBox4.Text = bestFitness.ToString();
            //Output Max Weight
            textBox2.Text = weightCap.ToString();
        }

        //Print Matrix in String Form 
        private String printMatrix(int[,] numArray)
        {
            String matrixString = "";

            matrixString += "Chromosomes: [";

            for (int i = 0; i < population; i++)
            {
                for (int j = 0; j < numOfItems; j++)
                {
                    matrixString += numArray[i, j].ToString();
                    matrixString += " ";
                }

                if(i == (population - 1))
                {
                    continue;
                }
                else
                {
                    matrixString += ", ";
                }
            }

            matrixString += "]";

            return matrixString;
        }

        //Print Array in String Form
        private String printSingleMatrix(double[] numArray)
        {
            String matrixString = "[";

            for (int i = 0; i < population; i++)
            {
                matrixString += numArray[i].ToString();

                if (i == (population - 1))
                {
                    matrixString += "]";
                }
                else
                {
                    matrixString += ", ";
                }
            }

            return matrixString;
        }
    }

}
