using DatabaseManager.UserManagement;
using Nancy;
using Nancy.Security;
using System.Threading.Tasks;

namespace TrivialWikiAPI.Friends
{
    public class FriendsModule : NancyModule
    {
        private readonly UserManager userManager = new UserManager();
        public FriendsModule()
        {
            this.RequiresAuthentication();

            Get["/getFriends"] = param => GetFriends();

            Post["/addFriend/{userName}", true] = async (param, p) => await AddNewFriend(param.userName);
        }

        private Response GetFriends()
        {
            var currentUser = Context.CurrentUser;
            if (currentUser == null)
            {
                return HttpStatusCode.MethodNotAllowed;
            }
            var friends = userManager.GetAllFriendsForUser(currentUser.UserName);
            return Response.AsJson(friends);
        }

        private async Task<Response> AddNewFriend(string userName)
        {
            if (userName == null)
            {
                return HttpStatusCode.BadRequest;
            }
            var userExists = await userManager.UserExists(userName);
            if (!userExists)
            {
                return HttpStatusCode.NotFound;
            }
            var currentUser = Context.CurrentUser;
            if (currentUser == null)
            {
                return HttpStatusCode.MethodNotAllowed;
            }
            await userManager.AddNewFriendToUser(currentUser.UserName, userName);
            return HttpStatusCode.OK;
        }
    }
}