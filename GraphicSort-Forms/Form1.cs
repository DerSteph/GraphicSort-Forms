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
        static Pen stift = new Pen(Color.Blue, 8);
        static Pen deleter = new Pen(Color.LightGray, 8);
        static Graphics grafik;
        Stopwatch stopwatch = new Stopwatch();
        static int design = 1;
        static int[] a = new int[100];
        private Task sortierer;
        static bool abbrechen = false;
        static int zeit = 0;

        static int swaps = 0;
        static int compares = 0;

        public Form1()
        {
            InitializeComponent();
            comboBox1.DataSource = new ComboItem[]
            {
                new ComboItem {ID = 1, Text = "BubbleSort"},
                new ComboItem {ID = 2, Text = "StephSort"},
                new ComboItem {ID = 3, Text = "InsertionSort"},
                new ComboItem {ID = 4, Text = "MinSelectionSort"},
                new ComboItem {ID = 5, Text = "MaxSelectionSort"},
                new ComboItem {ID = 6, Text = "BogoSort"}
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
                if (abbrechen == true)
                {
                    abbrechen = false;
                    break;
                }
                for (int j = 0; j < a.Length - 1 - i; j++)
                {
                    //ColorCompare(j, j+1);
                    AddCompare();
                    Thread.Sleep(zeit * 10);
                    if (a[j] > a[j + 1])
                    {
                        int h = a[j + 1];
                        a[j + 1] = a[j];
                        a[j] = h;
                        AddSwap();
                        ChangePosition(j, j + 1);
                        SetTime();
                    }
                }
            }
        }

        private void InsertionSort()
        {
            for (int i = 1; i < a.Length; i++)
            {
                if (abbrechen == true)
                {
                    abbrechen = false;
                    break;
                }
                int k = i;
                for (k = i; k > 0; k--)
                {
                    ColorCompare(k, k - 1);
                    AddCompare();
                    if (a[k] < a[k - 1])
                    {
                        AddSwap();
                        int h = a[k];
                        a[k] = a[k - 1];
                        a[k - 1] = h;
                        ChangePosition(k, k - 1);
                        SetTime();
                    }
                }
            }
        }
        private void MaxSelectionSort()
        {
            for (int i = a.Length - 1; i >= 0; i--)
            {
                if (abbrechen == true)
                {
                    abbrechen = false;
                    break;
                }
                int max = i;
                for (int j = i - 1; j >= 0; j--)
                {
                    ColorCompare(j, max);
                    AddCompare();
                    if (a[j] > a[max])
                    {
                        max = j;
                    }
                }
                AddSwap();
                int h = a[max];
                a[max] = a[i];
                a[i] = h;
                ChangePosition(i, max);
                SetTime();
                //Thread.Sleep(50);
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
                if (abbrechen == true)
                {
                    abbrechen = false;
                    break;
                }
                int min = i;
                for (int j = i + 1; j < a.Length; j++)
                {
                    ColorCompare(j, min);
                    AddCompare();
                    if (a[j] < a[min])
                    {
                        min = j;
                    }
                }
                AddSwap();
                int h = a[min];
                a[min] = a[i];
                a[i] = h;
                ChangePosition(min, i);
                SetTime();
                //Thread.Sleep(50);
            }
        }
        private void StephSort()
        {
            bool solange = true;
            while (solange == true)
            {
                if (abbrechen == true)
                {
                    abbrechen = false;
                    break;
                }
                solange = false;
                for (int i = 0; i < a.Length - 1; i++)
                {
                    ColorCompare(i, i + 1);
                    AddCompare();
                    if (a[i] > a[i + 1])
                    {
                        solange = true;
                        AddSwap();
                        int h = a[i];
                        a[i] = a[i + 1];
                        a[i + 1] = h;
                        ChangePosition(i, i + 1);
                        SetTime();
                    }
                }
            }
        }
        private void BogoSort()
        {
            bool sortiert = false;
            while (sortiert == false)
            {
                if (abbrechen == true)
                {
                    abbrechen = false;
                    break;
                }
                a = Enumerable.Range(0, 100).OrderBy(c => rnd.Next()).ToArray();
                Thread.Sleep(500);
                PostScreen(false);
                SetTime();
                sortiert = true;
                for (int i = 0; i < a.Length - 1; i++)
                {
                    ColorCompare(i, i + 1);
                    AddCompare();
                    if (a[i] > a[i + 1])
                    {
                        sortiert = false;
                    }
                }
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

        private void ChangePosition(int x1, int x2)
        {
            if (design == 1)
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

        private void ColorCompare(int x1, int x2)
        {
            if (design == 1)
            {
                Pen black = new Pen(Color.Black, 8);
                grafik.DrawLine(black, 5 + x1 * 8, 600, 5 + x1 * 8, 600 - a[x1] * 6 - 6);
                grafik.DrawLine(black, 5 + x2 * 8, 600, 5 + x2 * 8, 600 - a[x2] * 6 - 6);
                grafik.DrawLine(stift, 5 + x1 * 8, 600, 5 + x1 * 8, 600 - a[x1] * 6 - 6);
                grafik.DrawLine(stift, 5 + x2 * 8, 600, 5 + x2 * 8, 600 - a[x2] * 6 - 6);
            }
            /*else
            {
                grafik.DrawLine(deleter, 5 + x1 * 8, 600 - a[x2] * 6 - 8, 5 + x1 * 8, 600 - a[x2] * 6);
                grafik.DrawLine(stift, 5 + x1 * 8, 600 - a[x1] * 6 - 8, 5 + x1 * 8, 600 - a[x1] * 6);
                grafik.DrawLine(deleter, 5 + x2 * 8, 600 - a[x1] * 6 - 8, 5 + x2 * 8, 600 - a[x1] * 6);
                grafik.DrawLine(stift, 5 + x2 * 8, 600 - a[x2] * 6 - 8, 5 + x2 * 8, 600 - a[x2] * 6);
            }*/
        }

        private void SetTime()
        {
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
            ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            label5.Invoke(new Action(() => label5.Text = elapsedTime));
        }

        private void PostScreen(bool start)
        {
            if(start == true)
            {
                Thread.Sleep(100);
            }
            if (design == 1)
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

        private void ResetScreen()
        {
            swaps = 0;
            compares = 0;
            label1.Text = "S: 0";
            label6.Text = "C: 0";
            stopwatch.Reset();
            button1.Enabled = true;
            design = (int)comboBox2.SelectedValue;
            a = Enumerable.Range(0, 100).OrderBy(c => rnd.Next()).ToArray();
            SetColor();
            PostScreen(false);
        }

        private void AddSwap()
        {
            swaps = swaps + 1;
            label1.Invoke(new Action(() => label1.Text = "S: " + swaps));
        }

        private void AddCompare()
        {
            compares = compares + 1;
            label6.Invoke(new Action(() => label6.Text = "C: " + compares));
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            if (sortierer == null)
            {
                int auswahl = (int)comboBox1.SelectedValue;
                design = (int)comboBox2.SelectedValue;
                trackBar1.Enabled = false;
                zeit = trackBar1.Value;
                Debug.WriteLine(zeit);
                SetColor();
                PostScreen(false);
                button1.Enabled = false;
                button2.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                stopwatch.Start();
                // design auswählen
                design = (int)comboBox2.SelectedValue;
                // sortieren starten
                if (auswahl == 1)
                {
                    sortierer = Task.Run(() => BubbleSort());
                }
                else if (auswahl == 2)
                {
                    sortierer = Task.Run(() => StephSort());
                }
                else if (auswahl == 3)
                {
                    sortierer = Task.Run(() => InsertionSort());
                }
                else if (auswahl == 4)
                {
                    sortierer = Task.Run(() => MinSelectionSort());
                }
                else if (auswahl == 5)
                {
                    sortierer = Task.Run(() => MaxSelectionSort());
                }
                else if (auswahl == 6)
                {
                    sortierer = Task.Run(() => BogoSort());
                }
                else
                {
                    sortierer = Task.Run(() => BubbleSort());
                }
                await sortierer;
                sortierer = null;
                stopwatch.Stop();
                button1.Enabled = true;
                button2.Enabled = true;
                trackBar1.Enabled = true;
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResetScreen();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (sortierer != null)
            {
                abbrechen = true;
                sortierer = null;
            }
        }

        private async void Form1_Load_1(object sender, EventArgs e)
        {
            // beim Start muss er ein Random Bild laden
            a = Enumerable.Range(0, 100).OrderBy(c => rnd.Next()).ToArray();
            grafik = this.pictureBox1.CreateGraphics();
            await Task.Run(() =>
            PostScreen(true)
            );
        }

        /*private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetScreen();
        }*/
    }
    class ComboItem
    {
        public int ID { get; set; }
        public string Text { get; set; }
    }
}
