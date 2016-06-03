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
                    .Skip(messagesToSkip * 40)
                    .Take(40)
                    .Select(msg => new MessageDto()
                    {
                        UserName = msg.Sender.UserName,
                        Message = msg.MessageText
                    })
                    .ToListAsync();
                return messages;
            }
        }

        public void AddNewMessageToDatabase(MessageDto message)
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

                CleanDatabase(databaseContext);
                databaseContext.SaveChanges();
            }
        }

        private static void CleanDatabase(DatabaseContext dbContext)
        {
            var messagesCount = dbContext.Messages.Count();
            if (messagesCount > 250)
            {
                var messageToDelete = dbContext.Messages.First();
                dbContext.Messages.Remove(messageToDelete);
            }
        }
    }
}