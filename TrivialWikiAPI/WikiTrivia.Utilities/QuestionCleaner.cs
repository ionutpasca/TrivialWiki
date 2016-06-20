using System.Linq;
using System.Text.RegularExpressions;

namespace WikiTrivia.Utilities
{
    public static class QuestionCleaner
    {
        public static string RemovePunctuationFromEnd(string question)
        {
            question = Regex.Replace(question, @"\s+", " ", RegexOptions.Multiline);
            var result = question;
            for (var i = question.Length - 1; i > 0; i--)
            {
                if (char.IsPunctuation(question[i]) || question[i] == ' ')
                {
                    result = result.ReplaceAt(i, char.MinValue);
                }
                else
                {
                    return result;
                }
            }
            return result;
        }

        private static string ReplaceAt(this string value, int index, char newchar)
        {
            return value.Length <= index ? value :
                string.Concat(value.Select((c, i) => i == index ? newchar : c));
        }
    }
}
