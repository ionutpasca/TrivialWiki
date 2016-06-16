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
            }

            if (subjectWord != null)
            {
                var subjectAMOD = sentence.Dependencies.FirstOrDefault(d => d.Dep == "amod" && d.GovernorGloss == subjectWord.Word);
                if (subjectAMOD != null)
                {
                    answer = $"{subjectAMOD.DependentGloss} {subjectWord.Word}";
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
            var answerNumMod = sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nummod" && d.GovernorGloss == baseAnswer);
            if (answerNumMod != null)
            {
                var numModIsFirst = ElementIsBeforeWord(sentence.Words, answerNumMod.DependentGloss, baseAnswer);
                answer = numModIsFirst ? $"{answerNumMod.DependentGloss} {answer}" : $"{answer} {answerNumMod.DependentGloss}";
            }

            return answer;
        }

        private static string TreatAnswerCompound(SentenceInformationDto sentence, string baseAnswer, string answer)
        {
            var answerCompound = GetWordCompound(sentence, baseAnswer).ToList();
            foreach (var compound in answerCompound)
            {
                var compoundIsBefore = ElementIsBeforeWord(sentence.Words, compound.DependentGloss, baseAnswer);
                answer = compoundIsBefore ? $"{compound.DependentGloss} {answer}" : $"{answer} {compound.DependentGloss}";
            }
            return answer;
        }

        private static bool ElementIsBeforeWord(IEnumerable<WordInformationDto> words, string element, string word)
        {
            foreach (var wordInformationDto in words)
            {
                if (wordInformationDto.Word == element)
                {
                    return true;
                }
                if (wordInformationDto.Word == word)
                {
                    return false;
                }
            }
            return false;
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
            if (sentenceDependency != null && wordInformation != null && wordInformation.NamedEntityRecognition.ToLower() == "person")
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
