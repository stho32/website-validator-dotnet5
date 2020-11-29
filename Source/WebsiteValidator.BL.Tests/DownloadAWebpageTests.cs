using System;
using System.Net;
using WebsiteValidator.BL.Classes;
using Xunit;
using Xunit.Abstractions;

namespace WebsiteValidator.BL.Tests
{
    public class DownloadAWebpageTests
    {
        /// <summary>
        /// This is not exactly a unit test. It will check if the download functionality works
        /// by trying to download a sample page from github which is placed in the
        /// github pages site.
        /// </summary>
        [Fact]
        public async void Can_download_sample_from_github()
        {
            string url = "https://stho32.github.io/website-validator/can_download_sample.html";

            var downloader = new DownloadAWebpage();

            var result = await downloader.Download(url);

            Assert.Equal(HttpStatusCode.OK, result.HttpCode);
            Assert.Contains("automated test", result.RawContent);
        }
    }
}
