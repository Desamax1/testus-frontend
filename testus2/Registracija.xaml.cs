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

namespace testus2
{
    public partial class Registracija : Window
    {
        public Registracija()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login l = new Login();
            l.Show();
            Close();
        }

        public void Set_Email(string email)
        {
            Email.Text = email;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            char[] karakteri = new char[]
            {
                '@', '-', '_', ')', '(', '*', '/', '\\', '#', '!', '%', '^', '&', '=', '+', '?', '.', ',', '[', ']', '{', '}', ':', ';', '"', '\''
            };
            if (Ime.Text.Length == 0 || Prezime.Text.Length == 0 || Email.Text.Length == 0 || Password.Password.Length == 0)
            {
                MessageBox.Show("Morate popuniti sva polja!", "Registracija", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (Email.Text != EmailConfirm.Text)
            {
                MessageBox.Show("Unete E-Mail adrese se ne podudaraju!", "Registracija", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (Password.Password != PasswordConfirm.Password)
            {
                MessageBox.Show("Unete lozinke se ne podudaraju!", "Registracija", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (Password.Password.Length < 8)
            {
                MessageBox.Show("Lozinka se mora sastojati iz minimum 8 karaktera!", "Registracija", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (Password.Password.IndexOfAny(karakteri) < 0) {
                MessageBox.Show("Mora sadrzati barem jedan specijalni karakter!", "Registracija", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            HttpClient httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "email", Email.Text },
                { "password", Password.Password },
                { "ime", Ime.Text },
                { "prezime", Prezime.Text },
            });
            try
            {
                httpClient.Timeout = new TimeSpan(Login.TIMEOUT);
                var response = await httpClient.PostAsync(Login.URI + "/register", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Uspesna registracija!", "Registracija", MessageBoxButton.OK, MessageBoxImage.Information);
                    Login l = new Login();
                    l.Show();
                    Close();
                }
                else if ((int)response.StatusCode == 400)
                {
                    MessageBoxResult r = MessageBox.Show("Nalog sa unetom E-Mail adresom vec postoji.\nZelite li da se prebacite na stranicu za prijavu?", "Registracija", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (r == MessageBoxResult.Yes)
                    {
                        Login l = new Login();
                        l.Show();
                        l.Set_Email(Email.Text);
                        Close();
                    }
                }
            }
            catch
            {
                MessageBoxResult r = MessageBox.Show("Doslo je do greske prilikom registracije.\nZelite li da pokusate ponovo?", "Registracija", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (r == MessageBoxResult.No)
                {
                    Login l = new Login();
                    l.Show();
                    Close();
                }
            }
        }
    }
}
