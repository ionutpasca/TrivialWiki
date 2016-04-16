using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TrivialWikiAPI.DatabaseModels;
using TrivialWikiAPI.Utilities;

namespace TrivialWikiAPI.UserManagement
{
    public class UserManager
    {
        public async Task<List<User>> GetAllUsers()
        {
            using (var databaseContext = new DatabaseContext())
            {
                return await databaseContext.Users.ToListAsync();
            }
        }

        public async Task AddNewUserToDatabase(User user)
        {
            user.Points = 0;
            user.Password = Encrypt.GetMD5(user.Password);

            using (var databaseContext = new DatabaseContext())
            {
                user.Rank = await databaseContext.Users.CountAsync();

                var playerRole =await databaseContext.Roles.FirstAsync(r => r.Name == "Player");
                user.Roles.Add(playerRole);

                databaseContext.Users.Add(user);
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task ChangeUserPassword(User user)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var usr = await databaseContext.Users.SingleAsync(u => u.UserName == user.UserName);
                if (usr == null)
                {
                    return;
                }
                var password = Encrypt.GetMD5(user.Password);
                usr.Password = password;
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task RemoveUserFromDatabase(string userName)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var user = await databaseContext.Users.SingleAsync(u => u.UserName == userName);
                databaseContext.Users.Remove(user);
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task AddPointsToUser(string userName, int points)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var user = await databaseContext.Users.SingleAsync(u => u.UserName == userName);

                if (points > 0)
                {
                    await IncrementUserRank(points, databaseContext, user);
                }
                else
                {
                    await DecrementUserRank(points, databaseContext, user);
                }

                await databaseContext.SaveChangesAsync();
            }
        }
        private static async Task IncrementUserRank(int points, DatabaseContext databaseContext, User user)
        {
            var usersWithRankDepreciated = await databaseContext.Users
                .Where(u => u.Points > user.Points && u.Points < user.Points + points)
                .ToListAsync();
            if (usersWithRankDepreciated.Count == 0)
            {
                return;
            }
            usersWithRankDepreciated.ForEach(u => u.Rank = u.Rank - 1);

            user.Points = user.Points + points;
            user.Rank = user.Rank - usersWithRankDepreciated.Count;
        }

        private static async Task DecrementUserRank(int points, DatabaseContext databaseContext, User user)
        {
            var usersToIncreaseRank = await databaseContext.Users
                .Where(u => u.Points < user.Points && u.Points > user.Points + points)
                .ToListAsync();
            if (usersToIncreaseRank.Count == 0)
            {
                return;
            }
            usersToIncreaseRank.ForEach(u => u.Rank = u.Rank + 1);

            user.Points = user.Points + points;
            user.Rank = user.Rank - usersToIncreaseRank.Count;
        }

        public async Task ChangeUserRole(string userName, int roleId)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var user = await databaseContext.Users.SingleAsync(u => u.UserName == userName);
                var role = await databaseContext.Roles.SingleAsync(r => r.Id == roleId);
                user.Roles.Clear();
                user.Roles.Add(role);
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            using (var databaseContext = new DatabaseContext())
            {
                return await databaseContext.Users.AnyAsync(u => u.UserName == userName);
            }
        }

        public async Task<bool> RoleExists(int roleId)
        {
            using (var databaseContext = new DatabaseContext())
            {
                return await databaseContext.Roles.AnyAsync(r => r.Id == roleId);
            }
        }

    }
}