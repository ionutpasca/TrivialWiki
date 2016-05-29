using Microsoft.AspNet.SignalR;
using Nancy;
using Nancy.ModelBinding;
using System.Threading.Tasks;

namespace TrivialWikiAPI.Chat
{
    public class ChatModule : NancyModule
    {
        private readonly MessagesManages messageManager = new MessagesManages();
        public ChatModule()
        {
            Get["/getMessages/{skipCount}", true] = async (param, p) => await GetMessageBatch(param.skipCount);

            Post["/addMessage"] = _ => AddMessageToDatabase();
        }

        private async Task<object> GetMessageBatch(object skipCount)
        {
            return await messageManager.GetMessagesBatch((int)skipCount);
        }

        private Response AddMessageToDatabase()
        {
            var sentMessage = this.Bind<MessageDto>();
            if (sentMessage?.UserName == null)
            {
                return HttpStatusCode.BadRequest;
            }
            messageManager.AddNewMessageToDatabase(sentMessage);

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.AddMessage(sentMessage);

            return HttpStatusCode.OK;
        }
    }
}