using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace POSTagger
{
    public static class StringUtils
    {
        //private static readonly string referencesPath = ConfigurationManager.AppSettings["Tagger.References"];
        private static readonly string newParsePath = ConfigurationManager.AppSettings["Tagger.newParsePath"];
        private static readonly string listParsePath = ConfigurationManager.AppSettings["Tagger.ListParse"];

        public static string CleanText(string text, string referencesPath)
        {
            var result = text;
            //var lrb = result.IndexOf("(", StringComparison.Ordinal);
            //var rrb = result.IndexOf(")", StringComparison.Ordinal);

            //while (result.Contains("("))
            //{
            //    lrb = result.IndexOf("(", StringComparison.Ordinal);
            //    rrb = result.IndexOf(")", StringComparison.Ordinal);
            //    result = result.Remove(lrb, rrb - lrb + 1);
            //}

            const string paranthesisRegex = "(\\(.*\\))";
            result = Regex.Replace(result, paranthesisRegex, "");

            var regex = new Regex("={2,5}");
            Match res1;
            var references = "";
            do
            {
                res1 = regex.Match(result);
                var res2 = res1.NextMatch();

                var between = result.Remove(res2.Index + res2.Value.Length).Substring(res1.Index);
                if (IsLink(between))
                {
                    references = result.Substring(res1.Index);
                    result = result.Remove(res1.Index);
                    break;
                }

                result = result.Remove(res1.Index, res2.Index + res2.Value.Length - res1.Index);

            } while (res1.Index != 0);

            using (var file = new System.IO.StreamWriter(referencesPath))
            {
                file.WriteLine(references);
            }

            result = Regex.Replace(result, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            return result;
        }

        public static string Parse(string parseSentence)
        {
            var result = "0";
            var newline = parseSentence.IndexOf("\r\n", StringComparison.Ordinal);
            while ((newline + 2) < parseSentence.Length)
            {
                result += parseSentence.Remove(newline + 2);
                parseSentence = parseSentence.Substring(newline + 2);
                var spaces = parseSentence.IndexOf("(", StringComparison.Ordinal);
                if (spaces == -1)
                {
                    result += parseSentence;
                    break;
                }

                parseSentence = parseSentence.Substring(spaces);
                result += spaces;
                newline = parseSentence.IndexOf("\r\n", StringComparison.Ordinal);
            }
            using (var file = new System.IO.StreamWriter(newParsePath))
            {
                file.WriteLine(result);
            }
            return result;
        }

        public static ArrayList ListParse(string parseSentence)
        {
            var result = new ArrayList();
            var newline = parseSentence.IndexOf("\r\n", StringComparison.Ordinal);
            var spaces = 0;
            while (newline != -1)
            {
                var item = spaces + parseSentence.Remove(newline);
                result.Add(item);
                parseSentence = parseSentence.Substring(newline + 2);
                spaces = parseSentence.IndexOf("(", StringComparison.Ordinal);
                if (spaces == -1)
                {
                    break;
                }
                parseSentence = parseSentence.Substring(spaces);
                newline = parseSentence.IndexOf("\r\n", StringComparison.Ordinal);
                if (newline != -1 || parseSentence.Length <= 0) continue;
                var lastItem = spaces + parseSentence;
                result.Add(lastItem);
            }
            using (var file = new System.IO.StreamWriter(listParsePath))
            {
                foreach (var variable in result)
                {
                    file.WriteLine(variable);
                }
            }
            return result;
        }

        private static bool IsLink(string text)
        {
            var list = new string[] { "See also", "Notes", "References", "Further reading", "External links", "Navigation templates" };
            return list.Any(text.Contains);
        }
    }
}
