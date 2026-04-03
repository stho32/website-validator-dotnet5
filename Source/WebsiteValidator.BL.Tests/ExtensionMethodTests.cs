using System.Net;
using System.Threading.Tasks;
using WebsiteValidator.BL.Classes;
using WebsiteValidator.BL.ExtensionMethods;
using WebsiteValidator.BL.Interfaces;
using Xunit;

namespace WebsiteValidator.BL.Tests
{
    public class StringArrayExtensionMethodsTests
    {
        [Fact]
        public void ToAbsoluteUrls_konvertiert_relative_URLs()
        {
            var links = new[] { "/page1", "/page2" };
            var result = links.ToAbsoluteUrls("https://example.com");

            Assert.Equal(2, result.Length);
            Assert.Equal("https://example.com/page1", result[0]);
            Assert.Equal("https://example.com/page2", result[1]);
        }

        [Fact]
        public void ToAbsoluteUrls_filtert_ungueltige_URLs()
        {
            var links = new[] { "/page1", "mailto:test@test.com", "#anchor" };
            var result = links.ToAbsoluteUrls("https://example.com");

            Assert.Single(result);
            Assert.Equal("https://example.com/page1", result[0]);
        }
    }

    public class WebpageExtensionMethodsTests
    {
        [Fact]
        public void ExtractUrls_extrahiert_Links_aus_Webpage()
        {
            var webpage = Task.FromResult<IWebpage>(
                new Webpage("https://example.com", "<html><body><a href=\"https://link.com\">Link</a></body></html>", 100, HttpStatusCode.OK));

            var urls = webpage.ExtractUrls();

            Assert.Single(urls);
            Assert.Equal("https://link.com", urls[0]);
        }

        [Fact]
        public void ExtractUrls_gibt_leeres_Array_bei_keinen_Links()
        {
            var webpage = Task.FromResult<IWebpage>(
                new Webpage("https://example.com", "<html><body>No links</body></html>", 50, HttpStatusCode.OK));

            var urls = webpage.ExtractUrls();

            Assert.Empty(urls);
        }
    }
}
