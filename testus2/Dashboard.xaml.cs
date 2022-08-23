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

        public static void UpdateAvatar(Image img)
        {
            BitmapImage i = new BitmapImage();
            i.BeginInit();
            i.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            i.CacheOption = BitmapCacheOption.OnLoad;
            i.UriSource = new Uri($"{Login.URI}/user/img/{Login.id}");
            i.EndInit();
            img.Source = i;
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

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateName(ImePrezime);
            UpdateAvatar(Profilna);
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // odjava
            if (File.Exists(Environment.CurrentDirectory + "\\loginToken.txt"))
            {
                File.Delete(Environment.CurrentDirectory + "\\loginToken.txt");
            }
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
