using DatabaseManager.DatabaseModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManager.UserManagement.Notifications
{
    public sealed class NotificationsManager
    {
        public async Task<NotificationDto> AddNotificationToUser(NotificationDto notification, string username)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var not = new Notification
                {
                    NotificationText = notification.NotificationText,
                    Sender = notification.Sender,
                    NotificationDate = notification.NotificationDate,
                    Seen = false
                };
                var user = await databaseContext.Users.SingleOrDefaultAsync(u => u.UserName == username);
                if (user != null)
                {
                    not.User = user;
                    databaseContext.Notifications.Add(not);

                    await ClearNotifications(user.UserName, databaseContext);
                }

                await databaseContext.SaveChangesAsync();

                databaseContext.Entry(not).GetDatabaseValues();
                return new NotificationDto
                {
                    Id = not.Id,
                    NotificationText = not.NotificationText,
                    NotificationDate = not.NotificationDate,
                    Sender = not.Sender,
                    Seen = not.Seen
                };
            }
        }

        private static async Task ClearNotifications(string username, DatabaseContext dbContext)
        {
            var nuberOfNot = await dbContext.Notifications
                                .Where(n => n.User.UserName == username)
                                .CountAsync();
            if (nuberOfNot > 20)
            {
                var oldestNot = dbContext.Notifications.Where(n => n.User.UserName == username)
                            .OrderBy(n => n.NotificationDate)
                            .FirstOrDefault();
                if (oldestNot != null)
                {
                    dbContext.Notifications.Remove(oldestNot);
                }
            }
        }

        public async Task MarkNotificationsAsSeen(string username, int notificationId)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var user = await databaseContext.Users.Include("Notifications")
                                .SingleOrDefaultAsync(u => u.UserName == username);

                var notification = user.Notifications.Single(n => n.Id == notificationId);
                notification.Seen = true;
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotifications(string username)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var user = await databaseContext.Users.Include("Notifications")
                            .SingleOrDefaultAsync(u => u.UserName == username);
                return user.Notifications.Select(n => new NotificationDto
                {
                    Id = n.Id,
                    NotificationDate = n.NotificationDate,
                    NotificationText = n.NotificationText,
                    Sender = n.Sender,
                    Seen = n.Seen
                })
                .ToList();
            }
        }
    }
}
