using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS489HW6
{
    class NeuralNetwork
    {
        private int[] layers;
        public float[][] neurons;
        public float[][] biases;
        public float[][][] weights;
        public string[] inputData;
        public int numOfLine;
        //public int[] activations;
        Random rand = new Random();

        public float fitness = 0;

        public NeuralNetwork(int[] layers)
        {
            this.layers = new int[layers.Length];

            for (int i = 0; i < layers.Length; i++)
            {
                this.layers[i] = layers[i];
            }

            //neurons = new float[layers[0]][];
            //biases = new float[layers[0]][];
            //weights = new float[layers[0]][][];

            InitNeurons();
            InitBiases();
            InitWeights();
        }

        private void InitNeurons()
        {
            List<float[]> neuronsList = new List<float[]>();

            for (int i = 0; i < layers.Length; i++)
            {
                neuronsList.Add(new float[layers[i]]);
            }

            neurons = neuronsList.ToArray();
        }

        private void InitBiases()
        {
            List<float[]> biasList = new List<float[]>();

            for (int i = 0; i < layers.Length; i++)
            {
                float[] bias = new float[layers[i]];

                for (int j = 0; j < layers[i]; j++)
                {
                    //Not sure about this...
                    bias[j] = (rand.Next() * 1) - 0.5f;
                }
                biasList.Add(bias);
            }
            biases = biasList.ToArray();
        }

        private void InitWeights()
        {
            List<float[][]> weightsList = new List<float[][]>();
            for (int i = 1; i < layers.Length; i++)
            {
                List<float[]> layerWeightsList = new List<float[]>();
                int neuronsInPreviousLayer = layers[i - 1];
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    float[] neuronWeights = new float[neuronsInPreviousLayer];
                    for (int k = 0; k < neuronsInPreviousLayer; k++)
                    {
                        neuronWeights[k] = (rand.Next() * 1) - 0.5f;
                    }
                    layerWeightsList.Add(neuronWeights);
                }
                weightsList.Add(layerWeightsList.ToArray());
            }
            weights = weightsList.ToArray();
        }

        public float activate(float value)
        {
            return (float)Math.Tanh(value);
        }

        public float[] FeedForward(float[] inputs)
        {
            //For Debugging
            //Console.WriteLine("Neurons Length = " + neurons.Length.ToString());
            //Console.WriteLine("Inputs Length = " + inputs.Length.ToString());

            for (int i = 0; i < neurons.Length; i++)
            {
                for (int j = 0; j < inputs.Length; j++)
                {
                    if (j < neurons[i].Length)
                    {
                        //Console.WriteLine("Neurons Length = " + neurons[i].Length.ToString());
                        //Console.WriteLine("i = " + i.ToString());
                        //Console.WriteLine("j = " + j.ToString());
                        neurons[i][j] = inputs[j];
                    }
                }
            }

            for (int i = 1; i < layers.Length; i++)
            {
                int layer = i - 1;
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    float value = 0f;
                    for (int k = 0; k < neurons[i - 1].Length; k++)
                    {
                        value += weights[i - 1][j][k] * neurons[i - 1][k];
                    }
                    neurons[i][j] = activate(value + biases[i][j]);
                }
            }
            return neurons[neurons.Length - 1];
        }

        public int CompareNetworks(NeuralNetwork other)
        {
            if (other == null)
            {
                return 1;
            }
            else if (fitness > other.fitness)
            {
                return 1;
            }else if(fitness < other.fitness)
            {
                return -1;
            }
            else
            {
                return 0;
            }

        }

        public void Load(string path)
        {
            string temporary = "";
            int counter = 0;
            string[] temp = new string[100];
            inputData = new string[100];
            int id = 0;

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        temporary = sr.ReadLine();
                        temp[id] = temporary;
                        //inputData[id] = temporary;
                        //For Debugging
                        Console.WriteLine(temporary);
                        inputData[id] = temporary;

                        id++;
                        counter++;
                    }
                    sr.Close();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read: ");
                Console.WriteLine(e.Message);
            }

            //Grab line count
            numOfLine = counter;
            //For Debugging
            //Console.WriteLine("The number of lines are... " + numOfLine.ToString());

            string[] ListLines = new string[numOfLine];
            int index = 1;

            for (int i = 0; i < numOfLine; i++)
            {
                ListLines[i] = temp[i];

                //For Debugging
                //Console.WriteLine("ListLines[] = " + ListLines[i].ToString());
            }

            //For Debugging
            Console.WriteLine("Being Reading File...");

            if (new FileInfo(path).Length > 0)
            {
                //For Debugging
                //Console.WriteLine("Inside the if path is longer then zero...");

                for (int i = 0; i < biases.Length; i++)
                {
                    for (int j = 0; j < biases[i].Length; j++)
                    {
                        biases[i][j] = float.Parse(ListLines[index]);
                        index++;
                    }
                }

                //For Debugging
                //Console.WriteLine("Index Before Second For Loop = " + index.ToString());

                for (int i = 0; i < weights.Length; i++)
                {
                    //Console.WriteLine("weights.length = " + weights.Length.ToString());
                    for (int j = 0; j < weights[i].Length; j++)
                    {
                        //Console.WriteLine("weights[i].length = " + weights[i].Length.ToString());
                        for (int k = 0; k < weights[i][j].Length; k++)
                        {
                            //Console.WriteLine("weights[i][j].length = " + weights[i][j].Length.ToString());
                            //Console.WriteLine("i = " + i.ToString() + ", j = " + j.ToString() + ", k = " + k.ToString());
                            //Console.WriteLine("ListLines Length = " + ListLines.Length);
                            //Console.WriteLine("Index = " + index.ToString());

                            if(index < 16)
                            {
                                weights[i][j][k] = float.Parse(ListLines[index]);
                            }

                            index++;
                        }
                    }
                }
            }

            Console.WriteLine("File Read!!!\r\n");
        }

        public void Mutate(int chance, float val)
        {
            for (int i = 0; i < biases.Length; i++)
            {
                for (int j = 0; j < biases[i].Length; j++)
                {
                    biases[i][j] = ((rand.Next()*(chance)) <= 5) ? biases[i][j] += (rand.Next()*(val-(-val)))+(val) : biases[i][j];
                }
            }

            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] = ((rand.Next() * (chance)) <= 5) ? weights[i][j][k] += (rand.Next() * (val - (-val))) + (val) : weights[i][j][k];

                    }
                }
            }
        }


    }
}
