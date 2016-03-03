using Nancy;
using Nancy.ModelBinding;
using System.Linq;
using TrivialWikiAPI.DatabaseModels;
using TrivialWikiAPI.Utilities;

namespace TrivialWikiAPI.UserManagement
{
    public class UserModule : NancyModule
    {
        private readonly UserManager userManager = new UserManager();

        public UserModule()
        {
        }
    }
}