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
using System.IO;


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

        public static BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public static async void UpdateAvatar(Image img)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(Login.TIMEOUT);

            var res = await httpClient.GetAsync($"{Login.URI}/user/img/{Login.id}");

            string base64string = await res.Content.ReadAsStringAsync();
            img.Source = ToImage(Convert.FromBase64String(base64string));
        }

        public static async void UpdateName(TextBlock tb)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(Login.TIMEOUT);
            var res = await httpClient.GetFromJsonAsync<_ImePrezime>(Login.URI + "/user/info/" + Login.id);
            if (res != null)
            {
                tb.Text = res.ime + " " + res.prezime;
            }
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateName(ImePrezime);
            UpdateAvatar(Profilna);
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
            UpdateAvatar(Profilna);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Main m = new Main();
            m.Show();
            Close();
        }
    }
}
