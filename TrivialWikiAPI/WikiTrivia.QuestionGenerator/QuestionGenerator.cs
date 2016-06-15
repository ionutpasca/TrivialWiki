using System.Linq;
using WikiTrivia.QuestionGenerator.Generators;
using WikiTrivia.QuestionGenerator.Model;
// ReSharper disable ConvertIfStatementToReturnStatement

namespace WikiTrivia.QuestionGenerator
{
    public static class QuestionGenerator
    {
        public static GeneratedQuestion Generate(SentenceInformationDto sentence)
        {
            var subjectFromSentence = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nsubjpass") ??
                                      sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nsubj");

            var subject = new WordInformationDto();
            if (subjectFromSentence != null)
            {
                subject = Helper.FindWordInList(sentence.Words, subjectFromSentence.DependentGloss);
            }

            var sentenceDOBJ = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "dobj");
            if (sentenceDOBJ != null && sentence.Words.Count() <= 10)
            {
                return BasedOnDOBJQGenerator.TreatSentenceWithDOBJ(sentence, sentenceDOBJ);
            }

            var sentenceCOP = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "cop");
            if (sentenceCOP != null)
            {
                return BasedOnCOPQGenerator.TreatSimpleCOPSentence(sentence, subject, sentenceCOP);
            }

            var questionBasedOnSubject = BasedOnSubjectQGenerator.CreateQuestionBasedOnSubject(sentence, subjectFromSentence, subject);
            if (questionBasedOnSubject != null)
            {
                return questionBasedOnSubject;
            }

            var sentenceNSUBJ = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nsubj");
            var sentenceNSUBJPASS = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nsubjpass");

            if (sentenceNSUBJ != null && sentenceNSUBJPASS == null)
            {
                return BasedOnNSUBJQGenerator.TreatSentenceWithNSUBJ(sentence, sentenceNSUBJ, subject);
            }

            var sentenceXCOMP = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "xcomp");
            if (sentenceXCOMP != null)
            {
                return BasedOnXCOMPQGenerator.TreatSimpleXCOMPSentence(sentence, subjectFromSentence, subject, sentenceXCOMP);
            }

            var sentenceCC = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "cc");
            if (sentenceCC != null)
            {
                return BasedOnCCQGenerator.TreatSimpleCCSentence(sentence, subject);
            }



            return null;
        }
    }
}
