using DatabaseManager.UserManagement.Notifications;
using Microsoft.AspNet.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WikiTrivia.Core
{
    public class NotificationsCore
    {
        private readonly NotificationsManager notificationsManager = new NotificationsManager();

        public async Task SendProposedTopicNotification(string username, string topicName)
        {
            var notification = new NotificationDto
            {
                NotificationDate = DateTime.Now,
                NotificationText = $"You will be noticed after '{topicName}' topic is processed",
                Sender = "WikiTrivia"
            };
            await NotifyUser(notification, username);
        }

        public async Task NotifyUser(NotificationDto notification, string username)
        {
            var not = await notificationsManager.AddNotificationToUser(notification, username);
            var user = WikiTriviaHandler.connectedUsers.SingleOrDefault(u => u.Username == username);
            if (user == null)
            {
                return;
            }
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            context.Clients.Client(user.ConnectionId).notify(not);
        }
    }
}
