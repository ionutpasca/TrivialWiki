using Newtonsoft.Json;
using System.IO;
using System.Net;
using WikipediaResourceFinder.Models;


namespace WikipediaResourceFinder
{
    public sealed class ResourceFinder : IResourceFinder
    {
        public string GetWikipediaRawText(string topic)
        {
            var client = new WebClient();
            string query = "http://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&explaintext=1&titles=" + topic;
            using (var stream = client.OpenRead(query))
            using (var reader = new StreamReader(stream))
            {
                var serializer = new JsonSerializer();
                var result = serializer.Deserialize<WikipediaResponse>(new JsonTextReader(reader));

                SaveRawTextToFile(result);
                foreach (var page in result.Query.Pages)
                {
                    //Console.WriteLine(page.Value.Extract);
                    return page.Value.Extract;
                }
            }
            return null;
        }


        public void SaveRawTextToFile(WikipediaResponse toSave)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Licenta\Files\WikiResult.txt"))
            {
                foreach (var page in toSave.Query.Pages)
                {
                    //Console.WriteLine(page.Value.Extract);
                    file.WriteLine(page.Value.Extract);
                }
            }
        }
    }
}