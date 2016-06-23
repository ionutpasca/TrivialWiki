using DatabaseManager.Topics;
using Nancy;
using System.Threading.Tasks;
using WikipediaResourceFinder;
using WikiTrivia.Core;

namespace TrivialWikiAPI.Topics
{
    public class TopicsModule : NancyModule
    {
        private readonly TopicsManager topicsManager = new TopicsManager();
        private readonly NotificationsCore notificationCore = new NotificationsCore();
        //private readonly Tagger tagger = new Tagger();

        public TopicsModule()
        {
            Get["/topicNames", true] = async (param, p) => await GetAllTopicNames();
            Get["/detailedTopics", true] = async (param, p) => await GetTopicsWithDetails();

            Get["/activeTopics", true] = async (param, p) => await GetActiveTopics();
            Get["/inactiveTopics", true] = async (param, p) => await GetInactiveTopics();
            Get["/getProposedTopics", true] = async (param, p) => await GetProposedTopics();

            Get["/inactiveQuestions/{topic}", true] = async (param, p) => await GetInactiveQuestionsForTopic(param.topic);
            Get["/activeQuestions/{topic}", true] = async (param, p) => await GetActiveQuestionsForTopic(param.topic);

            Post["/topic/{topicName}", true] = async (param, p) => await AddNewTopic(param.topicName);
            Post["/enableTopic/{topicName}", true] = async (param, p) => await EnableTopic(param.topicName);
            Post["/proposeTopic/{topicName}", true] = async (param, p) => await ProposeTopic(param.topicName);
            Post["/processTopic/{topicName}", true] = async (param, p) => await ProcessTopic(param.topicName);
        }

        private async Task<object> ProcessTopic(string topicName)
        {
            var topic = await topicsManager.GetProposedTopic(topicName);
            if (topic == null)
            {
                return HttpStatusCode.BadRequest;
            }
            var currentUser = Context.CurrentUser;

            var resourceFinder = new ResourceFinder();
            var topicImage = resourceFinder.GetWikipediaImageForTopic(topicName);

            await topicsManager.AddNewTopicToDatabase(topicName, topicImage);

            topicName = topicName.Replace(" ", "_");
            //await tagger.GetWikipediaResources(topicName);
            //await tagger.ProcessWikipediaText(topicName);
            //tagger.GenerateQuestions(topicName);

            await topicsManager.SaveQuestionsFromFile(topicName);


            await notificationCore.SendTopicProcessedNotification(currentUser.UserName, topicName);
            if (topic.User.UserName != currentUser.UserName)
            {
                await notificationCore.SendTopicProcessedNotification(topic.User.UserName, topicName);
            }

            await topicsManager.DeleteProposedTopic(topicName);
            return HttpStatusCode.OK;
        }

        private async Task<object> GetProposedTopics()
        {
            var proposedTopics = await topicsManager.GetProposedTopics();
            return Response.AsJson(proposedTopics);
        }

        private async Task<object> GetTopicsWithDetails()
        {
            var topics = await topicsManager.GetDetailedTopics();
            return Response.AsJson(topics);
        }

        private async Task<Response> ProposeTopic(string topicName)
        {
            if (topicName == null || topicName.Length > 20)
            {
                return HttpStatusCode.BadRequest;
            }
            var curentUser = Context.CurrentUser;
            var topicExists = await topicsManager.ProposedTopicExists(topicName);
            if (topicExists)
            {
                return HttpStatusCode.Conflict;
            }
            await topicsManager.AddProposedTopic(topicName, curentUser.UserName);
            await notificationCore.SendProposedTopicNotification(curentUser.UserName, topicName);
            return HttpStatusCode.OK;
        }

        private async Task<object> EnableTopic(string topicName)
        {
            var topicExists = await topicsManager.TopicExists(topicName);
            if (!topicExists)
            {
                return HttpStatusCode.NotFound;
            }
            await topicsManager.EnableTopic(topicName);
            return HttpStatusCode.OK;
        }

        private async Task<Response> GetActiveQuestionsForTopic(string topic)
        {
            var questions = await topicsManager.GetActiveQuestionsForTopic(topic);
            return questions == null ? HttpStatusCode.NotFound : Response.AsJson(questions);
        }

        private async Task<Response> GetInactiveQuestionsForTopic(string topic)
        {
            var questions = await topicsManager.GetInactiveQuestionsForTopic(topic);
            return questions == null ? HttpStatusCode.NotFound : Response.AsJson(questions);
        }

        private async Task<object> GetAllTopicNames()
        {
            var topics = await topicsManager.GetAllTopicNames();
            return Response.AsJson(topics);
        }

        private async Task<object> GetActiveTopics()
        {
            var topics = await topicsManager.GetTopics();
            return Response.AsJson(topics);
        }

        private async Task<object> GetInactiveTopics()
        {
            var topics = await topicsManager.GetTopics(false);
            return Response.AsJson(topics);
        }

        private async Task<object> AddNewTopic(string topicName)
        {
            if (topicName == null)
            {
                return HttpStatusCode.BadRequest;
            }
            var topicExists = await topicsManager.TopicExists(topicName);
            if (topicExists)
            {
                return HttpStatusCode.Conflict;
            }

            var resourceFinder = new ResourceFinder();
            var topicImage = resourceFinder.GetWikipediaImageForTopic(topicName);

            await topicsManager.AddNewTopicToDatabase(topicName, topicImage);
            //await tagger.GetWikipediaResources(topicName);
            //await tagger.ProcessWikipediaText(topicName);

            //tagger.GenerateQuestions(topicName);
            await topicsManager.SaveQuestionsFromFile(topicName);
            return HttpStatusCode.OK;
        }
    }
}