using Nancy;

namespace TrivialWikiAPI.UserManagement
{
    public class UserModule : NancyModule
    {
        private readonly UserManager userManager = new UserManager();

        public UserModule()
        {
            Get["/users"] = param =>
            {
                return userManager.GetAllUsers();
                //return JsonConvert.SerializeObject(allUsers, Formatting.Indented,
                //    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            };
        }

    }
}