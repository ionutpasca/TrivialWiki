using System.Linq;
using WikiTrivia.QuestionGenerator.Model;

namespace WikiTrivia.QuestionGenerator
{
    public static class QuestionGenerator
    {
        public static GeneratedQuestion Generate(SentenceInformationDto sentence)
        {
            var subjectFromSentence = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nsubjpass") ??
                                      sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nsubj");

            var subjectPossession = Helper.GetSubjectPossession(sentence);

            var subject = new WordInformationDto();
            if (subjectFromSentence != null)
            {
                subject = Helper.FindWordInList(sentence.Words, subjectFromSentence.DependentGloss);
            }

            var sentenceDOBJ = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "dobj");
            if (sentenceDOBJ != null)
            {
                return GeneratorCore.TreatSentenceWithDOBJ(sentence, sentenceDOBJ);
            }

            var questionBasedOnSubject = GeneratorCore.CreateQuestionBasedOnSubject(sentence, subjectFromSentence, subject,
                subjectPossession);
            if (questionBasedOnSubject != null)
            {
                return questionBasedOnSubject;
            }

            var sentenceNSUBJ = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nsubj");
            var sentenceNSUBJPASS = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nsubjpass");

            if (sentenceNSUBJ != null && sentenceNSUBJPASS == null)
            {
                return GeneratorCore.TreatSentenceWithNSUBJ(sentence, sentenceNSUBJ, subjectPossession, subject);
            }

            var sentenceXCOMP = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "xcomp");
            if (sentenceXCOMP != null)
            {
                return GeneratorCore.TreatSimpleXCOMPSentence(sentence, subjectFromSentence, subjectPossession, subject, sentenceXCOMP);
            }

            var sentenceCC = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "cc");
            if (sentenceCC != null)
            {
                return GeneratorCore.TreatSimpleCCSentence(sentence, subject);
            }

            var sentenceCOP = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "cop");
            if (sentenceCOP != null)
            {
                return GeneratorCore.TreatSimpleCOPSentence(sentence, subjectPossession, subject, sentenceCOP);
            }

            return null;
        }
    }
}
