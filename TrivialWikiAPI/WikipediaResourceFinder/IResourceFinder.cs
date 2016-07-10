using System.Threading.Tasks;
using WikipediaResourceFinder.Models;

namespace WikipediaResourceFinder
{
    public interface IResourceFinder
    {
        Task GetWikipediaRawText(string topic, string filePath);
        Task SaveRawTextToFile(WikipediaResponse response, string filePath);
    }
}