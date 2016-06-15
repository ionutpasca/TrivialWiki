using WikiTrivia.QuestionGenerator.Model;

namespace WikiTrivia.QuestionGenerator.Generators
{
    public static class BasedOnCOPQGenerator
    {
        public static GeneratedQuestion TreatSimpleCOPSentence(SentenceInformationDto sentence, WordInformationDto subject,
           SentenceDependencyDto sentenceCOP)
        {
            var subjectWord = Helper.FindWordInList(sentence.Words, subject.Word);

            var answer = AnswerGenerator.GenerateAnswer(sentence, sentenceCOP, subjectWord: subject);
            var copPartOfSpeech = Helper.FindWordInList(sentence.Words, sentenceCOP.GovernorGloss).PartOfSpeech;

            string question;

            if (subjectWord.NamedEntityRecognition.ToLower() == "person" ||
                subjectWord.PartOfSpeech.ToLower() == "nnp" ||
                subjectWord.PartOfSpeech.ToLower() == "nns")
            {
                question = $"{sentence.SentenceText.Replace(answer, "Who")}?";
                question = Helper.TrimQuestion(question, "Who");
                return new GeneratedQuestion { Answer = answer, Question = question };
            }

            question = copPartOfSpeech == "JJ" || copPartOfSpeech == "NNS" ?
                $"{sentence.SentenceText.Replace(answer, "What")}?" :
                $"{sentence.SentenceText.Replace(answer, "Which")}?";
            question = Helper.TrimQuestion(question, "What");
            question = Helper.TrimQuestion(question, "Which");
            return new GeneratedQuestion { Answer = answer, Question = question };
        }
    }
}
