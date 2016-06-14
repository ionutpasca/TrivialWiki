using edu.stanford.nlp.pipeline;
using java.io;
using java.util;
using Newtonsoft.Json.Linq;
using POSTagger.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Console = System.Console;

namespace POSTagger
{
    public class TextProcessing
    {
        private static readonly string stanfordJarRoot = ConfigurationManager.AppSettings["Stanford.JarRoot"];
        private static readonly string outputJsonPath = ConfigurationManager.AppSettings["Tagger.OutputJson"];
        private static readonly string outputTestPath = ConfigurationManager.AppSettings["Tagger.outputTest"];
        private static readonly string outputTestJsonPath = ConfigurationManager.AppSettings["Tagger.OutputTestJson"];

        private static ArrayList _patternList;
        //private const string JarRoot = @"D:\Licenta\stanford-corenlp-full-2015-12-09";

        public TextProcessing()
        {

        }
        public void ProcessText(string text)
        {
            var props = new Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse");
            props.setProperty("ner.useSUTime", "0");

            // We should change current directory, so StanfordCoreNLP could find all the model files automatically
            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(stanfordJarRoot);
            var pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);
            Console.WriteLine("Starting to parse.");
            // Annotation
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            // Result - Pretty Print
            Console.WriteLine("Parsing complete.. writing to file.");
            string jsonOutput;
            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.jsonPrint(annotation, new PrintWriter(stream));
                jsonOutput = stream.toString();
                stream.close();
            }

            using (var file = new System.IO.StreamWriter(outputJsonPath))
            {
                file.WriteLine(jsonOutput);

            }
            Console.WriteLine("Processing complete.");
        }

        public static void ProcessJson()
        {

        }

        public static void FillInTheGaps()
        {
            int nrOfQuestions = 0, nrOfSentences = 0;
            var jsonOutput = System.IO.File.ReadAllText(outputJsonPath);

            var joText = JObject.Parse(jsonOutput);
            var joSentences = (JArray)joText["sentences"];
            var file = new StreamWriter(outputTestPath);
            var fileJson = new StreamWriter(outputTestJsonPath);
            var jsonArray = new JArray();
            var sentenceForest = new ArrayList();

            foreach (var jToken in joSentences)
            {
                var sentence = (JObject)jToken;
                nrOfSentences++;
                var tokens = sentence.GetValue("tokens");
                var bd = sentence.GetValue("collapsed-ccprocessed-dependencies");
                var answer = "";

                foreach (JObject word in bd)
                {
                    var dep = word.GetValue("dep");
                    var value = dep.ToString();
                    if (value.Equals("nsubj"))
                    {
                        answer = word.GetValue("dependentGloss").ToString();
                    }

                }
                if (IsNotUseful(answer))
                    continue;
                var originalSentence = "";
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
                var jsonObj = new JObject { { "index", nrOfQuestions }, { "Question", originalSentence }, { "Answer", answer } };
                jsonArray.Add(jsonObj);
                originalSentence = nrOfQuestions + ":" + originalSentence;
                file.WriteLine(originalSentence + "Answer:" + answer);
                nrOfQuestions++;
            }
            Console.WriteLine("Sentences:" + nrOfSentences);
            Console.WriteLine("Questions:" + nrOfQuestions);
            var jsonQuestions = new JObject { { "Questions", jsonArray } };
            fileJson.Write(jsonQuestions.ToString());
            file.Close();
            fileJson.Close();
        }


        public List<SentenceInformation> GetSentencesInformationFromJson()
        {
            var jsonOutput = System.IO.File.ReadAllText(outputJsonPath);
            var joText = JObject.Parse(jsonOutput);
            var joSentences = (JArray)joText["sentences"];

            var sentencesInformation = new List<SentenceInformation>();
            foreach (var jToken in joSentences)
            {
                var sentence = (JObject)jToken;
                var sentenceProc = new SentenceProcessing(sentence);
                var sentenceInfo = new SentenceInformation(sentenceProc.GetSentenceText(), sentenceProc.GetDependencies(), sentenceProc.GetWordInformation());
                sentencesInformation.Add(sentenceInfo);
            }
            return sentencesInformation;
        }

        private static bool IsNotUseful(string answer)
        {
            const string pronouns = "I, me, he, she, herself, you, it, that, they, this, each, few, many, which, who, whoever, whose, someone, everybody, " +
                                    "Me, He, She, Herself, You, It, That, They, This, Each, Few, Many, Which, Who, Whoever, Whose, Someone, Everybody";
            var toreturn = pronouns.Contains(answer) || answer.Equals("");
            return toreturn;
        }
        private static bool IsSubject(string subjects, string word)
        {
            var list = subjects.Split(',');
            return list.Any(elem => elem.Equals(word));
        }

        public static string GetQuestion(ArrayList sentence, Map sentenceParts, ArrayList tags)
        {
            var question = "";
            foreach (ArrayList pattern in _patternList)
            {
                var toTransform = SentenceUtils.GetMatchingPart(sentence, tags, sentenceParts, pattern);
                if (toTransform.size() > 2)
                {
                    question = Transform(toTransform, pattern);
                }
            }

            return question;
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
