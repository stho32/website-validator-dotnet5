using System;
using System.Linq;
using System.Xml.Linq;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes
{
    public class SitemapParser : ISitemapParser
    {
        private static readonly XNamespace SitemapNs = "http://www.sitemaps.org/schemas/sitemap/0.9";

        public string[] ExtractUrls(string sitemapXml)
        {
            if (string.IsNullOrWhiteSpace(sitemapXml))
                return Array.Empty<string>();

            try
            {
                var doc = XDocument.Parse(sitemapXml);
                var root = doc.Root;

                if (root == null)
                    return Array.Empty<string>();

                return root.Descendants(SitemapNs + "loc")
                    .Select(e => e.Value.Trim())
                    .Where(url => !string.IsNullOrWhiteSpace(url))
                    .ToArray();
            }
            catch
            {
                return Array.Empty<string>();
            }
        }
    }
}
