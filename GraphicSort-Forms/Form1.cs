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
        static int[] a = new int[100];
        static Graphics grafik;
        static Pen stift = new Pen(Color.Blue, 8);
        static Pen deleter = new Pen(Color.LightGray, 8);
        Stopwatch stopwatch = new Stopwatch();

        private Task sortierer;

        static bool abbrechen = false;
        static int zeit = 0;
        static int design = 1;
        static bool enablecolorcompare = false;

        static int swaps = 0;
        static int compares = 0;

        public Form1()
        {
            // Wird beim Start ausgeführt
            InitializeComponent();
            comboBox1.DataSource = new ComboItem[]
            {
                new ComboItem {ID = 1, Text = "BubbleSort"},
                new ComboItem {ID = 2, Text = "InsertionSort"},
                new ComboItem {ID = 3, Text = "MinSelectionSort"},
                new ComboItem {ID = 4, Text = "MaxSelectionSort"},
                new ComboItem {ID = 5, Text = "BogoSort"}
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

        /*
         * Sortieralgorithen
         * 
         */

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
                    if (abbrechen == true)
                    {
                        break;
                    }
                    Thread.Sleep(zeit * 10);
                    AddCompare();
                    SetTime();
                    ColorCompare(j, j + 1);
                    if (a[j] > a[j + 1])
                    {
                        int h = a[j + 1];
                        a[j + 1] = a[j];
                        a[j] = h;
                        AddSwap();
                        ChangePosition(j, j + 1);
                        ColorSwap(j, j+1);
                    }
                }
            }
        }

        private void InsertionSort()
        {
            for (int i = 1; i < a.Length; i++)
            {
                int k = i;
                if (abbrechen == true)
                {
                    abbrechen = false;
                    break;
                }
                for (k = i; k > 0; k--)
                {
                    if (abbrechen == true)
                    {
                        break;
                    }
                    SetTime();
                    AddCompare();
                    ColorCompare(k, k - 1);
                    if (a[k] < a[k - 1])
                    {
                        Thread.Sleep(zeit * 10);
                        AddSwap();
                        int h = a[k];
                        a[k] = a[k - 1];
                        a[k - 1] = h;
                        ChangePosition(k, k - 1);
                        ColorSwap(k, k-1);
                    }
                }
            }
        }
        private void MaxSelectionSort()
        {
            for (int i = a.Length - 1; i >= 0; i--)
            {
                int max = i;
                if (abbrechen == true)
                {
                    abbrechen = false;
                    break;
                }
                for (int j = i - 1; j >= 0; j--)
                {
                    if (abbrechen == true)
                    {
                        break;
                    }
                    AddCompare();
                    ColorCompare(j, max);
                    SetTime();
                    if (a[j] > a[max])
                    {
                        max = j;
                    }
                }
                Thread.Sleep(zeit * 10);
                AddSwap();
                int h = a[max];
                a[max] = a[i];
                a[i] = h;
                ColorSwap(i, max);
                ChangePosition(i, max);
            }
        }
        private void MinSelectionSort()
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                int min = i;
                if (abbrechen == true)
                {
                    abbrechen = false;
                    break;
                }
                for (int j = i + 1; j < a.Length; j++)
                {
                    if (abbrechen == true)
                    {
                        break;
                    }
                    AddCompare();
                    ColorCompare(j, min);
                    SetTime();
                    if (a[j] < a[min])
                    {
                        min = j;
                    }
                }
                Thread.Sleep(zeit * 10);
                AddSwap();
                int h = a[min];
                a[min] = a[i];
                a[i] = h;
                ColorSwap(min, i);
                ChangePosition(min, i);
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
                sortiert = true;
                for (int i = 0; i < a.Length - 1; i++)
                {
                    SetTime();
                    ColorCompare(i, i + 1);
                    AddCompare();
                    if (a[i] > a[i + 1])
                    {
                        sortiert = false;
                        break;
                    }
                }
            }
        }

        /*
         * Ausführbare Methoden für Grafik und Array
         * 
         * 
         */

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
                stift.Color = Color.Blue;
            }
        }

        private void ChangePosition(int x1, int x2)
        {
            if (design == 1)
            {
                grafik.DrawLine(deleter, 4 + x1 * 8, 600, 4 + x1 * 8, 0);
                grafik.DrawLine(stift, 4 + x1 * 8, 600, 4 + x1 * 8, 600 - a[x1] * 6);
                grafik.DrawLine(deleter, 4 + x2 * 8, 600, 4 + x2 * 8, 0);
                grafik.DrawLine(stift, 4 + x2 * 8, 600, 4 + x2 * 8, 600 - a[x2] * 6);
            }
            else
            {
                grafik.DrawLine(deleter, 4 + x1 * 8, 600 - a[x2] * 6, 4 + x1 * 8, 600 - a[x2] * 6+6);
                grafik.DrawLine(stift, 4 + x1 * 8, 600 - a[x1] * 6, 4 + x1 * 8, 600 - a[x1] * 6+6);
                grafik.DrawLine(deleter, 4 + x2 * 8, 600 - a[x1] * 6, 4 + x2 * 8, 600 - a[x1] * 6+6);
                grafik.DrawLine(stift, 4 + x2 * 8, 600 - a[x2] * 6, 4 + x2 * 8, 600 - a[x2] * 6+6);
            }
        }

        private void ColorCompare(int x1, int x2)
        {
            if (design == 1 && enablecolorcompare == true)
            {
                Pen black = new Pen(Color.Black, 8);
                grafik.DrawLine(black, 4 + x1 * 8, 600, 4 + x1 * 8, 600 - a[x1] * 6);
                grafik.DrawLine(black, 4 + x2 * 8, 600, 4 + x2 * 8, 600 - a[x2] * 6);
                Thread.Sleep(50);
                grafik.DrawLine(stift, 4 + x1 * 8, 600, 4 + x1 * 8, 600 - a[x1] * 6);
                grafik.DrawLine(stift, 4 + x2 * 8, 600, 4 + x2 * 8, 600 - a[x2] * 6);
            }
        }

        private void ColorSwap(int x1, int x2)
        {
            if (design == 1 && enablecolorcompare == true)
            {
                Pen brown = new Pen(Color.Brown, 8);
                grafik.DrawLine(brown, 4 + x1 * 8, 600, 4 + x1 * 8, 600 - a[x1] * 6);
                grafik.DrawLine(brown, 4 + x2 * 8, 600, 4 + x2 * 8, 600 - a[x2] * 6);
                Thread.Sleep(zeit * 10);
                grafik.DrawLine(stift, 4 + x1 * 8, 600, 4 + x1 * 8, 600 - a[x1] * 6);
                grafik.DrawLine(stift, 4 + x2 * 8, 600, 4 + x2 * 8, 600 - a[x2] * 6);
            }
        }

        private void SetTime()
        {
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds, stopwatch.Elapsed.Milliseconds / 10);
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
                    grafik.DrawLine(deleter, 4 + i * 8, 600, 4 + i * 8, 0);
                    grafik.DrawLine(stift, 4 + i * 8, 600, 4 + i * 8, 600 - a[i] * 6);
                }
            }
            else
            {
                for (int i = 0; i < 100; i++)
                {
                    grafik.DrawLine(deleter, 4 + i * 8, 600, 4 + i * 8, 0);
                    grafik.DrawLine(stift, 4 + i * 8, 600 - a[i] * 6, 4 + i * 8, 600 - a[i] * 6+6);
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
            label5.Text = "00:00:00";
            button1.Enabled = true;
            design = (int)comboBox2.SelectedValue;
            CreateArray();
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

        private void CreateArray()
        {
            if (checkBox1.Checked == false)
            {
                Random rnd = new Random();
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = rnd.Next(1, 101);
                }
            }
            else
            {
                a = Enumerable.Range(1, 100).OrderBy(c => rnd.Next()).ToArray();
            }
            PostScreen(false);
        }

        /*
         * Buttons, Labels, Schieberegler, Dropdownmenü
         * 
         */

        //Startbutton
        private async void button1_Click(object sender, EventArgs e)
        {
            bool notsorted = false;
            for (int i = 0; i < a.Length-1; i++)
            {
                if(a[i] > a[i+1])
                {
                    notsorted = true;
                    break;
                }
            }
            // prüft, ob bereits eine Instanz läuft
            if (sortierer == null && notsorted == true)
            {
                // prüfen der parameter für Farbe, Dauer und Aussehen
                int auswahl = (int)comboBox1.SelectedValue;
                design = (int)comboBox2.SelectedValue;
                zeit = trackBar1.Value;
                SetColor();
                // generiert nochmal ein Bild, falls nach einem Abbruch mit anderen Parametern gearbeitet wird
                PostScreen(false);
                // deaktiviert die Steuerknöpfe und Regler
                //trackBar1.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                button3.Enabled = true;
                checkBox1.Enabled = false;
                stopwatch.Start();
                // Wählt den Algorithmus aus
                if (auswahl == 1)
                {
                    sortierer = Task.Run(() => BubbleSort());
                }
                else if (auswahl == 2)
                {
                    sortierer = Task.Run(() => InsertionSort());
                }
                else if (auswahl == 3)
                {
                    sortierer = Task.Run(() => MinSelectionSort());
                }
                else if (auswahl == 4)
                {
                    sortierer = Task.Run(() => MaxSelectionSort());
                }
                else if (auswahl == 5)
                {
                    sortierer = Task.Run(() => BogoSort());
                }
                else
                {
                    sortierer = Task.Run(() => BubbleSort());
                }
                // Algorithmus wird asynchron ausgeführt, damit sich das Programm nicht aufhängt, während es läuft
                await sortierer;
                // Löscht die Instanz wieder, stoppt die Uhr und aktiviert die Knöpfe
                sortierer = null;
                stopwatch.Stop();
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = false;
                //trackBar1.Enabled = true;
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                checkBox1.Enabled = true;
            }
        }

        // neues zufälliges Array erstellen
        private void button2_Click(object sender, EventArgs e)
        {
            ResetScreen();
        }

        // Abbruchknopf -> in jedem Algorithmus ist eine Abbruchbedingung erhalten
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
            // da er beim Start manchmal länger braucht die Picturebox zu laden, geschiet dies asynchron zu einem späteren Zeitpunkt
            a = Enumerable.Range(1, 100).OrderBy(c => rnd.Next()).ToArray();
            grafik = this.pictureBox1.CreateGraphics();
            await Task.Run(() =>
            PostScreen(true)
            );
        }

        // Slidepanel für die Geschwindigkeit
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            zeit = trackBar1.Value;
            // Prüft ob der Wert über 4 ist -> er folgt Färbung von Vergleichen und Tauschvorgängen
            if(trackBar1.Value > 4)
            {
                enablecolorcompare = true;
            }
            else
            {
                enablecolorcompare = false;
            }
        }

        // Soll gucken, ob Checkbox makiert ist -> wenn ja, soll er ein Array mit Zahlen erstellen, bei der jede nur einmalig vorhanden ist
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CreateArray();
        }
    }
    // Definierung des Arrays für die Dropdownmenüs
    class ComboItem
    {
        public int ID { get; set; }
        public string Text { get; set; }
    }
}
