using System.Collections.Generic;

namespace POSTagger
{
    public class SentenceInformation
    {
        public SentenceInformation(string text, ParseTree tree, List<SentenceDependency> dependencies, List<WordInformation> words)
        {
            SentenceText = text;
            Tree = tree;
            Dependencies = dependencies;
            Words = words;
        }

        public string SentenceText { get; set; }
        public ParseTree Tree { get; set; }
        public List<SentenceDependency> Dependencies { get; set; }
        public List<WordInformation> Words { get; set; }

        public string GetTokens()
        {
            return Tree.GetTokens();
        }
    }
}
