using WikiTrivia.QuestionGenerator.Model;
// ReSharper disable InvertIf

namespace WikiTrivia.QuestionGenerator.Generators
{
    public static class BasedOnNSUBJQGenerator
    {
        public static GeneratedQuestion TreatSentenceWithNSUBJ(SentenceInformationDto sentence, SentenceDependencyDto sentenceNSUBJ,
            WordInformationDto subject)
        {
            var subjectRelation = Helper.FindWordInList(sentence.Words, sentenceNSUBJ.GovernorGloss);

            var answer = AnswerGenerator.GenerateAnswer(sentence, subjectWord: subject);
            if (subjectRelation.PartOfSpeech.ToLower() == "vbz")
            {
                string question;
                if (subject.NamedEntityRecognition.ToLower() == "person" ||
                    subject.PartOfSpeech.ToLower() == "prp" ||
                    subject.PartOfSpeech.ToLower() == "prp$")
                {
                    var questionText = sentence.SentenceText.Replace(answer, "Who ");
                    question = $"{questionText}?";
                    question = Helper.TrimQuestion(question, "Who");
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
                if (subject.NamedEntityRecognition.ToLower() != "o")
                {
                    var questionText = sentence.SentenceText.Replace(answer, "Which ");
                    question = $"{questionText}?";
                    question = Helper.TrimQuestion(question, "Which");
                }
                else
                {
                    var questionText = sentence.SentenceText.Replace(answer, "What ");
                    question = $"{questionText}?";
                    question = Helper.TrimQuestion(question, "What");
                }
                return new GeneratedQuestion { Answer = answer, Question = question };
            }
            return null;
        }
    }
}
