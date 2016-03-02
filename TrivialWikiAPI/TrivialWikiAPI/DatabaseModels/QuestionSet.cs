using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrivialWikiAPI.DatabaseModels
{
    public class QuestionSet
    {
        public QuestionSet()
        {
            this.Topics = new HashSet<Topic>();
        }

        [Key]
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public ICollection<string> FillerAnswers { get; set; }
        public int UsageCount { get; set; }
        public int CorrectAnswerCount { get; set; }
        public ICollection<Topic> Topics { get; set; }
    }   
}