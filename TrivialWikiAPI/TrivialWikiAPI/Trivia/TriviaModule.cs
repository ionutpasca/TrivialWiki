using DatabaseManager.Trivia;
using DatabaseManager.UserManagement;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using System.Linq;
using System.Threading.Tasks;
using WikiTrivia.Core;
using WikiTrivia.TriviaCore;
using WikiTrivia.TriviaCore.Hubs;
using WikiTrivia.TriviaCore.Models;

namespace TrivialWikiAPI.Trivia
{
    public class TriviaModule : NancyModule
    {
        private readonly TriviaManager triviaManager = new TriviaManager();
        private readonly TriviaCore triviaCore = new TriviaCore();
        private readonly UserManager userManager = new UserManager();

        public TriviaModule()
        {
            this.RequiresAuthentication();

            Get["/getOnlineUsers"] = param => GetOnlineUsers();
            Get["/triviaTables"] = param => GetTriviaTables();
            Get["/getLastQuestions", true] = async (param, p) => await GetLastQuestions();

            Post["/addResponse", true] = async (param, p) => await AddResponseToDatabase();
        }

        private Response GetTriviaTables()
        {
            var tables = TriviaUserHandler.TriviaTables.Select(t => new TriviaTableDto
            {
                TableName = t.TableName,
                Topic = t.Topic
            }).ToList();
            return Response.AsJson(tables);
        }

        private Response GetOnlineUsers()
        {
            var users = WikiTriviaHandler.connectedUsers.Select(d => d.Username).ToList();
            return Response.AsJson(users);
        }

        private async Task<Response> AddResponseToDatabase()
        {
            var currentUser = Context.CurrentUser;

            var table = triviaCore.GetTableForUser(currentUser.UserName);

            var currentQuestion = table.CurrentTriviaQuestion;
            var sentResponse = this.Bind<TriviaMessageDto>();
            if (sentResponse?.Sender == null)
            {
                return HttpStatusCode.BadRequest;
            }
            triviaManager.AddTriviaMessageToDatabase(sentResponse);
            //triviaCore.BroadcastMessage(sentResponse);

            if (sentResponse.MessageText.ToLower() == currentQuestion.Answer.ToLower())
            {
                var pointsToAdd = GetAwardedPoints(table);
                await userManager.AddPointsToUser(sentResponse.Sender, pointsToAdd);

                SendCorrectAnswerResponse(sentResponse.Sender, pointsToAdd);
                //triviaCore.BroadcastQuestion();
                return HttpStatusCode.OK;
            }

            if (sentResponse.MessageText.ToLower() == "hint" && table.HintCommandsCount < 3)
            {
                table.HintCommandsCount += 1;
                var hintNumber = table.HintCommandsCount;
                //triviaCore.BroadcastHint(hintNumber);
                return HttpStatusCode.OK;
            }

            return HttpStatusCode.OK;
        }

        private void SendCorrectAnswerResponse(string user, int receivedPoints)
        {
            var response = new TriviaMessageDto { Sender = "TriviaBot", MessageText = $"{user} gave the correct answer and received {receivedPoints} points!" };
            triviaManager.AddTriviaMessageToDatabase(response);
            //triviaCore.BroadcastMessage(response);
        }

        private async Task<Response> GetLastQuestions()
        {
            var messages = await triviaManager.GetLastTriviaQuestions();
            return this.Response.AsJson(messages);
        }

        private static int GetAwardedPoints(TriviaTable triviaTable)
        {
            var hintsGiven = triviaTable.HintCommandsCount;
            switch (hintsGiven)
            {
                case 3:
                    return 5;
                case 2:
                    return 10;
                case 1:
                    return 15;
                case 0:
                    return 25;
                default:
                    return 0;
            }
        }
    }
}