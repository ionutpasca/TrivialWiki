using DatabaseManager.Trivia;
using Microsoft.AspNet.SignalR;
using Nancy;
using Nancy.ModelBinding;
using System.Threading.Tasks;
using WikiTrivia.TriviaCore;
using WikiTrivia.TriviaCore.Hubs;

namespace TrivialWikiAPI.Trivia
{
    public class TriviaModule : NancyModule
    {
        private readonly TriviaManager triviaManager = new TriviaManager();
        private readonly TriviaCore triviaCore = new TriviaCore();
        public TriviaModule()
        {
            Get["/getLastQuestions", true] = async (param, p) => await GetLastQuestions();

            Post["/addResponse"] = _ => AddResponseToDatabase();
        }

        private Response AddResponseToDatabase()
        {
            var currentQuestion = CurrentTriviaQuestion.currentTriviaQuestion;
            var sentResponse = this.Bind<TriviaMessageDto>();
            if (sentResponse?.Sender == null)
            {
                return HttpStatusCode.BadRequest;
            }
            triviaManager.AddTriviaMessageToDatabase(sentResponse);

            if (sentResponse.MessageText == currentQuestion.Answer)
            {
                SendCorrectAnswerResponse();
                triviaCore.BroadcastQuestion();
                return HttpStatusCode.OK;
            }

            var context = GlobalHost.ConnectionManager.GetHubContext<TriviaHub>();
            context.Clients.All.AddMessage(sentResponse);

            if (sentResponse.MessageText.ToLower() == "hint")
            {
                CurrentTriviaQuestion.hintCommandsCount += 1;
                var hintNumber = CurrentTriviaQuestion.hintCommandsCount >= 3 ? 4 : CurrentTriviaQuestion.hintCommandsCount;
                triviaCore.BroadcastHint(hintNumber);
                return HttpStatusCode.OK;
            }

            CurrentTriviaQuestion.numberOfWrongAnswers += 1;
            if (CurrentTriviaQuestion.numberOfWrongAnswers % 3 == 0)
            {
                var hintNumber = GetHintNumber();
                triviaCore.BroadcastHint(hintNumber);
            }

            return HttpStatusCode.OK;
        }

        private static void SendCorrectAnswerResponse()
        {
            var response = new TriviaMessageDto() { Sender = "TriviaBot", MessageText = "Congratulation!" };
            var context = GlobalHost.ConnectionManager.GetHubContext<TriviaHub>();
            context.Clients.All.AddMessage(response);
        }

        private static int GetHintNumber()
        {
            var numberOfWrongAns = CurrentTriviaQuestion.numberOfWrongAnswers;
            if (numberOfWrongAns > 9)
            {
                return 4;
            }
            if (numberOfWrongAns > 6)
            {
                return 3;
            }
            return numberOfWrongAns > 3 ? 2 : 1;
        }

        private async Task<Response> GetLastQuestions()
        {
            var messages = await triviaManager.GetLastTriviaQuestions();
            return this.Response.AsJson(messages);
        }
    }
}