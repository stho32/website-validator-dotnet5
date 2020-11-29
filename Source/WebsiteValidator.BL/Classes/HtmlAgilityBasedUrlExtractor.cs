using System.Collections.Generic;
using HtmlAgilityPack;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes
{
    public class HtmlAgilityBasedUrlExtractor : IUrlExtractor
    {
        public string[] ExtractUrls(string pageContents)
        {
            var result = new List<string>();

            var document = new HtmlDocument();
            document.LoadHtml(pageContents);
            var linkNodes = document.DocumentNode.SelectNodes("//a[@href]");

            if (linkNodes != null)
            {
                foreach (var link in linkNodes)
                {
                    var linkUrl = link.GetAttributeValue("href", "");
                    if (!string.IsNullOrWhiteSpace(linkUrl))
                    {
                        result.Add(linkUrl);
                    }
                }
            }

            return result.ToArray();
        }
    }
}