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
        public PlayPage()
        {
            InitializeComponent();
        }

        private void HomeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var main = new MainPage("");
            this.NavigationService?.Navigate(main);
        }

        private void InfiniteBtn_OnClick(object sender, RoutedEventArgs e)
        {
            firstOption = true;
            InfiniteTriviaDescription.Visibility = System.Windows.Visibility.Visible;
            FriendsTriviaDescription.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void FriendsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            InfiniteTriviaDescription.Visibility = System.Windows.Visibility.Collapsed;
            FriendsTriviaDescription.Visibility = System.Windows.Visibility.Visible;
        }

        private void StartFriendsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ba tu chiar esti prost?");
        }

        private void StartInfiniteBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var infinite = new GeneralGamePage();
            this.NavigationService?.Navigate(infinite);
        }
    }
}