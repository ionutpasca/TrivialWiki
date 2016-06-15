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
            var baseAnswer = GetBaseAnswer(sentenceDependency, wordInformation ?? subjectWord);
            var answer = baseAnswer;
            var answerDet = GetWordDET(sentence, answer);

            if (answer != string.Empty)
            {
                var answerCompound = GetWordCompound(sentence, baseAnswer);
                if (answerCompound != null)
                {
                    var compoundIsBefore = CompoundIsBeforeDOBJ(sentence.Words, answerCompound.DependentGloss, baseAnswer);
                    answer = compoundIsBefore ? $"{answerCompound.DependentGloss} {answer}" : $"{answer} {answerCompound.DependentGloss}";
                }

                var answerPoss = GetAnswerPossession(sentence, baseAnswer);
                if (answerPoss != null)
                {
                    var possCase = GetPossessionCase(sentence, answerPoss.DependentGloss);
                    answer = possCase != null
                        ? $"{answerPoss.DependentGloss}{possCase.DependentGloss} {answer}"
                        : $"{answerPoss.DependentGloss} {answer}";
                }
            }

            if (subjectWord != null)
            {
                var subjectAMOD = sentence.Dependencies.FirstOrDefault(d => d.Dep == "amod" && d.GovernorGloss == subjectWord.Word);
                if (subjectAMOD != null)
                {
                    answer = $"{subjectAMOD.DependentGloss} {subjectWord.Word}";
                }
            }

            //var answerWord = Helper.FindWordInList(sentence.Words, answer);

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

        private static SentenceDependencyDto GetWordCompound(SentenceInformationDto sentence, string word)
        {
            return sentence.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "compound" && d.GovernorGloss == word);
        }

        private static string GetBaseAnswer(SentenceDependencyDto sentenceDependency, WordInformationDto wordInformation)
        {
            if (wordInformation != null)
            {
                return wordInformation.Word;
            }
            return sentenceDependency != null ? sentenceDependency.GovernorGloss : string.Empty;
        }
    }
}
