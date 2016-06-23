using DatabaseManager.Trivia;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiTrivia.TriviaCore.Hubs;
using WikiTrivia.TriviaCore.Models;
using WikiTrivia.Utilities;
// ReSharper disable PossibleNullReferenceException

namespace WikiTrivia.TriviaCore
{
    public sealed class TriviaCore
    {
        private static readonly TriviaManager triviaManager = new TriviaManager();


        public async Task InitializeTrivia()
        {

            CreateMainTriviaTable();
            var tables = TriviaUserHandler.TriviaTables;
            foreach (var table in tables)
            {
                await InitializeCurrentTriviaQuestion(table, table.Topic);
            }
        }

        private static void CreateMainTriviaTable()
        {
            var mainTable = new TriviaTable
            {
                TableName = "Public Table",
                Topic = "Superman"
            };
            TriviaUserHandler.TriviaTables.Add(mainTable);
        }

        public async Task BroadcastQuestion(string tableName, string topic)
        {
            var table = TriviaUserHandler.TriviaTables.SingleOrDefault(t => t.TableName == tableName);
            await InitializeCurrentTriviaQuestion(table, topic);

            var questionMessage = table.CurrentTriviaQuestion.QuestionText;
            var questionToSend = new TriviaMessageDto
            {
                Sender = "TriviaBot",
                MessageText = questionMessage
            };

            triviaManager.AddTriviaMessageToDatabase(questionToSend);

            var context = GlobalHost.ConnectionManager.GetHubContext<TriviaHub>();
            var clients = GetConnectedUsersForTable(tableName);
            context.Clients.Users(clients).AddMessage(questionToSend);
        }

        public void BroadcastMessage(TriviaMessageDto message, string tableName)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TriviaHub>();
            var clients = GetConnectedUsersForTable(tableName);

            context.Clients.Users(clients);
        }

        public void BroadcastHint(int hintNumber, string tableName)
        {
            var hint = GetHintByNumber(hintNumber, tableName);
            if (hint == string.Empty)
            {
                return;
            }
            var questionToSend = new TriviaMessageDto { Sender = "Trivia Bot", MessageText = $"Hint : {hint}" };
            triviaManager.AddTriviaMessageToDatabase(questionToSend);

            var context = GlobalHost.ConnectionManager.GetHubContext<TriviaHub>();
            var clients = GetConnectedUsersForTable(tableName);

            context.Clients.Users(clients).AddMessage(questionToSend);
        }

        private static async Task InitializeCurrentTriviaQuestion(TriviaTable table, string topic)
        {
            table.CurrentTriviaQuestion = await triviaManager.GetNewQuestion(topic);
            table.HintCommandsCount = 0;
            InitializeHintsForCurrentQuestion(table);
        }

        private static void InitializeHintsForCurrentQuestion(TriviaTable table)
        {
            var answer = table.CurrentTriviaQuestion.Answer;

            var hint1 = TriviaHintGenerator.GenerateHintForQuestion(answer);
            var hint2 = TriviaHintGenerator.GenerateHintForQuestion(answer, hint1);
            var hint3 = TriviaHintGenerator.GenerateHintForQuestion(answer, hint2);

            table.CurrentTriviaQuestion.Hint = new QuestionHint()
            {
                FirstHint = hint1,
                SecondHint = hint2,
                ThirdHint = hint3
            };
        }

        private static string GetHintByNumber(int hintNumber, string tableName)
        {
            var table = TriviaUserHandler.TriviaTables.Single(t => t.TableName == tableName);
            switch (hintNumber)
            {
                case 1:
                    return table.CurrentTriviaQuestion.Hint.FirstHint;
                case 2:
                    return table.CurrentTriviaQuestion.Hint.SecondHint;
                case 3:
                    return table.CurrentTriviaQuestion.Hint.ThirdHint;
                default:
                    return string.Empty;
            }
        }

        private static List<string> GetConnectedUsersForTable(string tableName)
        {
            return TriviaUserHandler.TriviaTables.
               FirstOrDefault(t => t.TableName == tableName)
               .ConnectedUsers.Select(u => u.ConnectionId)
               .ToList();
        }

        public TriviaTable GetTableForUser(string username)
        {
            return TriviaUserHandler.TriviaTables
                .Single(t => t.ConnectedUsers
                    .Count(c => c.Username == username) != 0);
        }

        public void SendUserCurrentQuestion(string connectionId, string tableName)
        {
            var table = TriviaUserHandler.TriviaTables.Single(t => t.TableName == tableName);
            var context = GlobalHost.ConnectionManager.GetHubContext<TriviaHub>();

            var questionMessage = table.CurrentTriviaQuestion.QuestionText;
            var questionToSend = new TriviaMessageDto
            {
                Sender = "TriviaBot",
                MessageText = questionMessage
            };
            context.Clients.Client(connectionId).AddMessage(questionToSend);
        }
    }
}
