using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace DestkopTrivialWiki
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (Username.Text == "" || Password.Text == "") return;
            using (var client = new HttpClient())
            {
                try
                {
                    var responseString =
                        client.GetStringAsync("http://localhost:4605/login?username=" + Username.Text + "&password=" +
                                              Password.Text).Result;
                    var joResponse = JObject.Parse(responseString);
                    var main = new MainPage(joResponse.GetValue("securityToken").ToString());
                    this.NavigationService?.Navigate(main);
                }
                catch (Exception)
                {
                    MessageBox.Show("Esti prost?");
                }
            }
            Username.Text = "";
            Password.Text = "";
        }
        private void HomeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var main = new MainPage("");
            this.NavigationService?.Navigate(main);
        }
    }
}
