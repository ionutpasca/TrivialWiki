using WikiTrivia.QuestionGenerator.Model;
// ReSharper disable StringIndexOfIsCultureSpecific.1

namespace WikiTrivia.QuestionGenerator.Generators
{
    public static class BasedOnSubjectQGenerator
    {
        public static GeneratedQuestion CreateQuestionBasedOnSubject(SentenceInformationDto sentence, SentenceDependencyDto subjectFromSentence,
           WordInformationDto subject)
        {
            if (subjectFromSentence == null)
            {
                return null;
            }
            var subjectRelationWord = Helper.FindWordInList(sentence.Words, subjectFromSentence.GovernorGloss);
            var answer = AnswerGenerator.GenerateAnswer(sentence, wordInformation: subject);

            if (subjectRelationWord.PartOfSpeech == "VBZ" ||
                subjectRelationWord.PartOfSpeech == "VBN" ||
                subjectRelationWord.PartOfSpeech == "VBD")
            {
                string question;

                if (subject.NamedEntityRecognition.ToLower() == "person" ||
                    subject.PartOfSpeech.ToLower() == "prp" ||
                    subject.PartOfSpeech.ToLower() == "nnp")
                {
                    question = sentence.SentenceText.Replace(answer, "Who");
                    question = Helper.TrimQuestion(question, "Who");
                }
                else
                {
                    question = sentence.SentenceText.Replace(answer, "What");
                    question = Helper.TrimQuestion(question, "What");
                }
                return new GeneratedQuestion { Answer = answer, Question = question };
            }
            if (subjectRelationWord.PartOfSpeech == "NN" ||
                subjectRelationWord.PartOfSpeech == "NNS" ||
                subjectRelationWord.PartOfSpeech == "NNP" ||
                subjectRelationWord.PartOfSpeech == "NNPS")
            {

            }

            return null;
        }
    }
}
