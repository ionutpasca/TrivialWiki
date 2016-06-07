using DatabaseManager.Leaderboard;
using Nancy;
using Nancy.Security;
using System.Threading.Tasks;

namespace TrivialWikiAPI.Leaderboard
{
    public class LeaderboardModule : NancyModule
    {
        private readonly LeaderboardManager leaderboardManager = new LeaderboardManager();
        public LeaderboardModule()
        {
            this.RequiresAuthentication();

            Get["/leaderBoard/firstThree", true] = async (param, p) => await GetFirstThreeUsersFromLeaderboard();
            Get["/leaderBoard/{pageNumber}", true] = async (param, p) => await GetUserLeaderBoard(param.PageNumber);
        }

        private async Task<Response> GetFirstThreeUsersFromLeaderboard()
        {
            var firstThreeUsers = await leaderboardManager.GetFirstThreeUsersFromLeaderboard();
            return this.Response.AsJson(firstThreeUsers);
        }

        private async Task<Response> GetUserLeaderBoard(int pageNumber)
        {
            var leaderBoardUsers = await leaderboardManager.GetUsersLeaderBoard(pageNumber);
            return this.Response.AsJson(leaderBoardUsers);
        }
    }
}