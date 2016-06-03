using DatabaseManager.DatabaseModels;
using DatabaseManager.UserManagement.Login;
using Nancy;
using Nancy.ModelBinding;
using Newtonsoft.Json;
using WikiTrivia.Utilities;

namespace TrivialWikiAPI.UserManagement.Login
{
    public class LoginModule : NancyModule
    {
        private readonly LoginManager loginManager = new LoginManager();

        public LoginModule()
        {
            Get["/login"] = param => LoginUser();
            Get["/loginWithFacebook"] = param => LoginUserWithFacebook();
        }

        private static dynamic LoginUserWithFacebook()
        {
            return HttpStatusCode.OK;
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

            var userRole = loggedUser.Role.Name;

            var data = new LoginResponse
            {
                UserName = loggedUser.UserName,
                Email = loggedUser.Email,
                Rank = loggedUser.Rank,
                Role = userRole,
                SecurityToken = loggedUser.SecurityToken
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented,
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
    }
}