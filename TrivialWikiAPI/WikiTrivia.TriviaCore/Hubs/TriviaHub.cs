using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using WikiTrivia.TriviaCore.Models;

namespace WikiTrivia.TriviaCore.Hubs
{
    [HubName("triviaHub")]
    public class TriviaHub : Hub
    {
        public void AddMessage(TriviaQuestionDto question)
        {
            Clients.All.addMessage(question);
        }

        public void Heartbeat()
        {
            Clients.All.heartbeat();
        }

        public override Task OnConnected()
        {
            return (base.OnConnected());
        }
    }
}
