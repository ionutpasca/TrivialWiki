using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace DestkopTrivialWiki
{
    /// <summary>
    /// Interaction logic for UserManagementPage.xaml
    /// </summary>
    public partial class UserManagementPage : Page
    {
        private string token = "";
        private HttpClient client;
        private ObservableCollection<DataObject> list;
        private int listLength;
        public UserManagementPage(string token)
        {
            this.token = token;
            client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
            list = new ObservableCollection<DataObject>();
            InitializeComponent();
            //GetUsers(1, 0);

            AddBtn.IsEnabled = false;
            dataGrid1.CanUserResizeColumns = false;
            dataGrid1.CanUserResizeRows = false;
        }

        private async void AddBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var index = dataGrid1.SelectedIndex;
            if (index == -1)
                return;
            if (index == listLength)
            {
                var selectedUser = (DataObject)this.dataGrid1.SelectedValue;

                var values = new Dictionary<string, string>
            {
                { "Username", selectedUser.Name },
                { "Email", selectedUser.Email },
                { "Password", selectedUser.Password }
            };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://localhost:4605/addNewUser", content);

                var responseString = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseString.ToString());

                list.RemoveAt(index);
            }
            AddBtn.IsEnabled = false;
            dataGrid1.CanUserAddRows = true;
        }

        private async void DeleteBtn_OnClickClick(object sender, RoutedEventArgs e)
        {
            int index = dataGrid1.SelectedIndex;
            if (index == -1)
                return;
            if (index < listLength)
            {
                var selectedUser = (DataObject)this.dataGrid1.SelectedValue;

                var values = new Dictionary<string, string>
                {
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://localhost:4605/removeUser/" + selectedUser.Name, content);

                var responseString = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseString.ToString());

                list.RemoveAt(index);
            }
        }

        private async void UpdateBtn_OnClicklick(object sender, RoutedEventArgs e)
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

        private void ResetBtn_OnClicksetBtn_Click(object sender, RoutedEventArgs e)
        {
            GetUsers(1, 0);
        }

        private void LoadBtn_OnClickBtn_Click(object sender, RoutedEventArgs e)
        {
            int index = (list.Count + 1) / 10 + 1;
            GetUsers(index, list.Count % 10);
        }

        private void dataGrid1_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            AddBtn.IsEnabled = true;
        }
        private void GetUsers(int index, int position)
        {
            var responseString = client.GetStringAsync("http://localhost:4605/getUserBatch/" + index);

            JObject joResponse = JObject.Parse(responseString.Result);
            JArray array = (JArray)joResponse["users"];
            Console.WriteLine(array.ToString());
            if (index == 1)
            {
                list.Clear();
            }
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
                if (position == 0)
                    list.Add(new DataObject() { Name = name, Email = email, Points = points, Role = role, Rank = rank, Password = "Secret" });
                else
                    position--;
            }
            listLength = list.Count;
            this.dataGrid1.ItemsSource = list;
        }

        private void HomeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var main = new MainPage(token);
            this.NavigationService?.Navigate(main);
        }
    }

    public class DataObject
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Points { get; set; }
        public string Role { get; set; }
        public int Rank { get; set; }
        public string Password { get; set; }
    }
}
