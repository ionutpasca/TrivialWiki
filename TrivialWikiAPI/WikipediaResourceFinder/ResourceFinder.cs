using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WikipediaResourceFinder.Models;
using WikipediaResourceFinder.Models.Thumbnails;

// ReSharper disable AssignNullToNotNullAttribute
namespace WikipediaResourceFinder
{
    public sealed class ResourceFinder : IResourceFinder
    {
        public async Task GetWikipediaRawText(string topic, string filePath)
        {
            using (var client = new WebClient())
            {
                var query = "http://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&explaintext=1&titles=" + topic;
                using (var stream = client.OpenRead(query))
                using (var reader = new StreamReader(stream))
                {
                    var serializer = new JsonSerializer();
                    var result = serializer.Deserialize<WikipediaResponse>(new JsonTextReader(reader));
                    await SaveRawTextToFile(result, filePath);
                }
            }
        }

        public string GetWikipediaImageForTopic(string topic)
        {
            using (var client = new WebClient())
            {
                var query = "https://en.wikipedia.org/w/api.php?action=query&prop=pageimages&format=json&pithumbsize=300&titles=" + topic;
                var response = client.DownloadString(new Uri(query));
                var result = JsonConvert.DeserializeObject<RootThumbnail>(response);
                if (result.Query.Pages.FirstOrDefault().Value.Thumbnail != null)
                {
                    return result.Query.Pages.FirstOrDefault().Value.Thumbnail.Source;
                }
                return null;
            }
        }

        public async Task SaveRawTextToFile(WikipediaResponse toSave, string filePath)
        {
            using (var file = new StreamWriter(filePath))
            {
                foreach (var page in toSave.Query.Pages)
                {
                    await file.WriteLineAsync(page.Value.Extract);
                }
            }
        }
    }
}