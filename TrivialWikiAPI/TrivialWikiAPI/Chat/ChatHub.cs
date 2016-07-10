using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrivialWikiAPI.Chat
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
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
            return (base.OnConnected());
        }
    }
}