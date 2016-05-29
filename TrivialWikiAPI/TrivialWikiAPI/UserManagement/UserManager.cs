﻿using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TrivialWikiAPI.DatabaseModels;
using TrivialWikiAPI.Utilities;

namespace TrivialWikiAPI.UserManagement
{
    public class UserManager
    {
        public async Task<UserResponseWithCount> GetUsersBatch(string queryString, int pageNumber = 1)
        {
            var usersToSkip = (pageNumber - 1) * 10;
            using (var databaseContext = new DatabaseContext())
            {
                var users = await databaseContext.Users.Include("Role")
                    .Where(u => queryString == null || u.UserName.Contains(queryString))
                    .OrderBy(u => u.Rank)
                    .Skip(usersToSkip)
                    .Take(10)
                    .Select(u => new UserResponse
                    {
                        Username = u.UserName,
                        Email = u.Email,
                        Role = u.Role.Name,
                        Rank = u.Rank,
                        Points = u.Points
                    })
                    .ToListAsync();

                return new UserResponseWithCount()
                {
                    Users = users,
                    TotalNumberOfUsers = await GetNumberOfUsers(queryString)
                };
            }
        }

        private async Task<int> GetNumberOfUsers(string queryString)
        {
            using (var databaseContext = new DatabaseContext())
            {
                return await databaseContext.Users
                    .Where(u => u.UserName.Contains(queryString))
                    .CountAsync();
            }
        }

        public async Task AddNewUserToDatabase(User user)
        {
            user.Points = 0;
            user.Password = Encrypt.GetMD5(user.Password);

            using (var databaseContext = new DatabaseContext())
            {
                user.Rank = await databaseContext.Users.CountAsync() + 1;

                var playerRole = await databaseContext.Roles.FirstAsync(r => r.Name == "Player");
                user.Role = playerRole;

                databaseContext.Users.Add(user);
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task UpdateUser(UserResponse user)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var dbUser = await databaseContext.Users.Include("Role")
                    .SingleAsync(u => u.UserName == user.Username);
                if (user.Role != dbUser.Role.Name)
                {
                    var role = await databaseContext.Roles.SingleAsync(r => r.Name == user.Role);
                    dbUser.Role = null;
                    dbUser.Role = role;
                }
                if (dbUser.Email != user.Email)
                {
                    dbUser.Email = user.Email;
                }

                var pointsToAdd = user.Points - dbUser.Points;
                if (pointsToAdd != 0)
                {
                    await AddPointsToUser(dbUser.UserName, user.Points - dbUser.Points);
                }
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
                await UpdateUsersRankAfterOneUserIsRemoved();
                await databaseContext.SaveChangesAsync();
            }
        }

        private async Task UpdateUsersRankAfterOneUserIsRemoved()
        {
            using (var databaseContext = new DatabaseContext())
            {
                var users = await databaseContext.Users.ToListAsync();
                users.ForEach(u => u.Rank = u.Rank - 1);
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
                .Where(u =>
                    u.Points >= user.Points
                    && u.Points < (user.Points + points)
                    && u.UserName != user.UserName
                ).ToListAsync();

            user.Points = user.Points + points;

            if (usersWithRankDepreciated.Count == 0)
            {
                return;
            }
            usersWithRankDepreciated.ForEach(u => u.Rank = u.Rank + 1);

            user.Rank = user.Rank - usersWithRankDepreciated.Count;
        }

        private static async Task DecrementUserRank(int points, DatabaseContext databaseContext, User user)
        {
            var usersToIncreaseRank = await databaseContext.Users
                .Where(u =>
                    u.Points > (user.Points + points)
                    && u.Points <= user.Points
                    && u.UserName != user.UserName)
                .ToListAsync();

            user.Points = user.Points + points;

            if (usersToIncreaseRank.Count == 0)
            {
                return;
            }
            usersToIncreaseRank.ForEach(u => u.Rank = u.Rank - 1);

            user.Rank = user.Rank + usersToIncreaseRank.Count;
        }

        public async Task ChangeUserRole(string userName, int roleId)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var user = await databaseContext.Users.SingleAsync(u => u.UserName == userName);
                var role = await databaseContext.Roles.SingleAsync(r => r.Id == roleId);
                user.Role = null;
                user.Role = role;
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

        public async Task<bool> EmailExists(string email)
        {
            using (var databaseContext = new DatabaseContext())
            {
                return await databaseContext.Users.AnyAsync(u => u.Email == email);
            }
        }

        public async Task ChangeUserAvatar(byte[] image, string username)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var user = await databaseContext.Users
                    .SingleAsync(u => u.UserName == username);
                user.Avatar = image;
                await databaseContext.SaveChangesAsync();
            }
        }
    }
}