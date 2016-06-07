using DatabaseManager.Trivia;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WikiTrivia.TriviaCore.Hubs
{
    public static class TriviaUserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
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
            if (TriviaUserHandler.ConnectedIds.Contains(Context.ConnectionId))
            {
                return Task.FromResult(0);
            }
            TriviaUserHandler.ConnectedIds.Add(Context.ConnectionId);
            if (CurrentTriviaQuestion.currentTriviaQuestion == null)
            {
                triviaCore.BroadcastQuestion();
            }
            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            TriviaUserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}
