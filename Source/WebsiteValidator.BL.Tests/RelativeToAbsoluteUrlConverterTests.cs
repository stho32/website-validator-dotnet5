using NUnit.Framework;
using WebsiteValidator.BL.Classes;

namespace WebsiteValidator.BL.Tests
{
    [TestFixture]
    public class UrlToAbsolutUrlConverterTests
    {
        private readonly UrlToAbsolutUrlConverter _converter = new();

        [Test]
        public void Einfache_Umwandlung()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "/helloWorld.php");
            Assert.That(result, Is.EqualTo("https://whatever.com/helloWorld.php"));
        }

        [Test]
        public void Wenn_absolute_HTTPS_URL_uebergeben_wird_dann_nichts_tun()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "https://whatever.com/test-fuer-unsere-seite");
            Assert.That(result, Is.EqualTo("https://whatever.com/test-fuer-unsere-seite"));
        }

        [Test]
        public void Wenn_absolute_HTTP_URL_uebergeben_wird_dann_nichts_tun()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "http://example.com/page");
            Assert.That(result, Is.EqualTo("http://example.com/page"));
        }

        [Test]
        public void Wenn_die_URL_auf_Slash_endet_dann_Slash_entfernen()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "https://whatever.com/test-fuer-unsere-seite/");
            Assert.That(result, Is.EqualTo("https://whatever.com/test-fuer-unsere-seite"));
        }

        [Test]
        public void Mailto_Links_werden_ignoriert()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "mailto:test@example.com");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Tel_Links_werden_ignoriert()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "tel:+491234567890");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Javascript_Links_werden_ignoriert()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "javascript:void(0)");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Data_URLs_werden_ignoriert()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "data:image/png;base64,abc");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Fragment_Links_werden_ignoriert()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "#section");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Leere_URLs_werden_ignoriert()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "");
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Protocol_relative_URLs_werden_durchgereicht()
        {
            var result = _converter.ToAbsoluteUrl("https://whatever.com", "//cdn.example.com/script.js");
            Assert.That(result, Is.EqualTo("//cdn.example.com/script.js"));
        }

        [Test]
        public void Relative_URL_ohne_fuehrenden_Slash_wird_korrekt_aufgeloest()
        {
            var result = _converter.ToAbsoluteUrl("https://example.com", "service.html");
            Assert.That(result, Is.EqualTo("https://example.com/service.html"));
        }

        [Test]
        public void Relative_URL_ohne_fuehrenden_Slash_mit_Trailing_Slash_Base()
        {
            var result = _converter.ToAbsoluteUrl("https://example.com/", "index.html");
            Assert.That(result, Is.EqualTo("https://example.com/index.html"));
        }

        [Test]
        public void Relative_URL_in_Unterverzeichnis_ohne_fuehrenden_Slash()
        {
            var result = _converter.ToAbsoluteUrl("https://example.com", "sub/page.html");
            Assert.That(result, Is.EqualTo("https://example.com/sub/page.html"));
        }

        [Test]
        public void Batch_Konvertierung_filtert_ungueltige_URLs()
        {
            var links = new[] { "/page.html", "mailto:test@test.com", "https://extern.com", "#top", "/about" };
            var result = _converter.ToAbsoluteUrl("https://whatever.com", links);

            Assert.That(result, Has.Length.EqualTo(3));
            Assert.That(result[0], Is.EqualTo("https://whatever.com/page.html"));
            Assert.That(result[1], Is.EqualTo("https://extern.com"));
            Assert.That(result[2], Is.EqualTo("https://whatever.com/about"));
        }
    }
}
