using DatabaseManager.DatabaseModels;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Security;
using Nancy.TinyIoc;
using System.Diagnostics;
using System.Linq;
using WikiTrivia.Utilities;

namespace TrivialWikiAPI
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            ConfigStatelessAuthentication(pipelines);

            var x = QuestionCleaner.RemovePunctuationFromEnd("hehe , dsada . ,?");
            Debug.WriteLine(x);
            //using (var dbC = new DatabaseContext())
            //{
            //    var not = new Notification
            //    {
            //        NotificationText = "Welcome to WikiTrivia. We hope you have fun!",
            //        Seen = false,
            //        Sender = "WikiTrivia",
            //        NotificationDate = DateTime.Now
            //    };

            //    var user = dbC.Users.Single(u => u.UserName == "ionut");
            //    not.User = user;
            //    dbC.Notifications.Add(not);
            //    dbC.SaveChanges();

            //    var questionSet = new QuestionSet() { QuestionText = "Which is the best IDE?", CorrectAnswer = "Visual Studion" };
            //    var topic = new Topic() { Name = "IT" };
            //    topic.Questions.Add(questionSet);
            //    dbC.Topics.Add(topic);

            //    var role = new Role() { Name = "Admin" };
            //    dbC.Roles.Add(role);
            //    dbC.SaveChanges();

            //    var user = new User() { Email = "pascaionut@yahoo.com", UserName = "ionut" };
            //    var pass = Encrypt.GetMD5("1234");
            //    user.AccountCreationDate = DateTime.Now;
            //    user.Password = pass;
            //    var role1 = dbC.Roles.Single(r => r.Name == "Admin");
            //    user.Role = role1;

            //    dbC.Users.Add(user);
            //    dbC.SaveChanges();
            //}
        }

        private static void ConfigStatelessAuthentication(IPipelines pipelines)
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