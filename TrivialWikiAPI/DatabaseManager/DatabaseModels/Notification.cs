using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseManager.DatabaseModels
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string Sender { get; set; }
        public User User { get; set; }
        public string NotificationText { get; set; }
        public DateTime NotificationDate { get; set; }
        public bool Seen { get; set; }
    }
}
