using NUnit.Framework;
using WebsiteValidator.BL.Classes;

namespace WebsiteValidator.BL.Tests
{
    [TestFixture]
    public class SitemapParserTests
    {
        private SitemapParser _parser;

        [SetUp]
        public void SetUp()
        {
            _parser = new SitemapParser();
        }

        [Test]
        public void ExtractUrls_parst_Standard_Sitemap()
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
  <url><loc>https://example.com/page1</loc></url>
  <url><loc>https://example.com/page2</loc></url>
  <url><loc>https://example.com/page3</loc></url>
</urlset>";

            var urls = _parser.ExtractUrls(xml);

            Assert.That(urls, Has.Length.EqualTo(3));
            Assert.That(urls[0], Is.EqualTo("https://example.com/page1"));
            Assert.That(urls[1], Is.EqualTo("https://example.com/page2"));
            Assert.That(urls[2], Is.EqualTo("https://example.com/page3"));
        }

        [Test]
        public void ExtractUrls_gibt_leeres_Array_bei_leerer_Sitemap()
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
</urlset>";

            var urls = _parser.ExtractUrls(xml);

            Assert.That(urls, Is.Empty);
        }

        [Test]
        public void ExtractUrls_parst_Sitemap_Index()
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<sitemapindex xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
  <sitemap><loc>https://example.com/sitemap1.xml</loc></sitemap>
  <sitemap><loc>https://example.com/sitemap2.xml</loc></sitemap>
</sitemapindex>";

            var urls = _parser.ExtractUrls(xml);

            Assert.That(urls, Has.Length.EqualTo(2));
            Assert.That(urls[0], Is.EqualTo("https://example.com/sitemap1.xml"));
            Assert.That(urls[1], Is.EqualTo("https://example.com/sitemap2.xml"));
        }

        [Test]
        public void ExtractUrls_gibt_leeres_Array_bei_ungueltiger_XML()
        {
            var xml = "This is not valid XML at all <><>";

            var urls = _parser.ExtractUrls(xml);

            Assert.That(urls, Is.Empty);
        }

        [Test]
        public void ExtractUrls_gibt_leeres_Array_bei_leerem_String()
        {
            var urls = _parser.ExtractUrls("");

            Assert.That(urls, Is.Empty);
        }

        [Test]
        public void ExtractUrls_gibt_leeres_Array_bei_null()
        {
            var urls = _parser.ExtractUrls(null);

            Assert.That(urls, Is.Empty);
        }

        [Test]
        public void ExtractUrls_ignoriert_leere_Loc_Eintraege()
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
  <url><loc>https://example.com/page1</loc></url>
  <url><loc>  </loc></url>
  <url><loc>https://example.com/page2</loc></url>
</urlset>";

            var urls = _parser.ExtractUrls(xml);

            Assert.That(urls, Has.Length.EqualTo(2));
        }

        [Test]
        public void ExtractUrls_trimmt_Whitespace_aus_URLs()
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
  <url><loc>  https://example.com/page1  </loc></url>
</urlset>";

            var urls = _parser.ExtractUrls(xml);

            Assert.That(urls[0], Is.EqualTo("https://example.com/page1"));
        }
    }
}
