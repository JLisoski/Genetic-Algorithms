using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS489HW4
{
    public partial class Form1 : Form
    {
        //Declare Knapsack [i,0] = w and [i,1] = value
        static int[,] knapsack = new int[,] { { 6, 18 }, { 14, 60 }, { 13, 47 }, { 9, 55 }, { 11, 53 }, { 16, 72 }, { 20, 90 }, { 17, 83 }, { 3, 21 }, { 5, 16 } };
		static int weightCap = 35;
        static int[,] calcMatrix;
        static int maxValue;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Console.WriteLine("Systems our go!...");
        }

        public static int dynamicKnapsack(int [,] numArray)
        {
			//Find number of items
			int itemCount = numArray.Length / 2;

			//Declare calculation matrix
			calcMatrix = new int[itemCount + 1, weightCap + 1];

			for (int i = 0; i <= itemCount; ++i)
			{
				for (int w = 0; w <= weightCap; ++w)
				{
					if (i == 0 || w == 0)
						calcMatrix[i, w] = 0;
					else if (knapsack[i - 1, 0] <= w)
						calcMatrix[i, w] = Math.Max(knapsack[i - 1, 1] + calcMatrix[i - 1, w - knapsack[i - 1,0]], calcMatrix[i - 1, w]);
					else
						calcMatrix[i, w] = calcMatrix[i - 1, w];
				}
			}

            maxValue = calcMatrix[itemCount, weightCap];
			return calcMatrix[itemCount, weightCap];
		}

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Item#\tWeight\tValue\r\n1\t6\t18\r\n2\t14\t60\r\n3\t13\t47\r\n4\t9\t55\r\n5\t11\t53\r\n6\t16\t72\r\n7\t20\t90\r\n8\t17\t83\r\n9\t3\t21\r\n10\t5\t16\r\n";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int result = dynamicKnapsack(knapsack);
            textBox2.Text = printMatrix(calcMatrix);
            textBox3.Text = maxValue.ToString();
            textBox4.Text = weightCap.ToString();
        }

        private String printMatrix(int [,] numArray)
        {
            String matrixString = "";

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 36; j++)
                {
                    matrixString += numArray[i, j].ToString();
                    matrixString += " ";
                }

                matrixString += Environment.NewLine;

            }

            return matrixString;
        }
    }
}
