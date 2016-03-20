using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using TrivialWikiAPI.DatabaseModels;
using TrivialWikiAPI.UserManagement.Login;
using TrivialWikiAPI.Utilities;

namespace TrivialWikiAPI.UserManagement
{
    public class LoginModule : NancyModule
    {
        private readonly LoginManager loginManager = new LoginManager();

        public LoginModule()
        {
            Get["/login"] = param => LoginUser();
        }

        private dynamic LoginUser()
        {
            var user = this.Bind<User>();
            if (user == null)
            {
                return HttpStatusCode.Unauthorized;
            }

            var userPassword = Encrypt.GetMD5(user.Password);

            var loggedUser = loginManager.Login(user.UserName, userPassword);
            if (loggedUser == null)
            {
                return HttpStatusCode.Unauthorized;
            }

            var userRoles = loggedUser.Roles.Select(r => r.Name).ToList();

            var data = new LoginResponse
            {
                Avatar = loggedUser.Avatar,
                UserName = loggedUser.UserName,
                FirstName = loggedUser.FirstName,
                LastName = loggedUser.LastName,
                Email = loggedUser.Email,
                Rank = loggedUser.Rank,
                Roles = userRoles,
                SecurityToken = loggedUser.SecurityToken
            };

            return Response.AsJson(data);
        }
    }
}