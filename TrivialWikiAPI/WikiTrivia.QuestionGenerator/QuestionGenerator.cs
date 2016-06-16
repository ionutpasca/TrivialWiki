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

            var sentenceIN = sentence.Dependencies.FirstOrDefault(d => d.Dep == "nmod:in");
            var sentenceDate = Helper.GetSentenceDate(sentence);
            if (sentenceDate != null)
            {
                var composedAnswer = AnswerGenerator.GenerateAnswer(sentence, subjectWord: sentenceDate);

                var answerCase = sentence.Dependencies.FirstOrDefault(d => d.Dep == "case" && d.GovernorGloss == sentenceDate.Word);
                if (answerCase != null)
                {
                    var secondAnswerCase = sentence.Dependencies.FirstOrDefault(d => d.Dep == "case" &&
                                                d.GovernorGloss == answerCase.GovernorGloss &&
                                                d.DependentGloss != answerCase.DependentGloss);
                    var wordsToReplace = $"{answerCase.DependentGloss} {composedAnswer}";

                    if (secondAnswerCase != null)
                    {
                        wordsToReplace = $"{answerCase.DependentGloss} {secondAnswerCase.DependentGloss} {composedAnswer}";
                    }

                    var firstWord = sentence.Words.First();

                    var verbeWord = Helper.FindWordInList(sentence.Words, subjectFromSentence.GovernorGloss);
                    var verbeAuxPass = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "auxpass" &&
                                            d.GovernorGloss == verbeWord.Word);

                    var question = sentence.SentenceText.Replace(wordsToReplace, "")
                        .Replace(firstWord.Word, firstWord.Word.ToLower());
                    if (verbeWord.PartOfSpeech.ToLower() == "vbd")
                    {
                        question = question.Replace(verbeWord.Word, verbeWord.Lemma);
                    }
                    if (verbeAuxPass != null)
                    {
                        question = question.Replace(verbeAuxPass.DependentGloss, "");
                        question = $"When {verbeAuxPass.DependentGloss} {question}?";
                        return new GeneratedQuestion { Answer = sentenceDate.Word, Question = question };
                    }
                    question = $"When did {question}?";
                    return new GeneratedQuestion { Answer = composedAnswer, Question = question };
                }
            }

            if (sentenceIN != null)
            {
                var inWord = Helper.FindWordInList(sentence.Words, sentenceIN.DependentGloss);
                if (inWord.PartOfSpeech.ToLower() == "nn")
                {
                    string answer;
                    var nounOF = sentence.Dependencies.FirstOrDefault(d => d.Dep == "nmod:of" &&
                                            d.GovernorGloss == inWord.Word);
                    if (nounOF != null)
                    {
                        answer = nounOF.DependentGloss;
                        var verbe = Helper.FindWordInList(sentence.Words, subjectFromSentence.GovernorGloss);
                        var verbeAND = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "conj:and" &&
                                                d.GovernorGloss == verbe.Word);
                        var verbeAndWord = Helper.FindWordInList(sentence.Words, verbeAND.DependentGloss);
                        if (verbe.PartOfSpeech.ToLower() == "vbz")
                        {
                            var question = $"Where does {subject.Word} {verbeAndWord.Word} and {verbe.Word}?";
                            return new GeneratedQuestion { Answer = answer, Question = question };
                        }
                        else
                        {
                            var question = $"Where did {subject.Word} {verbeAndWord.Lemma} and {verbe.Lemma}?";
                            return new GeneratedQuestion { Answer = answer, Question = question };
                        }
                    }
                }
            }

            var sentenceAGENT = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nmod:agent");
            if (sentenceAGENT != null)
            {
                var answer = sentenceAGENT.DependentGloss;

                var subjectDet = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "det" &&
                                    d.GovernorGloss == subject.Word);
                var subj = subject.Word;
                if (subjectDet != null)
                {
                    subj = $"{subjectDet.DependentGloss} {subject.Word}";
                }
                //var answer = AnswerGenerator.GenerateAnswer(sentence, subjectWord: subject);

                var otherAgents = sentence.Dependencies.Where(d => d.Dep.ToLower() == "nmod:agent" &&
                                    d.GovernorGloss == sentenceAGENT.GovernorGloss &&
                                    d.DependentGloss != answer);
                var connectionCC = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "cc" &&
                                    d.GovernorGloss == answer);
                foreach (var agent in otherAgents)
                {
                    answer = $"{answer} {connectionCC.DependentGloss} {agent.DependentGloss}";
                }

                var verbe = Helper.FindWordInList(sentence.Words, sentenceAGENT.GovernorGloss);
                var answerWord = Helper.FindWordInList(sentence.Words, sentenceAGENT.DependentGloss);
                if (answerWord.PartOfSpeech.ToLower() == "nnp" ||
                    answerWord.NamedEntityRecognition.ToLower() == "person")
                {
                    var question = $"Who {verbe.Word} {subj.ToLower()}?";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }

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
            if (questionBasedOnSubject != null)
            {
                return questionBasedOnSubject;
            }

            return null;
        }
    }
}
