using WebsiteValidator.BL.Classes;
using Xunit;

namespace WebsiteValidator.BL.Tests
{
    public class UrlToAbsolutUrlConverterTests
    {
        private readonly UrlToAbsolutUrlConverter _converter = new();

        [Fact]
        public void Einfache_Umwandlung()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "/helloWorld.php");
            Assert.Equal("https://whatever.com/helloWorld.php", result);
        }

        [Fact]
        public void Wenn_absolute_HTTPS_URL_uebergeben_wird_dann_nichts_tun()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "https://whatever.com/test-fuer-unsere-seite");
            Assert.Equal("https://whatever.com/test-fuer-unsere-seite", result);
        }

        [Fact]
        public void Wenn_absolute_HTTP_URL_uebergeben_wird_dann_nichts_tun()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "http://example.com/page");
            Assert.Equal("http://example.com/page", result);
        }

        [Fact]
        public void Wenn_die_URL_auf_Slash_endet_dann_Slash_entfernen()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "https://whatever.com/test-fuer-unsere-seite/");
            Assert.Equal("https://whatever.com/test-fuer-unsere-seite", result);
        }

        [Fact]
        public void Mailto_Links_werden_ignoriert()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "mailto:test@example.com");
            Assert.Null(result);
        }

        [Fact]
        public void Tel_Links_werden_ignoriert()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "tel:+491234567890");
            Assert.Null(result);
        }

        [Fact]
        public void Javascript_Links_werden_ignoriert()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "javascript:void(0)");
            Assert.Null(result);
        }

        [Fact]
        public void Data_URLs_werden_ignoriert()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "data:image/png;base64,abc");
            Assert.Null(result);
        }

        [Fact]
        public void Fragment_Links_werden_ignoriert()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "#section");
            Assert.Null(result);
        }

        [Fact]
        public void Leere_URLs_werden_ignoriert()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "");
            Assert.Null(result);
        }

        [Fact]
        public void Protocol_relative_URLs_werden_durchgereicht()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "//cdn.example.com/script.js");
            Assert.Equal("//cdn.example.com/script.js", result);
        }

        [Fact]
        public void Batch_Konvertierung_filtert_ungueltige_URLs()
        {
            var links = new[] { "/page.html", "mailto:test@test.com", "https://extern.com", "#top", "/about" };
            var result = _converter.ToAbsoluteUrl("https://whatever.com", links);

            Assert.Equal(3, result.Length);
            Assert.Equal("https://whatever.com/page.html", result[0]);
            Assert.Equal("https://extern.com", result[1]);
            Assert.Equal("https://whatever.com/about", result[2]);
        }
    }
}
