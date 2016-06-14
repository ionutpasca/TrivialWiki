using System.Collections.Generic;
using System.Linq;

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

        public SentenceDependency GetDependency(string dependent)
        {
            return Dependencies.FirstOrDefault(dep => dep.Dependent.Equals(dependent));
        }

        public SentenceDependency GetSubject()
        {
            foreach (var dep in Dependencies)
            {
                if (!dep.Dep.Equals("nsubj")) continue;
                var word = Words.FirstOrDefault(w => w.Index == int.Parse(dep.Dependent));
                return word != null && AcceptPartOfSpeech(word.PartOfSpeech) ? dep : null;
            }
            return null;
        }

        public SentenceDependency GetSubjectNer()
        {
            foreach (var dep in Dependencies)
            {
                if (!dep.Dep.Equals("nsubj")) continue;
                var word = Words.FirstOrDefault(w => w.Index == int.Parse(dep.Dependent));
                return word != null && AcceptPartOfSpeech(word.PartOfSpeech) && !word.NamedEntityRecognition.Equals("O") ? dep : null;
            }
            return null;
        }

        public List<SentenceDependency> GetCompoundSubject()
        {
            return null;
        }

        public List<int> GetCompoundIndex(string governor, string subjectIndex)
        {
            return (from dep in Dependencies where IsCompoundSubject(dep, subjectIndex) select int.Parse(dep.Dependent)).ToList();
        }

        public bool IsCompoundSubject(SentenceDependency dep, string subjectIndex)
        {
            if (!dep.Dep.Equals("compound")) return false;
            return dep.Governor.Equals(subjectIndex) || IsCompoundSubject(GetDependency(dep.Governor), subjectIndex);
        }

        public Dictionary<int, bool> GetSentenceFormat()
        {
            return null;
        }

        private bool AcceptPartOfSpeech(string pos)
        {
            return StringUtils.AcceptedPos.Contains(pos);
        }
    }
}