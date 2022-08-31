using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Immutable;

namespace testus2
{
    public partial class Main : Window
    {
        HttpClient client = new HttpClient();
        int brojZad;
        string[]? selectedAnswers;
        int selektovaniIndeks = 0;

        public Main()
        {
            InitializeComponent();
        }
        private void Kraj()
        {
            MessageBox.Show("kraj");
            NapraviTest.testId = -1;
            Dashboard d = new Dashboard();
            d.Show();
            Close();
        }
        private void SledeceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ZadaciListBox.SelectedIndex == brojZad - 1)
            {
                if (MessageBox.Show("Da li ste sigurni da zelite da predate test?", "Predajte test", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Kraj();
                }
            }
            ZadaciListBox.SelectedIndex++;
            selektovaniIndeks = ZadaciListBox.SelectedIndex;
        }
        private void PrethodnoButton_Click(object sender, RoutedEventArgs e)
        {
            ZadaciListBox.SelectedIndex--;
            selektovaniIndeks = ZadaciListBox.SelectedIndex;
        }
        private void ResetDisplay()
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
        private void ZadaciListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ucitajZadatak();
            // handlovanje dugmica
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
            // provera da li je na pitanje odgovoreno
            if (ZadaciListBox.SelectedIndex == -1)
            {
                return;
            }
            ResetDisplay();
            if (selectedAnswers is not null)
            {
                if ((selectedAnswers[ZadaciListBox.SelectedIndex] != string.Empty))
                    switch (selectedAnswers[ZadaciListBox.SelectedIndex])
                    {
                        case "Ans1":
                            {
                                b1.BorderThickness = new Thickness(3);
                                b1.Height = 86;
                                b1.Width = 206;
                            }
                            break;
                        case "Ans2":
                            {
                                b2.BorderThickness = new Thickness(3);
                                b2.Height = 86;
                                b2.Width = 206;
                            }
                            break;
                        case "Ans3":
                            {
                                b3.BorderThickness = new Thickness(3);
                                b3.Height = 86;
                                b3.Width = 206;
                            }
                            break;
                        case "Ans4":
                            {
                                b4.BorderThickness = new Thickness(3);
                                b4.Height = 86;
                                b4.Width = 206;
                            }
                            break;
                    }
            }
        }
        private void ucitajZadatak() {
            // TODO: implenetirati logiku za preuzimanje zadataka sa backend-a
        }

        private static string CleanUp(string s)
        {
            s = s.Replace(@"@", "");
            s = s.Replace(@"\left|", "");
            s = s.Replace(@"\right|", "");
            s = s.Replace(@"\ ", "");
            s = s.Replace(@" ", @"\,\,");
            return s;
        }

        private void HandleAns(object sender, MouseButtonEventArgs e)
        {
            selektovaniIndeks = ZadaciListBox.SelectedIndex;
            ZadaciListBox.Items[ZadaciListBox.SelectedIndex] = $"⚫ Pitanje br. {(ZadaciListBox.SelectedIndex + 1)}";
            if (selectedAnswers is not null)
                selectedAnswers[selektovaniIndeks] = (sender as Image).Name;
            ZadaciListBox.SelectedIndex = selektovaniIndeks;
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // GET-u test sa servera
            var res = await client.GetAsync($"{Login.URI}/test?testId={NapraviTest.testId}");
            JObject testData = JObject.Parse(await res.Content.ReadAsStringAsync());
            int vreme = 600;
            brojZad = Convert.ToInt32(testData["zadCount"]);
            string[] zadaci = Convert.ToString(testData["zadaci"]).Split(';');
            // potrebno je debloatovati sve stringove
            for (int i = 0; i < zadaci.Length; i++)
            {
                zadaci[i] = CleanUp(zadaci[i]);
            }

            ZadaciListBox.SelectedIndex = 0;
            selectedAnswers = new string[brojZad];
            for (int i = 0; i < selectedAnswers.Length; i++)
            {
                selectedAnswers[i] = string.Empty;
            }

            DispatcherTimer _timer = new DispatcherTimer();
            TimeSpan _time = TimeSpan.FromSeconds(vreme);
            _timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, delegate
            {
                Timer.Text = _time.ToString("c").Substring(3);
                if (_time == TimeSpan.Zero)
                {
                    _timer.Stop();
                    Kraj();
                }
                _time = _time.Subtract(TimeSpan.FromSeconds(1));
            }, Application.Current.Dispatcher);

            for (int i = 0; i < brojZad; i++)
                ZadaciListBox.Items.Add("⚪ Pitanje br. " + (i + 1).ToString());

            PrethodnoButton.IsEnabled = false;

            _timer.Start();
        }
    }
}