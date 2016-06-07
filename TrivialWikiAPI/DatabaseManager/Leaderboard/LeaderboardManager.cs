using DatabaseManager.DatabaseModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManager.Leaderboard
{
    public sealed class LeaderboardManager
    {
        public async Task<List<LeaderboardResponse>> GetFirstThreeUsersFromLeaderboard()
        {
            using (var databaseContext = new DatabaseContext())
            {
                return await databaseContext.Users
                    .OrderByDescending(u => u.Points)
                    .Take(3)
                    .Select(u => new LeaderboardResponse()
                    {
                        Username = u.UserName,
                        NumberOfPoints = u.Points
                    })
                    .ToListAsync();
            }
        }

        public async Task<List<LeaderboardResponse>> GetUsersLeaderBoard(int pageNumber)
        {
            var usersToSkip = (pageNumber - 1) * 10;
            using (var databaseContext = new DatabaseContext())
            {
                return await databaseContext.Users
                    .OrderByDescending(u => u.Points)
                    .Skip(usersToSkip + 3)
                    .Take(10)
                    .Select(u => new LeaderboardResponse()
                    {
                        Username = u.UserName,
                        NumberOfPoints = u.Points
                    })
                    .ToListAsync();
            }
        }
    }
}
