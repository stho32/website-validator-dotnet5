using System;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using WebsiteValidator.BL.Classes;

namespace WebsiteValidator.BL.Tests
{
    [TestFixture]
    public class DownloadAWebpageTests
    {
        /// <summary>
        /// This is not exactly a unit test. It will check if the download functionality works
        /// by trying to download a sample page from github which is placed in the
        /// github pages site.
        /// </summary>
        [Test]
        public async Task Downloading_a_webpage_with_good_certificate_works()
        {
            string url = "https://stho32.github.io/website-validator-dotnet5/can_download_sample.html";

            var downloader = new DownloadAWebpage(false);
            var result = await downloader.Download(url);

            Assert.That(result.HttpCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(result.RawContent, Does.Contain("automated test"));
        }

        [Test]
        public void Downloading_a_webpage_with_bad_certificate_doesnt_work()
        {
            string url = "https://stho32.github.io/website-validator-dotnet5/can_download_sample.html";

            var downloader = new DownloadAWebpage(false);
            downloader.ForceBadCertificate = true;

            Assert.Throws<AggregateException>(() =>
            {
                downloader.Download(url).Wait();
            });
        }

        [Test]
        public async Task Downloading_a_webpage_with_bad_certificate_but_us_ignoring_it_works()
        {
            string url = "https://stho32.github.io/website-validator-dotnet5/can_download_sample.html";

            var downloader = new DownloadAWebpage(true);
            var result = await downloader.Download(url);

            Assert.That(result.HttpCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
