﻿using System;
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
using System.Globalization;
using ExifPhotoReader;

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
        //List<string> bin = new List<string>();
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
            //bin.Clear();
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

            sp.Children.Clear();
            foreach (string c in list_vyber)
            {
                HodDoVyberu(System.IO.Path.Combine(c));
            }
            GC.Collect();
        }

        void Rotate(Image i, string soubor)
        {
            try
            {
                ExifImageProperties exifImage = ExifPhoto.GetExifDataPhoto(list_souboru[pointer]);
                //ExifReader.ExifReader r = new ExifReader.ExifReader(list_souboru[pointer]);
                //string orientace = "1";
                //foreach (ExifProperty item in r.GetExifProperties())
                //{
                //    if (item.ExifPropertyName == "Orientation")
                //    {
                //        orientace = item.ToString();
                //        goto aaa;
                //    }
                //}
                //aaa:
                if (exifImage.Orientation == ExifPhotoReader.Orientation.Rotate270)
                {
                    image1.RenderTransform = new RotateTransform(270);
                }
                else if (exifImage.Orientation == ExifPhotoReader.Orientation.Rotate90)
                {
                    image1.RenderTransform = new RotateTransform(90);
                }
                else
                {
                    image1.RenderTransform = new RotateTransform(0);
                }
            }
            catch { }
        }

        string[] list_vyber = new string[1];
        int pointer = 0;

        void PrehrajObrazek()
        {
            list_vyber = Directory.GetFiles(tbTo.Text);
            try
            {
                BitmapImage a = new BitmapImage();
                a.BeginInit();
                a.UriSource = new Uri(list_souboru[pointer - 2]);
                a.CacheOption = BitmapCacheOption.OnLoad; 
                a.EndInit();
                image00.Source = a;
                if (list_vyber.Contains(System.IO.Path.Combine(tbTo.Text, System.IO.Path.GetFileName(list_souboru[pointer - 2]))))
                    lbl00.Background = new SolidColorBrush(Colors.Green);
                else
                    lbl00.Background = new SolidColorBrush(Colors.WhiteSmoke);

                Rotate(image00, list_souboru[pointer - 2]);
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
                aa.UriSource = new Uri(list_souboru[pointer - 1]);
                aa.CacheOption = BitmapCacheOption.OnLoad;
                aa.EndInit();
                image0.Source = aa;
                if (list_vyber.Contains(System.IO.Path.Combine(tbTo.Text, System.IO.Path.GetFileName(list_souboru[pointer - 1]))))
                    lbl0.Background = new SolidColorBrush(Colors.Green);
                else
                    lbl0.Background = new SolidColorBrush(Colors.WhiteSmoke);

                Rotate(image0, list_souboru[pointer - 1]);
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
                b.UriSource = new Uri(list_souboru[pointer]);
                b.CacheOption = BitmapCacheOption.OnLoad;
                b.EndInit();
                image1.Source = b;
                if (list_vyber.Contains(System.IO.Path.Combine(tbTo.Text, System.IO.Path.GetFileName(list_souboru[pointer]))))
                {
                    lbl1.Background = new SolidColorBrush(Colors.Green);
                    ve_vyberu = true;
                }
                else
                {
                    lbl1.Background = new SolidColorBrush(Colors.WhiteSmoke);
                    ve_vyberu = false;
                }

                Rotate(image1, list_souboru[pointer]);
            }
            catch (Exception eee)
            {
                image1.Source = null;
                image1.RenderTransform = new RotateTransform(0);
                lbl1.Background = new SolidColorBrush(Colors.WhiteSmoke);
                ve_vyberu = false;
            }
            try
            {
                BitmapImage bb = new BitmapImage();
                bb.BeginInit();
                bb.UriSource = new Uri(list_souboru[pointer + 1]);
                bb.CacheOption = BitmapCacheOption.OnLoad;
                bb.EndInit();
                image2.Source = bb;
                if (list_vyber.Contains(System.IO.Path.Combine(tbTo.Text, System.IO.Path.GetFileName(list_souboru[pointer + 1]))))
                    lbl2.Background = new SolidColorBrush(Colors.Green);
                else
                    lbl2.Background = new SolidColorBrush(Colors.WhiteSmoke);

                Rotate(image2, list_souboru[pointer + 1]);
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
                bbb.UriSource = new Uri(list_souboru[pointer + 2]);
                bbb.CacheOption = BitmapCacheOption.OnLoad;
                bbb.EndInit();
                image3.Source = bbb;
                if (list_vyber.Contains(System.IO.Path.Combine(tbTo.Text, System.IO.Path.GetFileName(list_souboru[pointer + 2]))))
                    lbl3.Background = new SolidColorBrush(Colors.Green);
                else
                    lbl3.Background = new SolidColorBrush(Colors.WhiteSmoke);

                Rotate(image3, list_souboru[pointer + 2]);
            }
            catch
            {
                image3.Source = null;
                image3.RenderTransform = new RotateTransform(0);
                lbl3.Background = new SolidColorBrush(Colors.WhiteSmoke);
            }

            tbl1.Text = (pointer + 1).ToString();
            tbl2.Text = list_souboru.Count.ToString();
            tbl3.Text = list_vyber.Count().ToString();

            pb.Value = (double)(pointer + 1) * 100 / list_souboru.Count;
            //pb1.Value = (double)(list_vyber.Count()) * 100 / list_souboru.Count;

            lblPocet.Visibility = System.Windows.Visibility.Visible;
            lbl1.Focus();
        }

        bool ve_vyberu = false;

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDown1(e.Key);
        }

        private void KeyDown1(Key e)
        {
            try
            {
                if (e == Key.Right)
                {
                    if (pointer == list_souboru.Count - 1)
                    {
                        //MessageBox.Show("Konec.");
                        return;
                    }
                }
                if (e == Key.Left)
                {
                    if (pointer == 0)
                    {
                        //MessageBox.Show("Nelze.");
                        return;
                    }
                }
                if (e == Key.Down)
                {
                    string new_path = System.IO.Path.Combine(tbTo.Text, System.IO.Path.GetFileName(list_souboru[pointer]));
                    File.Copy(list_souboru[pointer], new_path, true);
                    HodDoVyberu(new_path);
                    PrehrajObrazek();
                }
                if (e == Key.Right)
                {
                    //bin.Add(list_souboru[0]);
                    //list_souboru.RemoveAt(0);
                    pointer++;
                    if (pointer > list_souboru.Count - 1)
                        pointer = list_souboru.Count - 1;
                    PrehrajObrazek();
                }
                if (e == Key.Left)
                {
                    //list_souboru.Insert(0, bin[bin.Count - 1]);
                    //bin.RemoveAt(bin.Count - 1);
                    pointer--;
                    if (pointer < 0)
                        pointer = 0;
                    PrehrajObrazek();
                }
                if (e == Key.Up)
                {
                    string new_path = System.IO.Path.Combine(tbTo.Text, System.IO.Path.GetFileName(list_souboru[pointer]));
                    File.Delete(new_path);
                    VyhodZVyberu(new_path);
                    PrehrajObrazek();
                }
                this.Focus();
            }
            catch
            {

            }
        }

        private void HodDoVyberu(string cesta)
        {
            VyhodZVyberu(cesta);

            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(cesta);
            b.CacheOption = BitmapCacheOption.OnLoad;
            b.EndInit();
            Image iii = new Image();
            iii.Source = b;
            iii.Margin = new Thickness(3, 0, 0, 0);

            Rotate(iii, cesta);

            iii.Tag = cesta;
            iii.MouseLeftButtonDown += Iii_MouseLeftButtonDown;
            sp.Children.Add(iii);
            iii.BringIntoView();
        }

        private void Iii_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string tag = (sender as Image).Tag.ToString();
            string fn = System.IO.Path.GetFileName(tag);
            string c = System.IO.Path.Combine(tbFrom.Text, fn);
            int ppp = list_souboru.IndexOf(c);
            if (ppp >= 0)
            {
                pointer = ppp;
                PrehrajObrazek();
            }
        }

        private void VyhodZVyberu(string cesta)
        {
            List<Image> e = sp.Children.OfType<Image>().Where(q => q.Tag.ToString().Contains(cesta)).ToList();
            foreach (Image iii in e)
            {
                sp.Children.Remove(iii);
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
            //bin.Clear();
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            row5.Height = new GridLength(100, GridUnitType.Star);
            colL.Width = new GridLength(150, GridUnitType.Star);
            colR.Width = new GridLength(150, GridUnitType.Star);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            row5.Height = new GridLength(0);
            colL.Width = new GridLength(100, GridUnitType.Star);
            colR.Width = new GridLength(100, GridUnitType.Star);
        }

        private void btn10_Click(object sender, RoutedEventArgs e)
        {
            Inkrement(10);
        }

        private void btn20_Click(object sender, RoutedEventArgs e)
        {
            Inkrement(20);
        }

        private void btn50_Click(object sender, RoutedEventArgs e)
        {
            Inkrement(50);
        }

        void Inkrement(int i)
        {
            pointer += i;
            if (pointer < 0)
                pointer = 0;
            if (pointer > list_souboru.Count - 1)
                pointer = list_souboru.Count - 1;
            PrehrajObrazek();
        }

        private void btnZpet_Click(object sender, RoutedEventArgs e)
        {
            KeyDown1(Key.Left);
        }

        private void btnVyber_Click(object sender, RoutedEventArgs e)
        {
            if (ve_vyberu)
            {
                KeyDown1(Key.Up);
            }
            else
            {
                KeyDown1(Key.Down);
            }
        }

        private void btnVpred_Click(object sender, RoutedEventArgs e)
        {
            KeyDown1(Key.Right);
        }

        private void btn_min50_Copy_Click(object sender, RoutedEventArgs e)
        {
            Inkrement(-50);
        }

        private void btn_min20_Copy_Click(object sender, RoutedEventArgs e)
        {
            Inkrement(-20);
        }

        private void btn_min10_Copy_Click(object sender, RoutedEventArgs e)
        {
            Inkrement(-10);
        }
    }
}
