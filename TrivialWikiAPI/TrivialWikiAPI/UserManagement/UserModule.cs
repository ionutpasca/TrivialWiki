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
    }
}