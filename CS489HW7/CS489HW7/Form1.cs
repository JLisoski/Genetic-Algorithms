using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace CS489HW7
{
    public partial class Form1 : Form
    {

        static int inputLayer;
        static int hiddenLayer;
        static int outputLayer;
        static int[] layers;
        static NeuralNetwork currentNetwork;
        static NeuralNetwork newNetwork;
        List<int> trainData;
        List<int> testData;
        float[] error;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("System is go!");
        }

        //Check inputed node counts
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
                //Default to 785
                inputLayer = 785;
                textBox4.Text += "Defaulting to 785 Neurons in the Input Layer!\r\n";
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
                    inputLayer = 785;
                    textBox4.Text += "Defaulting to 785 Neurons in the Input Layer!\r\n";
                }
                catch (FormatException)
                {
                    textBox4.Text += "The " + textBox1.GetType().Name + " value " + textBox1.Text + " is not in a recognizable format.\r\n";
                    inputLayer = 785;
                    textBox4.Text += "Defaulting to 785 Neurons in the Input Layer!\r\n";
                }
            }

            //Check for entered hidden layer amount, if not default
            if (textBox2.Text == "")
            {
                //Default to 265
                hiddenLayer = 265;
                textBox4.Text += "Defaulting to 265 Neurons in the Hidden Layer!\r\n";
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
                    hiddenLayer = 265;
                    textBox4.Text += "Defaulting to 265 Neurons in the Hidden Layer!\r\n";
                }
                catch (FormatException)
                {
                    textBox4.Text += "The " + textBox2.GetType().Name + " value " + textBox2.Text + " is not in a recognizable format.\r\n";
                    hiddenLayer = 265;
                    textBox4.Text += "Defaulting to 265 Neurons in the Hidden Layer!\r\n";
                }
            }

            //Check for entered output layer amount, if not default
            if (textBox3.Text == "")
            {
                //Default to 10
                outputLayer = 10;
                textBox4.Text += "Defaulting to 10 Neurons in the Output Layer!\r\n";
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
                    outputLayer = 10;
                    textBox4.Text += "Defaulting to 10 Neurons in the Output Layer!\r\n";
                }
                catch (FormatException)
                {
                    textBox4.Text += "The " + textBox1.GetType().Name + " value " + textBox1.Text + " is not in a recognizable format.\r\n";
                    outputLayer = 10;
                    textBox4.Text += "Defaulting to 10 Neurons in the Output Layer!\r\n";
                }
            }

            //Set currentNetwork using the constructor with the parameters layers
            layers = new int[] { inputLayer, hiddenLayer, outputLayer };

            currentNetwork = new NeuralNetwork(layers);
            textBox4.Text += "Neural Network Created!\r\n";
            textBox4.Text += "*********************************************************\r\n";
            textBox6.Text = inputLayer.ToString();
            textBox7.Text = hiddenLayer.ToString();
            textBox8.Text = outputLayer.ToString();
        }

        //Load MNIST Data
        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
            textBox4.ScrollBars = ScrollBars.Vertical;
            textBox4.Text += "******Start Reading Input!******\r\n\r\n";

            //Load Train Data
            string temporary = "";
            trainData = new List<int>();
            string path = "";
            string trainFilename = "mnist_train.csv";
            //int counter = 0;

            //Check if file path textBox empty or not
            if (textBox9.Text == "")
            {
                textBox4.Text += "Loading Default Input File...\r\n";
                path = Path.Combine(Environment.CurrentDirectory, @"CS489HW7\", trainFilename);
                Console.WriteLine(path);
            }
            else
            {
                textBox4.Text += "Loading Input File from File Path...\r\n";
                path = @textBox9.Text;
            }

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        temporary = reader.ReadLine();

                        //For Debugging
                        //Console.WriteLine(temporary);

                        if (String.IsNullOrEmpty(temporary))
                        {
                            continue;
                        }
                        else
                        {
                            foreach (var s in temporary.Split(','))
                            {
                                int num;
                                if (int.TryParse(s, out num))
                                {
                                    //Normalize and Add to list of data
                                    trainData.Add(normalizePixel(num));

                                    //For Debugging
                                    //if (counter <= 10)
                                    //{
                                    //    textBox4.Text += trainData[counter] + "\r\n";
                                    //    counter++;
                                    //}
                                }
                            }

                        }
                    }
                }

                //For Debugging 
                textBox4.Text += "Done Loading Input...\r\n\r\n";
            }
            catch (IOException error)
            {
                Console.WriteLine("The file could not be read: ");
                Console.WriteLine(error.Message);
                textBox4.Text += "******Try Again!******";
            }
            catch(FormatException f)
            {
                Console.WriteLine("Format Excpetion!");
                Console.WriteLine(f.Message);
                textBox4.Text += "******Try Again!******";
            }

            //Load Test Data 
            temporary = "";
            testData = new List<int>();
            path = "";
            string testFilename = "mnist_test.csv";
            //counter = 0;

            textBox4.Text += "Loading Test Data...\r\n";
            path = Path.Combine(Environment.CurrentDirectory, @"CS489HW7\", testFilename);

            //For Debugging
            //Console.WriteLine(path);

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        temporary = reader.ReadLine();

                        //For Debugging
                        //Console.WriteLine(temporary);

                        if (String.IsNullOrEmpty(temporary))
                        {
                            continue;
                        }
                        else
                        {
                            foreach (var s in temporary.Split(','))
                            {
                                int num;
                                if (int.TryParse(s, out num))
                                {
                                    //Normalize and Add to list of data
                                    testData.Add(normalizePixel(num));

                                    //For Debugging
                                    //if (counter <= 10)
                                    //{
                                    //    textBox4.Text += testData[counter] + "\r\n";
                                    //    counter++;
                                    //}
                                }
                            }

                        }
                    }
                }

                //For Debugging 
                textBox4.Text += "Done Loading Test Data...\r\n\r\n";
                //For Debugging 
                textBox4.Text += "******Done Reading Input!******\r\n";
            }
            catch (IOException error)
            {
                Console.WriteLine("The file could not be read: ");
                Console.WriteLine(error.Message);
                textBox4.Text += "******Try Again!******";
            }
            catch (FormatException f)
            {
                Console.WriteLine("Format Excpetion!");
                Console.WriteLine(f.Message);
                textBox4.Text += "******Try Again!******";
            }

        }

        //Train MNIST Data
        private void button3_Click(object sender, EventArgs e)
        {
            //For Debugging
            Console.WriteLine("Inside Train...");

            //Clear textBox
            textBox4.Clear();
            textBox4.ScrollBars = ScrollBars.Vertical;

            Console.WriteLine("Calling networkDriver in Train button...");
            //Call network driver function 
            networkDriver();
            Console.WriteLine("Back from networkDriver in Train button...");

            Console.WriteLine("Leaving Train...");
        }

        private int normalizePixel(int number)
        {
            int result = ((255 - number) / 255);
            return result;
        }

        private void networkDriver()
        {

            Console.WriteLine("Inside Network Driver...");

            int maxEpochs;
            
            if(textBox11.Text == "")
            {
                maxEpochs = 3;
                textBox10.Text = maxEpochs.ToString();
            }
            else
            {
                try
                {
                    maxEpochs = Convert.ToInt32(textBox11.Text);
                    textBox10.Text = maxEpochs.ToString();
                }
                catch (OverflowException)
                {
                    textBox4.Text += textBox1.Text + " is outside the range of the Int32 type.\r\n";
                    maxEpochs = 3;
                    textBox4.Text += "Defaulting to 3 Epochs!\r\n";
                    textBox10.Text = maxEpochs.ToString();
                }
                catch (FormatException)
                {
                    textBox4.Text += "The " + textBox1.GetType().Name + " value " + textBox1.Text + " is not in a recognizable format.\r\n";
                    maxEpochs = 3;
                    textBox4.Text += "Defaulting to 3 Epochs!\r\n";
                    textBox10.Text = maxEpochs.ToString();
                }
            }

            int epoch = 0;

            while(epoch < maxEpochs)
            {
                //Create new network
                newNetwork = new NeuralNetwork(layers);
                //Set to currentNetwork
                newNetwork = currentNetwork;

                Console.WriteLine("Inside While Loop...");

                var learnRate = 1f / ((epoch + 1) * 100);

                Console.WriteLine("Calling Train Network...");

                //Train Network
                trainNetwork(newNetwork);

                Console.WriteLine("Back from Train Network...");

                int correct = 0;
                int incorrect = 0;

                error = new float[newNetwork.layers[2]];
                int trainIndex = 0;

                for (int i = 0; i < testData.Count; i++)
                {
                    
                    for (int k = 0; k < newNetwork.layers[0]; k++)
                    {
                        if(trainIndex < testData.Count)
                        {
                            newNetwork.neurons[0][k] = testData[trainIndex];
                            trainIndex++;
                        }
                        else
                        {
                            continue;
                        }

                    }

                    Console.WriteLine("Calling Feed Forward...");
                    currentNetwork.FeedForward(newNetwork.neurons[0]);
                    Console.WriteLine("Back from Feed Forward...");

                    var maxOutputValue = 0f;
                    var index = 0;
                    for (int k = 0; k < newNetwork.layers[2]; k++)
                    {
                        var outputValue = newNetwork.neurons[2][k];
                        if(outputValue > maxOutputValue)
                        {
                            maxOutputValue = outputValue;
                            index = k;
                        }
                    }

                    var expectedMaxOutputValue = 0f;
                    var expectedIndex = 0;

                    trainIndex = 0;

                    for (int k = 0; k < trainData.Count; k++)
                    {
                        var outputValue = trainData[trainIndex];
                        if (outputValue > expectedMaxOutputValue)
                        {
                            expectedMaxOutputValue = outputValue;
                            expectedIndex = k;
                        }

                        trainIndex++;
                    }

                    if (index == expectedIndex) {
                        correct++;
                    }
                    else
                    {
                        incorrect++;
                    }
                }

                if(epoch % 100 == 0 && epoch != maxEpochs)
                {
                    //Output what epoch
                    textBox4.Text += "Epoch = " + epoch.ToString() + "\r\n";
                    textBox4.Text += "Error = " + error.ToString() + "\r\n";
                }

                textBox4.Text += "Epoch: " + epoch.ToString() + " Learning Rate: " + currentNetwork.learningRate.ToString() + " Correct: " + correct.ToString() + " Incorrect: " + incorrect.ToString() + "\r\n";

                //Increment epoch.
                epoch++;

                //Compare networks
                int result = newNetwork.CompareNetworks(currentNetwork);

                if(result == 1)
                {
                    currentNetwork = newNetwork;
                }
                else
                {
                    continue;
                }
            }
        }

        private void trainNetwork(NeuralNetwork network)
        {
            Console.WriteLine("Inside trainNetwork function...");

            //foreach (var data in trainData)
            //{

            int counter = 0;
            while (counter < trainData.Count)
            {

                for (int i = 0; i < network.layers[0]; i++)
                {
                    network.neurons[0][i] = trainData[counter];
                    counter++;
                }

                Console.WriteLine("Call Feed Forward in trainNetwork...");
                network.FeedForward(network.neurons[0]);
                Console.WriteLine("Back from Feed Forward in trainNetwork...");

                Console.WriteLine("Calling calculateError in trainNetwork...");
                error = network.calculateErrors(trainData);
                Console.WriteLine("Back from calculateError in trainNetwork...");

                for (int i = network.layers[1] - 1; i >= 0; i--)
                {
                    var networkLayer = network.neurons[1][i];

                    Console.WriteLine("Calling Mutate in trainNetwork...");
                    network.Mutate((int)network.learningRate, 1f);
                    Console.WriteLine("Back from Mutate in trainNetwork...");
                    Console.WriteLine("Calling PropograteError in trainNetwork...");
                    error = network.PropagateErrors(error);
                    Console.WriteLine("Back from PropograteError in trainNetwork...");
                }
            }
            //}

            textBox5.Text = error.ToString();
            Console.WriteLine("Leaving trainNetwork function...");
        }
    }
}
