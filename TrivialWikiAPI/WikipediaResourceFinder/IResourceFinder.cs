namespace WikipediaResourceFinder
{
    public interface IResourceFinder
    {
        string GetWikipediaRawText(string topic);
        void SaveRawTextToFile(string treSaVedem);
    }
}