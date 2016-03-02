using System;
using System.ComponentModel.DataAnnotations;

namespace TrivialWikiAPI.DatabaseModels
{
    public class ChatRoom
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public string MessageText { get; set; }
        public DateTime Timestamp { get; set; }
    }
}