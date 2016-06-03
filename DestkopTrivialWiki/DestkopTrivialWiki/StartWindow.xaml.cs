using System.Windows;

namespace DestkopTrivialWiki
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
            NavigationFrame.Navigate(new MainPage(""));
        }
    }
}
