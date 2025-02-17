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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace testus2
{
    public partial class Login : Window
    {
        public static long id = -1;
        public static readonly string URI = @"http://najebaosam.ga:8000";
        public static readonly long TIMEOUT = 10000000;
        HttpClient httpClient = new HttpClient();
        public Login()
        {
            InitializeComponent();
        }

        public void Set_Email(string email)
        {
            Email.Text = email;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Registracija r = new Registracija();
            r.Show();
            Close();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Email.Text == "")
            {
                MessageBox.Show("Morate uneti E-Mail!", "Prijava", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (Password.Password == "")
            {
                MessageBox.Show("Morate uneti lozinku!", "Prijava", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            try
            {
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "email", Email.Text },
                    { "password", Password.Password },
                });
                string loginUri = $"{URI}/login";
                if (RememberBox.IsChecked == true)
                {
                    loginUri += "?remember=true";
                }
                var r = await httpClient.PostAsync(loginUri, content);
                switch ((int)r.StatusCode)
                {
                    case 200:
                        string raw = await r.Content.ReadAsStringAsync();
                        JObject obj= JObject.Parse(raw);
                        id = Convert.ToInt64(obj["id"]);
                        // proveri da li je vracen connection string, i ako jeste zapamti ga
                        if (obj["loginString"] is not null)
                        {
                            File.WriteAllText(Environment.CurrentDirectory + "\\loginToken.txt", obj["loginString"].ToString());
                        }
                        // prebaci na dashboard
                        Dashboard d = new Dashboard();
                        d.Show();
                        Close();
                        return;
                    case 404:
                        MessageBoxResult mbr = MessageBox.Show("Ne postoji korisnik sa zadatim E-Mailom.\nZelite li da predjete na stranicu za registraciju?", "Prijava", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (mbr == MessageBoxResult.Yes)
                        {
                            Registracija reg = new Registracija();
                            reg.Show();
                            reg.Set_Email(Email.Text);
                            Close();
                        }
                        return;
                    case 401:
                        MessageBox.Show("Uneli ste pogresu lozinku ili adresu E-poste.", "Prijava", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    case 500:
                        MessageBox.Show("Doslo je do greske prilikom prijave.\nMolimo Vas pokusajte ponovo.", "Prijava", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + '\n' + URI + "/login", "Prijava", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            httpClient.Timeout = new TimeSpan(TIMEOUT);
            if (File.Exists(Environment.CurrentDirectory + "\\loginToken.txt"))
            {
                string loginString = File.ReadAllText(Environment.CurrentDirectory + "\\loginToken.txt");
                // posalji GET request na backend sa sadrzajem login tokena
                // ako server posalje OK resonse, uloguj automatski
                string raw = await httpClient.GetStringAsync($"{URI}/login/{loginString}");
                JObject json = JObject.Parse(raw);
                id = Convert.ToInt64(json["id"]);
                Dashboard d = new Dashboard();
                d.Show();
                Close();
            }
        }
    }
}
