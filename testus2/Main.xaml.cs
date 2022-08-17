using System;
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
using System.Collections.Generic;

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
        public int brojZad = 200;
        public byte[] selektovaniZadaci;
        public int ignorisi;
        public int xd;
        public int xd2;


        public Main()
        {
            InitializeComponent();


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
            if (ZadaciListBox.SelectedIndex == brojZad - 1)
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da predate test?", "Predajte test", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Kraj();
                }
            }
            xd = ZadaciListBox.SelectedIndex + 1;
            ZadaciListBox.SelectedIndex++;

        }

        private void PrethodnoButton_Click(object sender, RoutedEventArgs e)
        {
            xd = ZadaciListBox.SelectedIndex - 1;
            ZadaciListBox.SelectedIndex--;
        }

        private void ZadaciListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //xd = ZadaciListBox.SelectedIndex;
            //ZadaciListBox.Items[ZadaciListBox.SelectedIndex] = $"⚫ Pitanje br. {(ZadaciListBox.SelectedIndex + 1)}";
            //ignorisi = 1;
            //selektovaniZadaci[xd] = 1;
            //ZadaciListBox.SelectedIndex = xd;
            updateData();
        }
        private void updateData()
        {
          //  MessageBox.Show(ZadaciListBox.SelectedIndex.ToString()+" promenio se index");
            ucitajZadatak();
            if (ZadaciListBox.SelectedIndex == 0)
            {
                PrethodnoButton.IsEnabled = false;
                SledeceButton.Content = "Sledece pitanje";
            }
            else if (ZadaciListBox.SelectedIndex == brojZad - 1)
            {
                SledeceButton.Content = "Predajte test";
                PrethodnoButton.IsEnabled = true;
            }
            else
            {
                SledeceButton.Content = "Sledece pitanje";
                PrethodnoButton.IsEnabled = true;
            }
            if (ZadaciListBox.SelectedIndex == -1) {
               // MessageBox.Show("OVO GLEDAJ " + selektovaniZadaci[xd]);
                switch (selektovaniZadaci[xd])
                {
                    case 1:
                        {
                            b1.Height = 80;
                            b1.Width = 200;
                            b2.Height = 80;
                            b2.Width = 200;
                            b3.Height = 80;
                            b3.Width = 200;
                            b4.Height = 80;
                            b4.Width = 200;
                            b1.BorderThickness = new Thickness(0);
                            b2.BorderThickness = new Thickness(0);
                            b3.BorderThickness = new Thickness(0);
                            b4.BorderThickness = new Thickness(0);
                            b1.BorderThickness = new Thickness(3);
                            b1.Height = 86;
                            b1.Width = 206;
                        }
                        break;
                    case 2:
                        {
                            b1.Height = 80;
                            b1.Width = 200;
                            b2.Height = 80;
                            b2.Width = 200;
                            b3.Height = 80;
                            b3.Width = 200;
                            b4.Height = 80;
                            b4.Width = 200;
                            b1.BorderThickness = new Thickness(0);
                            b2.BorderThickness = new Thickness(0);
                            b3.BorderThickness = new Thickness(0);
                            b4.BorderThickness = new Thickness(0);
                            b2.BorderThickness = new Thickness(3);
                            b2.Height = 86;
                            b2.Width = 206;
                        }
                        break;
                    case 3:
                        {
                            b1.Height = 80;
                            b1.Width = 200;
                            b2.Height = 80;
                            b2.Width = 200;
                            b3.Height = 80;
                            b3.Width = 200;
                            b4.Height = 80;
                            b4.Width = 200;
                            b1.BorderThickness = new Thickness(0);
                            b2.BorderThickness = new Thickness(0);
                            b3.BorderThickness = new Thickness(0);
                            b4.BorderThickness = new Thickness(0);
                            b3.BorderThickness = new Thickness(3);
                            b3.Height = 86;
                            b3.Width = 206;
                        }
                        break;
                    case 4:
                        {
                            b1.Height = 80;
                            b1.Width = 200;
                            b2.Height = 80;
                            b2.Width = 200;
                            b3.Height = 80;
                            b3.Width = 200;
                            b4.Height = 80;
                            b4.Width = 200;
                            b1.BorderThickness = new Thickness(0);
                            b2.BorderThickness = new Thickness(0);
                            b3.BorderThickness = new Thickness(0);
                            b4.BorderThickness = new Thickness(0);
                            b4.BorderThickness = new Thickness(3);
                            b4.Height = 86;
                            b4.Width = 206;
                        }
                        break;
                    case 0:
                        {
                            b1.Height = 80;
                            b1.Width = 200;
                            b2.Height = 80;
                            b2.Width = 200;
                            b3.Height = 80;
                            b3.Width = 200;
                            b4.Height = 80;
                            b4.Width = 200;
                            b1.BorderThickness = new Thickness(0);
                            b2.BorderThickness = new Thickness(0);
                            b3.BorderThickness = new Thickness(0);
                            b4.BorderThickness = new Thickness(0);
                        }
                        break;

                }
                return;
            }
            if ((selektovaniZadaci[ZadaciListBox.SelectedIndex] == 1 || selektovaniZadaci[ZadaciListBox.SelectedIndex] == 2 || selektovaniZadaci[ZadaciListBox.SelectedIndex] == 3 || selektovaniZadaci[ZadaciListBox.SelectedIndex] == 4) )
            {
               // MessageBox.Show(ZadaciListBox.SelectedIndex.ToString());
                ignorisi = 0;
                switch (selektovaniZadaci[ZadaciListBox.SelectedIndex])
                {
                    case 1:
                        {
                            b1.Height = 80;
                            b1.Width = 200;
                            b2.Height = 80;
                            b2.Width = 200;
                            b3.Height = 80;
                            b3.Width = 200;
                            b4.Height = 80;
                            b4.Width = 200;
                            b1.BorderThickness = new Thickness(0);
                            b2.BorderThickness = new Thickness(0);
                            b3.BorderThickness = new Thickness(0);
                            b4.BorderThickness = new Thickness(0);
                            b1.BorderThickness = new Thickness(3);
                            b1.Height = 86;
                            b1.Width = 206;
                        }
                        break;
                    case 2:
                        {
                            b1.Height = 80;
                            b1.Width = 200;
                            b2.Height = 80;
                            b2.Width = 200;
                            b3.Height = 80;
                            b3.Width = 200;
                            b4.Height = 80;
                            b4.Width = 200;
                            b1.BorderThickness = new Thickness(0);
                            b2.BorderThickness = new Thickness(0);
                            b3.BorderThickness = new Thickness(0);
                            b4.BorderThickness = new Thickness(0);
                            b2.BorderThickness = new Thickness(3);
                            b2.Height = 86;
                            b2.Width = 206;
                        }
                        break;
                    case 3:
                        {
                            b1.Height = 80;
                            b1.Width = 200;
                            b2.Height = 80;
                            b2.Width = 200;
                            b3.Height = 80;
                            b3.Width = 200;
                            b4.Height = 80;
                            b4.Width = 200;
                            b1.BorderThickness = new Thickness(0);
                            b2.BorderThickness = new Thickness(0);
                            b3.BorderThickness = new Thickness(0);
                            b4.BorderThickness = new Thickness(0);
                            b3.BorderThickness = new Thickness(3);
                            b3.Height = 86;
                            b3.Width = 206;
                        }
                        break;
                    case 4:
                        {
                            b1.Height = 80;
                            b1.Width = 200;
                            b2.Height = 80;
                            b2.Width = 200;
                            b3.Height = 80;
                            b3.Width = 200;
                            b4.Height = 80;
                            b4.Width = 200;
                            b1.BorderThickness = new Thickness(0);
                            b2.BorderThickness = new Thickness(0);
                            b3.BorderThickness = new Thickness(0);
                            b4.BorderThickness = new Thickness(0);
                            b4.BorderThickness = new Thickness(3);
                            b4.Height = 86;
                            b4.Width = 206;
                        }
                        break;

                }
            }
            else
            {
                b1.Height = 80;
                b1.Width = 200;
                b2.Height = 80;
                b2.Width = 200;
                b3.Height = 80;
                b3.Width = 200;
                b4.Height = 80;
                b4.Width = 200;
                b1.BorderThickness = new Thickness(0);
                b2.BorderThickness = new Thickness(0);
                b3.BorderThickness = new Thickness(0);
                b4.BorderThickness = new Thickness(0);
            }
        }


        private async void ucitajZadatak() {

            //HttpClient client = new HttpClient();
            //client.Timeout = new TimeSpan(Login.TIMEOUT);
            //var res= await client.GetAsync(Login.URI+"/vezbanje");

            //var jsonObject = JsonNode.Parse(res.ToString());
        }

        private void Ans1_MouseDown(object sender, MouseButtonEventArgs e)
        {
             xd=ZadaciListBox.SelectedIndex;
            ZadaciListBox.Items[ZadaciListBox.SelectedIndex] = $"⚫ Pitanje br. {(ZadaciListBox.SelectedIndex+1)}";
            ignorisi = 1;
            selektovaniZadaci[xd] = 1;
            xd2=ZadaciListBox.SelectedIndex;
            ZadaciListBox.SelectedIndex = xd;
            //MessageBox.Show(xd.ToString() + " " + ZadaciListBox.SelectedIndex);
        }

        private void Ans2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            xd = ZadaciListBox.SelectedIndex;
            ZadaciListBox.Items[ZadaciListBox.SelectedIndex] = "⚫ Pitanje br. " + (ZadaciListBox.SelectedIndex + 1);
            ignorisi = 1;
            selektovaniZadaci[xd] = 2;
            ZadaciListBox.SelectedIndex = xd;
        }

        private void Ans3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            xd = ZadaciListBox.SelectedIndex;
            ZadaciListBox.Items[ZadaciListBox.SelectedIndex] = "⚫ Pitanje br. " + (ZadaciListBox.SelectedIndex + 1);
            ignorisi = 1;
            selektovaniZadaci[xd] = 3;
            ZadaciListBox.SelectedIndex = xd;
        }

        private void Ans4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            xd = ZadaciListBox.SelectedIndex;
            ZadaciListBox.Items[ZadaciListBox.SelectedIndex] = "⚫ Pitanje br. " + (ZadaciListBox.SelectedIndex + 1);
            ignorisi = 1;
            selektovaniZadaci[xd] = 4;
            ZadaciListBox.SelectedIndex = xd;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ZadaciListBox.SelectedIndex = 0;
            selektovaniZadaci = new byte[brojZad];
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
            for (int i = 0; i < brojZad; i++) ZadaciListBox.Items.Add("⚪ Pitanje br. " + (i + 1));

            PrethodnoButton.IsEnabled = false;


            _timer.Start();
        }
    }
}
