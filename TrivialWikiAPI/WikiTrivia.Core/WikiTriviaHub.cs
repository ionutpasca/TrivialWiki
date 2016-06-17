using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WikiTrivia.Core
{
    public static class WikiTriviaHandler
    {
        public static HashSet<string> connectedUsers = new HashSet<string>();
    }

    [HubName("triviaHub")]
    public class TriviaHub : Hub
    {

        public override Task OnConnected()
        {
            var token = Context.Headers["User"];
            if (token == null)
            {
                return Task.FromResult(0);
            }

            if (WikiTriviaHandler.connectedUsers.Contains(token))
            {
                return Task.FromResult(0);
            }
            WikiTriviaHandler.connectedUsers.Add(token);
            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var token = Context.Headers["User"];
            if (token == null)
            {
                return Task.FromResult(0);
            }
            WikiTriviaHandler.connectedUsers.Remove(token);
            return base.OnDisconnected(stopCalled);
        }
    }
}
