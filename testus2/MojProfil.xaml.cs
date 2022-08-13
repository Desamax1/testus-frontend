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
using Microsoft.Win32;
using ImageMagick;

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
            Dashboard.UpdateName(ImePrezime);
            Dashboard.UpdateAvatar(Profilna);
        }

        private string ConvertImage(string path)
        {
            try
            {
                using (var image = new MagickImage(path))
                {
                    var size = new MagickGeometry(120, 120);
                    size.IgnoreAspectRatio = true;

                    image.Resize(size);

                    image.Format = MagickFormat.WebP;

                    return image.ToBase64();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "Slike|*.png;*.jpg;*.jpeg;*.webp;*.gif;*.bmp;*.jfif;*.tiff";
            f.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (f.ShowDialog() == true) // == true je neophodno jer je wpf bas dobar i ne konta da je true po defaultu boolean
            {
                string b64 = ConvertImage(f.FileName);

                if (b64 == null)
                {
                    MessageBox.Show("Doslo je do greske prilikom konverzije slike.\nDa li je sve u redu sa fajlom?", "Avatar", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(Login.TIMEOUT);

                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "avatar", b64 }
                });
                
                var res = await httpClient.PostAsync($"{Login.URI}/user/img/{Login.id}", content);
                if ((int)res.StatusCode < 400)
                {
                    MessageBox.Show("Avatar uspesno promenjen!", "Avatar", MessageBoxButton.OK, MessageBoxImage.Information);
                    BitmapImage b = new BitmapImage();
                    b.BeginInit();
                    b.UriSource = new Uri($"{Login.URI}/user/img/{Login.id}");
                    b.EndInit();
                    Profilna.Source = b;
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Doslo je do greske prilikom promene avatara.\nPokusajte ponovo kasnije.", "Avatar", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
