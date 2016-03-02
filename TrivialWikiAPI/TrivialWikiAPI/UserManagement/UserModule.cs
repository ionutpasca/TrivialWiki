using Nancy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrivialWikiAPI.DatabaseModels;

namespace TrivialWikiAPI.UserManagement
{
    public class UserModule : NancyModule
    {
        private readonly UserManager userManager = new UserManager();

        public UserModule()
        {
            Get["/users"] = param =>
            {
                var allUsers = userManager.GetAllUsers();
                return allUsers;
                //return JsonConvert.SerializeObject(allUsers, Formatting.Indented,
                //    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            };
        }

    }
}