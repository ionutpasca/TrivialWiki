using System;
using System.Collections.Generic;
using System.Linq;
using WikiTrivia.QuestionGenerator.Model;

namespace WikiTrivia.QuestionGenerator
{
    public static class Helper
    {
        public static string GetSubjectPossession(SentenceInformationDto res)
        {
            var subjectHasPossesion = res.Dependencies.FirstOrDefault(d => d.Dep.ToLower() == "nmod:poss");
            var subjectPoss = string.Empty;

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
            return indexOfKey != 0 ? question.Substring(indexOfKey, question.Length - indexOfKey) : question;
        }
    }
}
