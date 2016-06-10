namespace POSTagger
{
    public class WordInformation
    {
        public string Word { get; set; }
        public string PartOfSpeech { get; set; }
        public string NamedEntityRecognition { get; set; }
        public string Lemma { get; set; }

        public WordInformation(string w, string pos, string ner, string lemma)
        {
            Word = w;
            PartOfSpeech = pos;
            NamedEntityRecognition = ner;
            Lemma = lemma;
        }
    }
}
