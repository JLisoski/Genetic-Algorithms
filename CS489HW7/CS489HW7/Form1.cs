using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS489HW7
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
            Console.WriteLine("System is Go!");
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
                //Default to 2
                inputLayer = 2;
                textBox4.Text += "Defaulting to 2 Neurons in the Input Layer!\r\n";
            }
            else
            {
                //If not empty, convert to Int
                try
                {
                    inputLayer = Convert.ToInt32(textBox1.Text);
                    textBox4.Text += "Setting up " + inputLayer.ToString() + " Neurons in the Input Layer!\r\n";
                }
                catch (OverflowException)
                {
                    textBox4.Text += textBox1.Text + " is outside the range of the Int32 type.\r\n";
                    inputLayer = 2;
                    textBox4.Text += "Defaulting to 2 Neurons in the Input Layer!\r\n";
                }
                catch (FormatException)
                {
                    textBox4.Text += "The " + textBox1.GetType().Name + " value " + textBox1.Text + " is not in a recognizable format.\r\n";
                    inputLayer = 2;
                    textBox4.Text += "Defaulting to 2 Neruons in the Input Layer!\r\n";
                }
            }

            //Check for entered hidden layer amount, if not default
            if (textBox2.Text == "")
            {
                //Default to 4
                hiddenLayer = 4;
                textBox4.Text += "Defaulting to 4 Neurons in the Hidden Layer!\r\n";
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
                    hiddenLayer = 4;
                    textBox4.Text += "Defaulting to 4 Neurons in the Hidden Layer!\r\n";
                }
                catch (FormatException)
                {
                    textBox4.Text += "The " + textBox2.GetType().Name + " value " + textBox2.Text + " is not in a recognizable format.\r\n";
                    hiddenLayer = 4;
                    textBox4.Text += "Defaulting to 4 Neurons in the Hidden Layer!\r\n";
                }
            }

            //Check for entered output layer amount, if not default
            if (textBox3.Text == "")
            {
                //Default to 2
                outputLayer = 2;
                textBox4.Text += "Defaulting to 2 Neurons in the Output Layer!\r\n";
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
                    outputLayer = 2;
                    textBox4.Text += "Defaulting to 2 Neurons in the Output Layer!\r\n";
                }
                catch (FormatException)
                {
                    textBox4.Text += "The " + textBox1.GetType().Name + " value " + textBox1.Text + " is not in a recognizable format.\r\n";
                    outputLayer = 2;
                    textBox4.Text += "Defaulting to 2 Neurons in the Output Layer!\r\n";
                }
            }

            //Set currentNetwork using the constructor with the parameters layers
            layers = new int[] { inputLayer, hiddenLayer, outputLayer };

            currentNetwork = new NeuralNetwork(layers);
            textBox4.Text += "Neural Network Created!\r\n";
            textBox4.Text += "*********************************************************\r\n";
        }
    }
}
