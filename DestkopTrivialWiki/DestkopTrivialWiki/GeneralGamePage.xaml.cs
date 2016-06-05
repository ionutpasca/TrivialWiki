using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DestkopTrivialWiki
{
    /// <summary>
    /// Interaction logic for GeneralGamePage.xaml
    /// </summary>
    public partial class GeneralGamePage : Page
    {
        private readonly HttpClient client;
        private int skip = 1;
        private readonly string token;
        public GeneralGamePage(string token)
        {
            this.token = token;
            InitializeComponent();
            var chat = new Chat.Chat();
            client = new HttpClient();
            Connect();
            GetMessages();
        }

        private void HomeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var main = new MainPage(token);
            this.NavigationService?.Navigate(main);
        }

        private void GetMessages()
        {
            string request = "http://localhost:4605/getMessages/";
            request += 0;
            var responseString = client.GetStringAsync(request);
            var joResponse = JArray.Parse(responseString.Result);

            foreach (JObject message in joResponse)
            {
                var toAdd = (string)message.GetValue("userName") + ":" + (string)message.GetValue("message");
                ChatBox.Items.Insert(0, toAdd);
            }
        }

        private async void SendBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var values = new Dictionary<string, string>
                {
                   { "UserName", "test" },
                   { "Message", MessageSrc.Text}
                };

            var content = new FormUrlEncodedContent(values);

            client.PostAsync("http://localhost:4605/addMessage", content);

        }

        public void Connect()
        {
            var hubConnection = new HubConnection("http://localhost:4605/signalr");
            var chatHubProxy = hubConnection.CreateHubProxy("chatHub");
            ServicePointManager.DefaultConnectionLimit = 10;
            hubConnection.Start().Wait();

            chatHubProxy.On("addMessage", message =>
            {
                var toAdd = message["UserName"] + ":" + message["Message"];
            }
            );
        }

        private void ChatBox_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            if (VisualTreeHelper.GetChildrenCount(ChatBox) > 0)
            {
                Border border = (Border)VisualTreeHelper.GetChild(ChatBox, 0);
                ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }
        }
    }
}
