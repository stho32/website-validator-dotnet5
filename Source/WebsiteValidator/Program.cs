using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using WebsiteValidator.BL.Classes;
using WebsiteValidator.BL.ExtensionMethods;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator
{
    class Program
    {
        static int Main(string[] args)
        {
            var urlOption = new Option<string>("--url", "-u") { Description = "The url of the website you would like to crawl.", Required = true };

            var linksOption = new Option<bool>("--links", "-l") { Description = "List all links that you can find." };

            var crawlOption = new Option<bool>("--crawl", "-c") { Description = "Crawl the full page and list all links." };

            var sslOption = new Option<bool>("--ignore-ssl") { Description = "Ignores SSL certificate" };

            var humanOption = new Option<bool>("--human", "-h") { Description = "Human readable output (instead of json)" };

            var outputOption = new Option<string>("--output", "-o") { Description = "Where to save the results. Without the option i'll write on the screen." };

            var limitOption = new Option<int>("--limit") { Description = "Maximum number of pages to crawl." };

            var additionalEntryPointsOption = new Option<string>("--additionalEntrypoints", "--ae") { Description = "A simple text file with a list of urls, for e.g. sitemap-links..." };

            var sitemapOption = new Option<bool>("--sitemap", "-s") { Description = "Automatically fetch sitemap.xml and include all URLs as entry points." };

            var validateHtmlOption = new Option<bool>("--validate-html", "--vh") { Description = "Validate HTML of each crawled page and report errors." };

            var rootCommand = new RootCommand("WebsiteValidator, a tool to crawl a website and validate it")
            {
                urlOption,
                linksOption,
                sslOption,
                humanOption,
                crawlOption,
                outputOption,
                limitOption,
                additionalEntryPointsOption,
                sitemapOption,
                validateHtmlOption
            };

            rootCommand.SetAction((parseResult) =>
            {
                var url = parseResult.GetValue(urlOption);
                var links = parseResult.GetValue(linksOption);
                var ignoreSsl = parseResult.GetValue(sslOption);
                var human = parseResult.GetValue(humanOption);
                var crawl = parseResult.GetValue(crawlOption);
                var output = parseResult.GetValue(outputOption);
                var limit = parseResult.GetValue(limitOption);
                var additionalEntryPoints = parseResult.GetValue(additionalEntryPointsOption);
                var sitemap = parseResult.GetValue(sitemapOption);
                var validateHtml = parseResult.GetValue(validateHtmlOption);

                ProcessCommand(url, links, ignoreSsl, human, crawl, output, limit, additionalEntryPoints, sitemap, validateHtml);
            });

            return rootCommand.Parse(args).Invoke();
        }

        private static void ProcessCommand(string url, bool links, bool ignoreSsl, bool human, bool crawl, string output,
            int limit, string additionalEntryPoints, bool sitemap, bool validateHtml)
        {
            var outputHelper = new OutputHelperFactory().Get(human, output);

            if (links) ListLinksForUrl(url, ignoreSsl, outputHelper);

            var allAdditionalLinks = new List<string>();

            if (!string.IsNullOrWhiteSpace(additionalEntryPoints))
            {
                allAdditionalLinks.AddRange(File.ReadAllLines(additionalEntryPoints));
            }

            if (sitemap)
            {
                var sitemapUrls = FetchSitemapUrls(url, ignoreSsl);
                allAdditionalLinks.AddRange(sitemapUrls);
            }

            if (crawl) CrawlUrl(url, ignoreSsl, outputHelper, limit, allAdditionalLinks.Distinct().ToArray(), validateHtml);
        }

        private static string[] FetchSitemapUrls(string baseUrl, bool ignoreSsl)
        {
            var sitemapUrl = baseUrl.TrimEnd('/') + "/sitemap.xml";
            Console.WriteLine($"Fetching sitemap from {sitemapUrl} ...");

            try
            {
                IDownloadAWebpage downloader = new DownloadAWebpage(ignoreSsl);
                var webpage = downloader.Download(sitemapUrl).Result;

                if ((int)webpage.HttpCode < 200 || (int)webpage.HttpCode > 299)
                {
                    Console.WriteLine($"Warning: Could not fetch sitemap (HTTP {(int)webpage.HttpCode}). Continuing without sitemap.");
                    return Array.Empty<string>();
                }

                var parser = new SitemapParser();
                var urls = parser.ExtractUrls(webpage.RawContent);

                var sitemapIndexUrls = urls.Where(u => u.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)).ToArray();
                var pageUrls = urls.Where(u => !u.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)).ToList();

                foreach (var subSitemapUrl in sitemapIndexUrls)
                {
                    try
                    {
                        var subWebpage = downloader.Download(subSitemapUrl).Result;
                        if ((int)subWebpage.HttpCode >= 200 && (int)subWebpage.HttpCode <= 299)
                        {
                            var subUrls = parser.ExtractUrls(subWebpage.RawContent);
                            pageUrls.AddRange(subUrls);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Warning: Could not fetch sub-sitemap {subSitemapUrl}: {e.Message}");
                    }
                }

                Console.WriteLine($"Found {pageUrls.Count} URLs in sitemap.");
                return pageUrls.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Warning: Could not fetch sitemap: {e.Message}. Continuing without sitemap.");
                return Array.Empty<string>();
            }
        }

        private static void CrawlUrl(string url, bool ignoreSsl, IOutputHelper outputHelper, int limit,
            string[] additionalKnownLinks, bool validateHtml = false)
        {
            IDownloadAWebpage downloadWebpage = new DownloadAWebpage(ignoreSsl);

            var crawler = new Crawler(url, downloadWebpage, outputHelper, limit, additionalKnownLinks, validateHtml);
            var result = crawler.CrawlEverything();

            outputHelper.Write("crawlresult", result);
        }

        private static void ListLinksForUrl(string url, bool ignoreSsl, IOutputHelper outputHelper)
        {
            IDownloadAWebpage downloadWebpage = new DownloadAWebpage(ignoreSsl);

            var links =
                downloadWebpage
                    .Download(url)
                    .ExtractUrls()
                    .ToAbsoluteUrls(url);

            outputHelper.Write("links", links);
        }
    }
}
