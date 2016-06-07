using System;

namespace DatabaseManager.Trivia
{
    public struct QuestionHint
    {
        public string FirstHint { get; set; }
        public string SecondHint { get; set; }
        public string ThirdHint { get; set; }
    }

    public class TriviaQuestionDto
    {
        public string QuestionText { get; set; }
        public string Answer { get; set; }
        public DateTime Timestamp { get; set; }
        public string FirstHint { get; set; }
        public QuestionHint Hint { get; set; }
    }
}
