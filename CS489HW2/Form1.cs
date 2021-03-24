using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS489HW2
{
    public partial class Form1 : Form
    {
        //Hard-coded Coordinate Sets
        int[,] numSet1 = new int[,] { { 834, 707 }, { 843, 626 }, { 140, 733 }, { 109, 723 }, { 600, 747 }, { 341, 94 }, { 657, 197 }, { 842, 123 }, { 531, 194 }, { 286, 336 } };
        int[,] numSet2 = new int[,] { { 8, 377 }, { 450, 352 }, { 519, 290 }, { 398, 604 }, { 417, 496 }, { 57, 607 }, { 119, 4 }, { 166, 663 }, { 280, 622 }, { 531, 571 } };
        int[,] numSet3 = new int[,] { { 518, 995 }, { 590, 935 }, { 600, 985 }, { 151, 225 }, { 168, 657 }, { 202, 454 }, { 310, 717 }, { 425, 802 }, { 480, 940 }, { 300, 1035 } };

        //Bool Button Click Variables
        private bool button1WasClicked = false;
        private bool button2WasClicked = false;

        //Multidimensional weight matrix
        double[,] weightMatrix = new double[10, 10];
        //Total distance variable 
        double totalDistance = 0.0;
        double currDistance1 = 0.0;

        //Initialize a local minimum and maxDistance
        //double min;
        //double maxDistance = 9999.99;
        //Declare a visited and best path array
        int[] visited = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] bestPath = new int[10];
        //Global Random Variable
        Random rand = new Random();


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Set clicked to True
            button1WasClicked = true;

            //Outputs to textbox
            textBox1.Text = "Label\tX-point\tY-point\r\n0\t834\t707\r\n1\t843\t626\r\n2\t140\t733\r\n3\t109\t723\r\n4\t600\t747\r\n5\t341\t94\r\n6\t657\t197\r\n7\t842\t123\r\n8\t531\t194\r\n9\t286\t336\r\n";

            //Outputs to console
            //Console.WriteLine(String.Join(" ", numSet1.Cast<int>()));

            //Clear Chart
            chart1.Series["Best Path"].Points.Clear();

            for (int i = 0; i < (numSet1.Length / 2); i++)
            {
                chart1.Series["Best Path"].Points.AddXY(numSet1[i, 0], numSet1[i, 1]);
                chart1.Series["Best Path"].Points[i].Label = i.ToString();
            }

            //Call makeWeightMatrix
            makeWeightMatrix(numSet1);
            //Console.WriteLine(String.Join(", ", weightMatrix.Cast<double>()));

            //Call simAnnealing
            simAnnealing(numSet1);
            //Output Distance
            textBox2.Text = totalDistance.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Set clicked to True
            button2WasClicked = true;

            //Outputs to textbox
            textBox1.Text = "Label\tX-point\tY-point\r\n0\t8\t377\r\n1\t450\t352\r\n2\t519\t290\r\n3\t398\t604\r\n4\t417\t496\r\n5\t57\t607\r\n6\t119\t4\r\n7\t166\t663\r\n8\t280\t622\r\n9\t531\t571\r\n";

            //Outputs to console
            //Console.WriteLine(String.Join(" ", numSet2.Cast<int>()));

            //Clear Chart
            chart1.Series["Best Path"].Points.Clear();

            for (int i = 0; i < (numSet2.Length / 2); i++)
            {
                chart1.Series["Best Path"].Points.AddXY(numSet2[i, 0], numSet2[i, 1]);
                chart1.Series["Best Path"].Points[i].Label = i.ToString();
            }

            //Call makeWeightMatrix()
            makeWeightMatrix(numSet2);
            //Console.WriteLine(String.Join(", ", weightMatrix.Cast<double>()));

            //Call simAnnealing
            simAnnealing(numSet2);
            //Output Distance
            textBox2.Text = totalDistance.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Outputs to textbox
            textBox1.Text = "Label\tX-point\tY-point\r\n0\t518\t995\r\n1\t590\t935\r\n2\t600\t985\r\n3\t151\t225\r\n4\t168\t657\r\n5\t202\t454\r\n6\t310\t717\r\n7\t425\t802\r\n8\t480\t940\r\n9\t300\t1035\r\n";
            //Outputs to console
            //Console.WriteLine(String.Join(" ", numSet3.Cast<int>()));

            //Clear Chart
            chart1.Series["Best Path"].Points.Clear();

            for (int i = 0; i < (numSet3.Length / 2); i++)
            {
                chart1.Series["Best Path"].Points.AddXY(numSet3[i, 0], numSet3[i, 1]);
                chart1.Series["Best Path"].Points[i].Label = i.ToString();
            }

            //Call makeWeightMatrix()
            makeWeightMatrix(numSet3);
            //Console.WriteLine(String.Join(", ", weightMatrix.Cast<double>()));

            //Call simAnnealing
            simAnnealing(numSet3);
            //Output Distance
            textBox2.Text = totalDistance.ToString();
        }
        private void makeWeightMatrix(int[,] numArray)
        {
            //Initialize/Clear weightMatrix to zeros
            for (int i = 0; i < (numArray.Length / 2); i++)
            {
                for (int j = 0; j < (numArray.Length / 2); j++)
                {
                    weightMatrix[i, j] = 0;
                }
            }

            //Calculate weights and add to weightMatrix
            for (int i = 0; i < (numArray.Length / 2); i++)
            {
                for (int j = 0; j < (numArray.Length / 2); j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    else
                    {
                        weightMatrix[i, j] = Math.Sqrt(Math.Pow(((numArray[(j), 0] - numArray[i, 0])), 2) + Math.Pow((numArray[j, 1] - numArray[i, 1]), 2));
                    }
                }
            }

        }

        private void computeDistance(int[] pathWeights)
        {
            //Reset Visited
            visited = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < pathWeights.Length-1; i++)
            {
                if (i == 0)
                {
                    //For first iteration
                    totalDistance += weightMatrix[pathWeights[i], pathWeights[i + 1]];
                    visited[pathWeights[i]] = 1;
                    visited[pathWeights[i + 1]] = 1;
                }
                else if (visited[pathWeights[i]] == 1 || visited[pathWeights[i+1]] == 1)
                {
                    //Continue loop if one of the points has been visited 
                    continue;
                }
                else
                {
                    //Adjust totalDistance and set points as visited
                    totalDistance += weightMatrix[pathWeights[i], pathWeights[i + 1]];
                    visited[pathWeights[i]] = 1;
                    visited[pathWeights[i + 1]] = 1;
                }
            }
        }

        private void setCurr2Next(int[] cPath, int[] nPath)
        {
            for (int i = 0; i < cPath.Length; i++)
            {
                //Set current path to next path
                cPath[i] = nPath[i];
            }
        }

        private void setBestPath(int[] bPath)
        {
            for (int i = 0; i < bPath.Length; i++)
            {
                //Set best path to passed path
                bestPath[i] = bPath[i];
            }
        }

        private void simAnnealing(int[,] numArray)
        {
            //Simulated Annealing Variables 
            int[] currentPath = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int[] nextPath = new int[10];
            int iteration = -1;

            //Set totalDistance to currentPath by calling computeDistance
            computeDistance(currentPath);

            //Calculation variables
            double p;
            double a = 0.999;
            double temperature = totalDistance;
            double e = 0.001;
            double delta;

            //While temperature is greater than e
            while (temperature > e)
            {
                //Initialize distance variables for each iteration
                totalDistance = 0.0;
                currDistance1 = 0.0;

                //Increase iteration
                iteration++;
                //find next path
                findNext(currentPath, nextPath);
                //Console.WriteLine("Current Path: " + String.Join(", ", currentPath.Cast<int>()));
                //Console.WriteLine("Next Path: " + String.Join(", ", nextPath.Cast<int>()));

                //Set delta to next distance - current distance;
                currDistance1 = totalDistance;
                //Calculate next distance
                computeDistance(nextPath);
                delta = totalDistance - currDistance1;

                //If delta less than 0
                if (delta < 0)
                {
                    //Set Best Path
                    setBestPath(currentPath);
                    //Set current to next
                    setCurr2Next(currentPath, nextPath);
                    //Adjust Total Distance
                    totalDistance = delta + totalDistance;
                }
                else
                {
                    //Set p to random double value
                    p = rand.NextDouble();
                    //If p less than -delta/temperature do...
                    if (p < Math.Exp(-delta/temperature))
                    {
                        //Set Best Path
                        setBestPath(currentPath);
                        //Set current to next
                        setCurr2Next(currentPath, nextPath);
                        //Adjust Total Distance
                        totalDistance = delta + totalDistance;
                    }
                }

                //Cooling process
                temperature *= a;
            }

            //Output Best Path
            textBox3.Text = String.Join("\r\n", bestPath.Cast<int>());
            //Output Temperature
            textBox4.Text = temperature.ToString();
            //Output Iterations
            textBox5.Text = iteration.ToString();

            //Output to Console Temp and Iterations
            //Console.WriteLine("Temperature = " + temperature);
            //Console.WriteLine("Iterations = " + iteration);
        }

        private void findNext(int[] cArray, int[] nArray)
        {
            //Save next as current
            for (int i = 0; i < cArray.Length; i++)
            {
                nArray[i] = cArray[i];
            }
            
            //Randomly choose two indexes
            //Declare new random object and seed
            int temp1 = rand.Next(10);
            int temp2 = rand.Next(10);

            //Swap the positions in the next array
            int temp3 = nArray[temp1];
            nArray[temp1] = nArray[temp2];
            nArray[temp2] = temp3;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Output to console to make sure start up is fine
            //Console.WriteLine("System is starting up...");
        }

        private void button4_Click(object sender, EventArgs e)
        {

            //Clear Chart
            chart1.Series["Best Path"].Points.Clear();

            //Check which button was clicked
            if (button1WasClicked)
            {
                //Plot Best Path
                for (int i = 0; i < (numSet1.Length / 2); i++)
                {
                    chart1.Series["Best Path"].Points.AddXY(numSet1[bestPath[i], 0], numSet1[bestPath[i], 1]);
                    chart1.Series["Best Path"].Points[i].Label = i.ToString();
                }

            }
            else if (button2WasClicked)
            {
                //Plot Best Path
                for (int i = 0; i < (numSet2.Length / 2); i++)
                {
                    chart1.Series["Best Path"].Points.AddXY(numSet2[bestPath[i], 0], numSet2[bestPath[i], 1]);
                    chart1.Series["Best Path"].Points[i].Label = i.ToString();
                }

            }
            else
            {
                //Plot Best Path
                for (int i = 0; i < (numSet3.Length / 2); i++)
                {
                    chart1.Series["Best Path"].Points.AddXY(numSet3[bestPath[i], 0], numSet3[bestPath[i], 1]);
                    chart1.Series["Best Path"].Points[i].Label = i.ToString();
                }

            }
        }
    }
}
