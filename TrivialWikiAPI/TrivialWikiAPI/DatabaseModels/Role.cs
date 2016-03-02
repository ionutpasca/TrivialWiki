using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrivialWikiAPI.DatabaseModels
{
    public class Role
    {
        public Role()
        {
            this.Users = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}