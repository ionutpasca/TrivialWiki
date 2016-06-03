using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseManager.DatabaseModels
{
    public class RelatedTopic
    {
        public RelatedTopic()
        {
            this.Topics = new HashSet<Topic>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Topic> Topics { get; set; }
    }
}