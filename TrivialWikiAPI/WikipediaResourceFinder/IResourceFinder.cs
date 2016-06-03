using WikipediaResourceFinder.Models;

namespace WikipediaResourceFinder
{
    public interface IResourceFinder
    {
        string GetWikipediaRawText(string topic);
        void SaveRawTextToFile(WikipediaResponse response);
    }
}