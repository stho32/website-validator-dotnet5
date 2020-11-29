using System.Security.Cryptography.X509Certificates;
using WebsiteValidator.BL.Classes;
using WebsiteValidator.BL.Interfaces;
using Xunit;

namespace WebsiteValidator.BL.Tests
{
    public class RelativeToAbsoluteUrlConverterTests
    {
        [Fact]
        public void Einfache_Umwandlung()
        {
            var relativeUrl = "/helloWorld.php";
            var baseUrl = "https://whatever.com";

            var converter = new RelativeToAbsoluteUrlConverter();
            var result = converter.ToAbsoluteUrl(baseUrl, relativeUrl);

            Assert.Equal("https://whatever.com/helloWorld.php", result);
        }
    }
}