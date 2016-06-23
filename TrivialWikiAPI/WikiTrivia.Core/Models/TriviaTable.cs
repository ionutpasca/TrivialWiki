using System.Collections.Generic;

namespace WikiTrivia.Core.Models
{
    public class TriviaTable
    {
        public string TableName { get; set; }
        public List<ConnectedUser> ConnectedUsers { get; set; }
    }
}
