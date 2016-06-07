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

        public void AddTriviaMessageToDatabase(TriviaMessageDto response)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var triviaMessage = new TriviaMessage()
                {
                    Sender = response.Sender,
                    MessageText = response.MessageText,
                    Timestamp = DateTime.Now
                };
                databaseContext.TriviaMessages.Add(triviaMessage);
                CleanDatabase(databaseContext);
                databaseContext.SaveChanges();
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

        public TriviaQuestionDto GetNewQuestion()
        {
            using (var databaseContext = new DatabaseContext())
            {
                var rand = new Random();
                var noOfQuestions = databaseContext.QuestionSets.Count();
                var questionsToSkip = rand.Next(noOfQuestions);

                var x = databaseContext.QuestionSets
                    .OrderBy(u => u.Id)
                    .Skip(questionsToSkip)
                    .Take(1)
                    .Select(q => new TriviaQuestionDto()
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
