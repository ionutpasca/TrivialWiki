namespace WikiTrivia.QuestionGenerator.Model
{
    public class WordInformationDto
    {
        public string Word { get; }
        public string PartOfSpeech { get; }
        public string NamedEntityRecognition { get; }
        public string Lemma { get; }

        public WordInformationDto(string word, string partOfSpeech, string ner, string lemma)
        {
            Word = word;
            PartOfSpeech = partOfSpeech;
            NamedEntityRecognition = ner;
            Lemma = lemma;
        }

        public WordInformationDto() { }
    }
}
