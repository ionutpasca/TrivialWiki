using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Windows;

namespace DestkopTrivialWiki
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (username.Text != "" && password.Text != "")
            {
                using (var client = new HttpClient())
                {
                    var responseString = client.GetStringAsync("http://localhost:4605/login?username=" + username.Text + "&password=" + password.Text).Result;
                    JObject joResponse = JObject.Parse(responseString);
                    MainWindow main = new MainWindow(joResponse.GetValue("securityToken").ToString());
                    App.Current.MainWindow = main;
                    this.Close();
                    main.Show();
                }
            }

            else
                Console.Out.WriteLine("Error");
        }
    }
}
