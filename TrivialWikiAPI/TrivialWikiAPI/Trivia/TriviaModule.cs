using DatabaseManager.Trivia;
using DatabaseManager.UserManagement;
using Nancy;
using Nancy.ModelBinding;
using System.Threading.Tasks;
using WikiTrivia.TriviaCore;

namespace TrivialWikiAPI.Trivia
{
    public class TriviaModule : NancyModule
    {
        private readonly TriviaManager triviaManager = new TriviaManager();
        private readonly TriviaCore triviaCore = new TriviaCore();
        private readonly UserManager userManager = new UserManager();

        public TriviaModule()
        {
            Get["/getLastQuestions", true] = async (param, p) => await GetLastQuestions();

            Post["/addResponse", true] = async (param, p) => await AddResponseToDatabase();
        }

        private async Task<Response> AddResponseToDatabase()
        {
            var currentQuestion = CurrentTriviaQuestion.currentTriviaQuestion;
            var sentResponse = this.Bind<TriviaMessageDto>();
            if (sentResponse?.Sender == null)
            {
                return HttpStatusCode.BadRequest;
            }
            triviaManager.AddTriviaMessageToDatabase(sentResponse);
            triviaCore.BroadcastMessage(sentResponse);

            if (sentResponse.MessageText.ToLower() == currentQuestion.Answer.ToLower())
            {
                var pointsToAdd = GetAwardedPoints();
                await userManager.AddPointsToUser(sentResponse.Sender, pointsToAdd);

                SendCorrectAnswerResponse(sentResponse.Sender, pointsToAdd);
                triviaCore.BroadcastQuestion();
                return HttpStatusCode.OK;
            }

            if (sentResponse.MessageText.ToLower() == "hint" && CurrentTriviaQuestion.hintCommandsCount < 3)
            {
                CurrentTriviaQuestion.hintCommandsCount += 1;
                var hintNumber = CurrentTriviaQuestion.hintCommandsCount;
                triviaCore.BroadcastHint(hintNumber);
                return HttpStatusCode.OK;
            }

            return HttpStatusCode.OK;
        }

        private void SendCorrectAnswerResponse(string user, int receivedPoints)
        {
            var response = new TriviaMessageDto() { Sender = "TriviaBot", MessageText = $"{user} gave the correct answer and received {receivedPoints} points!" };
            triviaManager.AddTriviaMessageToDatabase(response);
            triviaCore.BroadcastMessage(response);
        }

        private async Task<Response> GetLastQuestions()
        {
            var messages = await triviaManager.GetLastTriviaQuestions();
            return this.Response.AsJson(messages);
        }

        private static int GetAwardedPoints()
        {
            var hintsGiven = CurrentTriviaQuestion.hintCommandsCount;
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