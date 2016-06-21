using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseManager.DatabaseModels
{
    public class Topic
    {
        public Topic(string topicName)
        {
            Name = topicName;
            RelatedTopics = new HashSet<RelatedTopic>();
            Questions = new HashSet<QuestionSet>();
        }

        public Topic()
        {
            RelatedTopics = new HashSet<RelatedTopic>();
            Questions = new HashSet<QuestionSet>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RelatedTopic> RelatedTopics { get; set; }
        public ICollection<QuestionSet> Questions { get; set; }
        public int Likes { get; set; }
        public bool IsActive { get; set; }
        public string ThumbnailPath { get; set; }
    }
}