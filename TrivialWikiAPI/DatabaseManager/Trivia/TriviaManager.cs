using DatabaseManager.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManager.Trivia
{
    public sealed class TriviaManager
    {
        public async Task<List<TriviaMessageDto>> GetLastTriviaQuestions()
        {
            using (var databaseContext = new DatabaseContext())
            {
                return await databaseContext.TriviaMessages
                    .OrderByDescending(u => u.Timestamp)
                    .Take(30)
                    .Select(u => new TriviaMessageDto()
                    {
                        Sender = u.Sender,
                        MessageText = u.MessageText
                    })
                    .ToListAsync();
            }
        }

        public async Task AddTriviaMessageToDatabase(TriviaMessageDto response)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var triviaMessage = new TriviaMessage
                {
                    Sender = response.Sender,
                    MessageText = response.MessageText,
                    Timestamp = DateTime.Now,
                };
                databaseContext.TriviaMessages.Add(triviaMessage);
                CleanDatabase(databaseContext);
                await databaseContext.SaveChangesAsync();
            }
        }

        private static void CleanDatabase(DatabaseContext databaseContext)
        {
            var messagesCount = databaseContext.TriviaMessages.Count();
            if (messagesCount > 50)
            {
                var messageToDelete = databaseContext.TriviaMessages.First();
                databaseContext.TriviaMessages.Remove(messageToDelete);
            }
        }

        public async Task<TriviaQuestionDto> GetNewQuestion(string topic)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var rand = new Random();
                var noOfQuestions = await databaseContext.QuestionSets.CountAsync(t => t.Topic.Name == topic);
                var questionsToSkip = rand.Next(noOfQuestions);


                var x = databaseContext.QuestionSets
                    .Where(t => t.Topic.Name == topic)
                    .OrderBy(u => u.Id)
                    .Skip(questionsToSkip)
                    .Take(1)
                    .Select(q => new TriviaQuestionDto
                    {
                        QuestionText = q.QuestionText,
                        Answer = q.CorrectAnswer,
                        Timestamp = DateTime.Now
                    }).First();

                return x;
            }
        }
    }
}
