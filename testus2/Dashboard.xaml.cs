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
using System.Net.Http;
using System.Net.Http.Json;


namespace testus2
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        class _ImePrezime
        {
            public string? ime { get; set; }
            public string? prezime { get; set; }
        }
        public Dashboard()
        {
            InitializeComponent();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(Login.URI + "/img/user/" + Login.id);
            b.EndInit();
            Profilna.Source = b;

            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(Login.TIMEOUT);
            var res = await httpClient.GetFromJsonAsync<_ImePrezime>(Login.URI + "/user/info/" + Login.id);
            if (res != null)
            {
                ImePrezime.Text = res.ime + " " + res.prezime;
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Login.id = -1;
            Login l = new Login();
            l.Show();
            Close();
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            MojProfil mp = new MojProfil();
            mp.ShowDialog();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Main m = new Main();
            m.Show();
            Close();
        }
    }
}
