using System.ComponentModel.DataAnnotations;

namespace DatabaseManager.DatabaseModels
{
    public class ProposedTopic
    {
        [Key]
        public int Id { get; set; }
        public string TopicName { get; set; }
        public User User { get; set; }
    }
}
