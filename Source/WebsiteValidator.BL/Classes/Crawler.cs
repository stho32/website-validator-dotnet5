using System;
using System.Collections.Generic;
using System.Linq;
using WebsiteValidator.BL.ExtensionMethods;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes
{
    public class Crawler
    {
        private readonly IDownloadAWebpage _downloadWebpage;
        private readonly IOutputHelper _outputHelper;
        private readonly Dictionary<string, bool> _urlsWithScrapedStatus = new Dictionary<string, bool>();
        private readonly List<IUrlInformation> _scrapeResults = new List<IUrlInformation>();
        private string _baseUrl;
        
        public Crawler(string url, IDownloadAWebpage downloadWebpage, IOutputHelper outputHelper)
        {
            _downloadWebpage = downloadWebpage;
            _outputHelper = outputHelper;
            _urlsWithScrapedStatus.Add(url, false);
            _baseUrl = url;
        }

        public IUrlInformation[] CrawlEverything()
        {
            var nextUrl = GetNextUrlToCrawl();

            while (!string.IsNullOrWhiteSpace(nextUrl))
            {
                _urlsWithScrapedStatus[nextUrl] = true;
                Console.WriteLine($" - processing {nextUrl} ... (n to stop)");
                if (Console.ReadLine() == "n")
                {
                    break;
                }

                try
                {
                    var links =
                        _downloadWebpage
                            .Download(nextUrl)
                            .ExtractUrls()
                            .ToAbsoluteUrls(nextUrl);

                    _scrapeResults.Add(new UrlInformation(
                        nextUrl, 
                        links, 
                        -1));
                    
                    foreach (var link in links)
                    {
                        if (!_urlsWithScrapedStatus.ContainsKey(link))
                        {
                            _urlsWithScrapedStatus.Add(link, false);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                nextUrl = GetNextUrlToCrawl();
            }

            return _scrapeResults.ToArray();
        }

        private string GetNextUrlToCrawl()
        {
            var potenialUrlsToCrawl = 
                _urlsWithScrapedStatus.
                    FirstOrDefault(x => 
                        x.Value == false
                        && x.Key.StartsWith(_baseUrl)).Key;

            return potenialUrlsToCrawl;
        }
    }
}