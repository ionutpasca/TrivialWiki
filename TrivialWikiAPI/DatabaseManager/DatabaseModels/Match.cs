using System.ComponentModel.DataAnnotations;

namespace DatabaseManager.DatabaseModels
{
    public class Match
    {
        [Key]
        public int Id { get; set; }
        public User Loser { get; set; }
        public User Winner { get; set; }
        public int QuestionCount { get; set; }
        public int FirstUserCorrectAnswers { get; set; }
        public int SecondUserCorrectAnswers { get; set; }
        public Topic Topic { get; set; }
    }
}