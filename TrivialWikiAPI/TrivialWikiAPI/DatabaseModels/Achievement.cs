using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrivialWikiAPI.DatabaseModels
{
    public class Achievement
    {
        public Achievement()
        {
            this.Users = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Threshold { get; set; }
        public byte[] Icon { get; set; }
        public ICollection<User> Users { get; set; }
    }
}