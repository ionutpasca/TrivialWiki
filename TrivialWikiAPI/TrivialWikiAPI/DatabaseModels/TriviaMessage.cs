using System;
using System.ComponentModel.DataAnnotations;

namespace TrivialWikiAPI.DatabaseModels
{
    public class TriviaMessage
    {
        [Key]
        public int Id { get; set; }

        public string Sender { get; set; }
        public string MessageText { get; set; }
        public DateTime Timestamp { get; set; }
    }
}