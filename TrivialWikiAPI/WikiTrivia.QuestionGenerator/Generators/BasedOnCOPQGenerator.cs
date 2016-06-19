using WikiTrivia.QuestionGenerator.Model;

namespace WikiTrivia.QuestionGenerator.Generators
{
    public static class BasedOnCOPQGenerator
    {
        public static GeneratedQuestion TreatSimpleCOPSentence(SentenceInformationDto sentence, WordInformationDto subject,
           SentenceDependencyDto sentenceCOP)
        {

            var subjectWord = Helper.FindWordInList(sentence.Words, subject.Word);
            if (subjectWord == null || subject.PartOfSpeech.ToLower() == "prp" ||
                subject.PartOfSpeech.ToLower() == "nnps")
            {
                return null;
            }
            var answer = TreatSentenceCOP(sentence, subject, sentenceCOP);

            var copPartOfSpeech = Helper.FindWordInList(sentence.Words, sentenceCOP.GovernorGloss).PartOfSpeech;
            var baseAnswer = Helper.FindWordInList(sentence.Words, sentenceCOP.GovernorGloss);
            string question;

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
                var questionText = Helper.TrimQuestionAfter(sentence.SentenceText, answer);
                questionText = questionText.Replace(answer, "")
                                    .Replace(sentenceCOP.DependentGloss, "");
                question = $"Who {sentenceCOP.DependentGloss} {questionText}";
                return new GeneratedQuestion { Answer = answer, Question = question };
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

        private static string TreatSentenceCOP(SentenceInformationDto sentence, WordInformationDto subject, SentenceDependencyDto sentenceCOP)
        {
            var sentenceCOPWord = Helper.FindWordInList(sentence.Words, sentenceCOP.GovernorGloss);
            var answer = sentenceCOPWord.NamedEntityRecognition.ToLower() == "person" ?
                AnswerGenerator.GenerateAnswer(sentence, sentenceCOP) :
                AnswerGenerator.GenerateAnswer(sentence, subjectWord: subject);
            return answer;
        }
    }
}
