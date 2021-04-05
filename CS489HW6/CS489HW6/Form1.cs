using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS489HW6
{
    public partial class Form1 : Form
    {

        static int inputLayer;
        static int hiddenLayer;
        static int outputLayer;
        static int[] layers;
        static NeuralNetwork currentNetwork;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("System is go!!!");
        }

        public void neuralNetwork()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Clear out textbox
            textBox4.Clear();

            //Initialize the network
            textBox4.Text += "*********************************************************\r\n";
            textBox4.Text += "Initializing Neural Network...\r\n";

            //Check for entered intput layer amount, if not default
            if (textBox1.Text == "")
            {
                //Default to 3
                inputLayer = 3;
                textBox4.Text += "Defaulting to 3 Neurons in the Input Layer!\r\n";
            }
            else
            {
                //If not empty, convert to Int
                try
                {
                    inputLayer = Convert.ToInt32(textBox1.Text);
                    textBox4.Text += "Setting up " + inputLayer.ToString() + " Neurons in the Input Layer!\r\n";
                }
                catch(OverflowException)
                {
                    textBox4.Text += textBox1.Text + " is outside the range of the Int32 type.\r\n";
                    inputLayer = 3;
                    textBox4.Text += "Defaulting to 3 Neurons in the Input Layer!\r\n";
                }
                catch (FormatException)
                {
                    textBox4.Text += "The " + textBox1.GetType().Name + " value " + textBox1.Text + " is not in a recognizable format.\r\n";
                    inputLayer = 3;
                    textBox4.Text += "Defaulting to 3 Neruons in the Input Layer!\r\n";
                }
            }

            //Check for entered hidden layer amount, if not default
            if (textBox2.Text == "")
            {
                //Default to 6
                hiddenLayer = 6;
                textBox4.Text += "Defaulting to 6 Neurons in the Hidden Layer!\r\n";
            }
            else
            {
                //If not empty, convert to Int
                try
                {
                    hiddenLayer = Convert.ToInt32(textBox2.Text);
                    textBox4.Text += "Setting up " + hiddenLayer.ToString() + " Neurons in the Hidden Layer!\r\n";
                }
                catch (OverflowException)
                {
                    textBox4.Text += textBox2.Text + " is outside the range of the Int32 type.\r\n";
                    hiddenLayer = 6;
                    textBox4.Text += "Defaulting to 6 Neurons in the Hidden Layer!\r\n";
                }
                catch (FormatException)
                {
                    textBox4.Text += "The " + textBox2.GetType().Name + " value " + textBox2.Text + " is not in a recognizable format.\r\n";
                    hiddenLayer = 6;
                    textBox4.Text += "Defaulting to 6 Neurons in the Hidden Layer!\r\n";
                }
            }

            //Check for entered output layer amount, if not default
            if (textBox3.Text == "")
            {
                //Default to 3
                outputLayer = 3;
                textBox4.Text += "Defaulting to 3 Neurons in the Output Layer!\r\n";
            }
            else
            {
                //If not empty, convert to Int
                try
                {
                    outputLayer = Convert.ToInt32(textBox3.Text);
                    textBox4.Text += "Setting up " + outputLayer.ToString() + " Neurons in the Output Layer!\r\n";
                }
                catch (OverflowException)
                {
                    textBox4.Text += textBox1.Text + " is outside the range of the Int32 type.\r\n";
                    outputLayer = 3;
                    textBox4.Text += "Defaulting to 3 Neurons in the Output Layer!\r\n";
                }
                catch (FormatException)
                {
                    textBox4.Text += "The " + textBox1.GetType().Name + " value " + textBox1.Text + " is not in a recognizable format.\r\n";
                    outputLayer = 3;
                    textBox4.Text += "Defaulting to 3 Neurons in the Output Layer!\r\n";
                }
            }

            //Set currentNetwork using the constructor with the parameters layers
            layers = new int[] { inputLayer, hiddenLayer, outputLayer };

            //Console.WriteLine("Layers: \r\n");
            //for (int i = 0; i < layers.Length; i++)
            //{
            //    Console.WriteLine(layers[i].ToString() + "\r\n");
            //}

            currentNetwork = new NeuralNetwork(layers);
            textBox4.Text += "Neural Network Created!\r\n";
            textBox4.Text += "*********************************************************\r\n";
        }

        //Print Matrix in String Form 
        private String printDoubleMatrix(float[][] numArray)
        {
            String matrixString = "";

            matrixString += "[";

            for (int i = 0; i < numArray.Length; i++)
            {
                for (int j = 0; j < numArray[i].Length; j++)
                {
                    matrixString += numArray[i].ToString();
                    matrixString += " ";
                }

                matrixString += "]";
            }

            return matrixString;
        }

        //Print Array in String Form
        private String printSingleMatrix(string[] numArray)
        {
            String matrixString = "[";

            for (int i = 0; i < numArray.Length; i++)
            {
                matrixString += numArray[i].ToString();

                if (i == (currentNetwork.numOfLine/2)-1)
                {
                    matrixString += "]\r\n[";
                }
                else
                {
                    if(i == currentNetwork.numOfLine - 1)
                    {
                        matrixString += "]";
                        break;
                    }
                    else
                    {
                        matrixString += ", ";
                    }
                }
            }

            return matrixString;
        }

        private String printTripleMatrix(float[][][] numArray)
        {
            String matrixString = "";

            matrixString += "[";

            for (int i = 0; i < numArray.Length; i++)
            {
                for (int k = 0; k < numArray[i].Length; k++)
                {
                    for (int j = 0; j < numArray[i][k].Length; j++)
                    {
                        matrixString += numArray[i].ToString();
                        matrixString += " ";
                    }

                    if (i == (numArray.Length - 1))
                    {
                        continue;
                    }
                    else
                    {
                        matrixString += ", ";
                    }
                }
            }

            matrixString += "]";

            return matrixString;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //Clear TextBox
            textBox6.Clear();

            //Gives output box a vertical scroll bar
            textBox4.ScrollBars = ScrollBars.Vertical;
            textBox6.ScrollBars = ScrollBars.Vertical;

            //Load the input file
            textBox4.Text += "*********************************************************\r\n";
            textBox4.Text += "Loading Input File...\r\n";

            //Check if file path has been entered
            if (textBox5.Text == "")
            {
                //Default to Test.txt
                textBox4.Text += "Defaulting to XOR/AND File!\r\n";
                try
                {
                    currentNetwork.Load(@"C:\Users\JLiso\source\repos\JLisoski\CS-489\CS489HW6\CS489HW6\XORAND.txt");
                    textBox4.Text += "Input File Loaded!!!\r\n";
                    textBox6.Text += printSingleMatrix(currentNetwork.inputData);
                }
                catch (System.IO.FileNotFoundException)
                {
                    textBox4.Text += "File Not Found!!!\r\n";
                    textBox4.Text += "Input File Failed to Load!\r\n";
                }
            }
            else
            {
                try
                {
                    //Call Neural Network Load Function 
                    currentNetwork.Load(@textBox5.Text);
                    textBox4.Text += "Input File Loaded!!!\r\n";
                    textBox6.Text += printSingleMatrix(currentNetwork.inputData);
                }
                catch (System.IO.FileNotFoundException)
                {
                    textBox4.Text += "File Not Found!\r\n";
                    textBox4.Text += "Input File Failed to Load!\r\n";
                }
            }

            textBox4.Text += "*********************************************************\r\n";
            textBox4.Text += "Neural Network Information...\r\n";
            textBox4.Text += "\r\n";
            textBox4.Text += "Neurons:\r\n";
            for (int i = 0; i < currentNetwork.neurons.Length; i++)
            {
                for (int j = 0; j < currentNetwork.neurons[i].Length; j++)
                {

                    if (j == currentNetwork.neurons[i].Length - 1)
                    {
                        textBox4.Text += "Neurons[" + i.ToString() + "][" + j.ToString() + "] = " + currentNetwork.neurons[i][j].ToString();
                    }
                    else
                    {

                        textBox4.Text += "Neurons[" + i.ToString() + "][" + j.ToString() + "] = " + currentNetwork.neurons[i][j].ToString() + ", ";

                    }

                }

                textBox4.Text += "\r\n";
            }

            textBox4.Text += "\r\n";
            textBox4.Text += "Biases:\r\n";
            for (int i = 0; i < currentNetwork.biases.Length; i++)
            {
                for (int j = 0; j < currentNetwork.biases[i].Length; j++)
                {

                    if (j == currentNetwork.biases[i].Length - 1)
                    {
                        textBox4.Text += "Biases[" + i.ToString() + "][" + j.ToString() + "] = " + currentNetwork.biases[i][j].ToString();
                    }
                    else
                    {

                        textBox4.Text += "Biases[" + i.ToString() + "][" + j.ToString() + "] = " + currentNetwork.biases[i][j].ToString() + ", ";

                    }

                }

                textBox4.Text += "\r\n";
            }

            textBox4.Text += "\r\n";
            textBox4.Text += "Weights:\r\n";
            for (int i = 0; i < currentNetwork.weights.Length; i++)
            {
                for (int j = 0; j < currentNetwork.weights[i].Length; j++)
                {
                    for (int k = 0; k < currentNetwork.weights[i][j].Length; k++)
                    {
                        if (k == currentNetwork.weights[i][j].Length - 1)
                        {
                            textBox4.Text += "Weights[" + i.ToString() + "][" + j.ToString() + "][" + k.ToString() + "] = " + currentNetwork.weights[i][j][k].ToString();
                        }
                        else
                        {

                            textBox4.Text += "Weights[" + i.ToString() + "][" + j.ToString() + "][" + k.ToString() + "] = " + currentNetwork.weights[i][j][k].ToString() + ", ";

                        }
                    }

                    textBox4.Text += "\r\n";
                }
            }

            textBox4.Text += "\r\n";
            textBox4.Text += "Fitness = " + currentNetwork.fitness.ToString() + "\r\n";
            textBox4.Text += "*********************************************************\r\n";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Clear TextBox
            textBox4.Clear();
            textBox7.Clear();

            //Declare Float Array of Inputs
            float[] inputs = new float[currentNetwork.numOfLine];

            for (int i = 0; i < currentNetwork.numOfLine; i++)
            {
                inputs[i] = float.Parse(currentNetwork.inputData[i]);
            }

            float[] output = currentNetwork.FeedForward(inputs);
            currentNetwork.Mutate(2, 0.5f);

            //Output Prediction
            for (int i = 0; i < currentNetwork.neurons.Length; i++)
            {
                for (int j = 0; j < currentNetwork.neurons[i].Length; j++)
                {
                    if(currentNetwork.neurons[i][j] < 0.5)
                    {
                        currentNetwork.neurons[i][j] = 0;
                    }
                    else
                    {
                        currentNetwork.neurons[i][j] = 1;
                    }

                    if (i==0 && j==0)
                    {
                        textBox7.Text += "[" + currentNetwork.neurons[i][j].ToString();
                    }
                    else
                    {
                         
                        textBox7.Text += "," + currentNetwork.neurons[i][j].ToString();

                    }
                }
            }

            textBox7.Text += "]\r\n";

            //Print Network Info
            textBox4.Text += "*********************************************************\r\n";
            textBox4.Text += "Neural Network Information...\r\n";
            textBox4.Text += "\r\n";
            textBox4.Text += "Neurons:\r\n";
            for (int i = 0; i < currentNetwork.neurons.Length; i++)
            {
                for (int j = 0; j < currentNetwork.neurons[i].Length; j++)
                {

                    if(j == currentNetwork.neurons[i].Length-1)
                    {
                        textBox4.Text += "Neurons[" + i.ToString() + "][" + j.ToString() + "] = " + currentNetwork.neurons[i][j].ToString();
                    }
                    else
                    {

                        textBox4.Text += "Neurons[" + i.ToString() + "][" + j.ToString() + "] = " + currentNetwork.neurons[i][j].ToString() + ", ";

                    }

                }

                textBox4.Text += "\r\n";
            }

            textBox4.Text += "\r\n";

            textBox4.Text += "Biases:\r\n";
            for (int i = 0; i < currentNetwork.biases.Length; i++)
            {
                for (int j = 0; j < currentNetwork.biases[i].Length; j++)
                {

                    if (j == currentNetwork.biases[i].Length - 1)
                    {
                        textBox4.Text += "Biases[" + i.ToString() + "][" + j.ToString() + "] = " + currentNetwork.biases[i][j].ToString();
                    }
                    else
                    {

                        textBox4.Text += "Biases[" + i.ToString() + "][" + j.ToString() + "] = " + currentNetwork.biases[i][j].ToString() + ", ";

                    }

                }

                textBox4.Text += "\r\n";
            }

            textBox4.Text += "\r\n";

            textBox4.Text += "Weights:\r\n";
            for (int i = 0; i < currentNetwork.weights.Length; i++)
            {
                for (int j = 0; j < currentNetwork.weights[i].Length; j++)
                {
                    for (int k = 0; k < currentNetwork.weights[i][j].Length; k++)
                    {
                        if (k == currentNetwork.weights[i][j].Length - 1)
                        {
                            textBox4.Text += "Weights[" + i.ToString() + "][" + j.ToString() + "][" + k.ToString() + "] = " + currentNetwork.weights[i][j][k].ToString();
                        }
                        else
                        {

                            textBox4.Text += "Weights[" + i.ToString() + "][" + j.ToString() + "][" + k.ToString() + "] = " + currentNetwork.weights[i][j][k].ToString() + ", ";

                        }
                    }

                    textBox4.Text += "\r\n";
                }
            }

            textBox4.Text += "\r\n";

            textBox4.Text += "Fitness = " + currentNetwork.fitness.ToString() + "\r\n";
            textBox4.Text += "*********************************************************\r\n";
        }
    }
}
