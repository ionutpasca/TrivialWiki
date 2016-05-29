using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Threading.Tasks;

namespace TrivialWikiAPI.Chat
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        public void AddMessage(string name, string message)
        {
            Console.WriteLine("Hub AddMessage {0} {1}\n", name, message);
            Clients.All.addMessage(name, message);
        }

        public void Heartbeat()
        {
            Console.WriteLine("Hub Heartbeat\n");
            Clients.All.heartbeat();
        }

        public override Task OnConnected()
        {
            Console.WriteLine("Hub OnConnected {0}\n", Context.ConnectionId);
            return (base.OnConnected());
        }
    }
}