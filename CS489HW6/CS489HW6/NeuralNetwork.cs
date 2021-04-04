using System;
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

            neurons = new float[layers[0]][];
            biases = new float[layers[0]][];
            weights = new float[layers[0]][][];

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
            for (int i = 0; i < inputs.Length; i++)
            {
                neurons[0][i] = inputs[i];
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

            System.IO.StreamReader file = new System.IO.StreamReader(path);

            //Grab line count
            int numOfLine = System.IO.File.ReadLines(path).Count();
            string[] ListLines = new string[numOfLine];
            int index = 1;
            for (int i = 1; i < numOfLine; i++)
            {
                ListLines[i] = file.ReadLine();
            }

            //Close file
            file.Close();

            if (new System.IO.FileInfo(path).Length > 0)
            {
                for (int i = 0; i < biases.Length; i++)
                {
                    for (int j = 0; j < biases[i].Length; j++)
                    {
                        biases[i][j] = float.Parse(ListLines[index]);
                        index++;
                    }
                }
                for (int i = 0; i < weights.Length; i++)
                {
                    for (int j = 0; j < weights[i].Length; j++)
                    {
                        for (int k = 0; k < weights[i][j].Length; k++)
                        {
                            weights[i][j][k] = float.Parse(ListLines[index]);
                            index++;
                        }
                    }
                }
            }
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
