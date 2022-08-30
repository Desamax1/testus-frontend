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
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace testus2
{
    public partial class NapraviTest : Window
    {
        bool firstTime = true;
        HttpClient client = new HttpClient();
        Queue<string> oblasti = new Queue<string>();

        class Zadatak
        {
            private string oblast;
            private int tezina, broj;
            public string Oblast
            {
                get { return oblast; }
                set { oblast = value; }
            }
            public int Tezina
            {
                get { return tezina; }
                set { tezina = value; }
            }
            public int Broj
            {
                get { return broj; }
                set { broj = value; }
            }
            public Zadatak(string oblast, int tezina, int broj)
            {
                Oblast = oblast;
                Tezina = tezina;
                Broj = broj;
            }
        }
        List<Zadatak> zadaci = new List<Zadatak>();

        public NapraviTest()
        {
            InitializeComponent();
        }

        private async void FillListBox(string oblast)
        {
            MainListBox.Items.Clear();
            string res = await client.GetStringAsync($"{Login.URI}/oblasti/{oblast}");
            JArray oblasti = JArray.Parse(res);
            foreach (JObject oblastiObj in oblasti)
            {
                MainListBox.Items.Add(oblastiObj["oblast"].ToString());
            }
        }

        private void ShowTest()
        {
            Main m = new Main();
            m.Show();
            Close();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            // GET-uj sve oblasti sa servera i popuni lb
            FillListBox("all");
        }
        private async void NapredBtn_Click(object sender, RoutedEventArgs e)
        {
            if (firstTime)
            {
                SettingsGrid.Visibility = Visibility.Visible;
                foreach (var item in MainListBox.SelectedItems)
                {
                    // kopiraj imena svih oblasti kroz koje treba loop-ovati
                    oblasti.Enqueue(item.ToString());
                }
                firstTime = false;
                MainListBox.SelectionMode = SelectionMode.Single;
                NapredBtn_Click(sender, e);
            }
            else
            {
                try
                {
                    // ako se ovaj kod izvrsava to znaci da i dalje ima oblasti u queue-u
                    string item = oblasti.Dequeue();
                    FillListBox(item);
                    BrZad.Text = string.Empty;
                    Tezina.SelectedIndex = 0;
                }
                catch
                {
                    // ovaj deo koda se izvrsi kada je zavrseno sa svim oblastima
                    string genTestUrl = $"{Login.URI}/test?";
                    foreach (Zadatak z in zadaci)
                    {
                        genTestUrl += "&oblast=" + z.Oblast;
                        genTestUrl += "&broj=" + z.Broj.ToString();
                        genTestUrl += "&tezina=" + z.Tezina.ToString();
                    }
                    var res = await client.PostAsync(genTestUrl, null);
                    MessageBox.Show(await res.Content.ReadAsStringAsync());
                    ShowTest();
                }
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MainListBox.SelectedIndex < 0)
            {
                MessageBox.Show("Morate izabrati oblast!", "Dodavanje zadatka", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (BrZad.Text == string.Empty)
            {
                MessageBox.Show("Potrebno je da popunite sva polja!", "Dodavanje zadatka", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            zadaci.Add(new Zadatak(MainListBox.SelectedItem.ToString(), Tezina.SelectedIndex, Convert.ToInt32(BrZad.Text)));
        }
    }
}
