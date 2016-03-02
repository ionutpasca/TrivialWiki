using System.Collections.Generic;
using System.Linq;
using TrivialWikiAPI.DatabaseModels;

namespace TrivialWikiAPI.UserManagement
{
    public class UserManager
    {
        public List<User> GetAllUsers()
        {
            using (var databaseContext = new DatabaseContext())
            {
                return databaseContext.Users.ToList();
            }
        }
    }
}