using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TrivialWikiAPI.DatabaseModels;

namespace TrivialWikiAPI.Chat
{
    public sealed class MessagesManages
    {
        public async Task<List<MessageDto>> GetMessagesBatch(int messagesToSkip)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var messages = await databaseContext.Messages
                    .OrderByDescending(m => m.Timestamp)
                    .Skip(messagesToSkip)
                    .Take(25)
                    .Select(msg => new MessageDto()
                    {
                        UserName = msg.Sender.UserName,
                        Message = msg.MessageText
                    })
                    .ToListAsync();
                return messages;
            }
        }

        public async Task AddNewMessageToDatabase(MessageDto message)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var sender = databaseContext.Users.Single(u => u.UserName == message.UserName);
                var dbMessage = new Message()
                {
                    MessageText = message.Message,
                    Sender = sender,
                    Timestamp = DateTime.Now
                };
                databaseContext.Messages.Add(dbMessage);

                await CleanDatabase(databaseContext);
                await databaseContext.SaveChangesAsync();
            }
        }

        private async Task CleanDatabase(DatabaseContext dbContext)
        {
            var messagesCount = await dbContext.Messages.CountAsync();
            if (messagesCount > 100)
            {
                var messageToDelete = await dbContext.Messages.FirstAsync();
                dbContext.Messages.Remove(messageToDelete);
            }
        }
    }
}