using System;

namespace DatabaseManager.UserManagement.Notifications
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string NotificationText { get; set; }
        public DateTime NotificationDate { get; set; }
        public bool Seen { get; set; }
    }
}
