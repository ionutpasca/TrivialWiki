using POSTagger.EndPoint;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace POSTagger
{
    internal class Program
    {
        private static readonly string wikipediaRawResultPath = ConfigurationManager.AppSettings["Tagger.WikipediaResult"];
        private static readonly string cleanTextPath = ConfigurationManager.AppSettings["Tagger.CleanText"];
        private static readonly string questionPath = ConfigurationManager.AppSettings["Tagger.Question"];

        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                await MainAsync();
            }).Wait();
        }

        private static async Task MainAsync()
        {
            //var res = new ResourceFinder();
            //await res.GetWikipediaRawText("Superman", wikipediaRawResultPath);
            //var text = File.ReadAllText(wikipediaRawResultPath);

            //text = StringUtils.CleanText(text);

            //using (var file = new StreamWriter(cleanTextPath))
            //{
            //    await file.WriteLineAsync(text);
            //}

            //tpr.ProcessText(text);

            //Console.WriteLine(text.Length);
            //Console.WriteLine("Process new data?");

            var tagger = new Tagger();
            const string topic = "Coca-Cola";
            //await tagger.GetWikipediaResources(topic);
            //await tagger.ProcessWikipediaText(topic);
            tagger.GenerateQuestions(topic);
            Console.ReadLine();
        }
    }
}