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
            Thread.Sleep(200);
            for (int i = 0; i < 100; i++)
            {
                grafik.DrawLine(deleter, 5 + i * 8, 600, 5 + i * 8, 0);
                grafik.DrawLine(stift, 5 + i * 8, 600, 5 + i * 8, 600 - a[i] * 6 - 6);
            }
            comboBox1.DataSource = new ComboItem[]
            {
                new ComboItem {ID = 1, Text = "BubbleSort"},
                new ComboItem {ID = 2, Text = "StephSort"},
                new ComboItem {ID = 3, Text = "InsertionSort"},
                new ComboItem {ID = 4, Text = "MinSelectionSort"},
                new ComboItem {ID = 5, Text = "MaxSelectionSort"}
            };
            comboBox2.DataSource = new ComboItem[]
            {
                new ComboItem {ID = 1, Text = "bar"},
                new ComboItem {ID = 2, Text = "pixel"}
            };
            comboBox3.DataSource = new ComboItem[]
            {
                new ComboItem {ID = 1, Text = "blue"},
                new ComboItem {ID = 2, Text = "red"},
                new ComboItem {ID = 3, Text = "green"}
            };
        }
        private void BubbleSort()
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
                        ChangePosition(j, j + 1);
                    }
                }
            }
            for (int i = 0; i < 100; i++)
            {
                Debug.Write(a[i]);
            }
        }

        private void InsertionSort()
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
                        ChangePosition(k, k - 1);
                    }
                }
            }
        }
        private void MaxSelectionSort()
        {
            for (int i = a.Length - 1; i >= 0; i--)
            {
                int max = i;
                for (int j = i - 1; j >= 0; j--)
                {
                    if (a[j] > a[max])
                    {
                        max = j;
                    }
                }
                int h = a[max];
                a[max] = a[i];
                a[i] = h;
                ChangePosition(i, max);
                Thread.Sleep(50);
            }
            for (int i = 0; i < a.Length; i++)
            {
                Debug.Write(a[i] + ", ");
            }
        }
        private void MinSelectionSort()
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < a.Length; j++)
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

        private void SetColor()
        {
            int auswahl = (int)comboBox3.SelectedValue;
            if (auswahl == 1)
            {
                stift.Color = Color.Blue;
            }
            else if (auswahl == 2)
            {
                stift.Color = Color.Red;
            }
            else if (auswahl == 3)
            {
                stift.Color = Color.Green;
            }
            else
            {

            }
        }

        private void StephSort()
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

        private void ChangePosition(int x1, int x2)
        {
            int aussehen = (int)comboBox2.SelectedValue;

            if (aussehen == 1)
            {
                grafik.DrawLine(deleter, 5 + x1 * 8, 600, 5 + x1 * 8, 600 - a[x2] * 6 - 6);
                grafik.DrawLine(stift, 5 + x1 * 8, 600, 5 + x1 * 8, 600 - a[x1] * 6 - 6);
                grafik.DrawLine(deleter, 5 + x2 * 8, 600, 5 + x2 * 8, 600 - a[x1] * 6 - 6);
                grafik.DrawLine(stift, 5 + x2 * 8, 600, 5 + x2 * 8, 600 - a[x2] * 6 - 6);
            }
            else
            {
                grafik.DrawLine(deleter, 5 + x1 * 8, 600 - a[x2] * 6 - 8, 5 + x1 * 8, 600 - a[x2] * 6);
                grafik.DrawLine(stift, 5 + x1 * 8, 600 - a[x1] * 6 - 8, 5 + x1 * 8, 600 - a[x1] * 6);
                grafik.DrawLine(deleter, 5 + x2 * 8, 600 - a[x1] * 6 - 8, 5 + x2 * 8, 600 - a[x1] * 6);
                grafik.DrawLine(stift, 5 + x2 * 8, 600 - a[x2] * 6 - 8, 5 + x2 * 8, 600 - a[x2] * 6);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int auswahl = (int)comboBox1.SelectedValue;
            int aussehen = (int)comboBox2.SelectedValue;
            SetColor();
            if (auswahl == 1)
            {
                BubbleSort();
            }
            else if (auswahl == 2)
            {
                StephSort();
            }
            else if (auswahl == 3)
            {
                InsertionSort();
            }
            else if (auswahl == 4)
            {
                MinSelectionSort();
            }
            else if (auswahl == 5)
            {
                MaxSelectionSort();
            }
            else
            {

            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            button1.Enabled = true;
            a = Enumerable.Range(0, 100).OrderBy(c => rnd.Next()).ToArray();
            int aussehen = (int)comboBox2.SelectedValue;
            SetColor();
            if (aussehen == 1)
            {
                for (int i = 0; i < 100; i++)
                {
                    grafik.DrawLine(deleter, 5 + i * 8, 600, 5 + i * 8, 0);
                    grafik.DrawLine(stift, 5 + i * 8, 600, 5 + i * 8, 600 - a[i] * 6 - 6);
                }
            }
            else
            {
                for (int i = 0; i < 100; i++)
                {
                    grafik.DrawLine(deleter, 5 + i * 8, 600, 5 + i * 8, 0);
                    grafik.DrawLine(stift, 5 + i * 8, 600 - a[i] * 6 - 8, 5 + i * 8, 600 - a[i] * 6);
                }
            }
        }
    }
    class ComboItem
    {
        public int ID { get; set; }
        public string Text { get; set; }
    }
}
