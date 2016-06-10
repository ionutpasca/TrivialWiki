using Newtonsoft.Json.Linq;
using POSTagger.Model;
using System.Collections.Generic;

namespace POSTagger
{
    public class SentenceProcessing
    {
        public JObject Sentence { get; set; }

        public SentenceProcessing(JObject sentence)
        {
            this.Sentence = sentence;
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
                var lemma = word.GetValue("lemma").ToString();
                result.Add(new WordInformation(text, pos, ner, lemma));
            }
            return result;
        }
    }
}
