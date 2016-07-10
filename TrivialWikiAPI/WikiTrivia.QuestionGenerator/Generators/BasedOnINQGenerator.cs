using System.Linq;
using WikiTrivia.QuestionGenerator.Model;

namespace WikiTrivia.QuestionGenerator.Generators
{
    public static class BasedOnINQGenerator
    {
        public static GeneratedQuestion TreatSentenceWithIN(SentenceInformationDto sentence, SentenceDependencyDto sentenceIN,
            SentenceDependencyDto subjectFromSentence, WordInformationDto subject)
        {
            var inWord = Helper.FindWordInList(sentence.Words, sentenceIN.DependentGloss);
            if (inWord.PartOfSpeech.ToLower() != "nn")
            {
                return null;
            }
            var nounOF = sentence.Dependencies.FirstOrDefault(d => d.Dep == "nmod:of" &&
                                                                   d.GovernorGloss == inWord.Word);
            if (nounOF == null || subjectFromSentence == null)
            {
                return null;
            }
            var answer = nounOF.DependentGloss;
            var verbe = Helper.FindWordInList(sentence.Words, subjectFromSentence.GovernorGloss);
            var verbeAND = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "conj:and" &&
                                                                     d.GovernorGloss == verbe.Word);

            if (verbeAND != null)
            {
                var verbeAndWord = Helper.FindWordInList(sentence.Words, verbeAND.DependentGloss);

                if (verbe.PartOfSpeech.ToLower() == "vbz")
                {
                    var question = $"Where does {subject.Word} {verbeAndWord.Word} and {verbe.Word}";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
                else
                {
                    var question = $"Where did {subject.Word} {verbeAndWord.Lemma} and {verbe.Lemma}";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
            }
            if (verbe.PartOfSpeech.ToLower() == "vbz")
            {
                var question = $"Where does {subject.Word}  {verbe.Word}";
                return new GeneratedQuestion { Answer = answer, Question = question };
            }
            else
            {
                var question = $"Where did {subject.Word} {verbe.Lemma}";
                return new GeneratedQuestion { Answer = answer, Question = question };
            }
        }
    }
}
