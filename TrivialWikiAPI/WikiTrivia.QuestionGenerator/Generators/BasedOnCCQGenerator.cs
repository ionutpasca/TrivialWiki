using WikiTrivia.QuestionGenerator.Model;

namespace WikiTrivia.QuestionGenerator.Generators
{
    public static class BasedOnCCQGenerator
    {
        public static GeneratedQuestion TreatSimpleCCSentence(SentenceInformationDto sentence, WordInformationDto subject)
        {
            if (subject.Word == null)
            {
                return null;
            }
            var subjectWord = Helper.FindWordInList(sentence.Words, subject.Word);

            string question;
            var answer = AnswerGenerator.GenerateAnswer(sentence, subjectWord: subjectWord);

            if (subjectWord.PartOfSpeech.ToLower() == "nnp" ||
                     subjectWord.PartOfSpeech.ToLower() == "nns" ||
                     subjectWord.PartOfSpeech.ToLower() == "prp" ||
                     subjectWord.NamedEntityRecognition.ToLower() == "person")
            {
                question = $"{sentence.SentenceText.Replace(answer, "Who")}?";
                question = Helper.TrimQuestion(question, "Who");
            }
            else
            {
                question = $"{sentence.SentenceText.Replace(answer, "Who/What")}?";
                question = Helper.TrimQuestion(question, "Who/What");
            }
            return new GeneratedQuestion { Answer = answer, Question = question };
        }
    }
}
