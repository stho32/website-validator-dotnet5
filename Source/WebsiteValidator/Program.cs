using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using WebsiteValidator.BL.Classes;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator
{
    class Program
    {
        static int Main(string[] args)
        {
            var urlOption = new Option<string>(new []{ "--url", "-u" }, description: "The url of the website you would like to crawl.");
            urlOption.IsRequired = true;

            var linksOption = new Option<bool>(new[] { "--links", "-l" }, description: "List all links that you can find.");

            // Create a root command with some options
            var rootCommand = new RootCommand
            {
                urlOption,
                linksOption
            };

            rootCommand.Description = "WebsiteValidator, a tool to crawl a website and validate it";

            // Note that the parameters of the handler method are matched according to the names of the options
            rootCommand.Handler = CommandHandler.Create<string,bool>((url, links) =>
            {
                if (links)
                {
                    ListLinksForUrl(url);
                }
            });

            // Parse the incoming args and invoke the handler
            return rootCommand.InvokeAsync(args).Result;
        }

        private static void ListLinksForUrl(string url)
        {
            IDownloadAWebpage downloadWebpage = new DownloadAWebpage();
            IUrlExtractor extractor = new HtmlAgilityBasedUrlExtractor();
            
            var website = downloadWebpage.Download(url);
            var links = extractor.ExtractUrls(website.Result.RawContent);

            for (var index = 0; index < links.Length; index++)
            {
                var link = links[index];
                Console.WriteLine($"{index,4}. {link}");
            }
        }
    }
}
