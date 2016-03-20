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

        public void ChangeUserPassword(User user)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var usr = databaseContext.Users.Single(u => u.UserName == user.UserName);
                if (usr == null)
                {
                    return;
                }
                var password = Encrypt.GetMD5(user.Password);
                usr.Password = password;
                databaseContext.SaveChanges();
            }
        }

        public void RemoveUserFromDatabase(string userName)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var user = databaseContext.Users.Single(u => u.UserName == userName);
                databaseContext.Users.Remove(user);
                databaseContext.SaveChanges();
            }
        }

        public void AddPointsToUser(string userName, int points)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var user = databaseContext.Users.Single(u => u.UserName == userName);

                if (points > 0)
                {
                    IncrementUserRank(points, databaseContext, user);
                }
                else
                {
                    DecrementUserRank(points, databaseContext, user);
                }

                databaseContext.SaveChanges();
            }
        }
        private static void IncrementUserRank(int points, DatabaseContext databaseContext, User user)
        {
            var usersWithRankDepreciated = databaseContext.Users
                .Where(u => u.Points > user.Points && u.Points < user.Points + points)
                .ToList();
            if (usersWithRankDepreciated.Count == 0)
            {
                return;
            }
            usersWithRankDepreciated.ForEach(u => u.Rank = u.Rank - 1);

            user.Points = user.Points + points;
            user.Rank = user.Rank - usersWithRankDepreciated.Count;
        }

        private static void DecrementUserRank(int points, DatabaseContext databaseContext, User user)
        {
            var usersToIncreaseRank = databaseContext.Users
                .Where(u => u.Points < user.Points && u.Points > user.Points + points)
                .ToList();
            if (usersToIncreaseRank.Count == 0)
            {
                return;
            }
            usersToIncreaseRank.ForEach(u => u.Rank = u.Rank + 1);

            user.Points = user.Points + points;
            user.Rank = user.Rank - usersToIncreaseRank.Count;
        }

        public void ChangeUserRole(string userName, int roleId)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var user = databaseContext.Users.Single(u => u.UserName == userName);
                var role = databaseContext.Roles.Single(r => r.Id == roleId);
                user.Roles.Clear();
                user.Roles.Add(role);
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

        public bool RoleExists(int roleId)
        {
            using (var databaseContext = new DatabaseContext())
            {
                return databaseContext.Roles.Any(r => r.Id == roleId);
            }
        }

    }
}