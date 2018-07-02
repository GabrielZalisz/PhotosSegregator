using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Gat.Controls.Model;
using System.Globalization;
using ExifReader;

namespace Segregate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        List<string> list_souboru = new List<string>();
        List<string> bin = new List<string>();
        bool povoleno = false;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Directory.Exists(tbFrom.Text))
                {
                    MessageBox.Show("Původní složka neexistuje.");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Chyba");
                return;
            }

            try
            {
                if (!Directory.Exists(tbTo.Text))
                    Directory.CreateDirectory(tbTo.Text);
            }
            catch
            {
                //MessageBox.Show(@"Nová složka neexistuje a nepodařilo se ji vytvořit. Fotky budou uloženy do podsložky ""výběr"".");
                string aaa = tbFrom.Text + @"\výběr";
                Directory.CreateDirectory(aaa);
                tbTo.Text = aaa;
            }

            list_souboru.Clear();
            bin.Clear();
            list_vyber = null;
            lbl00.Background = new SolidColorBrush(Colors.WhiteSmoke);
            lbl0.Background = new SolidColorBrush(Colors.WhiteSmoke);
            lbl1.Background = new SolidColorBrush(Colors.WhiteSmoke);
            lbl2.Background = new SolidColorBrush(Colors.WhiteSmoke);
            lbl3.Background = new SolidColorBrush(Colors.WhiteSmoke);
            image00.Source = null;
            image0.Source = null;
            image1.Source = null;
            image2.Source = null;
            image3.Source = null;

            button1.IsEnabled = false;
            btnStop.IsEnabled = true;

            if (Directory.Exists(tbTo.Text) && Directory.Exists(tbFrom.Text))
            {
                povoleno = true;
            }

            try
            {
                list_souboru = Directory.GetFiles(tbFrom.Text).ToList();
                var q = from soubor in list_souboru
                        where soubor.ToLower().EndsWith("jpg") || soubor.ToLower().EndsWith("jepg")
                        select soubor;
                list_souboru = q.ToList();
            }
            catch
            {
                MessageBox.Show("Chyba");
                povoleno = false;
                return;
            }

            if (povoleno)
            {
                tbFrom.IsEnabled = false;
                tbTo.IsEnabled = false;
            }
            else
                return;

            PrehrajObrazek();
        }

        void Rotate(Image i, string soubor)
        {
            ExifReader.ExifReader r = new ExifReader.ExifReader(list_souboru[0]);
            string orientace = "1";
            foreach (ExifProperty item in r.GetExifProperties())
            {
                if (item.ExifPropertyName == "Orientation")
                {
                    orientace = item.ToString();
                    goto aaa;
                }
            }
            aaa:
            if (orientace == "8")
            {
                image1.RenderTransform = new RotateTransform(270);
            }
            else if (orientace == "6")
            {
                image1.RenderTransform = new RotateTransform(90);
            }
            else
            {
                image1.RenderTransform = new RotateTransform(0);
            }
        }

        string[] list_vyber = new string[1];

        void PrehrajObrazek()
        {
            list_vyber = Directory.GetFiles(tbTo.Text);
            try
            {
                BitmapImage a = new BitmapImage();
                a.BeginInit();
                a.UriSource = new Uri(bin[bin.Count - 2]);
                a.EndInit();
                image00.Source = a;
                if (list_vyber.Contains(System.IO.Path.Combine(tbTo.Text, System.IO.Path.GetFileName(bin[bin.Count - 2]))))
                    lbl00.Background = new SolidColorBrush(Colors.Green);
                else
                    lbl00.Background = new SolidColorBrush(Colors.WhiteSmoke);

                Rotate(image00, bin[bin.Count - 2]);
            }
            catch
            {
                image00.Source = null;
                image00.RenderTransform = new RotateTransform(0);
                lbl00.Background = new SolidColorBrush(Colors.WhiteSmoke);
            } 
            try
            {
                BitmapImage aa = new BitmapImage();
                aa.BeginInit();
                aa.UriSource = new Uri(bin[bin.Count - 1]);
                aa.EndInit();
                image0.Source = aa;
                if (list_vyber.Contains(System.IO.Path.Combine(tbTo.Text, System.IO.Path.GetFileName(bin[bin.Count - 1]))))
                    lbl0.Background = new SolidColorBrush(Colors.Green);
                else
                    lbl0.Background = new SolidColorBrush(Colors.WhiteSmoke);

                Rotate(image0, bin[bin.Count - 1]);
            }
            catch
            {
                image0.Source = null;
                image0.RenderTransform = new RotateTransform(0);
                lbl0.Background = new SolidColorBrush(Colors.WhiteSmoke);
            }
            try
            {
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(list_souboru[0]);
                b.EndInit();
                image1.Source = b;
                if (list_vyber.Contains(System.IO.Path.Combine(tbTo.Text, System.IO.Path.GetFileName(list_souboru[0]))))
                    lbl1.Background = new SolidColorBrush(Colors.Green);
                else
                    lbl1.Background = new SolidColorBrush(Colors.WhiteSmoke);

                Rotate(image1, list_souboru[0]);
            }
            catch (Exception eee)
            {
                image1.Source = null;
                image1.RenderTransform = new RotateTransform(0);
                lbl1.Background = new SolidColorBrush(Colors.WhiteSmoke);
            }
            try
            {
                BitmapImage bb = new BitmapImage();
                bb.BeginInit();
                bb.UriSource = new Uri(list_souboru[1]);
                bb.EndInit();
                image2.Source = bb;
                if (list_vyber.Contains(System.IO.Path.Combine(tbTo.Text, System.IO.Path.GetFileName(list_souboru[1]))))
                    lbl2.Background = new SolidColorBrush(Colors.Green);
                else
                    lbl2.Background = new SolidColorBrush(Colors.WhiteSmoke);

                Rotate(image2, list_souboru[1]);
            }
            catch
            {
                image2.Source = null;
                image2.RenderTransform = new RotateTransform(0);
                lbl2.Background = new SolidColorBrush(Colors.WhiteSmoke);
            }
            try
            {
                BitmapImage bbb = new BitmapImage();
                bbb.BeginInit();
                bbb.UriSource = new Uri(list_souboru[2]);
                bbb.EndInit();
                image3.Source = bbb;
                if (list_vyber.Contains(System.IO.Path.Combine(tbTo.Text, System.IO.Path.GetFileName(list_souboru[2]))))
                    lbl3.Background = new SolidColorBrush(Colors.Green);
                else
                    lbl3.Background = new SolidColorBrush(Colors.WhiteSmoke);

                Rotate(image3, list_souboru[2]);
            }
            catch
            {
                image3.Source = null;
                image3.RenderTransform = new RotateTransform(0);
                lbl3.Background = new SolidColorBrush(Colors.WhiteSmoke);
            }

            tbl1.Text = (bin.Count + 1).ToString();
            tbl2.Text = (list_souboru.Count + bin.Count).ToString();
            tbl3.Text = list_vyber.Count().ToString();

            lblPocet.Visibility = System.Windows.Visibility.Visible;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Right)
                {
                    if (list_souboru.Count == 1)
                    {
                        MessageBox.Show("Konec.");
                        return;
                    }
                }
                if (e.Key == Key.Left)
                {
                    if (bin.Count == 0)
                    {
                        MessageBox.Show("Nelze.");
                        return;
                    }
                }
                if (e.Key == Key.Down)
                {
                    File.Copy(list_souboru[0], System.IO.Path.Combine(tbTo.Text, System.IO.Path.GetFileName(list_souboru[0])), true);
                    //bin.Add(list_souboru[0]);
                    //list_souboru.RemoveAt(0);
                    PrehrajObrazek();
                }
                if (e.Key == Key.Right)
                {
                    bin.Add(list_souboru[0]);
                    list_souboru.RemoveAt(0);
                    PrehrajObrazek();
                }
                if (e.Key == Key.Left)
                {
                    list_souboru.Insert(0, bin[bin.Count - 1]);
                    bin.RemoveAt(bin.Count - 1);
                    PrehrajObrazek();
                }
                if (e.Key == Key.Up)
                {
                    File.Delete(System.IO.Path.Combine(tbTo.Text, System.IO.Path.GetFileName(list_souboru[0])));
                    PrehrajObrazek();
                }
                this.Focus();
            }
            catch
            {

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblPocet.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            lblPocet.Visibility = System.Windows.Visibility.Collapsed;
            list_souboru.Clear();
            list_vyber = null;
            bin.Clear();
            tbFrom.IsEnabled = true;
            tbTo.IsEnabled = true;
            btnStop.IsEnabled = false;
            button1.IsEnabled = true;
            image00.Source = null;
            image0.Source = null;
            image1.Source = null;
            image2.Source = null;
            image3.Source = null;
            lbl00.Background = new SolidColorBrush(Colors.WhiteSmoke);
            lbl0.Background = new SolidColorBrush(Colors.WhiteSmoke);
            lbl1.Background = new SolidColorBrush(Colors.WhiteSmoke);
            lbl2.Background = new SolidColorBrush(Colors.WhiteSmoke);
            lbl3.Background = new SolidColorBrush(Colors.WhiteSmoke);
        }

        private void btnFrom_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Navod n = new Navod();
            n.ShowDialog();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            string[] list = Directory.GetFiles(tbTo.Text);
            DateTime datum = new DateTime();
            int index_den = 0;
            int index = 1;
            foreach (string fotka in list)
            {
                DateTime dt = File.GetLastWriteTime(fotka);
                if (dt != datum)
                {
                    datum = dt;
                    index = 1;
                    index_den++;
                }
                CultureInfo polish = new CultureInfo("pl-PL");
                string name = index_den.ToString().PadLeft(2, '0') + " " + datum.DayOfWeek.ToString() + "(" + index.ToString().PadLeft(3, '0') + ").jpg";
                System.IO.File.Move(fotka, System.IO.Path.Combine( System.IO.Path.GetDirectoryName(fotka), name));

            }
        }
    }
}
