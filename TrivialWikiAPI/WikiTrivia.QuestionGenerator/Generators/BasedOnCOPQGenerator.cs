using System.Linq;
using System.Text.RegularExpressions;
using WikiTrivia.QuestionGenerator.Model;

namespace WikiTrivia.QuestionGenerator.Generators
{
    public static class BasedOnCOPQGenerator
    {
        public static GeneratedQuestion TreatSimpleCOPSentence(SentenceInformationDto sentence, WordInformationDto subject,
           SentenceDependencyDto sentenceCOP)
        {
            var copPartOfSpeech = Helper.FindWordInList(sentence.Words, sentenceCOP.GovernorGloss).PartOfSpeech;
            var copVerbe = Helper.FindWordInList(sentence.Words, sentenceCOP.DependentGloss);
            var baseAnswer = Helper.FindWordInList(sentence.Words, sentenceCOP.GovernorGloss);
            var subjectWord = Helper.FindWordInList(sentence.Words, subject.Word);

            if (MustReturnNull(subjectWord))
            {
                return null;
            }
            var answer = AnswerGenerator.GenerateAnswer(sentence, sentenceCOP, subjectWord:subjectWord);

            var firstWord = sentence.Words.First();
            string question;

            if (baseAnswer.NamedEntityRecognition.ToLower() == "person" && copVerbe.Lemma == "be")
            {
                return TreatCaseWhereAnswerIsPerson(sentence, firstWord, copVerbe, answer);
            }

            if (subjectWord.PartOfSpeech.ToLower() == "nn")
            {
                return TreatCaseWhereAnswerIsObject(sentence, answer);
            }

            if (copVerbe != null && copVerbe.Lemma == "be")
            {
                question = $"What {copVerbe.Word} {subjectWord.Word}";
                return new GeneratedQuestion { Answer = answer, Question = question };
            }

            if (subjectWord.NamedEntityRecognition.ToLower() == "person" ||
                subjectWord.PartOfSpeech.ToLower() == "nnp" ||
                subjectWord.PartOfSpeech.ToLower() == "nns")
            {
                question = $"{sentence.SentenceText.Replace(answer, "Who")}";
                question = Helper.TrimQuestion(question, "Who");
                return new GeneratedQuestion { Answer = answer, Question = question };
            }

            if (baseAnswer.NamedEntityRecognition.ToLower() == "person" ||
                baseAnswer.PartOfSpeech.ToLower() == "nnp")
            {
                return TreatCaseWithPersonAndVerbeNot_IS_(sentence, sentenceCOP, answer);
            }

            question = copPartOfSpeech == "JJ" || copPartOfSpeech == "NNS" ?
                $"{sentence.SentenceText.Replace(answer, "What")}" :
                $"{sentence.SentenceText.Replace(answer, "Which")}";

            if (sentence.SentenceText + "?" == question)
            {
                return null;
            }
            question = Helper.TrimQuestion(question, "What");
            question = Helper.TrimQuestion(question, "Which");
            return new GeneratedQuestion { Answer = answer, Question = question };
        }

        private static bool MustReturnNull(WordInformationDto wordInfo)
        {
            return wordInfo == null || wordInfo.PartOfSpeech.ToLower() == "prp" ||
                   wordInfo.PartOfSpeech.ToLower() == "nnps";
        }

        private static GeneratedQuestion TreatCaseWithPersonAndVerbeNot_IS_(SentenceInformationDto sentence,
            SentenceDependencyDto sentenceCOP, string answer)
        {
            var questionText = Helper.TrimQuestionAfter(sentence.SentenceText, answer);
            questionText = questionText.Replace(answer, "")
                .Replace(sentenceCOP.DependentGloss, "");
            string question = $"Who {sentenceCOP.DependentGloss} {questionText}";
            return new GeneratedQuestion { Answer = answer, Question = question };
        }

        private static GeneratedQuestion TreatCaseWhereAnswerIsPerson(SentenceInformationDto sentence,
            WordInformationDto firstWord, WordInformationDto copVerbe, string answer)
        {
            var questionText = sentence.SentenceText
                .Replace(firstWord.Word, firstWord.Lemma)
                .Replace(copVerbe.Word, "")
                .Replace(answer, "");
            string question = $"Who {copVerbe.Word} {questionText}";
            return new GeneratedQuestion { Answer = answer, Question = question };
        }

        private static GeneratedQuestion TreatCaseWhereAnswerIsObject(SentenceInformationDto sentence, string answer)
        {
            var answerWords = answer.Split(' ');
            string questionText = null;
            foreach (var answerWord in answerWords)
            {
                questionText = sentence.SentenceText
                .Replace(answerWord, "");
            }
            string question = $"What {questionText}";
            return new GeneratedQuestion { Answer = answer, Question = question };
        }
    }
}
