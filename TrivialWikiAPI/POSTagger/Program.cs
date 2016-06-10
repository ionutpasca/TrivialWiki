using WikipediaResourceFinder;
using Console = System.Console;

namespace POSTagger
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            //var pattern = new Pattern(@"D:\Licenta\Files\Patterns.txt");

            //SetPatterns();
            IResourceFinder res = new ResourceFinder();
            var text = res.GetWikipediaRawText("Pit_bull");
            text = StringUtils.CleanText(text);

            using (var file = new System.IO.StreamWriter(@"D:\Licenta\Files\CleanText.txt"))
            {
                file.WriteLine(text);

            }
            Console.WriteLine(text.Length);
            //var text = "Bob likes books.";
            //const string text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply. Superman is one of DC's most important superheroes.";
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