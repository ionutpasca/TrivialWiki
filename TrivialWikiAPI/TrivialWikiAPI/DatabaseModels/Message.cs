using System;
using System.ComponentModel.DataAnnotations;

namespace TrivialWikiAPI.DatabaseModels
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public User Sender { get; set; }
        public string MessageText { get; set; }
        public DateTime Timestamp { get; set; }
    }
}