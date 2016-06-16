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
            Get["/topicsWithoutQuestions", true] = async (param, p) => await GetTopicsWithoutQuestions();
            Get["/questions/{topic}"] = param => GetQuestionsForTopic(param.topic);

            Post["/topic/{topicName}", true] = async (param, p) => await AddNewTopic(param.topicName);
        }

        private Response GetQuestionsForTopic(string topic)
        {
            var questions = topicsManager.GetQuestionsForTopicFromFile(topic);
            return Response.AsJson(questions);
        }

        private async Task<object> GetAllTopicNames()
        {
            var topics = await topicsManager.GetAllTopicNames();
            return Response.AsJson(topics);
        }

        private async Task<object> GetTopicsWithoutQuestions()
        {
            var topics = await topicsManager.GetTopicsWithoutQuestions();
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
            return HttpStatusCode.OK;
        }
    }
}