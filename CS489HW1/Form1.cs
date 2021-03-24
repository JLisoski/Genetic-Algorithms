using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS489HW1
{
    public partial class Form1 : Form
    {
        //Hard-coded Coordinate Sets
        int[,] numSet1 = new int[,] { { 834, 707 }, { 843, 626 }, { 140, 733 }, { 109, 723 }, { 600, 747 }, { 341, 94 }, { 657, 197 }, { 842, 123 }, { 531, 194 }, { 286, 336 } };
        int[,] numSet2 = new int[,] { { 8, 377 }, { 450, 352 }, { 519, 290 }, { 398, 604 }, { 417, 496 }, { 57, 607 }, { 119, 4 }, { 166, 663 }, { 280, 622 }, { 531, 571 } };
        int[,] numSet3 = new int[,] { { 518, 995 }, { 590, 935 }, { 600, 985 }, { 151, 225 }, { 168, 657 }, { 202, 454 }, { 310, 717 }, { 425, 802 }, { 480, 940 }, { 300, 1035 } };

        //Int array to place the best path
        int[] bestPath = new int[10];
        //Visited array (0=not visited, 1=visited)
        int[] visited = new int[10];
        //Multidimensional weight matrix
        double[,] weightMatrix = new double[10, 10];
        //Total distance variable 
        double totalDistance;
        //Initialize a local minimum and maxDistance
        double min;
        double maxDistance = 9999.99;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("System starting up..");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Outputs to textbox
            Output.Text = "Label\tX-point\tY-point\r\n0\t834\t707\r\n1\t843\t626\r\n2\t140\t733\r\n3\t109\t723\r\n4\t600\t747\r\n5\t341\t94\r\n6\t657\t197\r\n7\t842\t123\r\n8\t531\t194\r\n9\t286\t336\r\n";

            //Outputs to console
            //Console.WriteLine(String.Join(" ", numSet1.Cast<int>()));

            //Clear Chart
            chart1.Series["Best Path"].Points.Clear();

            for (int i = 0; i < (numSet1.Length/2); i++)
            {
                chart1.Series["Best Path"].Points.AddXY(numSet1[i, 0], numSet1[i, 1]);
                chart1.Series["Best Path"].Points[i].Label = i.ToString();
            }

            //Call makeWeightMatrix
            makeWeightMatrix(numSet1);
            //Console.WriteLine(String.Join(", ", weightMatrix.Cast<double>()));

            //Call findPath
            findPath(numSet1);
            distanceTotal.Text = totalDistance.ToString();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            //Outputs to textbox
            Output.Text = "Label\tX-point\tY-point\r\n0\t8\t377\r\n1\t450\t352\r\n2\t519\t290\r\n3\t398\t604\r\n4\t417\t496\r\n5\t57\t607\r\n6\t119\t4\r\n7\t166\t663\r\n8\t280\t622\r\n9\t531\t571\r\n";

            //Outputs to console
            //Console.WriteLine(String.Join(" ", numSet2.Cast<int>()));

            //Clear Chart
            chart1.Series["Best Path"].Points.Clear();

            for (int i = 0; i < (numSet2.Length/2); i++)
            {
                chart1.Series["Best Path"].Points.AddXY(numSet2[i, 0], numSet1[i, 1]);
                chart1.Series["Best Path"].Points[i].Label = i.ToString();
            }

            //Call makeWeightMatrix()
            makeWeightMatrix(numSet2);
            //Console.WriteLine(String.Join(", ", weightMatrix.Cast<double>()));

            //Call findPath
            findPath(numSet2);
            distanceTotal.Text = totalDistance.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Outputs to textbox
            Output.Text = "Label\tX-point\tY-point\r\n0\t518\t995\r\n1\t590\t935\r\n2\t600\t985\r\n3\t151\t225\r\n4\t168\t657\r\n5\t202\t454\r\n6\t310\t717\r\n7\t425\t802\r\n8\t480\t940\r\n9\t300\t1035\r\n";
            //Outputs to console
            //Console.WriteLine(String.Join(" ", numSet3.Cast<int>()));

            //Clear Chart
            chart1.Series["Best Path"].Points.Clear();

            for (int i = 0; i < (numSet3.Length/2); i++)
            {
                chart1.Series["Best Path"].Points.AddXY(numSet3[i, 0], numSet3[i, 1]);
                chart1.Series["Best Path"].Points[i].Label = i.ToString();
            }

            //Call makeWeightMatrix()
            makeWeightMatrix(numSet3);
            //Console.WriteLine(String.Join(", ", weightMatrix.Cast<double>()));

            //Call findPath
            findPath(numSet3);
            distanceTotal.Text = totalDistance.ToString();
        }

        private void makeWeightMatrix(int[,] numArray)
        {
            //Initialize/Clear weightMatrix to zeros
            for (int i = 0; i < (numArray.Length/2); i++)
            {
                for (int j = 0; j < (numArray.Length/2); j++)
                {
                    weightMatrix[i, j] = 0;
                }
            }

            //Calculate weights and add to weightMatrix
            for (int i = 0; i < (numArray.Length/2); i++)
            {
                for (int j = 0; j < (numArray.Length/2); j++)
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
        private void findPath(int[,] numArray)
        {

            //Set totalDistacne to zero
            totalDistance = 0;

            for (int i = 0; i < (numArray.Length / 2); i++)
            {
                min = maxDistance;
                for (int j = 0; j < (numArray.Length / 2); j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    else
                    {
                        if (weightMatrix[i, j] <= min)
                        {
                            min = weightMatrix[i, j];
                        }
                    }

                }

                //Update totalDistance
                totalDistance = totalDistance + min;
                //Console.WriteLine("Total Distance = {0}", totalDistance.ToString());

            }

        }
    }
}
