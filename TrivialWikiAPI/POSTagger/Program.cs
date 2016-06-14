using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using WikiTrivia.QuestionGenerator;
using WikiTrivia.QuestionGenerator.Model;
using Console = System.Console;

namespace POSTagger
{
    internal class Program
    {
        private static readonly string wikipediaRawResultPath = ConfigurationManager.AppSettings["Tagger.WikipediaResult"];
        private static readonly string cleanTextPath = ConfigurationManager.AppSettings["Tagger.CleanText"];
        private static readonly string questionPath = ConfigurationManager.AppSettings["Tagger.Question"];


        public static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            //IResourceFinder res = new ResourceFinder();
            //await res.GetWikipediaRawText("Pit_bull", wikipediaRawResultPath);
            //var text = File.ReadAllText(wikipediaRawResultPath);

            //text = StringUtils.CleanText(text);

            //using (var file = new StreamWriter(cleanTextPath))
            //{
            //    await file.WriteLineAsync(text);
            //}
            //Console.WriteLine(text.Length);
            //Console.WriteLine("Process new data?");
            var text = "Bob likes trees";
            TextProcessing trp = new TextProcessing();
            trp.ProcessText(text);
            trp.BetterFill();
            var resultList = SentenceGenerator.GetSentences();

            foreach (var res in resultList)
            {
                var dependencies = res.Dependencies.Select(s => new SentenceDependencyDto(s.Dep, s.Governor,
                    s.GovernorGloss, s.Dependent, s.DependentGloss)).ToList();

                var words = res.Words.Select(w => new WordInformationDto(w.Word, w.PartOfSpeech, w.NamedEntityRecognition, w.Lemma)).ToList();

                var sentenceInfo = new SentenceInformationDto(res.SentenceText, dependencies, words);
                var question = QuestionGenerator.Generate(sentenceInfo);
            }

            Console.ReadLine();
        }
    }
}