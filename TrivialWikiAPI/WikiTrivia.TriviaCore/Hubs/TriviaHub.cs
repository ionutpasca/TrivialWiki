using DatabaseManager.Trivia;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WikiTrivia.TriviaCore.Hubs
{
    public static class TriviaUserHandler
    {
        public static HashSet<string> connectedUsers = new HashSet<string>();
    }

    [HubName("triviaHub")]
    public class TriviaHub : Hub
    {
        private readonly TriviaCore triviaCore = new TriviaCore();
        public void AddMessage(TriviaMessageDto question)
        {
            Clients.All.addMessage(question);
        }

        public void Heartbeat()
        {
            Clients.All.heartbeat();
        }

        public override Task OnConnected()
        {
            var token = Context.Headers["User"];
            if (token == null)
            {
                return Task.FromResult(0);
            }

            if (TriviaUserHandler.connectedUsers.Contains(token))
            {
                return Task.FromResult(0);
            }
            TriviaUserHandler.connectedUsers.Add(token);
            if (CurrentTriviaQuestion.currentTriviaQuestion == null)
            {
                triviaCore.BroadcastQuestion();
            }
            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var token = Context.Headers["User"];
            if (token == null)
            {
                return Task.FromResult(0);
            }
            TriviaUserHandler.connectedUsers.Remove(token);
            return base.OnDisconnected(stopCalled);
        }
    }
}
