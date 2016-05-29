using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;


namespace DestkopTrivialWiki
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string token;
        HttpClient client;
        public MainWindow(string token)
        {
            this.token = token;
            client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
            InitializeComponent();

            var responseString = client.GetStringAsync("http://localhost:4605/getUserBatch/1");

            JObject joResponse = JObject.Parse(responseString.Result);
            JArray array = (JArray)joResponse["users"];
            Console.WriteLine(array.ToString());

            var list = new ObservableCollection<DataObject>();
            string name;
            string email;
            int points;
            string role;
            int rank;
            foreach (JObject user in array)
            {
                name = (string)user.GetValue("username");
                email = (string)user.GetValue("email");
                points = Int32.Parse((string)user.GetValue("points"));
                role = (string)user.GetValue("role");
                rank = Int32.Parse((string)user.GetValue("rank"));
                list.Add(new DataObject() { Name = name, Email = email, Points = points, Role = role, Rank = rank });

            }
            this.dataGrid1.ItemsSource = list;
        }

        private async void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = (DataObject)this.dataGrid1.SelectedValue;

            var values = new Dictionary<string, string>
            {
                { "Username", selectedUser.Name },
                { "Points", selectedUser.Points.ToString()},
                { "Email", selectedUser.Email }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://localhost:4605/addNewUser", content);

            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString.ToString());

        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = (DataObject)this.dataGrid1.SelectedValue;

            var values = new Dictionary<string, string>
                {
                   { "Username", selectedUser.Name },
                   { "Points", selectedUser.Points.ToString()},
                   { "Email", selectedUser.Email },
                   { "Role", selectedUser.Role },
                   { "Rank", selectedUser.Rank.ToString() }
                };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://localhost:4605/updateUser", content);

            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString.ToString());

        }
    }

    public class DataObject
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Points { get; set; }
        public string Role { get; set; }
        public int Rank { get; set; }
    }
}
