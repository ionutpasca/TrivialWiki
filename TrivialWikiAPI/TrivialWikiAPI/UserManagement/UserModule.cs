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
            Post["/addNewUser"] = param => AddNewUserToDatabase();
            Post["/removeUser/{userName}"] = param => RemoveUser(param.userName);
            Post["/addPointsToUser/{userName}/{points}"] = param => AddPointsToUser(param.userName, param.points);

            Put["/changePassword"] = param => ChangeUserPassword();
            Put["/changeUserRole/{userName}/{roleId}"] = param => ChangeUserRole(param.userName, param.roleId);
        }

        private dynamic AddNewUserToDatabase()
        {
            var user = this.Bind<User>();
            if (string.IsNullOrEmpty(user?.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return HttpStatusCode.BadRequest;
            }

            var userExists = userManager.UserExists(user.UserName);
            if (userExists)
            {
                return HttpStatusCode.Conflict;
            }

            userManager.AddNewUserToDatabase(user);
            return HttpStatusCode.OK;
        }

        private dynamic ChangeUserPassword()
        {
            var user = this.Bind<User>();
            if (user?.UserName == null)
            {
                return HttpStatusCode.BadRequest;
            }

            var userExists = userManager.UserExists(user.UserName);
            if (!userExists)
            {
                return HttpStatusCode.BadRequest;
            }

            userManager.ChangeUserPassword(user);
            return HttpStatusCode.OK;
        }

        private dynamic RemoveUser(string userName)
        {
            if (userName == null)
            {
                return HttpStatusCode.BadRequest;
            }
            userManager.RemoveUserFromDatabase(userName);
            return HttpStatusCode.OK;
        }

        private dynamic AddPointsToUser(string userName, int points)
        {
            if (userName == null)
            {
                return HttpStatusCode.BadRequest;
            }
            if (points == 0)
            {
                return HttpStatusCode.Continue;
            }

            userManager.AddPointsToUser(userName, points);
            return HttpStatusCode.OK;
        }

        private dynamic ChangeUserRole(string userName, int roleId)
        {
            if (userName == null)
            {
                return HttpStatusCode.BadRequest;
            }
            if (!userManager.RoleExists(roleId))
            {
                return HttpStatusCode.BadRequest;
            }

            userManager.ChangeUserRole(userName, roleId);
            return HttpStatusCode.OK;
        }
    }
}