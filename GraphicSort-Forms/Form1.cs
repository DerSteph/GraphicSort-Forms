using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace GraphicSort_Forms
{
    public partial class Form1 : Form
    {
        // Globale Variablen und Objekte
        public static Random rnd = new Random();
        static Pen stift = new Pen(Color.Red, 8);
        static Pen deleter = new Pen(Color.LightGray, 8);
        static Graphics grafik;
        static int[] a = new int[100];

        public Form1()
        {
            InitializeComponent();
            // beim Start muss er ein Random Bild laden
            a = Enumerable.Range(0, 100).OrderBy(c => rnd.Next()).ToArray();
            grafik = this.pictureBox1.CreateGraphics();
        }
        private void BubbleSort_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a.Length - 1 - i; j++)
                {
                    if (a[j] > a[j + 1])
                    {
                        int h = a[j + 1];
                        a[j + 1] = a[j];
                        a[j] = h;
                        ChangePosition(j, j+1);
                    }
                }
            }
            for(int i = 0; i < 100; i++)
            {
                Debug.Write(a[i]);
            }
        }

        private void InsertionSort_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < a.Length; i++)
            {
                int k = i;
                for (k = i; k > 0; k--)
                {
                    if (a[k] < a[k - 1])
                    {
                        int h = a[k];
                        a[k] = a[k - 1];
                        a[k - 1] = h;
                        ChangePosition(k, k-1);
                    }
                }
            }
        }
        private void ChangePosition(int x1, int x2)
        {
            grafik.DrawLine(deleter, 5 + x1 * 8, 600, 5 + x1 * 8, 600 - a[x2] * 6 - 6);
            grafik.DrawLine(stift, 5 + x1 * 8, 600, 5 + x1 * 8, 600 - a[x1] * 6-6);
            grafik.DrawLine(deleter, 5 + x2 * 8, 600, 5 + x2 * 8, 600 - a[x1] * 6 - 6);
            grafik.DrawLine(stift, 5 + x2 * 8, 600, 5 + x2 * 8, 600 - a[x2] * 6-6);
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            label1.Visible = false;
            a = Enumerable.Range(0, 100).OrderBy(c => rnd.Next()).ToArray();
            for (int i = 0; i < 100; i++)
            {
                grafik.DrawLine(deleter, 5 + i * 8, 600, 5 + i * 8, 0);
                grafik.DrawLine(stift, 5 + i * 8, 600, 5 + i * 8, 600 - a[i] * 6 - 6);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            pictureBox1_Click_1(sender, e);
        }

        private void SelectionSort_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                int min = i;
                for (int j = i; j < a.Length; j++)
                {
                    if (a[j] < a[min])
                    {
                        min = j;
                    }
                }
                int h = a[min];
                a[min] = a[i];
                a[i] = h;
                ChangePosition(min, i);
                Thread.Sleep(50);
            }
        }

        private void StephSort_Click(object sender, EventArgs e)
        {
            bool solange = true;
            while (solange == true)
            {
                solange = false;
                for (int i = 0; i < a.Length - 1; i++)
                {
                    if (a[i] > a[i + 1])
                    {
                        solange = true;
                        int h = a[i];
                        a[i] = a[i + 1];
                        a[i + 1] = h;
                        Console.Beep(5000 + 50 * a[i], 20);
                        ChangePosition(i, i + 1);
                    }
                }
            }
        }
    }
}
