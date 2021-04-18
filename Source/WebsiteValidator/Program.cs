using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
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
            var urlOption = new Option<string>(new[] { "--url", "-u" }, description: "The url of the website you would like to crawl.");
            urlOption.IsRequired = true;

            var linksOption = new Option<bool>(new[] { "--links", "-l" }, description: "List all links that you can find.");
            var crawlOption = new Option<bool>(new[] { "--crawl", "-c" }, description: "Crawl the full page and list all links.");
            var sslOption = new Option<bool>(new[] { "--ignore-ssl" }, description: "Ignores SSL certificate");
            var humanOption = new Option<bool>(new[] {"--human", "-h"}, "Human readable output (instead of json)");

            // Create a root command with some options
            var rootCommand = new RootCommand
            {
                urlOption,
                linksOption,
                sslOption,
                humanOption,
                crawlOption
            };

            rootCommand.Description = "WebsiteValidator, a tool to crawl a website and validate it";

            // Note that the parameters of the handler method are matched according to the names of the options
            rootCommand.Handler = CommandHandler.Create<string, bool, bool, bool, bool>(ProcessCommand);

            // Parse the incoming args and invoke the handler
            return rootCommand.InvokeAsync(args).Result;
        }

        private static void ProcessCommand(string url, bool links, bool ignoreSsl, bool human, bool crawl)
        {
            var outputHelper = new OutputHelperFactory().Get(human);
            
            if (links) ListLinksForUrl(url, ignoreSsl, outputHelper);
            if (crawl) CrawlUrl(url, ignoreSsl, outputHelper);
        }

        private static void CrawlUrl(string url, bool ignoreSsl, IOutputHelper outputHelper)
        {
            IDownloadAWebpage downloadWebpage = new DownloadAWebpage(ignoreSsl);

            var crawler = new Crawler(url, downloadWebpage, outputHelper);
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
