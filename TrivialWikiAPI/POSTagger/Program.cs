using java.util;

namespace POSTagger
{
    internal class Program
    {
        private static ArrayList patternList;
        private static void Main(string[] args)
        {
            //var pattern = new Pattern(@"D:\Licenta\Files\Patterns.txt");

            //SetPatterns();
            //IResourceFinder res = new ResourceFinder();
            ////var wikiResult = res.GetWikipediaRawText("League_of_Legends");
            //const string text2 = "A rare black squirrel has become a regular visitor to a suburban garden";
            //var sentences = MaxentTagger.tokenizeText(new StringReader(text2)).toArray();
            //Map dict = new HashMap();


            //// Path to the folder with models extracted from `stanford - corenlp - 3.6.0 - models.jar`
            //const string jarRoot = @"..\..\..\..\paket-files\nlp.stanford.edu\stanford-corenlp-full-2015-12-09\models";

            //// Text for processing
            //const string text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

            //// Annotation pipeline configuration
            //var props = new Properties();
            //props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
            //props.setProperty("ner.useSUTime", "0");

            //// We should change current directory, so StanfordCoreNLP could find all the model files automatically
            //var curDir = Environment.CurrentDirectory;
            //Directory.SetCurrentDirectory(jarRoot);
            //var pipeline = new StanfordCoreNLP(props);
            //Directory.SetCurrentDirectory(curDir);

            //// Annotation
            //var annotation = new Annotation(text);
            //pipeline.annotate(annotation);

            //// Result - Pretty Print
            //using (var stream = new ByteArrayOutputStream())
            //{
            //    pipeline.prettyPrint(annotation, new PrintWriter(stream));
            //    Console.WriteLine(stream.toString());
            //    stream.close();
            //}
        }

        private static void SetPatterns()
        {
            patternList = new ArrayList();

            var pattern1 = new ArrayList();
            pattern1.add("NNP");
            pattern1.add("VBZ");
            pattern1.add("NNP");
            patternList.add(pattern1);

            var pattern2 = new ArrayList();
            pattern2.add("NNP");
            pattern2.add("VB");
            pattern2.add("NNP");
            pattern2.add("NNP");
            patternList.add(pattern2);

            var pattern3 = new ArrayList();
            pattern3.add("NNP");
            pattern3.add("VBD");
            pattern3.add("NNP");
            patternList.add(pattern3);

            var pattern4 = new ArrayList();
            pattern4.add("NNP");
            pattern4.add("VBD");
            pattern4.add("NNP");
            pattern4.add("NNP");
            patternList.add(pattern4);
        }

        public static Map GetMapOfSentence(string sentence)
        {
            Map toReturn = new HashMap();
            var pairs = sentence.Split(' ');
            foreach (var pair in pairs)
            {
                var keyVal = pair.Split('/');
                toReturn.put(keyVal[0], keyVal[1]);
            }
            return toReturn;
        }

        public static ArrayList GetSentenceTags(string sentence)
        {
            var toReturn = new ArrayList();
            var pairs = sentence.Split(' ');
            foreach (var pair in pairs)
            {
                var keyVal = pair.Split('/');
                toReturn.add(keyVal[1]);
            }
            toReturn.remove(toReturn.size() - 1);
            return toReturn;
        }

        public static string GetQuestion(ArrayList sentence, Map sentenceParts, ArrayList tags)
        {
            var question = "";
            foreach (ArrayList pattern in patternList)
            {
                var toTransform = GetMatchingPart(sentence, tags, sentenceParts, pattern);
                if (toTransform.size() > 2)
                {
                    question = Transform(toTransform, pattern);
                }
            }

            return question;
        }

        public static ArrayList GetMatchingPart(ArrayList sentence, ArrayList tags, Map sentenceParts, ArrayList pattern)
        {
            var toReturn = new ArrayList();
            var startingPosition = TagsContainPattern(tags, pattern);
            if (startingPosition == -1) return toReturn;
            var size = pattern.size() + startingPosition;
            for (var i = startingPosition; i < size; i++)
            {
                toReturn.add(sentence.get(i));
            }
            return toReturn;
        }

        public static int TagsContainPattern(ArrayList tags, ArrayList pattern)
        {
            int index = 0, position = 0;
            foreach (var tag in tags)
            {
                if (tag.Equals(pattern.get(index)))
                    index++;
                else
                    index = 0;
                if (index == pattern.size())
                    return position - (index - 1);
                position++;
            }
            return -1;
        }

        public static string Transform(ArrayList sentence, ArrayList pattern)
        {
            var result = "Who ";
            var answer = sentence.get(0).ToString();
            for (var i = 1; i < sentence.size(); i++)
            {
                result += sentence.get(i).ToString() + " ";
            }
            result += "? Answer: " + answer;
            return result;
        }
    }
}