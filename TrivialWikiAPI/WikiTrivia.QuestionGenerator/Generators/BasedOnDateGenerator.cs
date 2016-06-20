using System.Linq;
using WikiTrivia.QuestionGenerator.Model;

namespace WikiTrivia.QuestionGenerator.Generators
{
    public static class BasedOnDateGenerator
    {
        public static GeneratedQuestion TreatDateSentence(SentenceInformationDto sentence, WordInformationDto sentenceDate,
            SentenceDependencyDto subjectFromSentence)
        {
            var composedAnswer = AnswerGenerator.GenerateAnswer(sentence, subjectWord: sentenceDate);
            var sentencesCases = GetSentenceCases(sentence, sentenceDate, composedAnswer);
            var firstWord = sentence.Words.First();
            var question = sentence.SentenceText
                .Replace(sentencesCases, "");

            if (question.Contains(firstWord.Word) && !WordIsName(firstWord))
            {
                question = question.Replace(firstWord.Word, firstWord.Lemma);
            }

            if (subjectFromSentence != null)
            {
                return GenerateQuestion(sentence, sentenceDate, subjectFromSentence);
            }

            question = $"When did {question}";
            return new GeneratedQuestion { Answer = composedAnswer, Question = question };
        }

        private static GeneratedQuestion GenerateQuestion(SentenceInformationDto sentence, WordInformationDto sentenceDate,
            SentenceDependencyDto subjectFromSentence)
        {
            var question = string.Empty;
            var verbeWord = Helper.FindWordInList(sentence.Words, subjectFromSentence.GovernorGloss);
            var verbeAuxPass = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "auxpass" &&
                                                                         d.GovernorGloss == verbeWord.Word);
            if (verbeWord.PartOfSpeech.ToLower() == "vbd")
            {
                question = question.Replace(verbeWord.Word, verbeWord.Lemma);
            }

            var verbeAux = sentence.Dependencies.FirstOrDefault(d =>
                    d.Dep.ToLower() == "aux" &&
                    d.GovernorGloss == verbeWord.Word);

            if (verbeAux != null)
            {
                question = question.Replace(verbeAux.DependentGloss, "");
                question = $"When {verbeAux.DependentGloss} {question}";
                return new GeneratedQuestion { Answer = sentenceDate.Word, Question = question };
            }

            if (verbeAuxPass != null)
            {
                question = question.Replace(verbeAuxPass.DependentGloss, "");
                question = $"When {verbeAuxPass.DependentGloss} {question}";
                return new GeneratedQuestion { Answer = sentenceDate.Word, Question = question };
            }
            return null;
        }

        private static bool WordIsName(WordInformationDto word)
        {
            return word.NamedEntityRecognition.ToLower() == "person" ||
                   word.NamedEntityRecognition.ToLower() == "location";
        }

        private static string GetSentenceCases(SentenceInformationDto sentence, WordInformationDto sentenceDate,
            string composedAnswer)
        {
            var answerCases = sentence.Dependencies
                .Where(d => d.Dep == "case" && d.GovernorGloss == sentenceDate.Word)
                .ToList();

            var wordsToReplace = composedAnswer;
            foreach (var ansCase in answerCases)
            {
                wordsToReplace = Helper.ElementIsBeforeWord(sentence.Words, ansCase.DependentGloss, sentenceDate.Word)
                    ? $"{ansCase.DependentGloss} {composedAnswer}"
                    : $"{composedAnswer} {ansCase.DependentGloss}";
            }
            return wordsToReplace;
        }
    }
}
