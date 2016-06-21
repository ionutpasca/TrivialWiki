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
            var sentenceDET = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "det");
            var composedSubject = string.Empty;
            if (sentenceDET != null)
            {
                composedSubject = $"{sentenceDET.DependentGloss} {sentenceDET.GovernorGloss}";
            }

            SentenceDependencyDto subjectFromSentence;
            var sentenceIN = sentence.Dependencies.FirstOrDefault(d => d.Dep == "nmod:in");
            if (sentenceIN != null)
            {
                var verbeFromIn = sentenceIN.GovernorGloss;
                subjectFromSentence = sentence.Dependencies.FirstOrDefault(d => (d.Dep.ToLower() == "nsubjpass" ||
                                                d.Dep.ToLower() == "nsubj") && d.GovernorGloss == verbeFromIn);

            }
            else if (composedSubject != string.Empty)
            {
                subjectFromSentence = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nsubj");
            }
            else
            {
                subjectFromSentence = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nsubjpass") ??
                                   sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nsubj");
            }

            var subject = new WordInformationDto();
            if (subjectFromSentence != null)
            {
                subject = Helper.FindWordInList(sentence.Words, subjectFromSentence.DependentGloss);
            }


            var sentenceDate = Helper.GetSentenceDate(sentence);
            if (sentenceDate != null)
            {
                return BasedOnDateGenerator.TreatDateSentence(sentence, sentenceDate, subjectFromSentence);
            }

            if (sentenceIN != null)
            {
                return BasedOnINQGenerator.TreatSentenceWithIN(sentence, sentenceIN, subjectFromSentence, subject);
            }

            var sentenceCOP = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "cop");
            if (sentenceCOP != null)
            {
                return BasedOnCOPQGenerator.TreatSimpleCOPSentence(sentence, subject, sentenceCOP);
            }

            var sentenceAGENT = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nmod:agent");
            if (sentenceAGENT != null)
            {
                return BasedOnAGENTQGenerator.TreatSentenceWithAgent(sentence, sentenceAGENT, subject);
            }

            var sentenceDOBJ = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "dobj");
            if (sentenceDOBJ != null)
            {
                return BasedOnDOBJQGenerator.TreatSentenceWithDOBJ(sentence, sentenceDOBJ);
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

            var questionBasedOnSubject = BasedOnSubjectQGenerator.CreateQuestionBasedOnSubject(sentence, subjectFromSentence, subject);
            return questionBasedOnSubject;
        }
    }
}
