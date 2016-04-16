using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;
using java.io;
using java.util;
using WikipediaResourceFinder;
using Console = System.Console;

namespace POSTagger
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var tagger = new MaxentTagger("wsj-0-18-bidirectional-nodistsim.tagger");

            IResourceFinder res = new ResourceFinder();
            res.GetWikipediaRawText("Faltceva");

            var text = "A Part-Of-Speech Tagger (POS Tagger) is a piece of software that reads text"
                       + "in some language and assigns parts of speech to each word (and other token),"
                       + " such as noun, verb, adjective, etc., although generally computational "
                       + "applications use more fine-grained POS tags like 'noun-plural'.";

            var sentences = MaxentTagger.tokenizeText(new StringReader(text)).toArray();
            foreach (ArrayList sentence in sentences)
            {
                var taggedSentence = tagger.tagSentence(sentence);
                Console.WriteLine(Sentence.listToString(taggedSentence, false));
            }
            Console.ReadLine();
        }
    }
}