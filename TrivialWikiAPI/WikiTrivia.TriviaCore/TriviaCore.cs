using Microsoft.AspNet.SignalR;
using System.Timers;
using WikiTrivia.TriviaCore.Hubs;

namespace WikiTrivia.TriviaCore
{
    public sealed class TriviaCore
    {
        public void Run()
        {
            var aTimer = new Timer(10000);
            aTimer.Elapsed += BroadcastQuestion;
            aTimer.Interval = 7000;
            aTimer.Enabled = true;
        }

        private static void BroadcastQuestion(object source, ElapsedEventArgs e)
        {
            var question = QuestionsManager.GetNewQuestion().Result;
            var context = GlobalHost.ConnectionManager.GetHubContext<TriviaHub>();
            context.Clients.All.AddMessage(question);
        }
    }
}
