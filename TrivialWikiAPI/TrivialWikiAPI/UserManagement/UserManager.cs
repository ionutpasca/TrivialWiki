using System.Collections.Generic;
using System.Linq;
using TrivialWikiAPI.DatabaseModels;
using TrivialWikiAPI.Utilities;

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

        public void AddNewUserToDatabase(User user)
        {
            user.Points = 0;
            user.Password = Encrypt.GetMD5(user.Password);

            using (var databaseContext = new DatabaseContext())
            {
                user.Rank = databaseContext.Users.Count();

                var playerRole = databaseContext.Roles.FirstOrDefault(r => r.Name == "Player");
                user.Roles.Add(playerRole);

                databaseContext.Users.Add(user);
                databaseContext.SaveChanges();
            }
        }

        public bool UserExists(string userName)
        {
            using (var databaseContext = new DatabaseContext())
            {
                return databaseContext.Users.Any(u => u.UserName == userName);
            }
        }
    }
}