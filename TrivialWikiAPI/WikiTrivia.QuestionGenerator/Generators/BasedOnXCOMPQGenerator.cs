using System.Linq;
using WikiTrivia.QuestionGenerator.Model;

namespace WikiTrivia.QuestionGenerator.Generators
{
    public static class BasedOnXCOMPQGenerator
    {
        public static GeneratedQuestion TreatSimpleXCOMPSentence(SentenceInformationDto sentence, SentenceDependencyDto subjectFromRes,
            WordInformationDto subject, SentenceDependencyDto sentenceXCOMP)
        {
            var subjectPossession = Helper.GetSubjectPossession(sentence);

            var answer = AnswerGenerator.GenerateAnswer(sentence, subjectFromRes, subjectWord: subject);
            var predicate = sentence.Words.FirstOrDefault(w => w.Word == subjectFromRes.GovernorGloss);
            var wordInResult = sentence.Words.FirstOrDefault(w => w.Word == sentenceXCOMP.DependentGloss);
            if (wordInResult == null || predicate == null)
            {
                return null;
            }

            var pos = wordInResult.PartOfSpeech;
            if (pos != "JJ")
            {
                return null;
            }

            if (subjectPossession != null)
            {
                var question = $"How does {subjectPossession} {subject.Lemma} {predicate.Lemma}";
                return new GeneratedQuestion { Answer = answer, Question = question };
            }
            else
            {
                var question = $"How does {subject.Lemma} {predicate.Lemma}";
                return new GeneratedQuestion { Answer = answer, Question = question };
            }
        }
    }
}
