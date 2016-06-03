using System.Windows;
using System.Windows.Controls;

namespace DestkopTrivialWiki
{
    /// <summary>
    /// Interaction logic for GeneralGamePage.xaml
    /// </summary>
    public partial class GeneralGamePage : Page
    {
        public GeneralGamePage()
        {
            InitializeComponent();
        }

        private void HomeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var main = new MainPage("");
            this.NavigationService?.Navigate(main);
        }
    }
}
