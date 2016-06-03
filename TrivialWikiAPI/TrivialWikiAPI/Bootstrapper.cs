using DatabaseManager.DatabaseModels;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Security;
using Nancy.TinyIoc;
using System.Linq;
using WikiTrivia.TriviaCore;

namespace TrivialWikiAPI
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private readonly TriviaCore triviaCore = new TriviaCore();
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            ConfigStatelessAuthentication(pipelines);
            triviaCore.Run();
            //using (var dbC = new DatabaseContext())
            //{
            //    var questionSet = new QuestionSet() { QuestionText = "Which is the best IDE?", CorrectAnswer = "Visual Studion" };
            //    var topic = new Topic() { Name = "IT" };
            //    topic.Questions.Add(questionSet);
            //    dbC.Topics.Add(topic);
            //    dbC.SaveChanges();
            //}
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