using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseManager.DatabaseModels
{
    public class QuestionSet
    {

        [Key]
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public ICollection<string> FillerAnswers { get; set; }
        public int UsageCount { get; set; }
        public int CorrectAnswerCount { get; set; }
        public Topic Topic { get; set; }
        public bool IsValidated { get; set; }
    }
}