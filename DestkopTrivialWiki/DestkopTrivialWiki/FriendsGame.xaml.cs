using System.Windows;
using System.Windows.Controls;

namespace DestkopTrivialWiki
{
    /// <summary>
    /// Interaction logic for FriendsGame.xaml
    /// </summary>
    public partial class FriendsGame : Page
    {
        private readonly string token;
        public FriendsGame(string token)
        {
            this.token = token;
            InitializeComponent();
        }

        private void HomeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var main = new MainPage(token);
            this.NavigationService?.Navigate(main);
        }
    }
}
