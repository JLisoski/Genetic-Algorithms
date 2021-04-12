using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS489HW7
{
    class NeuralNetwork
    {
        public int[] layers;
        public float[][] neurons;
        public float[][] biases;
        public float[][][] weights;
        public string[] inputData;
        public int numOfLine;
        //public int[] activations;

        Random rand = new Random();

        public float fitness = 0;
        public float learningRate = 0.1f;
        public float cost = 0;

        public NeuralNetwork(int[] layers)
        {
            this.layers = new int[layers.Length];

            for (int i = 0; i < layers.Length; i++)
            {
                this.layers[i] = layers[i];
            }

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
                int neuronsInPreviousLayer = layers[i-1];

                for (int j = 0; j < layers[i]; j++)
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

            for (int i = 0; i < neurons[0].Length; i++)
            {
                //for (int j = 0; j < inputs.Length; j++)
                //{
                //    if (j < neurons[i].Length)
                //    {
                //        //Console.WriteLine("Neurons Length = " + neurons[i].Length.ToString());
                //        //Console.WriteLine("i = " + i.ToString());
                //        //Console.WriteLine("j = " + j.ToString());
                //        neurons[i][j] = inputs[j];
                //    }
                //}

                neurons[0][i] = inputs[i];
            }

            for (int i = 1; i < layers.Length; i++)
            {
                int layer = i - 1;
                for (int j = 0; j < layers[i]; j++)
                {
                    float value = 0f;
                    for (int k = 0; k < layers[i - 1]; k++)
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
            else if (cost < other.cost)
            {
                return 1;
            }else if(cost > other.cost)
            {
                return -1;
            }
            else
            {
                //cost == other.cost
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
                    biases[i][j] = ((rand.Next()*(chance)) <= 2) ? biases[i][j] += (rand.Next()*(val-(-val)))+(val) : biases[i][j];
                }
            }

            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] = ((rand.Next() * (chance)) <= 2) ? weights[i][j][k] += (rand.Next() * (val - (-val))) + (val) : weights[i][j][k];

                    }
                }
            }
        }

        public void backPropagate(float[] inputs, float[] expected)
        {
            float[] output = FeedForward(inputs);

            cost = 0;
            for (int i = 0; i < output.Length; i++)
            {
                cost += (float)Math.Pow(output[i] - expected[i], 2);
            }
            cost = cost / 2;

            float[][] gamma;

            List<float[]> gammaList = new List<float[]>();

            for (int i = 0; i < layers.Length; i++)
            {
                gammaList.Add(new float[layers[i]]);
            }

            gamma = gammaList.ToArray();

            int layer = layers.Length - 2;
            for (int i = 0; i < output.Length; i++)
            {
                gamma[layers.Length - 1][i] = (output[i] - expected[i]) * activate(output[i]);
            }

            for (int i = 0; i < layers[layers.Length-1]; i++)
            {
                biases[layers.Length - 2][i] -= gamma[layers.Length - 1][i] * learningRate;

                for (int j = 0; j < layers[layers.Length-2]; j++)
                {
                    weights[layers.Length - 2][i][j] -= gamma[layers.Length - 1][i] * neurons[layers.Length - 2][j] * learningRate;
                }
            }

            for (int i = layers.Length - 2; i > 0; i--)
            {
                layer = i - 1;
                for (int j = 0; j < layers[i]; j++)
                {
                    gamma[i][j] = 0;
                    for (int k = 0; k < gamma[i + 1].Length; k++)
                    {
                        gamma[i][j] += gamma[i + 1][k] * weights[i][k][j];
                    }
                    gamma[i][j] *= activate(neurons[i][j]);
                }
                for (int j = 0; j < layers[i]; j++)
                {
                    biases[i - 1][j] -= gamma[i][j] * learningRate;
                    for (int k = 0; k < layers[i - 1]; k++)
                    {
                        weights[i - 1][j][k] -= gamma[i][j] * neurons[i - 1][k] * learningRate;
                    }
                }
            }
        }

        public float[] calculateErrors(List<int> trainingData)
        {
            var errors = new float[this.layers[2]];
            for (int i = 0; i < this.layers[2]; i++)
            {
                var output = this.neurons[2][i];
                errors[i] = output * (1 - output) * (trainingData[i] - output);
            }

            return errors;
        }
        public float[] PropagateErrors(float[] errors)
        {
            var propagatedErrors = new float[this.layers[0]];

            for (int j = 0; j < this.layers[2]; j++)
            {
                var output = this.neurons[2][j];
                //Update the expected output for the next network layer
                for (var k = 0; k < propagatedErrors.Length; k++)
                {
                    propagatedErrors[k] += this.weights[0][j][k] * errors[j] * output * (1 - output);
                }
            }
            return propagatedErrors;
        }

        public void modifyWeights(float[] errors)
        {
            for (int i = 0; i < layers[2]; i++)
            {

                for (int j = 0; j < layers[1]; j++)
                {
                    for (int k = 0; k < layers[0]; k++)
                    {
                        weights[i][j][k] = weights[i][j][k] + errors[j] * neurons[0][k] * learningRate;
                    }
                }
            }
        }
    }
}
