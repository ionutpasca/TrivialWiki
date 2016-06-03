using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseManager.DatabaseModels
{
    public class Topic
    {
        public Topic()
        {
            this.RelatedTopics = new HashSet<RelatedTopic>();
            this.Questions = new HashSet<QuestionSet>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<RelatedTopic> RelatedTopics { get; set; }
        public ICollection<QuestionSet> Questions { get; set; }

    }
}