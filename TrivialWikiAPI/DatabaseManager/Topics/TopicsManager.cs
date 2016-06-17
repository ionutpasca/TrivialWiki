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

        public async Task<IEnumerable<string>> GetInactiveTopics()
        {
            using (var databaseContext = new DatabaseContext())
            {
                return await databaseContext.Topics
                    .Where(t => t.IsActive == false)
                    .Select(t => t.Name)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<TopicQuestionDto>> GetInactiveQuestionsForTopic(string topic)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var singleOrDefault = await databaseContext.Topics
                        .Include("Questions")
                        .SingleOrDefaultAsync(t => t.Name == topic);
                var questions = singleOrDefault?.Questions.Where(q => q.IsValidated == false);

                if (questions != null)
                {
                    return questions.Select(t => new TopicQuestionDto
                    {
                        QuestionId = t.Id,
                        Question = t.QuestionText,
                        Answer = t.CorrectAnswer
                    })
                    .ToList();
                }
            }
            return null;
        }

        public async Task<IEnumerable<TopicQuestionDto>> GetActiveQuestionsForTopic(string topic)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var singleOrDefault = await databaseContext.Topics
                        .Include("Questions")
                        .SingleOrDefaultAsync(t => t.Name == topic);
                var questions = singleOrDefault?.Questions.Where(q => q.IsValidated == true);

                if (questions != null)
                {
                    return questions.Select(t => new TopicQuestionDto
                    {
                        QuestionId = t.Id,
                        Question = t.QuestionText,
                        Answer = t.CorrectAnswer
                    })
                    .ToList();
                }
            }
            return null;
        }

        public async Task AddNewTopicToDatabase(string topicName)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var topic = new Topic(topicName) { IsActive = false };
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

        public async Task SaveQuestionsFromFile(string topic)
        {
            var questionsPath = $@"{resultBasePath}\{topic}\Questions.txt";
            using (var file = File.OpenText(questionsPath))
            {
                var serializer = new JsonSerializer();
                var result = (List<TopicQuestionDto>)serializer.Deserialize(file, typeof(List<TopicQuestionDto>));

                var questions = result.Select(q => new QuestionSet
                {
                    QuestionText = q.Question,
                    CorrectAnswer = q.Answer,
                    IsValidated = false
                })
                    .ToList();

                await SaveQuestionsToDatabase(questions, topic);

                ClearTopicFiles(topic);
            }
        }

        private async Task SaveQuestionsToDatabase(IEnumerable<QuestionSet> questions, string topic)
        {
            using (var db = new DatabaseContext())
            {
                var tpc = await db.Topics.SingleOrDefaultAsync(t => t.Name == topic);
                foreach (var question in questions)
                {
                    tpc.Questions.Add(question);
                }
                await db.SaveChangesAsync();
            }
        }

        private void ClearTopicFiles(string topic)
        {
            var topicPath = $@"{resultBasePath}\{topic}";
            if (Directory.Exists(topicPath))
            {
                Directory.Delete(topicPath, true);
            }
        }

        public async Task EnableTopic(string topicName)
        {
            using (var databaseContext = new DatabaseContext())
            {
                var topic = await databaseContext.Topics.SingleOrDefaultAsync(t => t.Name == topicName);
                topic.IsActive = true;
                await databaseContext.SaveChangesAsync();
            }
        }
    }
}
