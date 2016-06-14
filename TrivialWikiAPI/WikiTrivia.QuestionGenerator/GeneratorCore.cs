using System;
using System.Collections.Generic;
using System.Linq;
using WikiTrivia.QuestionGenerator.Model;

namespace WikiTrivia.QuestionGenerator
{
    public static class GeneratorCore
    {
        public static GeneratedQuestion TreatSentenceWithDOBJ(SentenceInformationDto sentence,
            SentenceDependencyDto sentenceDOBJ)
        {
            var answer = sentenceDOBJ.DependentGloss;
            var answerWord = Helper.FindWordInList(sentence.Words, answer);

            var answerDet = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "det" &&
                                            d.GovernorGloss == sentenceDOBJ.DependentGloss);
            if (answerDet != null)
            {
                answer = $"{answerDet.DependentGloss} {answer}";
            }

            var answerPoss = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nmod:poss" && d.GovernorGloss == answer);
            if (answerPoss != null)
            {
                var possCase = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "case" && d.GovernorGloss == answerPoss.DependentGloss);
                answer = possCase != null ?
                        $"{answerPoss.DependentGloss}{possCase.DependentGloss} {answer}" :
                        $"{answerPoss} {answer}";
            }

            var answerCompound = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "compound" && d.GovernorGloss == answerWord.Word);
            if (answerCompound != null)
            {
                var compoundIsBefore = CompoundIsBeforeDOBJ(sentence.Words, answerCompound.DependentGloss, answerWord.Word);
                answer = compoundIsBefore ? $"{answerCompound.DependentGloss} {answer}" : $"{answer} {answerCompound.DependentGloss}";
            }


            var verbe = Helper.FindWordInList(sentence.Words, sentenceDOBJ.GovernorGloss);


            var firstWord = sentence.Words.FirstOrDefault();
            var questionText = sentence.SentenceText
                                           .Replace(firstWord.Word, firstWord.Word.ToLower())
                                           .Replace(verbe.Word, verbe.Lemma);

            if (answerWord.PartOfSpeech.ToLower() == "nn" ||
                   answerWord.PartOfSpeech.ToLower() == "nns")
            {

                if (verbe.PartOfSpeech.ToLower() == "vbd")
                {
                    var question = $"What did {questionText}";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
                if (verbe.PartOfSpeech.ToLower() == "vbz")
                {
                    var question = $"What does {questionText}";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
                if (verbe.PartOfSpeech.ToLower() == "vbp")
                {
                    var question = $"What do {questionText}";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
            }
            if (answerWord.NamedEntityRecognition.ToLower() == "person")
            {
                if (verbe.PartOfSpeech.ToLower() == "vbd")
                {
                    var question = $"Who did {questionText}";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
                if (verbe.PartOfSpeech.ToLower() == "vbz")
                {
                    var question = $"What does {questionText}";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
                if (verbe.PartOfSpeech.ToLower() == "vbp")
                {
                    var question = $"What do {questionText}";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
            }

            var answerPOS = Helper.FindWordInList(sentence.Words, answer);
            var dobjAux = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "auxpass" &&
                                                               d.GovernorGloss == sentenceDOBJ.GovernorGloss);

            if (answerPOS.PartOfSpeech.ToLower() == "nnp" ||
                answerPOS.PartOfSpeech.ToLower() == "nn")
            {
                firstWord = sentence.Words.FirstOrDefault();
                var sentenceText = sentence.SentenceText.Replace(firstWord.Word, firstWord.Lemma)
                    .Replace(answer, "");
                if (dobjAux != null)
                {
                    sentenceText = sentenceText.Replace(dobjAux.DependentGloss, "");
                    var question = $"How {dobjAux.DependentGloss} {sentenceText}?";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
                else
                {
                    var question = $"How is {sentenceText}?";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
            }
            return null;
        }

        private static bool CompoundIsBeforeDOBJ(IEnumerable<WordInformationDto> words, string compound, string dobj)
        {
            foreach (var wordInformationDto in words)
            {
                if (wordInformationDto.Word == compound)
                {
                    return true;
                }
                if (wordInformationDto.Word == dobj)
                {
                    return false;
                }
            }
            return false;
        }

        public static GeneratedQuestion CreateQuestionBasedOnSubject(SentenceInformationDto sentence, SentenceDependencyDto subjectFromSentence,
            WordInformationDto subject, string subjectPoss)
        {
            var subjectRelationWord = Helper.FindWordInList(sentence.Words, subjectFromSentence.GovernorGloss);
            if (subjectRelationWord.PartOfSpeech == "VBZ" ||
                subjectRelationWord.PartOfSpeech == "VBN")
            {
                var answer = subject.Word;
                if (!string.IsNullOrEmpty(subjectPoss))
                {
                    answer = $"{subjectPoss} {answer}";
                }
                var subjectDet = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "det" &&
                                                                           d.GovernorGloss == subject.Word);
                if (subjectDet != null)
                {
                    answer = $"{subjectDet.DependentGloss} {answer}";
                }

                if (subject.NamedEntityRecognition.ToLower() == "person" ||
                    subject.PartOfSpeech.ToLower() == "prp" ||
                    subject.PartOfSpeech.ToLower() == "nn")
                {
                    var question = sentence.SentenceText.Replace(answer, "Who");
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
                else
                {
                    var question = sentence.SentenceText.Replace(answer, "What");
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
            }
            return null;
        }

        public static GeneratedQuestion TreatSentenceWithNSUBJ(SentenceInformationDto sentence, SentenceDependencyDto sentenceNSUBJ,
            string subjectPossession, WordInformationDto subject)
        {
            var subjectRelation = Helper.FindWordInList(sentence.Words, sentenceNSUBJ.GovernorGloss);
            var answer = subject.Word;
            if (!String.IsNullOrEmpty(subjectPossession))
            {
                answer = $"{subjectPossession} {answer}";
            }

            if (subjectRelation.PartOfSpeech.ToLower() == "vbz")
            {
                if (subject.NamedEntityRecognition.ToLower() == "person" ||
                    subject.PartOfSpeech.ToLower() == "prp" ||
                    subject.PartOfSpeech.ToLower() == "prp$")
                {
                    var questionText = sentence.SentenceText.Replace(answer, "Who ");
                    var question = $"{questionText}?";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
                if (subject.NamedEntityRecognition.ToLower() != "o")
                {
                    var questionText = sentence.SentenceText.Replace(answer, "Which ");
                    var question = $"{questionText}?";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
                else
                {
                    var questionText = sentence.SentenceText.Replace(answer, "What ");
                    var question = $"{questionText}?";
                    return new GeneratedQuestion { Answer = answer, Question = question };
                }
            }
            return null;
        }

        public static GeneratedQuestion TreatSimpleXCOMPSentence(SentenceInformationDto res, SentenceDependencyDto subjectFromRes,
            string subjectPoss, WordInformationDto subject, SentenceDependencyDto sentenceXCOMP)
        {
            //THIS IS FOR ANSWER
            //var sentenceADVMOD = result[0].Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "advmod");
            //if (sentenceADVMOD != null)
            //{
            //    var advmodWord = FindWordInList(result[0].Words, sentenceXCOMP.GovernorGloss);
            //}

            var predicate = res.Words.FirstOrDefault(w => w.Word == subjectFromRes.GovernorGloss);
            var wordInResult = res.Words.FirstOrDefault(w => w.Word == sentenceXCOMP.DependentGloss);
            if (wordInResult != null)
            {
                var pos = wordInResult.PartOfSpeech;
                if (pos == "JJ")
                {
                    if (subjectPoss != null)
                    {
                        var question = $"How does {subjectPoss} {subject.Lemma} {predicate.Lemma}?";
                    }
                    else
                    {
                        var question = $"How does {subject.Lemma} {predicate.Lemma}?";
                    }
                }
            }
            return null;
        }

        public static GeneratedQuestion TreatSimpleCCSentence(SentenceInformationDto sentence, WordInformationDto subject)
        {
            var subjectWord = Helper.FindWordInList(sentence.Words, subject.Word);

            var answer = subjectWord.Lemma;

            var sentenceAMOD = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "amod");
            if (sentenceAMOD != null && sentenceAMOD.GovernorGloss == subjectWord.Word)
            {
                answer = $"{sentenceAMOD.DependentGloss} {answer}";
            }
            var subjectDET = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "det");
            if (subjectDET != null && subjectDET.GovernorGloss == subjectWord.Word)
            {
                answer = $"{subjectDET.DependentGloss} {answer}";
            }
            if (subjectWord.PartOfSpeech.ToLower() == "nnp" ||
                     subjectWord.PartOfSpeech.ToLower() == "nns" ||
                     subjectWord.PartOfSpeech.ToLower() == "prp" ||
                     subjectWord.NamedEntityRecognition.ToLower() == "person")
            {
                var question = $"{sentence.SentenceText.Replace(answer, "Who")}?";
                return new GeneratedQuestion { Answer = answer, Question = question };
            }
            else
            {
                var question = $"{sentence.SentenceText.Replace(answer, "Who/What")}?";
                return new GeneratedQuestion { Answer = answer, Question = question };
            }
        }

        public static GeneratedQuestion TreatSimpleCOPSentence(SentenceInformationDto sentence, string subjectPoss, WordInformationDto subject,
            SentenceDependencyDto sentenceCOP)
        {
            var subjectWord = Helper.FindWordInList(sentence.Words, subject.Word);
            var answer = subjectWord.Word;
            if (!string.IsNullOrEmpty(subjectPoss))
            {
                answer = $"{subjectPoss} {answer}";
            }

            var subjectAMOD = sentence.Dependencies.FirstOrDefault(d => d.Dep == "amod");
            if (subjectAMOD != null && subjectAMOD.GovernorGloss == subjectWord.Word)
            {
                answer = $"{subjectAMOD.DependentGloss} {subjectWord.Word}";
            }

            var copPartOfSpeech = Helper.FindWordInList(sentence.Words, sentenceCOP.GovernorGloss).PartOfSpeech;

            if (subjectWord.NamedEntityRecognition.ToLower() == "person" ||
                subjectWord.PartOfSpeech.ToLower() == "nnp")
            {
                var question = $"{sentence.SentenceText.Replace(answer, "Who")}?";
                return new GeneratedQuestion { Answer = answer, Question = question };
            }
            if (copPartOfSpeech == "JJ")
            {
                var question = $"{sentence.SentenceText.Replace(answer, "What")}?";
                return new GeneratedQuestion { Answer = answer, Question = question };
            }
            else
            {
                var question = $"{sentence.SentenceText.Replace(answer, "Which")}?";
                return new GeneratedQuestion { Answer = answer, Question = question };
            }
        }
    }
}
