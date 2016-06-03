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
                var triviaMessage = new TriviaMessage()
                {
                    Sender = response.Sender,
                    MessageText = response.MessageText,
                    Timestamp = DateTime.Now
                };
                databaseContext.TriviaMessages.Add(triviaMessage);
                await CleanDatabase(databaseContext);
                await databaseContext.SaveChangesAsync();
            }
        }

        private static async Task CleanDatabase(DatabaseContext databaseContext)
        {
            var messagesCount = await databaseContext.TriviaMessages.CountAsync();
            if (messagesCount > 50)
            {
                var messageToDelete = await databaseContext.TriviaMessages.FirstAsync();
                databaseContext.TriviaMessages.Remove(messageToDelete);
            }
        }
    }
}
