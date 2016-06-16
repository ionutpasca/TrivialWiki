using DatabaseManager.DatabaseModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManager.Topics
{
    public sealed class TopicsManager
    {
        private static readonly string resultBasePath = ConfigurationManager.AppSettings["Tagger.Topic"];

        public async Task<IEnumerable<string>> GetAllTopicNames()
        {
            using (var databaseContext = new DatabaseContext())
            {
                return await databaseContext.Topics
                    .Select(t => t.Name)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<string>> GetTopicsWithoutQuestions()
        {
            using (var databaseContext = new DatabaseContext())
            {
                return await databaseContext.Topics
                    .Where(t => t.Questions.Count == 0)
                    .Select(t => t.Name)
                    .ToListAsync();
            }
        }

        public async Task AddNewTopicToDatabase(string topicName)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var topic = new Topic(topicName);
                databaseContext.Topics.Add(topic);
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task<bool> TopicExists(string topicName)
        {
            using (var databaseContext = new DatabaseContext())
            {
                return await databaseContext.Topics.AnyAsync(t => t.Name == topicName);
            }
        }

        public IEnumerable<TopicQuestionDto> GetQuestionsForTopicFromFile(string topic)
        {
            var questionsPath = $@"{resultBasePath}\{topic}\Questions.txt";
            using (var file = File.OpenText(questionsPath))
            {
                var serializer = new JsonSerializer();
                var result = (List<TopicQuestionDto>)serializer.Deserialize(file, typeof(List<TopicQuestionDto>));
                return result;
            }
        }
    }
}
