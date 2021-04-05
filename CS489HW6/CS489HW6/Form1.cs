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
        static int[] layers = new int[]{ inputLayer, hiddenLayer, outputLayer };
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
                textBox4.Text += "Defaulting to 3 Input Layers!\r\n";
            }
            else
            {
                //If not empty, convert to Int
                try
                {
                    inputLayer = Convert.ToInt32(textBox1.Text);
                    textBox4.Text += "Setting up " + inputLayer.ToString() + " Input Layers!\r\n";
                }
                catch(OverflowException)
                {
                    textBox4.Text += textBox1.Text + " is outside the range of the Int32 type.\r\n";
                    inputLayer = 3;
                    textBox4.Text += "Defaulting to 3 Input Layers!\r\n";
                }
                catch (FormatException)
                {
                    textBox4.Text += "The " + textBox1.GetType().Name + " value " + textBox1.Text + " is not in a recognizable format.\r\n";
                    inputLayer = 3;
                    textBox4.Text += "Defaulting to 3 Input Layers!\r\n";
                }
            }

            //Check for entered hidden layer amount, if not default
            if (textBox2.Text == "")
            {
                //Default to 6
                hiddenLayer = 6;
                textBox4.Text += "Defaulting to 6 Hidden Layers!\r\n";
            }
            else
            {
                //If not empty, convert to Int
                try
                {
                    hiddenLayer = Convert.ToInt32(textBox2.Text);
                    textBox4.Text += "Setting up " + hiddenLayer.ToString() + " Hidden Layers!\r\n";
                }
                catch (OverflowException)
                {
                    textBox4.Text += textBox2.Text + " is outside the range of the Int32 type.\r\n";
                    hiddenLayer = 6;
                    textBox4.Text += "Defaulting to 6 Hidden Layers!\r\n";
                }
                catch (FormatException)
                {
                    textBox4.Text += "The " + textBox2.GetType().Name + " value " + textBox2.Text + " is not in a recognizable format.\r\n";
                    hiddenLayer = 6;
                    textBox4.Text += "Defaulting to 6 Hidden Layers!\r\n";
                }
            }

            //Check for entered output layer amount, if not default
            if (textBox3.Text == "")
            {
                //Default to 3
                outputLayer = 3;
                textBox4.Text += "Defaulting to 3 Output Layers!\r\n";
            }
            else
            {
                //If not empty, convert to Int
                try
                {
                    outputLayer = Convert.ToInt32(textBox3.Text);
                    textBox4.Text += "Setting up " + outputLayer.ToString() + " Output Layers!\r\n";
                }
                catch (OverflowException)
                {
                    textBox4.Text += textBox1.Text + " is outside the range of the Int32 type.\r\n";
                    outputLayer = 3;
                    textBox4.Text += "Defaulting to 3 Input Layers!\r\n";
                }
                catch (FormatException)
                {
                    textBox4.Text += "The " + textBox1.GetType().Name + " value " + textBox1.Text + " is not in a recognizable format.\r\n";
                    outputLayer = 3;
                    textBox4.Text += "Defaulting to 3 Input Layers!\r\n";
                }
            }

            //Set currentNetwork using the constructor with the parameters layers
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
                for (int j = 0; j < numArray.Length; j++)
                {
                    matrixString += numArray[i].ToString();
                    matrixString += " ";
                }

                matrixString += "]";
            }

            return matrixString;
        }

        //Print Array in String Form
        private String printSingleMatrix(double[] numArray)
        {
            String matrixString = "[";

            for (int i = 0; i < numArray.Length; i++)
            {
                matrixString += numArray[i].ToString();

                if (i == (numArray.Length - 1))
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

        private String printTripleMatrix(float[][][] numArray)
        {
            String matrixString = "";

            matrixString += "[";

            for (int i = 0; i < numArray.Length; i++)
            {
                for (int k = 0; k < numArray.Length; k++)
                {
                    for (int j = 0; j < numArray.Length; j++)
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
            textBox4.Clear();

            //Gives output box a vertical scroll bar
            textBox4.ScrollBars = ScrollBars.Vertical;

            //Load the input file
            textBox4.Text += "*********************************************************\r\n";
            textBox4.Text += "Loading Input File...\r\n";

            //Check if file path has been entered
            if (textBox5.Text == "")
            {
                //Default to Test.txt
                textBox4.Text += "Defaulting to Test File!\r\n";
                try
                {
                    currentNetwork.Load(@"C:\Users\JLiso\source\repos\JLisoski\CS-489\CS489HW6\CS489HW6\Test.txt");
                    textBox4.Text += "Input File Loaded!!!\r\n";
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
                    currentNetwork.Load(textBox5.Text);
                    textBox4.Text += "Input File Loaded!!!";
                }
                catch (System.IO.FileNotFoundException)
                {
                    textBox4.Text += "File Not Found!\r\n";
                    textBox4.Text += "Input File Failed to Load!\r\n";
                }
            }

            textBox4.Text += "*********************************************************\r\n";
            textBox4.Text += "Printing Neural Network Information...\r\n";
            textBox4.Text += "Neurons = " + printDoubleMatrix(currentNetwork.neurons) + "\r\n";
            textBox4.Text += "Biases = " + printDoubleMatrix(currentNetwork.biases) + "\r\n";
            textBox4.Text += "Weights = " + printTripleMatrix(currentNetwork.weights) + "\r\n";
            textBox4.Text += "Fitness = " + currentNetwork.fitness.ToString() + "\r\n";
            textBox4.Text += "*********************************************************\r\n";
        }
    }
}
