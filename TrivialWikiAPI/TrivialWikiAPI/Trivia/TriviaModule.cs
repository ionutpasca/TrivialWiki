using DatabaseManager.Trivia;
using DatabaseManager.UserManagement;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using System.Collections.Generic;
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
            Get["/getTableTopic/{tableName}"] = param => GetTableTopic(param.tableName);
            Get["/getCurrentQuestion/{tableName}"] = param => GetCurrentQuestion(param.tableName);
            Get["/getLastQuestions", true] = async (param, p) => await GetLastQuestions();
            Get["/getUsersFromTable/{tableName}", true] = async (param, p) => await GetUsersFromTable(param.tableName);
            Get["/getNextQuestion", true] = async (param, p) => await GetNextQuestion();

            Post["/addResponse", true] = async (param, p) => await AddResponseToDatabase();
            Post["/createTable/{tableName}/{topic}", true] = async (param, p) => await CreateNewTable(param.tableName, param.topic);
        }

        private async Task<Response> CreateNewTable(string tableName, string topic)
        {
            var newTable = new TriviaTable { TableName = tableName, Topic = topic };
            TriviaUserHandler.TriviaTables.Add(newTable);
            await triviaCore.InitializeCurrentTriviaQuestion(newTable, topic);
            return HttpStatusCode.OK;
        }

        private async Task<Response> GetNextQuestion()
        {
            var currentUser = Context.CurrentUser;
            var table = triviaCore.GetTableForUser(currentUser.UserName);
            await triviaCore.BroadcastQuestion(table.TableName);
            return HttpStatusCode.OK;
        }

        private Response GetCurrentQuestion(string tableName)
        {
            var table = TriviaUserHandler.TriviaTables.Single(t => t.TableName == tableName);
            var questionMessage = table.CurrentTriviaQuestion.QuestionText;
            var questionToSend = new TriviaMessageDto
            {
                Sender = "TriviaBot",
                MessageText = questionMessage
            };
            return Response.AsJson(questionToSend);
        }

        private Response GetTableTopic(string tableName)
        {
            var table = TriviaUserHandler.TriviaTables.Single(t => t.TableName == tableName);
            return Response.AsJson(table.Topic);
        }

        private async Task<Response> GetUsersFromTable(string tableName)
        {
            var table = TriviaUserHandler.TriviaTables.Single(t => t.TableName == tableName);
            var users = table.ConnectedUsers.Select(u => u.Username)
                .ToList();
            var response = new List<UserWithPoints>();
            foreach (var user in users.ToList())
            {
                var userPoints = await userManager.GetUserPoints(user);
                response.Add(new UserWithPoints { Username = user, Points = userPoints });
            }
            return Response.AsJson(response);
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
            var currentUser = Context.CurrentUser;
            var users = WikiTriviaHandler.connectedUsers.Select(d => d.Username)
                .Where(d => d != currentUser.UserName)
                .ToList();
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
            await triviaManager.AddTriviaMessageToDatabase(sentResponse);
            triviaCore.BroadcastMessage(sentResponse, table.TableName);

            if (sentResponse.MessageText.ToLower() == currentQuestion.Answer.ToLower())
            {
                var pointsToAdd = GetAwardedPoints(table);
                await userManager.AddPointsToUser(sentResponse.Sender, pointsToAdd);

                await SendCorrectAnswerResponse(sentResponse.Sender, pointsToAdd, table.TableName);

                triviaCore.BroadcastPointsReceived(currentUser.UserName, table.TableName, pointsToAdd);
                await triviaCore.BroadcastQuestion(table.TableName);
                return HttpStatusCode.OK;
            }

            if (sentResponse.MessageText.ToLower() == "hint" && table.HintCommandsCount < 3)
            {
                table.HintCommandsCount += 1;
                var hintNumber = table.HintCommandsCount;
                await triviaCore.BroadcastHint(hintNumber, table.TableName);
                return HttpStatusCode.OK;
            }

            return HttpStatusCode.OK;
        }

        private async Task SendCorrectAnswerResponse(string user, int receivedPoints, string tableName)
        {
            var response = new TriviaMessageDto { Sender = "TriviaBot", MessageText = $"{user} gave the correct answer and received {receivedPoints} points!" };
            await triviaManager.AddTriviaMessageToDatabase(response);
            triviaCore.BroadcastCorrectAnswer(response, tableName);
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