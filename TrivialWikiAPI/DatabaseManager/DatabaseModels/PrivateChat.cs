using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseManager.DatabaseModels
{
    public class PrivateChat
    {
        [Key]
        public int Id { get; set; }
        public User FirstUser { get; set; }
        public User SecondUser { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}