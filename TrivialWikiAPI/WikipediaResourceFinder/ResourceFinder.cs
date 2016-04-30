using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using LinqToWiki.Generated;
using Newtonsoft.Json;
using WikipediaResourceFinder.Models;

namespace WikipediaResourceFinder
{
    public sealed class ResourceFinder : IResourceFinder
    {
        public string GetWikipediaRawText(string topic)
        {
            var client = new WebClient();

            using (var stream = client.OpenRead("http://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&explaintext=1&titles=superman"))
            using (var reader = new StreamReader(stream))
            {
                var serializer = new JsonSerializer();
                var result = serializer.Deserialize<WikipediaResponse>(new JsonTextReader(reader));

                foreach (var page in result.Query.Pages)
                    Console.WriteLine(page.Value);
            }
            return null;
        }


        public void SaveRawTextToFile(string treSaVedem)
        {
            throw new NotImplementedException();
        }
    }
}