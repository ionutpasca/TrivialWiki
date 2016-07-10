using DatabaseManager.UserManagement;
using DatabaseManager.UserManagement.Notifications;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiTrivia.Core
{
    public class NotificationsCore
    {
        private readonly NotificationsManager notificationsManager = new NotificationsManager();
        private readonly UserManager userManager = new UserManager();

        public void SendUserDisconnectedNotification(string userName)
        {
            //var friends = userManager.GetAllFriendsForUser(userName);
            //var context = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            //var usersToSendNotification = WikiTriviaHandler.connectedUsers
            //        .Where(u => friends.Contains(userName))
            //        .Select(u => u.ConnectionId)
            //        .ToList();
            //context.Clients.Clients(usersToSendNotification).userDisconnected(userName);
        }

        public List<string> GetOnlineUsers()
        {
            return WikiTriviaHandler.connectedUsers.Select(u => u.Username).ToList();
        }
        public async Task SendNewFriendNotification(string requester, string username)
        {
            var notification = new NotificationDto
            {
                NotificationDate = DateTime.Now,
                NotificationText = $"You are now friends with '{requester}'.",
                Sender = "WikiTrivia"
            };
            await NotifyUser(notification, username);
        }
        public async Task SendTopicProcessedNotification(string username, string topicName)
        {
            var notification = new NotificationDto
            {
                NotificationDate = DateTime.Now,
                NotificationText = $"Topic '{topicName}' has been processed.",
                Sender = "WikiTrivia"
            };
            await NotifyUser(notification, username);
        }

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
