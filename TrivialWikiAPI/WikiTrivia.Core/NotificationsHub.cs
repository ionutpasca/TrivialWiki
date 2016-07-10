using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiTrivia.Core.Models;

namespace WikiTrivia.Core
{
    public static class WikiTriviaHandler
    {
        public static List<ConnectedUser> connectedUsers = new List<ConnectedUser>();
    }

    [HubName("notificationsHub")]
    public class NotificationsHub : Hub
    {
        private readonly NotificationsCore notificationsCore = new NotificationsCore();
        public void AddNotification(string username, string notification)
        {

        }
        public override Task OnConnected()
        {
            var token = Context.Headers["User"];
            if (token == null)
            {
                return Task.FromResult(0);
            }

            if (WikiTriviaHandler.connectedUsers.Any(u => u.Username == token))
            {
                return Task.FromResult(0);
            }
            var connectionId = Context.ConnectionId;
            WikiTriviaHandler.connectedUsers.Add(new ConnectedUser
            {
                Username = token,
                ConnectionId = connectionId
            });
            WikiTriviaHandler.connectedUsers = WikiTriviaHandler.connectedUsers
                                                    .GroupBy(x => x.Username)
                                                    .Select(x => x.First())
                                                    .ToList();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var token = Context.Headers["User"];
            var connectionId = Context.ConnectionId;
            if (token == null && connectionId == null)
            {
                return Task.FromResult(0);
            }
            var user = WikiTriviaHandler.connectedUsers.SingleOrDefault(u => u.Username == token || u.ConnectionId == connectionId);
            if (user != null)
            {
                notificationsCore.SendUserDisconnectedNotification(user.Username);
            }
            WikiTriviaHandler.connectedUsers.Remove(user);
            return base.OnDisconnected(stopCalled);
        }
    }
}
