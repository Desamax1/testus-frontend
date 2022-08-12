﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Drawing;
using System.Text.Json.Nodes;
using System.Net.Http;

namespace testus2
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    /// 
    
    public partial class Main : Window
    {
        public int vreme = 1000;
        DispatcherTimer _timer;
        TimeSpan _time;
        public int brojZad = 20;

        public Main()
        {
            InitializeComponent();
            _time = TimeSpan.FromSeconds(vreme);

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                Timer.Text = _time.ToString("c").Substring(3);
                if (_time == TimeSpan.Zero && _timer != null)
                {
                    _timer.Stop();
                    Kraj();
                }
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
            for (int i = 0; i < brojZad; i++) ZadaciListBox.Items.Add(i.ToString() + "⚪");
            ZadaciListBox.SelectedIndex = 0;
            PrethodnoButton.IsEnabled = false;


            _timer.Start();

        }
        private void Kraj()
        {
            MessageBox.Show("kraj");
            Dashboard d = new Dashboard();
            d.Show();
            this.Close();
        }

        private void SledeceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ZadaciListBox.SelectedIndex==brojZad-1)
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da predate test?","Predajte test",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Kraj();
                }
            }
            ZadaciListBox.SelectedIndex++;

        }

        private void PrethodnoButton_Click(object sender, RoutedEventArgs e)
        {
            ZadaciListBox.SelectedIndex--;
        }

        private void ZadaciListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ucitajZadatak();
            if (ZadaciListBox.SelectedIndex == 0)
            {
                PrethodnoButton.IsEnabled=false;
                SledeceButton.Content = "Sledece pitanje";
            }
            else if (ZadaciListBox.SelectedIndex==brojZad-1)
            {
                SledeceButton.Content = "Predajte test";
                PrethodnoButton.IsEnabled = true;
            }
            else
            {
                SledeceButton.Content = "Sledece pitanje";
                PrethodnoButton.IsEnabled = true;
            }
        }
        private async void ucitajZadatak() {

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(Login.TIMEOUT);
            var res= await client.GetAsync(Login.URI+"/vezbanje");

            var jsonObject = JsonNode.Parse(res.ToString());
        }

        private void Ans1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            b1.BorderThickness = new Thickness(0);
            b2.BorderThickness = new Thickness(0);
            b3.BorderThickness = new Thickness(0);
            b4.BorderThickness = new Thickness(0);
            b1.BorderThickness = new Thickness(3);
        }

        private void Ans2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            b1.BorderThickness = new Thickness(0);
            b2.BorderThickness = new Thickness(0);
            b3.BorderThickness = new Thickness(0);
            b4.BorderThickness = new Thickness(0);
            b2.BorderThickness = new Thickness(3);
        }

        private void Ans3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            b1.BorderThickness = new Thickness(0);
            b2.BorderThickness = new Thickness(0);
            b3.BorderThickness = new Thickness(0);
            b4.BorderThickness = new Thickness(0);
            b3.BorderThickness = new Thickness(3);
        }

        private void Ans4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            b1.BorderThickness = new Thickness(0);
            b2.BorderThickness = new Thickness(0);
            b3.BorderThickness = new Thickness(0);
            b4.BorderThickness = new Thickness(0);
            b4.BorderThickness = new Thickness(3);
        }
    }
}
