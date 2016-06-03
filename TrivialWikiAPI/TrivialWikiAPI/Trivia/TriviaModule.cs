using Microsoft.AspNet.SignalR;
using Nancy;
using Nancy.ModelBinding;
using System.Threading.Tasks;

namespace TrivialWikiAPI.Trivia
{
    public class TriviaModule : NancyModule
    {
        private readonly TriviaManager triviaManager = new TriviaManager();
        public TriviaModule()
        {
            Get["/getLastQuestions", true] = async (param, p) => await GetLastQuestions();

            Post["/addResponse"] = _ => AddResponseToDatabase();
        }

        private async Task<Response> AddResponseToDatabase()
        {
            var sentResponse = this.Bind<TriviaMessageDto>();
            if (sentResponse?.Sender == null)
            {
                return HttpStatusCode.BadRequest;
            }
            await triviaManager.AddTriviaMessageToDatabase(sentResponse);
            var context = GlobalHost.ConnectionManager.GetHubContext<TriviaHub>();
            context.Clients.All.AddMessage(sentResponse);
            return HttpStatusCode.OK;
        }

        private async Task<Response> GetLastQuestions()
        {
            var messages = await triviaManager.GetLastTriviaQuestions();
            return this.Response.AsJson(messages);
        }
    }
}