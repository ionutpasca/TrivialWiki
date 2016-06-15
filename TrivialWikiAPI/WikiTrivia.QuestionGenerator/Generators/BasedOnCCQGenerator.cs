﻿using WikiTrivia.QuestionGenerator.Model;

namespace WikiTrivia.QuestionGenerator.Generators
{
    public static class BasedOnCCQGenerator
    {
        public static GeneratedQuestion TreatSimpleCCSentence(SentenceInformationDto sentence, WordInformationDto subject)
        {
            var subjectWord = Helper.FindWordInList(sentence.Words, subject.Word);

            //var answer = subjectWord.Lemma;

            //var sentenceAMOD = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "amod");
            //if (sentenceAMOD != null && sentenceAMOD.GovernorGloss == subjectWord.Word)
            //{
            //    answer = $"{sentenceAMOD.DependentGloss} {answer}";
            //}
            //var subjectDET = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "det");
            //if (subjectDET != null && subjectDET.GovernorGloss == subjectWord.Word)
            //{
            //    answer = $"{subjectDET.DependentGloss} {answer}";
            //}
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
