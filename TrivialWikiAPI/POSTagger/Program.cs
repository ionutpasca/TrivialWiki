using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using WikipediaResourceFinder;
using Console = System.Console;

namespace POSTagger
{
    internal class Program
    {
        private static readonly string wikipediaRawResultPath = ConfigurationManager.AppSettings["Tagger.WikipediaResult"];
        private static readonly string cleanTextPath = ConfigurationManager.AppSettings["Tagger.CleanText"];

        public static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            IResourceFinder res = new ResourceFinder();
            await res.GetWikipediaRawText("Pit_bull", wikipediaRawResultPath);
            var text = File.ReadAllText(wikipediaRawResultPath);

            text = StringUtils.CleanText(text);

            using (var file = new StreamWriter(cleanTextPath))
            {
                await file.WriteLineAsync(text);
            }
            Console.WriteLine(text.Length);
            Console.WriteLine("Process new data?");
            var answer = Console.ReadLine();
            var tpr = new TextProcessing();
            if (answer != null && answer.Equals("y"))
                tpr.ProcessText(text);
            TextProcessing.ProcessJson();
            Console.ReadLine();
        }
    }
}