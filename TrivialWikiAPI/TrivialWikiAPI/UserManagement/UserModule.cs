using Nancy;

namespace TrivialWikiAPI.UserManagement
{
    public class UserModule : NancyModule
    {
        private readonly UserManager userManager = new UserManager();
    }
}