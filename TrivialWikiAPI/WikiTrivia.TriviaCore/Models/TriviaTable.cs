using DatabaseManager.Trivia;
using System.Collections.Generic;

namespace WikiTrivia.TriviaCore.Models
{
    public class TriviaTable
    {
        public TriviaTable()
        {
            ConnectedUsers = new List<ConnectedUser>();
        }

        public string TableName { get; set; }
        public List<ConnectedUser> ConnectedUsers { get; set; }
        public string Topic { get; set; }
        public TriviaQuestionDto CurrentTriviaQuestion { get; set; }
        public int HintCommandsCount { get; set; }
    }
}
