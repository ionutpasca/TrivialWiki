using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrivialWikiAPI.DatabaseModels
{


    public class User
    {
        public User()
        {
            this.Achievements = new HashSet<Achievement>();
            this.Roles = new HashSet<Role>();
        }

        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<Role> Roles { get; set; }
        public Sex Sex { get; set; }

        public ICollection<Achievement> Achievements { get; set; }
        public byte[] Avatar { get; set; }
        public IEnumerable<string> Claims { get; set; }
        public int Points { get; set; }
        public int Rank { get; set; }

        public string SecurityToken { get; set; }
        public Statistics Statistic { get; set; }
    }
    public enum Sex
    {
        Male,
        Female,
        Other
    }
}