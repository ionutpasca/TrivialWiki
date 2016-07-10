using System.Collections.Generic;

namespace WikiTrivia.QuestionGenerator.Model
{
    public class SentenceInformationDto
    {
        public string SentenceText { get; }
        public IEnumerable<SentenceDependencyDto> Dependencies { get; }
        public IEnumerable<WordInformationDto> Words { get; }

        public SentenceInformationDto(string text, IEnumerable<SentenceDependencyDto> dependencies, IEnumerable<WordInformationDto> words)
        {
            SentenceText = text;
            Dependencies = dependencies;
            Words = words;
        }
    }
}
