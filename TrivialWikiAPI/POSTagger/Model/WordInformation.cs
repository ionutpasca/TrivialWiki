namespace POSTagger.Model
{
    public class WordInformation
    {
        public WordInformation(string w, string pos, string ner, string lemma)
        {
            Word = w;
            PartOfSpeech = pos;
            NamedEntityRecognition = ner;
            Lemma = lemma;
        }

        public WordInformation(int index, string w, string pos, string ner, string lemma, string after)
        {
            Index = index;
            Word = w;
            PartOfSpeech = pos;
            NamedEntityRecognition = ner;
            Lemma = lemma;
            After = after;
        }

        public int Index { get; set; }
        public string Word { get; set; }
        public string PartOfSpeech { get; set; }
        public string NamedEntityRecognition { get; set; }
        public string Lemma { get; set; }
        public string After { get; set; }

    }
}
