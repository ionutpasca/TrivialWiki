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
            table.ConnectedUsers.Add(new ConnectedUser()
            {
                Username = token,
                ConnectionId = connectionId
            });

            triviaCore.SendUserCurrentQuestion(connectionId, tableName);

            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var token = Context.Headers["User"];
            if (token == null)
            {
                return Task.FromResult(0);
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}
