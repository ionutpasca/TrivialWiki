using System.Collections.Generic;

namespace POSTagger
{
    public class SentenceInformation
    {
        public SentenceInformation(string text, ParseTree tree, List<SentenceDependency> dependencies)
        {
            SentenceText = text;
            Tree = tree;
            Dependencies = dependencies;
        }

        public string SentenceText { get; set; }
        public ParseTree Tree { get; set; }
        public List<SentenceDependency> Dependencies { get; set; }
    }
}
