namespace WikipediaResourceFinder.Models.Thumbnails
{
    public class Pageval
    {
        public int Pageid { get; set; }
        public int Ns { get; set; }
        public string Title { get; set; }
        public Thumbnail Thumbnail { get; set; }
        public string Pageimage { get; set; }
    }
}
