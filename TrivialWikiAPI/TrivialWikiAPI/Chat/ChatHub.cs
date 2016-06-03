using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrivialWikiAPI.Chat
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        private static List<string> users = new List<string>();

        public void Send(List<string> users)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.someoneConnected(users);
        }

        public void AddMessage(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }

        public void Heartbeat()
        {
            Clients.All.heartbeat();
        }

        public override Task OnConnected()
        {
            var clientId = GetClientId();

            if (users.IndexOf(clientId) == -1)
            {
                users.Add(clientId);
            }
            Send(users);

            Clients.All.someoneConnected();
            return (base.OnConnected());
        }

        private string GetClientId()
        {
            var clientId = "";
            if (Context.QueryString["clientId"] != null)
            {
                clientId = this.Context.QueryString["clientId"];
            }

            if (string.IsNullOrEmpty(clientId.Trim()))
            {
                clientId = Context.ConnectionId;
            }

            return clientId;
        }
    }
}