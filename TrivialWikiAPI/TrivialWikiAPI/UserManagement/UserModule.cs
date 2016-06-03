using DatabaseManager.DatabaseModels;
using DatabaseManager.UserManagement;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using System.Threading.Tasks;

namespace TrivialWikiAPI.UserManagement
{
    public class UserModule : NancyModule
    {
        private readonly UserManager userManager = new UserManager();

        public UserModule()
        {
            this.RequiresAuthentication();

            Get["/getUserBatch/{pageNumber}", true] = async (param, p) => await GetUsersBatch(param.PageNumber);
            Get["/emailExists/{email}", true] = async (param, p) => await GivenEmailExists(param.email);
            Get["/usernameExists/{userName}", true] = async (param, p) => await GivenUsernameExists(param.userName);
            Get["/accountCreationDate/{userName}"] = param => GetAccountCreationDate(param.userName);
            Get["/userPoints/{userName}", true] = async (param, p) => await GetUserPoints(param.userName);

            Post["/addNewUser", true] = async (param, p) => await AddNewUserToDatabase();
            Post["/updateUser", true] = async (param, p) =>
            {
                var user = this.Bind<UserResponse>();
                await UpdateUser(user);
                return HttpStatusCode.OK;
            };
            Post["/removeUser/{userName}", true] = async (param, p) => await RemoveUser(param.userName);
            Post["/addPointsToUser/{userName}/{points}", true] = async (param, p) => await AddPointsToUser(param.userName, param.points);
            Post["/changePassword/{userName}/{oldPass}/{newPass}", true] = async (param, p) => await ChangeUserPassword(param.userName, param.oldPass, param.newPass);

            Put["/changeUserRole/{userName}/{roleId}", true] = async (param, p) => await ChangeUserRole(param.userName, param.roleId);
        }

        private async Task<Response> GetUserPoints(string userName)
        {
            if (userName == null)
            {
                return HttpStatusCode.BadRequest;
            }
            var userExists = await userManager.UserExists(userName);
            if (!userExists)
            {
                return HttpStatusCode.BadRequest;
            }

            var points = await userManager.GetUserPoints(userName);
            return this.Response.AsJson(points);
        }

        private Response GetAccountCreationDate(string userName)
        {
            if (userName == null)
            {
                return HttpStatusCode.BadRequest;
            }
            var result = userManager.GetAccountCreationDate(userName);
            return this.Response.AsJson(result);
        }

        private async Task<Response> GetUsersBatch(int pageNumber)
        {
            string queryString = this.Request.Query["queryString"];
            var users = await userManager.GetUsersBatch(queryString, pageNumber);
            return this.Response.AsJson(users);
        }

        private async Task<Response> GivenUsernameExists(string username)
        {
            var usernameExists = await userManager.UserExists(username);
            return this.Response.AsJson(usernameExists);
        }

        private async Task<Response> GivenEmailExists(string email)
        {
            var userEmailExists = await userManager.EmailExists(email);
            return this.Response.AsJson(userEmailExists);
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

        private async Task UpdateUser(UserResponse user)
        {
            await userManager.UpdateUser(user);
        }

        private async Task<Response> RemoveUser(string userName)
        {
            if (userName == null) return HttpStatusCode.BadRequest;
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

        private async Task<Response> ChangeUserPassword(string username, string oldPass, string newPass)
        {
            if (oldPass == null || newPass == null)
            {
                return HttpStatusCode.BadRequest;
            }

            var userExists = await userManager.UserExists(username);
            if (!userExists)
            {
                return HttpStatusCode.BadRequest;
            }

            var passwordsMath = await userManager.PasswordMathForUser(username, oldPass);
            if (!passwordsMath)
            {
                return HttpStatusCode.NotFound;
            }

            await userManager.ChangeUserPassword(username, newPass);
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