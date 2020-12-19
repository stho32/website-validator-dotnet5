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
            var urlOption = new Option<string>(new[] { "--url", "-u" }, description: "The url of the website you would like to crawl.");
            urlOption.IsRequired = true;

            var linksOption = new Option<bool>(new[] { "--links", "-l" }, description: "List all links that you can find.");

            var sslOption = new Option<bool>(new[] { "--ignore-ssl" }, description: "Ignores SSL certificate");

            // Create a root command with some options
            var rootCommand = new RootCommand
            {
                urlOption,
                linksOption,
                sslOption
            };

            rootCommand.Description = "WebsiteValidator, a tool to crawl a website and validate it";

            // Note that the parameters of the handler method are matched according to the names of the options
            rootCommand.Handler = CommandHandler.Create<string, bool, bool>(ProcessCommand);

            // Parse the incoming args and invoke the handler
            return rootCommand.InvokeAsync(args).Result;
        }

        private static void ProcessCommand(string url, bool links, bool ignoreSsl)
        {
            if (links)
            {
                ListLinksForUrl(url, ignoreSsl);
            }
        }

        private static void ListLinksForUrl(string url, bool ignoreSsl)
        {
            IDownloadAWebpage downloadWebpage = new DownloadAWebpage(ignoreSsl);
            IUrlExtractor extractor = new HtmlAgilityBasedUrlExtractor();
            
            var website = downloadWebpage.Download(url);
            var links = extractor.ExtractUrls(website.Result.RawContent);
            var converter = new UrlToAbsolutUrlConverter();

            for (var index = 0; index < links.Length; index++)
            {
                var result = converter.ToAbsoluteUrl(url, links[index]);
                Console.WriteLine($"{index,4}. {result}");
            }
        }
    }
}
