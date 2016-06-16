using Newtonsoft.Json;
using POSTagger.Model;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace POSTagger.EndPoint
{
    public static class DirectoryManager
    {
        private static readonly string resultBasePath = ConfigurationManager.AppSettings["Tagger.Topic"];

        public static void CreateDirectoryForTopic(string topic)
        {
            var topicPath = $@"{resultBasePath}\{topic}";
            var directoryExists = Directory.Exists(topicPath);
            if (!directoryExists)
            {
                Directory.CreateDirectory(topicPath);
            }
        }

        public static string GetRawResultsPath(string topic)
        {
            return $@"{resultBasePath}\{topic}\RawResults.txt";
        }

        public static string GetCleanResultsPath(string topic)
        {
            return $@"{resultBasePath}\{topic}\CleanText.txt";
        }

        public static string GetOutputJsonPath(string topic)
        {
            return $@"{resultBasePath}\{topic}\OutputJson.txt";
        }

        private static string GetQuestionsPath(string topic)
        {
            return $@"{resultBasePath}\{topic}\Questions.txt";
        }

        public static string GetReferencesPath(string topic)
        {
            return $@"{resultBasePath}\{topic}\References.txt";
        }

        public static void WriteQuestionsToFile(IEnumerable<TopicQuestion> questions, string topic)
        {
            var questionsPath = GetQuestionsPath(topic);

            var serializedQuestions = JsonConvert.SerializeObject(questions, Formatting.Indented,
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            File.WriteAllText(questionsPath, serializedQuestions);
        }

        public static async Task WriteTextToFile(string text, string filePath)
        {
            using (var file = new StreamWriter(filePath))
            {
                await file.WriteLineAsync(text);
            }
        }
    }
}
