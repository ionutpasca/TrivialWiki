using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Security;
using Nancy.TinyIoc;
using System.Linq;
using TrivialWikiAPI.DatabaseModels;

namespace TrivialWikiAPI
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            ConfigStatelessAuthentication(pipelines);
        }

        private void ConfigStatelessAuthentication(IPipelines pipelines)
        {
            var config = new StatelessAuthenticationConfiguration(ctx => AuthenticateUser(ReadAuthToken(ctx)));
            StatelessAuthentication.Enable(pipelines, config);
        }

        private static string ReadAuthToken(NancyContext ctx)
        {
            var token = ctx.Request.Headers.Authorization;
            if (string.IsNullOrEmpty(token))
            {
                token = ctx.Request.Query.token;
            }
            return token;
        }

        private static IUserIdentity AuthenticateUser(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            using (var databaseContext = new DatabaseContext())
            {
                var user = databaseContext.Users
                    .Include("Role")
                    .FirstOrDefault(u => u.SecurityToken == token);

                return user;
            }
        }
    }
}