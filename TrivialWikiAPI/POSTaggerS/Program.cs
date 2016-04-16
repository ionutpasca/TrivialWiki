using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using java.io;
using java.util;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;
using Console = System.Console;

namespace POSTaggerS
{
    public class Program
    {
        static void Main(string[] args)
        {
            
        }

        public static List<string> GetPOSTag(string text)
        {
            
            var tagger = new MaxentTagger("wsj-0-18-bidirectional-nodistsim.tagger");
            var sentences = MaxentTagger.tokenizeText(new StringReader(text)).toArray();
            var toReturn = new List<string>();
            foreach (ArrayList sentence in sentences)
            {
                var taggedSentence = tagger.tagSentence(sentence);
                toReturn.Add(Sentence.listToString(taggedSentence, false));
            }
            return toReturn;
        }
    }
}