using System.Collections.Generic;

namespace POSTagger.Model
{
    public class SentenceInformation
    {
        public SentenceInformation(string text, List<SentenceDependency> dependencies, List<WordInformation> words)
        {
            SentenceText = text;
            Dependencies = dependencies;
            Words = words;
        }

        public string SentenceText { get; set; }
        public List<SentenceDependency> Dependencies { get; set; }
        public List<WordInformation> Words { get; set; }

    }
}
