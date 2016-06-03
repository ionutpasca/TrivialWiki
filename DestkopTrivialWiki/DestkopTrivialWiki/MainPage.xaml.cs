using System.Windows;
using System.Windows.Controls;

namespace DestkopTrivialWiki
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private string token = "";
        public MainPage()
        {
            InitializeComponent();

        }
        public MainPage(string token)
        {
            this.token = token;
            InitializeComponent();
            if (this.token.Equals(""))
                ManageBtn.Visibility = Visibility.Hidden;
        }

        private void LoginBtn_OnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var login = new LoginPage();
            this.NavigationService?.Navigate(login);
        }


        private void ManageBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var userManager = new UserManagementPage(token);
            this.NavigationService?.Navigate(userManager);
        }

        private void PlayBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var playPage = new PlayPage();
            this.NavigationService?.Navigate(playPage);
        }

        private void SideBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (SideBar.Visibility == System.Windows.Visibility.Collapsed)
            {
                SideBar.Visibility = System.Windows.Visibility.Visible;
                (sender as Button).Content = "<";
                (sender as Button).HorizontalAlignment = HorizontalAlignment.Right;
            }
            else
            {
                SideBar.Visibility = System.Windows.Visibility.Collapsed;
                (sender as Button).Content = ">";
                (sender as Button).HorizontalAlignment = HorizontalAlignment.Left;
            }
        }
    }
}
