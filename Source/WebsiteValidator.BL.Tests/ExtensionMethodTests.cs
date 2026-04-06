using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using WebsiteValidator.BL.Classes;
using WebsiteValidator.BL.ExtensionMethods;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Tests;

[TestFixture]
public class StringArrayExtensionMethodsTests
{
    [Test]
    public void ToAbsoluteUrls_konvertiert_relative_URLs()
    {
        var links = new[] { "/page1", "/page2" };
        var result = links.ToAbsoluteUrls("https://example.com");

        Assert.That(result, Has.Length.EqualTo(2));
        Assert.That(result[0], Is.EqualTo("https://example.com/page1"));
        Assert.That(result[1], Is.EqualTo("https://example.com/page2"));
    }

    [Test]
    public void ToAbsoluteUrls_filtert_ungueltige_URLs()
    {
        var links = new[] { "/page1", "mailto:test@test.com", "#anchor" };
        var result = links.ToAbsoluteUrls("https://example.com");

        Assert.That(result, Has.Length.EqualTo(1));
        Assert.That(result[0], Is.EqualTo("https://example.com/page1"));
    }

    [Test]
    public void ToAbsoluteUrls_entfernt_Duplikate()
    {
        var links = new[] { "/page1", "/page2", "/page1", "/page2", "/page1" };
        var result = links.ToAbsoluteUrls("https://example.com");

        Assert.That(result, Has.Length.EqualTo(2));
        Assert.That(result[0], Is.EqualTo("https://example.com/page1"));
        Assert.That(result[1], Is.EqualTo("https://example.com/page2"));
    }
}

[TestFixture]
public class WebpageExtensionMethodsTests
{
    [Test]
    public void ExtractUrls_extrahiert_Links_aus_Webpage()
    {
        var webpage = Task.FromResult<IWebpage>(
            new Webpage("https://example.com", "<html><body><a href=\"https://link.com\">Link</a></body></html>", 100, HttpStatusCode.OK));

        var urls = webpage.ExtractUrls();

        Assert.That(urls, Has.Length.EqualTo(1));
        Assert.That(urls[0], Is.EqualTo("https://link.com"));
    }

    [Test]
    public void ExtractUrls_gibt_leeres_Array_bei_keinen_Links()
    {
        var webpage = Task.FromResult<IWebpage>(
            new Webpage("https://example.com", "<html><body>No links</body></html>", 50, HttpStatusCode.OK));

        var urls = webpage.ExtractUrls();

        Assert.That(urls, Is.Empty);
    }
}
