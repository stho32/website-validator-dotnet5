using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WebsiteValidator.BL.Classes;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Tests
{
    [TestFixture]
    [NonParallelizable]
    public class CrawlerTests
    {
        private Mock<IDownloadAWebpage> CreateMockDownloader(params (string url, string content, HttpStatusCode status)[] pages)
        {
            var mock = new Mock<IDownloadAWebpage>();
            foreach (var (url, content, status) in pages)
            {
                mock.Setup(d => d.Download(url))
                    .ReturnsAsync(new Webpage(url, content, content.Length, status));
            }
            return mock;
        }

        private static IUrlInformation[] RunCrawler(Crawler crawler)
        {
            var originalOut = Console.Out;
            try
            {
                Console.SetOut(new StringWriter());
                return crawler.CrawlEverything();
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        [Test]
        public void CrawlEverything_mit_einer_Seite_ohne_Links()
        {
            var downloader = CreateMockDownloader(
                ("https://example.com", "<html><body>Hello</body></html>", HttpStatusCode.OK));
            var outputHelper = new Mock<IOutputHelper>();

            var crawler = new Crawler("https://example.com", downloader.Object, outputHelper.Object, 0, new string[0]);
            var result = RunCrawler(crawler);

            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(result[0].Url, Is.EqualTo("https://example.com"));
            Assert.That(result[0].HttpResponseCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void CrawlEverything_folgt_internen_Links()
        {
            var downloader = CreateMockDownloader(
                ("https://example.com", "<html><body><a href=\"https://example.com/page2\">Link</a></body></html>", HttpStatusCode.OK),
                ("https://example.com/page2", "<html><body>Page 2</body></html>", HttpStatusCode.OK));
            var outputHelper = new Mock<IOutputHelper>();

            var crawler = new Crawler("https://example.com", downloader.Object, outputHelper.Object, 0, new string[0]);
            var result = RunCrawler(crawler);

            Assert.That(result, Has.Length.EqualTo(2));
        }

        [Test]
        public void CrawlEverything_respektiert_Limit()
        {
            var downloader = CreateMockDownloader(
                ("https://example.com", "<html><body><a href=\"https://example.com/p2\">L</a></body></html>", HttpStatusCode.OK),
                ("https://example.com/p2", "<html><body><a href=\"https://example.com/p3\">L</a></body></html>", HttpStatusCode.OK),
                ("https://example.com/p3", "<html><body>End</body></html>", HttpStatusCode.OK));
            var outputHelper = new Mock<IOutputHelper>();

            var crawler = new Crawler("https://example.com", downloader.Object, outputHelper.Object, 1, new string[0]);
            var result = RunCrawler(crawler);

            Assert.That(result.Length, Is.LessThanOrEqualTo(2));
        }

        [Test]
        public void CrawlEverything_crawlt_keine_externen_Links()
        {
            var downloader = CreateMockDownloader(
                ("https://example.com", "<html><body><a href=\"https://external.com/page\">Extern</a></body></html>", HttpStatusCode.OK));
            var outputHelper = new Mock<IOutputHelper>();

            var crawler = new Crawler("https://example.com", downloader.Object, outputHelper.Object, 0, new string[0]);
            var result = RunCrawler(crawler);

            Assert.That(result, Has.Length.EqualTo(1));
            downloader.Verify(d => d.Download("https://external.com/page"), Times.Never);
        }

        [Test]
        public void CrawlEverything_besucht_jede_URL_nur_einmal()
        {
            var downloader = CreateMockDownloader(
                ("https://example.com", "<html><body><a href=\"https://example.com/page\">L</a></body></html>", HttpStatusCode.OK),
                ("https://example.com/page", "<html><body><a href=\"https://example.com\">Back</a></body></html>", HttpStatusCode.OK));
            var outputHelper = new Mock<IOutputHelper>();

            var crawler = new Crawler("https://example.com", downloader.Object, outputHelper.Object, 0, new string[0]);
            var result = RunCrawler(crawler);

            Assert.That(result, Has.Length.EqualTo(2));
            downloader.Verify(d => d.Download("https://example.com"), Times.Once);
            downloader.Verify(d => d.Download("https://example.com/page"), Times.Once);
        }

        [Test]
        public void CrawlEverything_mit_zusaetzlichen_EntryPoints()
        {
            var downloader = CreateMockDownloader(
                ("https://example.com", "<html><body>Main</body></html>", HttpStatusCode.OK),
                ("https://example.com/extra", "<html><body>Extra</body></html>", HttpStatusCode.OK));
            var outputHelper = new Mock<IOutputHelper>();

            var crawler = new Crawler("https://example.com", downloader.Object, outputHelper.Object, 0,
                new[] { "https://example.com/extra" });
            var result = RunCrawler(crawler);

            Assert.That(result, Has.Length.EqualTo(2));
        }

        [Test]
        public void CrawlEverything_behandelt_HTTP_500_mit_Retry()
        {
            var mock = new Mock<IDownloadAWebpage>();
            var callCount = 0;
            mock.Setup(d => d.Download("https://example.com"))
                .Returns(() =>
                {
                    callCount++;
                    if (callCount == 1)
                        return Task.FromResult<IWebpage>(new Webpage("https://example.com", "<html></html>", 13, HttpStatusCode.InternalServerError));
                    return Task.FromResult<IWebpage>(new Webpage("https://example.com", "<html></html>", 13, HttpStatusCode.OK));
                });
            var outputHelper = new Mock<IOutputHelper>();

            var crawler = new Crawler("https://example.com", mock.Object, outputHelper.Object, 0, new string[0]);
            var result = RunCrawler(crawler);

            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(callCount, Is.EqualTo(2));
        }
    }
}
