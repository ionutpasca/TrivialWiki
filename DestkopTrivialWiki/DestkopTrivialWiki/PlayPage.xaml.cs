using System.Windows;
using System.Windows.Controls;

namespace DestkopTrivialWiki
{
    /// <summary>
    /// Interaction logic for PlayPage.xaml
    /// </summary>
    public partial class PlayPage : Page
    {
        private bool firstOption;
        private string token;
        public PlayPage(string token)
        {
            InitializeComponent();
            this.token = token;
        }

        private void HomeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var main = new MainPage(token);
            this.NavigationService?.Navigate(main);
        }

        private void InfiniteBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!PlayBtn.IsEnabled)
                PlayBtn.IsEnabled = true;
            firstOption = true;
            InfiniteImage.Visibility = System.Windows.Visibility.Visible;
            InfiniteDescription.Visibility = System.Windows.Visibility.Visible;
            FriendsImage.Visibility = System.Windows.Visibility.Collapsed;
            FriendsDescription.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void FriendsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!PlayBtn.IsEnabled)
                PlayBtn.IsEnabled = true;
            firstOption = false;
            InfiniteImage.Visibility = System.Windows.Visibility.Collapsed;
            InfiniteDescription.Visibility = System.Windows.Visibility.Collapsed;
            FriendsImage.Visibility = System.Windows.Visibility.Visible;
            FriendsDescription.Visibility = System.Windows.Visibility.Visible;
        }

        private void StartFriendsBtn_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void StartInfiniteBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var infinite = new GeneralGamePage(token);
            this.NavigationService?.Navigate(infinite);
        }

        private void PlayBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (firstOption)
            {
                var infinite = new GeneralGamePage(token);
                this.NavigationService?.Navigate(infinite);
            }
            else
            {
                var friends = new FriendsGame(token);
                this.NavigationService?.Navigate(friends);
            }
        }
    }
}