using System;

namespace WikiTrivia.TriviaCore.Models
{
    public class TriviaQuestionDto
    {
        public string QuestionText { get; set; }
        public string Answer { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
