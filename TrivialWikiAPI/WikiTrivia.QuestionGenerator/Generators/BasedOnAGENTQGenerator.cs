using System.Linq;
using WikiTrivia.QuestionGenerator.Model;

namespace WikiTrivia.QuestionGenerator.Generators
{
    public static class BasedOnAGENTQGenerator
    {
        public static GeneratedQuestion TreatSentenceWithAgent(SentenceInformationDto sentence, SentenceDependencyDto sentenceAGENT,
            WordInformationDto subject)
        {
            var answer = sentenceAGENT.DependentGloss;

            var subjectDet = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "det" &&
                                                                       d.GovernorGloss == subject.Word);
            var subj = subject.Word;
            if (subjectDet != null)
            {
                subj = $"{subjectDet.DependentGloss} {subject.Word}";
            }
            //var answer = AnswerGenerator.GenerateAnswer(sentence, subjectWord: subject);

            var otherAgents = sentence.Dependencies.Where(d => d.Dep.ToLower() == "nmod:agent" &&
                                                               d.GovernorGloss == sentenceAGENT.GovernorGloss &&
                                                               d.DependentGloss != answer);
            var connectionCC = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "cc" &&
                                                                         d.GovernorGloss == answer);
            if (connectionCC != null)
            {
                foreach (var agent in otherAgents)
                {
                    answer = $"{answer} {connectionCC.DependentGloss} {agent.DependentGloss}";
                }
            }

            var verbe = Helper.FindWordInList(sentence.Words, sentenceAGENT.GovernorGloss);
            var answerWord = Helper.FindWordInList(sentence.Words, sentenceAGENT.DependentGloss);
            if (answerWord.PartOfSpeech.ToLower() == "nnp" ||
                answerWord.NamedEntityRecognition.ToLower() == "person")
            {
                var question = $"Who {verbe.Word} {subj.ToLower()}";
                return new GeneratedQuestion { Answer = answer, Question = question };
            }
            return null;
        }
    }
}
