using DatabaseManager.UserManagement;
using Nancy;
using Nancy.Security;
using System.Linq;
using System.Threading.Tasks;
using WikiTrivia.Core;

namespace TrivialWikiAPI.Friends
{
    public class FriendsModule : NancyModule
    {
        private readonly UserManager userManager = new UserManager();
        private readonly NotificationsCore notificationCore = new NotificationsCore();

        public FriendsModule()
        {
            this.RequiresAuthentication();

            Get["/getFriends"] = param => GetFriends();
            Get["/onlineFriends"] = param => GetOnlineFriends();

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

        private Response GetOnlineFriends()
        {
            var currentUser = Context.CurrentUser;
            if (currentUser == null)
            {
                return HttpStatusCode.MethodNotAllowed;
            }
            var friends = userManager.GetAllFriendsForUser(currentUser.UserName);
            var onlineUsers = notificationCore.GetOnlineUsers();
            var result = friends.Select(f => onlineUsers.Contains(f)).ToList();
            return Response.AsJson(result);
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
            await notificationCore.SendNewFriendNotification(currentUser.UserName, userName);
            await notificationCore.SendNewFriendNotification(userName, currentUser.UserName);
            return HttpStatusCode.OK;
        }
    }
}