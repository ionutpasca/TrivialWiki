using System.Collections.Generic;
using System.Linq;
using WikiTrivia.QuestionGenerator.Model;
// ReSharper disable InvertIf

namespace WikiTrivia.QuestionGenerator.Generators
{
    public static class AnswerGenerator
    {
        public static string GenerateAnswer(SentenceInformationDto sentence, SentenceDependencyDto sentenceDependency = null,
            WordInformationDto wordInformation = null, WordInformationDto subjectWord = null)
        {
            var baseAnswer = GetBaseAnswer(sentence, sentenceDependency, wordInformation ?? subjectWord);
            var answer = baseAnswer;
            var answerDet = GetWordDET(sentence, answer);

            if (answer != string.Empty)
            {
                answer = TreatAnswerCompound(sentence, baseAnswer, answer);

                answer = TreatAnswerNumMod(sentence, baseAnswer, answer);

                answer = TreatAnswerPossession(sentence, baseAnswer, answer);

                answer = TreatAmod(sentence, baseAnswer, answer);

                answer = TreatNMODSentence(sentence, baseAnswer, answer);
            }

            if (subjectWord != null)
            {
                var subjectAMOD = sentence.Dependencies.Where(d => d.Dep == "amod" && d.GovernorGloss == subjectWord.Word);
                foreach (var subjAmod in subjectAMOD)
                {
                    answer = $"{subjAmod.DependentGloss} {subjectWord.Word}";

                }
            }

            if (sentenceDependency != null && string.IsNullOrEmpty(answer))
            {
                answer = sentenceDependency.DependentGloss;
                answerDet = GetWordDET(sentence, sentenceDependency.DependentGloss);
            }

            if (wordInformation != null)
            {
                answerDet = GetWordDET(sentence, wordInformation.Word);
            }

            if (answerDet != null)
            {
                answer = $"{answerDet.DependentGloss} {answer}";
            }

            return answer;
        }

        private static string TreatNMODSentence(SentenceInformationDto sentence, string baseAnswer, string answer)
        {
            var sentenceNMODFor = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nmod:for" &&
                            d.GovernorGloss == baseAnswer);
            if (sentenceNMODFor != null)
            {
                answer = $"{answer} for";
                var compound = sentence.Dependencies.Where(d => d.Dep.ToLower() == "compound" &&
                                d.GovernorGloss == sentenceNMODFor.DependentGloss).ToList();
                foreach (var elem in compound)
                {
                    answer = Helper.ElementIsBeforeWord(sentence.Words, elem.DependentGloss, sentenceNMODFor.DependentGloss) ?
                        $"{answer} {elem.DependentGloss} {sentenceNMODFor.DependentGloss}" :
                        $"{answer} {sentenceNMODFor.DependentGloss} {elem.DependentGloss}";
                }
                if (compound.Count == 0)
                {
                    answer = $"{answer} for {sentenceNMODFor.DependentGloss}";
                }
            }
            return answer;
        }

        private static string TreatAmod(SentenceInformationDto sentence, string baseAnser, string answer)
        {
            var amodList = sentence.Dependencies.Where(d => d.Dep == "amod" && d.GovernorGloss == baseAnser).ToList();
            foreach (var subjAmod in amodList)
            {
                answer = Helper.ElementIsBeforeWord(sentence.Words, subjAmod.DependentGloss, baseAnser) ?
                    $"{subjAmod.DependentGloss} {answer}" :
                    $"{answer} {subjAmod.DependentGloss}";
            }
            return answer;
        }

        private static string TreatAnswerPossession(SentenceInformationDto sentence, string baseAnswer, string answer)
        {
            var answerPoss = GetAnswerPossession(sentence, baseAnswer);
            if (answerPoss != null)
            {
                var possCase = GetPossessionCase(sentence, answerPoss.DependentGloss);
                answer = possCase != null
                    ? $"{answerPoss.DependentGloss}{possCase.DependentGloss} {answer}"
                    : $"{answerPoss.DependentGloss} {answer}";
            }

            return answer;
        }

        private static string TreatAnswerNumMod(SentenceInformationDto sentence, string baseAnswer, string answer)
        {
            var sentenceNUMMOD = sentence.Dependencies
               .Where(d => d.Dep.ToLower() == "nummod" && d.GovernorGloss == baseAnswer)
               .ToList();
            foreach (var numMod in sentenceNUMMOD)
            {
                answer = Helper.ElementIsBeforeWord(sentence.Words, numMod.DependentGloss, baseAnswer) ?
                    $"{numMod.DependentGloss} {answer}" :
                    $"{answer} {numMod.DependentGloss}";
            }
            return answer;
        }

        private static string TreatAnswerCompound(SentenceInformationDto sentence, string baseAnswer, string answer)
        {
            var answerCompound = GetWordCompound(sentence, baseAnswer).ToList();
            answerCompound = OrderByWordPosition(answerCompound, sentence.Words);

            foreach (var compound in answerCompound)
            {
                var compoundIsBefore = Helper.ElementIsBeforeWord(sentence.Words, compound.DependentGloss, baseAnswer);
                answer = compoundIsBefore ? $"{compound.DependentGloss} {answer}" : $"{answer} {compound.DependentGloss}";
            }
            return answer;
        }

        private static List<SentenceDependencyDto> OrderByWordPosition(List<SentenceDependencyDto> initialList,
           IEnumerable<WordInformationDto> wordInfo)
        {
            for (var i = 0; i < initialList.Count - 1; i++)
            {
                for (var j = 0; j < initialList.Count; j++)
                {
                    if (Helper.ElementIsBeforeWord(wordInfo, initialList[i].DependentGloss, initialList[j].DependentGloss))
                    {
                        var aux = initialList[i];
                        initialList[i] = initialList[j];
                        initialList[j] = aux;
                    }
                }
            }
            return initialList;
        }

        private static SentenceDependencyDto GetAnswerPossession(SentenceInformationDto sentence, string answer)
        {
            return sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nmod:poss" && d.GovernorGloss == answer);
        }

        private static SentenceDependencyDto GetPossessionCase(SentenceInformationDto sentence, string poss)
        {
            return sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "case" && d.GovernorGloss == poss);
        }

        private static SentenceDependencyDto GetWordDET(SentenceInformationDto sentence, string word)
        {
            return sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "det" && d.GovernorGloss == word);
        }

        private static IEnumerable<SentenceDependencyDto> GetWordCompound(SentenceInformationDto sentence, string word)
        {
            return sentence.Dependencies.Where(d => d.Dep.ToLower() == "compound" && d.GovernorGloss == word);
        }

        private static string GetBaseAnswer(SentenceInformationDto sentence, SentenceDependencyDto sentenceDependency, WordInformationDto wordInformation)
        {
            if (sentenceDependency != null && wordInformation != null &&( wordInformation.NamedEntityRecognition.ToLower() == "person"
                || wordInformation.PartOfSpeech.ToLower()=="nn"))
            {
                return wordInformation.Word;
            }
            if (sentenceDependency != null)
            {
                var sentenceDep = Helper.FindWordInList(sentence.Words, sentenceDependency.DependentGloss);
                if (sentenceDep.PartOfSpeech.ToLower() == "vb" ||
                    sentenceDep.PartOfSpeech.ToLower() == "vbd" ||
                    sentenceDep.PartOfSpeech.ToLower() == "vbg" ||
                    sentenceDep.PartOfSpeech.ToLower() == "vbn" ||
                    sentenceDep.PartOfSpeech.ToLower() == "vbp" ||
                    sentenceDep.PartOfSpeech.ToLower() == "vbz")
                {
                    return sentenceDependency.GovernorGloss;
                }
                return sentenceDependency.DependentGloss;
            }
            return wordInformation != null ? wordInformation.Word : string.Empty;
        }
    }
}
