using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using TrivialWikiAPI.DatabaseModels;

namespace TrivialWikiAPI
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            using (var databaseContext = new DatabaseContext())
            {
                //var role = new Role() { Name = "Admin" };
                //databaseContext.Roles.Add(role);
                //databaseContext.SaveChanges();

                //var roles = databaseContext.Roles.ToList();
            }
        }
    }
}