using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace DestkopTrivialWiki
{
    /// <summary>
    /// Interaction logic for GeneralGamePage.xaml
    /// </summary>
    public partial class GeneralGamePage : Page
    {
        private readonly HttpClient _client;
        private int skip = 1;
        private readonly string _token;
        private List<JObject> _questionSet;
        private int _questionIndex;
        private string currentAnswer;
        public GeneralGamePage(string token)
        {
            this._token = token;
            this._questionSet = new List<JObject>();
            this._questionIndex = 0;
            InitializeComponent();
            var chat = new Chat.Chat();
            _client = new HttpClient();
            Connect();
            GetMessages();
            GetQuestions();
            LoadNextQuestion();
        }

        private void HomeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var main = new MainPage(_token);
            this.NavigationService?.Navigate(main);
        }

        private void GetMessages()
        {
            string request = "http://localhost:4605/getMessages/";
            request += 0;
            var responseString = _client.GetStringAsync(request);
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

            await _client.PostAsync("http://localhost:4605/addMessage", content);
            MessageSrc.Text = "";
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
                ChatBox.Items.Insert(ChatBox.Items.Count, toAdd);

            }
            );
        }

        private void AnswerBtn_OnClick(object sender, RoutedEventArgs e)
        {
            var answerString = AnswerBox.Text;
            AnswerBox.Text = "";
            if (answerString.Length == 0) return;
            if (answerString.Trim().ToLower().Equals(currentAnswer.ToLower()))
            {
                MessageBox.Show("Tu esti bun ma!");
                LoadNextQuestion();
            }
            else
            {
                MessageBox.Show("Ba tu esti prost ?!");
            }
        }

        private void GetQuestions()
        {
            var jsonOutput = System.IO.File.ReadAllText(@"D:\Licenta\Files\OutputTestJson.txt");
            var joText = JObject.Parse(jsonOutput);
            var questions = (JArray)joText["Questions"];
            int index = 0;
            foreach (var question in questions)
            {
                _questionSet.Insert(index, (JObject)question);
                index++;
            }
        }

        private void LoadNextQuestion()
        {
            var question = _questionSet.ElementAt(_questionIndex);
            QuestionText.Text = (string)question.GetValue("Question");
            currentAnswer = ((string)question.GetValue("Answer")).Trim();
            _questionIndex++;
        }
    }
}
