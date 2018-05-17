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
using System.Media;
using System.Net.Mime;

namespace GraphicSort_Forms
{
    public partial class FrmMain : Form
    {
        // Globale Variablen und Objekte
        private static readonly Random Rnd = new Random();
        static int[] _data = new int[100];
        static Graphics _grafik;
        private static readonly Pen Stift = new Pen(Color.Blue, 8);
        private static readonly Pen Deleter = new Pen(Color.LightGray, 8);
        private readonly Stopwatch stopwatch = new Stopwatch();

        private Task sortierer;

        private enum Design {
            Pixel = 0,
            Bars = 1 
        }

        static bool _abbrechen = false;
        static int _zeit = 0;
        static Design _design = Design.Bars;
        static bool _enablecolorcompare = false;

        static int _swaps = 0;
        static int _compares = 0;

        public FrmMain()
        {
            // Wird beim Start ausgeführt
            InitializeComponent();
            comboBox1.DataSource = new ComboItem[]
            {
                new ComboItem {ID = 1, Text = "Bubblesort"},
                new ComboItem {ID = 2, Text = "Insertionsort"},
                new ComboItem {ID = 3, Text = "MinSelectionsort"},
                new ComboItem {ID = 4, Text = "MaxSelectionsort"},
                new ComboItem {ID = 5, Text = "BogoSort"},
                new ComboItem {ID = 6, Text = "Quicksort"}
            };
            comboBox2.DataSource = new ComboItem[]
            {
                new ComboItem {ID = 1, Text = "Bar"},
                new ComboItem {ID = 2, Text = "Pixel"}
            };
            comboBox3.DataSource = new ComboItem[]
            {
                new ComboItem {ID = 1, Text = "Blue"},
                new ComboItem {ID = 2, Text = "Red"},
                new ComboItem {ID = 3, Text = "Green"}
            };
        }

        /*
         * Sortieralgorithen
         * 
         */

        private void BubbleSort()
        {
            for (var i = 0; i < _data.Length; i++)
            {
                if (_abbrechen == true)
                {
                    _abbrechen = false;
                    break;
                }
                for (var j = 0; j < _data.Length - 1 - i; j++)
                {
                    if (_abbrechen == true)
                    {
                        break;
                    }
                    Thread.Sleep(_zeit * 10);
                    AddCompare();
                    SetTime();
                    ColorCompare(j, j + 1);
                    if (_data[j] > _data[j + 1])
                    {
                        int h = _data[j + 1];
                        _data[j + 1] = _data[j];
                        _data[j] = h;
                        AddSwap();
                        ChangePosition(j, j + 1);
                        ColorSwap(j, j+1);
                    }
                }
            }
        }

        private void InsertionSort()
        {
            for (var i = 1; i < _data.Length; i++)
            {
                var k = i;
                if (_abbrechen == true)
                {
                    _abbrechen = false;
                    break;
                }
                for (k = i; k > 0; k--)
                {
                    if (_abbrechen == true)
                    {
                        break;
                    }
                    SetTime();
                    AddCompare();
                    ColorCompare(k, k - 1);
                    if (_data[k] < _data[k - 1])
                    {
                        Thread.Sleep(_zeit * 10);
                        int h = _data[k];
                        _data[k] = _data[k - 1];
                        _data[k - 1] = h;
                        AddSwap();
                        ChangePosition(k, k - 1);
                        ColorSwap(k, k-1);
                    }
                }
            }
        }
        private void MaxSelectionSort()
        {
            for (var i = _data.Length - 1; i >= 0; i--)
            {
                var max = i;
                if (_abbrechen == true)
                {
                    _abbrechen = false;
                    break;
                }
                for (var j = i - 1; j >= 0; j--)
                {
                    if (_abbrechen == true)
                    {
                        break;
                    }
                    AddCompare();
                    ColorCompare(j, max);
                    SetTime();
                    if (_data[j] > _data[max])
                    {
                        max = j;
                    }
                }
                Thread.Sleep(_zeit * 10);
                var h = _data[max];
                _data[max] = _data[i];
                _data[i] = h;
                AddSwap();
                ColorSwap(i, max);
                ChangePosition(i, max);
            }
        }
        private void MinSelectionSort()
        {
            for (var i = 0; i < _data.Length - 1; i++)
            {
                var min = i;
                if (_abbrechen == true)
                {
                    _abbrechen = false;
                    break;
                }
                for (var j = i + 1; j < _data.Length; j++)
                {
                    if (_abbrechen == true)
                    {
                        break;
                    }
                    AddCompare();
                    ColorCompare(j, min);
                    SetTime();
                    if (_data[j] < _data[min])
                    {
                        min = j;
                    }
                }
                Thread.Sleep(_zeit * 10);
                int h = _data[min];
                _data[min] = _data[i];
                _data[i] = h;
                AddSwap();
                ColorSwap(min, i);
                ChangePosition(min, i);
            }
        }
        private void BogoSort()
        {
            var sortiert = false;
            while (sortiert == false)
            {
                if (_abbrechen == true)
                {
                    _abbrechen = false;
                    break;
                }
                _data = Enumerable.Range(0, 100).OrderBy(c => Rnd.Next()).ToArray();
                Thread.Sleep(500);
                PostScreen(false);
                sortiert = true;
                for (var i = 0; i < _data.Length - 1; i++)
                {
                    Thread.Sleep(_zeit * 10);
                    SetTime();
                    ColorCompare(i, i + 1);
                    AddCompare();
                    if (_data[i] > _data[i + 1])
                    {
                        sortiert = false;
                        break;
                    }
                }
            }
        }

        private void QuickSort() {
            QuickSortRecursive(Enumerable.Range(0,99).Zip(_data, (i, d) => new Tuple<int, int>(i, d)).ToArray());
        }

        private void QuickSortRecursive(Tuple<int, int>[] list) {
            switch (list.Length)
            {
                case 1:
                    break;
                case 2:
                    SetTime();
                    ColorCompare(list[1].Item1, list[1].Item1);
                    if (list[0].Item2 > list[1].Item2)
                    {
                        ColorSwap(list[0].Item1, list[1].Item1);
                    }
                    AddCompare();
                    break;
                case 3:
                    var max = list.Select(t => t.Item2).Max();
                    var min = list.Select(t => t.Item2).Max();
                    var maxIndex = list.First(t => t.Item2 == max).Item1;
                    var minIndex = list.First(t => t.Item2 == min).Item1;

                    //Was wir hier wirklich versuchen, ist die Permutation \in S_3 (abc) (mit a, b, c den Rängen) in die Faktoren der Form (nm) zu zerlegen

                    switch (maxIndex)
                    {
                        case 0:
                            ColorSwap(minIndex == 2 ? list[0].Item1 : list[1].Item1, list[2].Item1);
                            break;
                        case 2 when minIndex == 1:
                            ColorSwap(list[0].Item1, list[1].Item1);
                            break;
                        case 2 when list[0].Item2 > list[2].Item2:
                            ColorSwap(list[0].Item1, list[2].Item1);
                            ColorSwap(list[1].Item1, list[0].Item1);
                            break;
                        case 2:
                            ColorSwap(list[1].Item1, list[2].Item1);
                            break;
                    }

                    break;
                default:
                    var pivot      = Ninther(list.Select(t => t.Item2).ToArray());
                    var pivotIndex = list.First(t => t.Item2 == pivot).Item1;
                    var a          = Enumerable.Range(0, pivotIndex).Select(i => list[i]).ToArray();
                    var b          = Enumerable.Range(pivotIndex + 1, list.Length - pivotIndex - 1).Select(i => list[i]).ToArray();

                    if (a.Length > 0) { QuickSortRecursive(a); }
                    if (b.Length > 0) { QuickSortRecursive(b); }
                    break;
            }
        }

        private static int Ninther(int[] a)
        {
            //Die Indices sind gegeben durch
            //Mathematica: Table[N[(k/3 + l/(3 (3 - 1))) n], {k, 0, 3 - 1}, {l, 0, 3 - 1}] // MatrixForm
            //
            //wobei n = a.Length - 1
            var n = a.Length - 1;

            return MedianOf3(
                MedianOf3(_data[0], _data[n/6], _data[n/3]),
                MedianOf3(_data[n/3], _data[n/2], _data[2*n/3]),
                MedianOf3(_data[2*n/3], _data[5*n/6], _data[n]));
        }

        private static int MedianOf3(int a, int b, int c) {
            return (a + b + c) - Math.Max(Math.Max(a, b), c) - Math.Min(Math.Min(a, b), c); //Problematisch wenn a+b+c den Maximalwert von int überstreigt
        }

        /*
         * Ausführbare Methoden für Grafik und Array
         * 
         * 
         */

        private void SetColor() {
            var auswahl = (int)comboBox3.SelectedValue;
            switch (auswahl) {
                case 1:
                    Stift.Color = Color.Blue;
                    break;
                case 2:
                    Stift.Color = Color.Red;
                    break;
                case 3:
                    Stift.Color = Color.Green;
                    break;
                default:
                    Stift.Color = Color.Blue;
                    break;
            }
        }

        private void ChangePosition(int x1, int x2) {
            _grafik.DrawLine(Deleter, 4 + x1 * 8, 600 - (1 - (int)_design) * _data[x2] * 6, 4 + x1 * 8, (1 - (int)_design) * (600 - _data[x2] * 6 + 6 ));
            _grafik.DrawLine(Stift,   4 + x1 * 8, 600 - (1 - (int)_design) * _data[x1] * 6, 4 + x1 * 8, 600 - _data[x1] * 6 + (1 - (int)_design) * 6);
            _grafik.DrawLine(Deleter, 4 + x2 * 8, 600 - (1 - (int)_design) * _data[x1] * 6, 4 + x2 * 8, (1 - (int)_design) * (600 - _data[x1] * 6 + 6));
            _grafik.DrawLine(Stift,   4 + x2 * 8, 600 - (1 - (int)_design) * _data[x2] * 6, 4 + x2 * 8, 600 - _data[x2] * 6 + (1 - (int)_design) * 6);
        }

        private void ColorCompare(int x1, int x2)
        {
            if (_design == Design.Bars && _enablecolorcompare == true)
            {
                var black = new Pen(Color.Black, 8);
                _grafik.DrawLine(black, 4 + x1 * 8, 600, 4 + x1 * 8, 600 - _data[x1] * 6);
                _grafik.DrawLine(black, 4 + x2 * 8, 600, 4 + x2 * 8, 600 - _data[x2] * 6);
                Thread.Sleep(50);
                _grafik.DrawLine(Stift, 4 + x1 * 8, 600, 4 + x1 * 8, 600 - _data[x1] * 6);
                _grafik.DrawLine(Stift, 4 + x2 * 8, 600, 4 + x2 * 8, 600 - _data[x2] * 6);
            }
        }

        private void ColorSwap(int x1, int x2)
        {
            if (_design == Design.Bars && _enablecolorcompare == true)
            {
                var brown = new Pen(Color.Brown, 8);
                _grafik.DrawLine(brown, 4 + x1 * 8, 600, 4 + x1 * 8, 600 - _data[x1] * 6);
                _grafik.DrawLine(brown, 4 + x2 * 8, 600, 4 + x2 * 8, 600 - _data[x2] * 6);
                Thread.Sleep(_zeit * 10);
                _grafik.DrawLine(Stift, 4 + x1 * 8, 600, 4 + x1 * 8, 600 - _data[x1] * 6);
                _grafik.DrawLine(Stift, 4 + x2 * 8, 600, 4 + x2 * 8, 600 - _data[x2] * 6);
            }
        }

        private void SetTime()
        {
            var elapsedTime =
                $"{stopwatch.Elapsed.Minutes:00}:{stopwatch.Elapsed.Seconds:00}:{stopwatch.Elapsed.Milliseconds / 10:00}";
            label5.Invoke(new Action(() => label5.Text = elapsedTime));
        }

        private void PostScreen(bool start)
        {
            if(start == true)
            {
                Thread.Sleep(100);
            }
            if (_design == Design.Bars)
            {
                for (var i = 0; i < 100; i++)
                {
                    _grafik.DrawLine(Deleter, 4 + i * 8, 600, 4 + i * 8, 0);
                    _grafik.DrawLine(Stift, 4 + i * 8, 600, 4 + i * 8, 600 - _data[i] * 6);
                }
            }
            else
            {
                for (int i = 0; i < 100; i++)
                {
                    _grafik.DrawLine(Deleter, 4 + i * 8, 600, 4 + i * 8, 0);
                    _grafik.DrawLine(Stift, 4 + i * 8, 600 - _data[i] * 6, 4 + i * 8, 600 - _data[i] * 6+6);
                }
            }
        }

        private void ResetScreen()
        {
            _swaps = 0;
            _compares = 0;
            label1.Text = "S: 0";
            label6.Text = "C: 0";
            stopwatch.Reset();
            label5.Text = "00:00:00";
            button1.Enabled = true;
            _design = (Design)(int)comboBox2.SelectedValue;
            CreateArray();
            SetColor();
            PostScreen(false);
        }

        private void AddSwap()
        {
            _swaps = _swaps + 1;
            label1.Invoke(new Action(() => label1.Text = "S: " + _swaps));
        }

        private void AddCompare()
        {
            _compares = _compares + 1;
            label6.Invoke(new Action(() => label6.Text = "C: " + _compares));
        }

        private void CreateArray()
        {
            if (checkBox1.Checked == false)
            {
                var rnd = new Random();
                for (int i = 0; i < _data.Length; i++)
                {
                    _data[i] = rnd.Next(1, 101);
                }
            }
            else
            {
                _data = Enumerable.Range(1, 100).OrderBy(c => Rnd.Next()).ToArray();
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
            var notsorted = false;
            for (var i = 0; i < _data.Length-1; i++)
            {
                if(_data[i] > _data[i+1])
                {
                    notsorted = true;
                    break;
                }
            }
            // prüft, ob bereits eine Instanz läuft
            if (sortierer == null && notsorted == true)
            {
                // prüfen der parameter für Farbe, Dauer und Aussehen
                var auswahl = (int)comboBox1.SelectedValue;
                _design = (Design)(2- (int)comboBox2.SelectedValue);
                _zeit = trackBar1.Value;
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
                switch (auswahl) {
                    // Wählt den Algorithmus aus
                    case 1:
                        sortierer = Task.Run(() => BubbleSort());
                        break;
                    case 2:
                        sortierer = Task.Run(() => InsertionSort());
                        break;
                    case 3:
                        sortierer = Task.Run(() => MinSelectionSort());
                        break;
                    case 4:
                        sortierer = Task.Run(() => MaxSelectionSort());
                        break;
                    case 5:
                        sortierer = Task.Run(() => BogoSort());
                        break;
                    case 6:
                        sortierer = Task.Run(() => QuickSort());
                        break;
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
                _abbrechen = true;
                sortierer = null;
            }
        }

        private async void Form1_Load_1(object sender, EventArgs e)
        {
            // da er beim Start manchmal länger braucht die Picturebox zu laden, geschiet dies asynchron zu einem späteren Zeitpunkt
            _data = Enumerable.Range(1, 100).OrderBy(c => Rnd.Next()).ToArray();
            _grafik = this.pictureBox1.CreateGraphics();
            await Task.Run(() =>
            PostScreen(true)
            );
        }

        // Slidepanel für die Geschwindigkeit
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            _zeit = trackBar1.Value;
            // Prüft ob der Wert über 4 ist -> er folgt Färbung von Vergleichen und Tauschvorgängen
            if(trackBar1.Value > 4)
            {
                _enablecolorcompare = true;
            }
            else
            {
                _enablecolorcompare = false;
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
