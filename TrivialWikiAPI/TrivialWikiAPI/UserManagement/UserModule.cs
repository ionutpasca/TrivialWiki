using System.Threading.Tasks;
using Nancy;
using Nancy.ModelBinding;
using TrivialWikiAPI.DatabaseModels;
using TrivialWikiAPI.Utilities;

namespace TrivialWikiAPI.UserManagement
{
    public class UserModule : NancyModule
    {
        private readonly UserManager userManager = new UserManager();

        public UserModule()
        {
            Post["/addNewUser", true] = async (param, p) => await AddNewUserToDatabase();
            Post["/removeUser/{userName}", true] = async (param, p) => await RemoveUser(param.userName);
            Post["/addPointsToUser/{userName}/{points}", true] = async (param, p) => await AddPointsToUser(param.userName, param.points);

            Put["/changePassword", true] = async (param, p) => await ChangeUserPassword();
            Put["/changeUserRole/{userName}/{roleId}", true] = async (param, p) => await ChangeUserRole(param.userName, param.roleId);
        }

        private async Task<Response> AddNewUserToDatabase()
        {
            var user = this.Bind<User>();
            if (string.IsNullOrEmpty(user?.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return HttpStatusCode.BadRequest;
            }

            var userExists = await userManager.UserExists(user.UserName);
            if (userExists)
            {
                return HttpStatusCode.Conflict;
            }

            await userManager.AddNewUserToDatabase(user);
            return HttpStatusCode.OK;
        }

        private async Task<Response> ChangeUserPassword()
        {
            var user = this.Bind<User>();
            if (user?.UserName == null)
            {
                return HttpStatusCode.BadRequest;
            }

            var userExists = await userManager.UserExists(user.UserName);
            if (!userExists)
            {
                return HttpStatusCode.BadRequest;
            }

            await userManager.ChangeUserPassword(user);
            return HttpStatusCode.OK;
        }

        private async Task<Response> RemoveUser(string userName)
        {
            if (userName == null)
            {
                return HttpStatusCode.BadRequest;
            }
            await userManager.RemoveUserFromDatabase(userName);
            return HttpStatusCode.OK;
        }

        private async Task<Response> AddPointsToUser(string userName, int points)
        {
            if (userName == null)
            {
                return HttpStatusCode.BadRequest;
            }
            if (points == 0)
            {
                return HttpStatusCode.Continue;
            }

            await userManager.AddPointsToUser(userName, points);
            return HttpStatusCode.OK;
        }

        private async Task<Response> ChangeUserRole(string userName, int roleId)
        {
            if (userName == null)
            {
                return HttpStatusCode.BadRequest;
            }
            var roleExists = await userManager.RoleExists(roleId);
            if (!roleExists)
            {
                return HttpStatusCode.BadRequest;
            }

            await userManager.ChangeUserRole(userName, roleId);
            return HttpStatusCode.OK;
        }
    }
}