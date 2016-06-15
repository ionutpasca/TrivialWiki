using System.Linq;
using WikiTrivia.QuestionGenerator.Model;

namespace WikiTrivia.QuestionGenerator.Generators
{
    public static class BasedOnXCOMPQGenerator
    {
        public static GeneratedQuestion TreatSimpleXCOMPSentence(SentenceInformationDto sentence, SentenceDependencyDto subjectFromRes,
            WordInformationDto subject, SentenceDependencyDto sentenceXCOMP)
        {
            //THIS IS FOR ANSWER
            //var sentenceADVMOD = result[0].Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "advmod");
            //if (sentenceADVMOD != null)
            //{
            //    var advmodWord = FindWordInList(result[0].Words, sentenceXCOMP.GovernorGloss);
            //}
            var subjectPossession = Helper.GetSubjectPossession(sentence);

            var answer = AnswerGenerator.GenerateAnswer(sentence, subjectFromRes, subjectWord: subject);
            var predicate = sentence.Words.FirstOrDefault(w => w.Word == subjectFromRes.GovernorGloss);
            var wordInResult = sentence.Words.FirstOrDefault(w => w.Word == sentenceXCOMP.DependentGloss);
            if (wordInResult == null)
            {
                return null;
            }

            var pos = wordInResult.PartOfSpeech;
            if (pos == "JJ")
            {
                if (subjectPossession != null)
                {
                    var question = $"How does {subjectPossession} {subject.Lemma} {predicate.Lemma}?";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
                else
                {
                    var question = $"How does {subject.Lemma} {predicate.Lemma}?";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
            }
            return null;
        }
    }
}
