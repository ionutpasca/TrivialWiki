using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiTrivia.TriviaCore.Models;

namespace WikiTrivia.TriviaCore.Hubs
{
    public static class TriviaUserHandler
    {
        public static List<TriviaTable> TriviaTables = new List<TriviaTable>();
    }

    [HubName("triviaHub")]
    public class TriviaHub : Hub
    {
        private readonly TriviaCore triviaCore = new TriviaCore();

        public override Task OnConnected()
        {
            var token = Context.Headers["User"];
            var tableName = Context.QueryString["tableName"];
            if (token == null)
            {
                return Task.FromResult(0);
            }

            var table = TriviaUserHandler.TriviaTables.SingleOrDefault(t => t.TableName == tableName);
            if (table == null)
            {
                return Task.FromResult(0);
            }

            var connectionId = Context.ConnectionId;
            if (triviaCore.TableHasUser(tableName, token))
            {
                return Task.FromResult(0);
            }
            table.ConnectedUsers.Add(new ConnectedUser
            {
                Username = token,
                ConnectionId = connectionId
            });

            triviaCore.BroadcastUserConnected(token, tableName);
            triviaCore.SendUserCurrentQuestion(connectionId, tableName);
            triviaCore.SendConnectedUsers(connectionId, tableName);

            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var tableName = Context.QueryString["tableName"];
            var table = TriviaUserHandler.TriviaTables.SingleOrDefault(t => t.TableName == tableName);

            var connectionId = Context.ConnectionId;
            if (table == null)
            {
                return base.OnDisconnected(stopCalled);
            }
            var user = table.ConnectedUsers.SingleOrDefault(u => u.ConnectionId == connectionId);
            if (user != null)
            {
                triviaCore.BroadcastUserDisconnected(user.Username, tableName);
                table.ConnectedUsers.Remove(user);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}
