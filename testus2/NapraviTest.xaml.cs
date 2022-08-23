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
        private void NapredBtn_Click(object sender, RoutedEventArgs e)
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
                }
                catch
                {
                    // ovaj deo koda se izvrsi kada je zavrseno sa svim oblastima
                    ShowTest();
                }
            }
        }
    }
}
