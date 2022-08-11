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
    /// Interaction logic for MojProfil.xaml
    /// </summary>
    public partial class MojProfil : Window
    {
        class _ImePrezime
        {
            public string ime { get; set; }
            public string prezime { get; set; }
        }
        public MojProfil()
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
    }
}
