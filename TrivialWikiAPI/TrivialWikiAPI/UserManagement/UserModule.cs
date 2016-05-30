using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;
using Nancy.Security;
using System.Drawing;
using System.Threading.Tasks;
using System.Web;
using TrivialWikiAPI.DatabaseModels;
using TrivialWikiAPI.Utilities;

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
            Get["/usernameExists/{username}", true] = async (param, p) => await GivenUsernameExists(param.username);

            Post["/addNewUser", true] = async (param, p) => await AddNewUserToDatabase();
            Post["/updateUser", true] = async (param, p) =>
            {
                var user = this.Bind<UserResponse>();
                await UpdateUser(user);
                return HttpStatusCode.OK;
            };
            Post["/removeUser/{userName}", true] = async (param, p) => await RemoveUser(param.userName);
            Post["/addPointsToUser/{userName}/{points}", true] = async (param, p) => await AddPointsToUser(param.userName, param.points);
            Post["/changeAvatar", true] = async (param, p) => await ChangeAvatar();
            Post["/changeAvatarAsBase64", true] = async (param, p) => await ChangeAvatarAsBase64();

            Put["/changePassword", true] = async (param, p) => await ChangeUserPassword();
            Put["/changeUserRole/{userName}/{roleId}", true] = async (param, p) => await ChangeUserRole(param.userName, param.roleId);
        }

        private async Task<object> ChangeAvatarAsBase64()
        {
            var base64Image = this.Request.Body.AsString();
            if (base64Image == null)
            {
                return HttpStatusCode.BadRequest;
            }
            var currentUser = this.Context.CurrentUser;
            await userManager.ChangeUserAvatar(base64Image, currentUser.UserName);
            return HttpStatusCode.OK;
        }

        private async Task<object> ChangeAvatar()
        {
            var currentUser = this.Context.CurrentUser;
            var file = HttpContext.Current.Request.Files[0];
            var bitmapImage = new Bitmap(file.InputStream);
            var height = 200;
            var width = (200 * bitmapImage.Width) / bitmapImage.Height;
            var resizedImage = AvatarManager.ResizeImage(bitmapImage, file.FileName, width, height);
            await userManager.ChangeUserAvatar(resizedImage, currentUser.UserName);

            return HttpStatusCode.OK;
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
            if (usernameExists)
            {
                return this.Response.AsJson(true);
            }
            return this.Response.AsJson(false);
        }

        private async Task<Response> GivenEmailExists(string email)
        {
            var userEmailExists = await userManager.EmailExists(email);
            if (userEmailExists)
            {
                return this.Response.AsJson(true);
            }
            return this.Response.AsJson(false);
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