using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiTrivia.Utilities
{
    public static class TriviaHintGenerator
    {
        public static string GenerateHintForQuestion(string answer, string initialHint = null)
        {
            var hint = initialHint?.ToList() ?? answer.Select(ch => '*').ToList();

            var charactersFromHint = hint.Where(ch => ch != '*');
            var charactersFromAnswer = answer.Select(ch => ch);

            var validCharacters = ComputePossibleCharactersForHint(charactersFromAnswer, charactersFromHint);
            var newCharacterToShow = GetRandomCharacter(validCharacters);

            var indexesOfTheCharaterToShow = GetCharacterIndexes(answer, newCharacterToShow);

            foreach (var i in indexesOfTheCharaterToShow)
            {
                hint[i] = newCharacterToShow;
            }
            return CreateStringFromEnumerable(hint);
        }

        private static IEnumerable<char> ComputePossibleCharactersForHint(IEnumerable<char> charactersFromAnswer, IEnumerable<char> charactersFromHint)
        {
            return charactersFromAnswer.Where(c => !charactersFromHint.Contains(c)).ToList();
        }

        private static IEnumerable<int> GetCharacterIndexes(string answer, char character)
        {
            var result = new List<int>();
            for (var i = 0; i < answer.Length; i++)
            {
                if (answer[i] == character)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        private static char GetRandomCharacter(IEnumerable<char> characterList)
        {
            var rand = new Random();
            var enumerable = characterList as char[] ?? characterList.ToArray();
            var indexOfChar = rand.Next(enumerable.Length);
            return enumerable.ElementAt(indexOfChar);
        }

        private static string CreateStringFromEnumerable(IEnumerable<char> characters)
        {
            var stringBuilder = new StringBuilder();
            foreach (var c in characters)
            {
                stringBuilder.Append(c);
            }
            return stringBuilder.ToString();
        }
    }
}
