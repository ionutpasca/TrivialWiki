using DatabaseManager.Topics;
using Nancy;
using System.Threading.Tasks;

namespace TrivialWikiAPI.Topics
{
    public class TopicsModule : NancyModule
    {
        private readonly TopicsManager topicsManager = new TopicsManager();
        //private readonly Tagger tagger = new Tagger();

        public TopicsModule()
        {
            Get["/topicNames", true] = async (param, p) => await GetAllTopicNames();
            Get["/inactiveTopics", true] = async (param, p) => await GetInactiveTopics();

            Get["/inactiveQuestions/{topic}", true] = async (param, p) => await GetInactiveQuestionsForTopic(param.topic);
            Get["/activeQuestions/{topic}", true] = async (param, p) => await GetActiveQuestionsForTopic(param.topic);


            Post["/topic/{topicName}", true] = async (param, p) => await AddNewTopic(param.topicName);
            Post["/enableTopic/{topicName}", true] = async (param, p) => await EnableTopic(param.topicName);
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

        private async Task<object> GetInactiveTopics()
        {
            var topics = await topicsManager.GetInactiveTopics();
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

            await topicsManager.AddNewTopicToDatabase(topicName);
            //await tagger.GetWikipediaResources(topicName);
            //await tagger.ProcessWikipediaText(topicName);

            //tagger.GenerateQuestions(topicName);
            await topicsManager.SaveQuestionsFromFile(topicName);
            return HttpStatusCode.OK;
        }
    }
}