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
            if (Username.Text == "")
            {
                ErrorBlock.Text = "The username can not be empty!";
                ErrorBlock.Visibility = Visibility.Visible;
                return;
            }
            if (Password.Password == "")
            {
                ErrorBlock.Text = "The password can not be empty!";
                ErrorBlock.Visibility = Visibility.Visible;
                return;
            }
            using (var client = new HttpClient())
            {
                try
                {
                    if (Username.Text.Equals("") || Password.Password.Equals(""))
                        throw new Exception("Username and password invalid!");
                    var responseString =
                        client.GetStringAsync("http://localhost:4605/login?username=" + Username.Text + "&password=" +
                                              Password.Password).Result;
                    var joResponse = JObject.Parse(responseString);
                    var main = new MainPage(joResponse.GetValue("SecurityToken").ToString());
                    this.NavigationService?.Navigate(main);
                }
                catch (Exception)
                {
                    ErrorBlock.Text = "Username or password invalid! Please try again.";
                    ErrorBlock.Visibility = Visibility.Visible;
                }
            }
            Username.Text = "";
            Password.Password = "";
        }
        private void HomeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var main = new MainPage("");
            this.NavigationService?.Navigate(main);
        }
    }
}
