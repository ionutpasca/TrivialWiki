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

            //var resultList = SentenceGenerator.GetSentences();

            var tpr = new TextProcessing();
            var resultList = tpr.GetSentencesInformationFromJson();

            foreach (var sentence in resultList)
            {
                var dependencies = sentence.Dependencies.Select(s => new SentenceDependencyDto(s.Dep, s.Governor,
                    s.GovernorGloss, s.Dependent, s.DependentGloss)).ToList();

                var words = sentence.Words.Select(w => new WordInformationDto(w.Word, w.PartOfSpeech, w.NamedEntityRecognition, w.Lemma)).ToList();

                var sentenceInfo = new SentenceInformationDto(sentence.SentenceText, dependencies, words);
                if (Helper.SentenceIsInvalid(sentenceInfo))
                {
                    continue;
                }
                if (sentence.Dependencies.Count > 20 && !Helper.SentenceContainsYear(sentenceInfo))
                {
                    continue;
                }
                var question = QuestionGenerator.Generate(sentenceInfo);
            }

            Console.ReadLine();
        }
    }
}