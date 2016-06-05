using edu.stanford.nlp.pipeline;
using java.io;
using java.util;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using WikipediaResourceFinder;
using Console = System.Console;

namespace POSTagger
{
    internal class Program
    {
        private static ArrayList patternList;
        private const string _jarRoot = @"D:\Licenta\stanford-corenlp-full-2015-12-09";
        private static void Main(string[] args)
        {
            //var pattern = new Pattern(@"D:\Licenta\Files\Patterns.txt");

            //SetPatterns();
            IResourceFinder res = new ResourceFinder();
            //var text = res.GetWikipediaRawText("superman");
            var text = "Bob likes books.";
            //const string text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply. Superman is one of DC's most important superheroes.";
            ProcessText(text);
            ProcessJson();
            Console.ReadLine();

        }

        public static void ProcessText(string text)
        {

            // Annotation pipeline configuration

            var props = new Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse");
            props.setProperty("ner.useSUTime", "0");


            // We should change current directory, so StanfordCoreNLP could find all the model files automatically
            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(_jarRoot);
            var pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

            // Annotation
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);
            // Result - Pretty Print
            string conllOutput;
            string jsonOutput;
            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.jsonPrint(annotation, new PrintWriter(stream));
                jsonOutput = stream.toString();
                stream.close();
            }

            using (var file = new System.IO.StreamWriter(@"D:\Licenta\Files\OutputJson.txt"))
            {
                file.WriteLine(jsonOutput);

            }

        }

        private static void ProcessJson()
        {
            int nrOfQuestions = 0, nrOfSentences = 0;
            var jsonOutput = System.IO.File.ReadAllText(@"D:\Licenta\Files\OutputJson.txt");
            var subjects = "";
            var roots = "";

            var joText = JObject.Parse(jsonOutput);
            var joSentences = (JArray)joText["sentences"];
            var file = new System.IO.StreamWriter(@"D:\Licenta\Files\OutputTest.txt");

            foreach (JObject sentence in joSentences)
            {
                nrOfSentences++;
                var tokens = sentence.GetValue("tokens");
                var index = sentence.GetValue("index");
                Console.WriteLine(index);

                var bd = sentence.GetValue("basic-dependencies");
                string answer = "";

                foreach (JObject word in bd)
                {
                    JToken dep = word.GetValue("dep");
                    var value = dep.ToString();
                    if (value.Equals("ROOT"))
                        roots += word.GetValue("dependentGloss") + ",";
                    if (value.Equals("nsubj"))
                    {
                        subjects += word.GetValue("dependentGloss") + ",";
                        answer = word.GetValue("dependentGloss").ToString();
                    }

                }
                if (IsNotUseful(answer))
                    continue;
                string originalSentence = "";
                foreach (JObject word in tokens)
                {
                    var txt = word.GetValue("word").ToString();
                    if (txt.Equals(answer))
                    {
                        originalSentence += "____________ ";
                        continue;
                    }
                    originalSentence += txt + word.GetValue("after");
                }
                originalSentence = nrOfQuestions + ":" + originalSentence;
                file.WriteLine(originalSentence + "Answer:" + answer);
                nrOfQuestions++;
                //Console.WriteLine(originalSentence);

            }
            //Console.WriteLine(roots);
            //Console.WriteLine(subjects);
            Console.WriteLine("Sentences:" + nrOfSentences);
            Console.WriteLine("Questions:" + nrOfQuestions);

        }

        private static bool IsNotUseful(string answer)
        {
            string pronouns = "I, me, he, she, herself, you, it, that, they, each, few, many, who, whoever, whose, someone, everybody, Me, He, She, Herself, You, It, That, They, Each, Few, Many, Who, Whoever, Whose, Someone, Everybody";
            bool toreturn = pronouns.Contains(answer) || answer.Equals("");
            return toreturn;
        }
        private static bool IsSubject(string subjects, string word)
        {
            var list = subjects.Split(',');
            return list.Any(elem => elem.Equals(word));
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