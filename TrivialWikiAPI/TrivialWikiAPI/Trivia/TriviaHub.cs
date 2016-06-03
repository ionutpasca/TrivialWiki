using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Threading.Tasks;

namespace TrivialWikiAPI.Trivia
{
    [HubName("triviaHub")]
    public class TriviaHub : Hub
    {
        public void AddResponse(string name, string response)
        {


            Clients.All.addMessage(name, response);
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