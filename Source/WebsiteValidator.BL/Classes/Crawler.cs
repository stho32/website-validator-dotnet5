using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using WebsiteValidator.BL.ExtensionMethods;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes
{
    public class Crawler
    {
        private readonly IDownloadAWebpage _downloadWebpage;
        private readonly IOutputHelper _outputHelper;
        private readonly int _limit;
        private readonly Dictionary<string, bool> _urlsWithScrapedStatus = new Dictionary<string, bool>();
        private readonly List<IUrlInformation> _scrapeResults = new List<IUrlInformation>();
        private string _baseUrl;
        
        public Crawler(string url, IDownloadAWebpage downloadWebpage, IOutputHelper outputHelper, int limit)
        {
            _downloadWebpage = downloadWebpage;
            _outputHelper = outputHelper;
            _limit = limit;
            _urlsWithScrapedStatus.Add(url, false);
            _baseUrl = url;
        }

        public IUrlInformation[] CrawlEverything()
        {
            var nextUrl = GetNextUrlToCrawl();
            var scrapedUrls = 0;

            while (!string.IsNullOrWhiteSpace(nextUrl))
            {
                _urlsWithScrapedStatus[nextUrl] = true;
                StatusReport(scrapedUrls);
                scrapedUrls += 1; 
                Console.WriteLine($" - processing {nextUrl} ...");

                try
                {
                    var download = _downloadWebpage.Download(nextUrl); 
                    var links = download
                            .ExtractUrls()
                            .ToAbsoluteUrls(_baseUrl);

                    _scrapeResults.Add(new UrlInformation(
                        nextUrl, 
                        links, 
                        download.Result.HttpCode,
                        download.Result.RawContent,
                        ExtractInnerText(download.Result.RawContent)
                        ));
                    
                    foreach (var link in links)
                    {
                        if (!_urlsWithScrapedStatus.ContainsKey(link))
                        {
                            _urlsWithScrapedStatus.Add(link, false);
                            //Console.WriteLine($"     + {link}");
                        }
                        else
                        {
                            //Console.WriteLine($"     / {link} already known");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                nextUrl = GetNextUrlToCrawl();

                // if limit given, we only crawl n pages
                if (_limit > 0)
                {
                    if (scrapedUrls > _limit)
                    {
                        break;
                    }
                }
            }

            return _scrapeResults.ToArray();
        }

        private string ExtractInnerText(string resultRawContent)
        {
            if (!resultRawContent.ToLower().Contains("html"))
            {
                return "";
            }

            try
            {
                var document = new HtmlDocument();
                document.LoadHtml(resultRawContent);
                return document.DocumentNode.InnerText;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private void StatusReport(int scrapedUrls)
        {
            var potenialUrlsToCrawl = 
                _urlsWithScrapedStatus.
                    Where(x => 
                        x.Value == false
                        && x.Key.StartsWith(_baseUrl)).ToArray();
            
            Console.WriteLine($"## STATUS : {scrapedUrls} urls scraped, {_urlsWithScrapedStatus.Count} urls known, {potenialUrlsToCrawl.Length} urls to crawl remaining");
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