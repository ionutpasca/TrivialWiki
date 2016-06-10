using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace POSTagger
{
    public class SentenceProcessing
    {
        public JObject Sentence { get; set; }

        public SentenceProcessing(JObject Sentence)
        {
            this.Sentence = Sentence;
        }

        public List<SentenceDependency> GetDependencies()
        {
            var basicDep = Sentence.GetValue("collapsed-ccprocessed-dependencies");
            var dependencies = new List<SentenceDependency>();
            foreach (JObject word in basicDep)
            {
                var dep = word.GetValue("dep").ToString();
                var governor = word.GetValue("governor").ToString();
                var governorGloss = word.GetValue("governorGloss").ToString();
                var dependent = word.GetValue("dependent").ToString();
                var dependentGloss = word.GetValue("dependentGloss").ToString();
                var sentenceDep = new SentenceDependency(dep, governor, governorGloss, dependent, dependentGloss);
                dependencies.Add(sentenceDep);
            }
            return dependencies;
        }

        public ParseTree GetParseTree()
        {
            // ParseTree
            var parse = Sentence.GetValue("parse");
            var toParse = new string(parse.ToString().ToCharArray());
            var tree = new ParseTree(StringUtils.ListParse(toParse), 0);
            tree.ParseSubTrees();
            return tree;
        }

        public string GetSentenceText()
        {
            var tokens = Sentence.GetValue("tokens");
            var originalSentence = "";
            foreach (JObject word in tokens)
            {
                var txt = word.GetValue("word").ToString();
                originalSentence += txt + word.GetValue("after");
            }
            return originalSentence;
        }

        public List<WordInformation> GetWordInformation()
        {
            var tokens = Sentence.GetValue("tokens");
            var result = new List<WordInformation>();
            foreach (JObject word in tokens)
            {
                var index = int.Parse(word.GetValue("index").ToString());
                var text = word.GetValue("word").ToString();
                var pos = word.GetValue("pos").ToString();
                var ner = word.GetValue("ner").ToString();
                result.Insert(index, new WordInformation(text, pos, ner));
            }
            return result;
        }
    }
}
