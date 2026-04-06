using NUnit.Framework;
using WebsiteValidator.BL.Classes;

namespace WebsiteValidator.BL.Tests;

[TestFixture]
public class UrlExtractorTests
{
    [Test]
    public void Extract_Url()
    {
        string html = "<a href=\"https://github.com\">";

        var extractor = new HtmlAgilityBasedUrlExtractor();
        var urls = extractor.ExtractUrls(html);

        Assert.That(urls, Has.Length.EqualTo(1));
        Assert.That(urls[0], Is.EqualTo("https://github.com"));
    }
}
