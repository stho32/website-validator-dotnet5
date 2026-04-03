using System;
using System.CommandLine;
using System.IO;
using WebsiteValidator.BL.Classes;
using WebsiteValidator.BL.ExtensionMethods;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator
{
    class Program
    {
        static int Main(string[] args)
        {
            var urlOption = new Option<string>("--url", "The url of the website you would like to crawl.");
            urlOption.Aliases.Add("-u");
            urlOption.Required = true;

            var linksOption = new Option<bool>("--links", "List all links that you can find.");
            linksOption.Aliases.Add("-l");

            var crawlOption = new Option<bool>("--crawl", "Crawl the full page and list all links.");
            crawlOption.Aliases.Add("-c");

            var sslOption = new Option<bool>("--ignore-ssl", "Ignores SSL certificate");

            var humanOption = new Option<bool>("--human", "Human readable output (instead of json)");
            humanOption.Aliases.Add("-h");

            var outputOption = new Option<string>("--output", "Where to save the results. Without the option i'll write on the screen.");
            outputOption.Aliases.Add("-o");

            var limitOption = new Option<int>("--limit", "Maximum number of pages to crawl.");

            var additionalEntryPointsOption = new Option<string>(
                "--additionalEntrypoints",
                "A simple text file with a list of urls, for e.g. sitemap-links...");
            additionalEntryPointsOption.Aliases.Add("--ae");

            var rootCommand = new RootCommand("WebsiteValidator, a tool to crawl a website and validate it")
            {
                urlOption,
                linksOption,
                sslOption,
                humanOption,
                crawlOption,
                outputOption,
                limitOption,
                additionalEntryPointsOption
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

                ProcessCommand(url, links, ignoreSsl, human, crawl, output, limit, additionalEntryPoints);
            });

            return rootCommand.Parse(args).Invoke();
        }

        private static void ProcessCommand(string url, bool links, bool ignoreSsl, bool human, bool crawl, string output,
            int limit, string additionalEntryPoints)
        {
            var outputHelper = new OutputHelperFactory().Get(human, output);

            if (links) ListLinksForUrl(url, ignoreSsl, outputHelper);

            if (!string.IsNullOrWhiteSpace(additionalEntryPoints))
            {
                var additionalKnownLinks = File.ReadAllLines(additionalEntryPoints);
                if (crawl) CrawlUrl(url, ignoreSsl, outputHelper, limit, additionalKnownLinks);
            }
            else
            {
                if (crawl) CrawlUrl(url, ignoreSsl, outputHelper, limit, Array.Empty<string>());
            }
        }

        private static void CrawlUrl(string url, bool ignoreSsl, IOutputHelper outputHelper, int limit,
            string[] additionalKnownLinks)
        {
            IDownloadAWebpage downloadWebpage = new DownloadAWebpage(ignoreSsl);

            var crawler = new Crawler(url, downloadWebpage, outputHelper, limit, additionalKnownLinks);
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
