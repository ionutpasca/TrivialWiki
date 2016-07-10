using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiTrivia.QuestionGenerator.Model;

namespace WikiTrivia.QuestionGenerator
{
    public static class Helper
    {
        public static string GetSubjectPossession(SentenceInformationDto res)
        {
            var subjectHasPossesion = res.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nmod:poss");
            var subjectPoss = String.Empty;

            if (subjectHasPossesion != null)
            {
                subjectPoss = FindWordInList(res.Words, subjectHasPossesion.DependentGloss).Word;
            }

            return subjectPoss;
        }

        public static WordInformationDto FindWordInList(IEnumerable<WordInformationDto> words, string wordToFind)
        {
            return words.FirstOrDefault(w => w.Word == wordToFind);
        }

        public static string TrimQuestion(string question, string keyWordUsed)
        {
            var indexOfKey = question.IndexOf(keyWordUsed, StringComparison.Ordinal);
            return indexOfKey != -1 ?
                question.Substring(indexOfKey, question.Length - indexOfKey) :
                question;
        }

        public static string TrimQuestionAfter(string question, string keyWord)
        {
            var indexOfKey = question.IndexOf(keyWord, StringComparison.Ordinal);
            return indexOfKey != -1
                ? question.Substring(0, indexOfKey + keyWord.Length)
                : question;
        }

        public static bool StringIsYear(string input)
        {
            return input.All(Char.IsDigit) && input.Length <= 4;
        }

        public static bool SentenceContainsYear(SentenceInformationDto sentence)
        {
            if (sentence.Words.Any(w => w.NamedEntityRecognition.ToLower() == "data"))
            {
                return true;
            }
            var sentenceIN = sentence.Dependencies.Where(d => d.Dep == "nmod:in");
            return sentenceIN.Any(sentenceDep => StringIsYear(sentenceDep.DependentGloss));
        }

        public static bool SentenceIsInvalid(SentenceInformationDto sentence)
        {
            return sentence.Words.Any(w => w.Word.ToLower() == "i");
        }

        public static WordInformationDto GetSentenceDate(SentenceInformationDto sentence)
        {
            return sentence.Words.FirstOrDefault(w => w.NamedEntityRecognition.ToLower() == "date");
        }

        public static bool ElementIsBeforeWord(IEnumerable<WordInformationDto> words, string element, string word)
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

        public static string ConvertStringArrayToString(string[] array)
        {
            var builder = new StringBuilder();
            foreach (var value in array)
            {
                builder.Append(value);
                builder.Append('.');
            }
            return builder.ToString();
        }
    }
}
