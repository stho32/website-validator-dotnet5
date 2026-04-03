using System;
using System.IO;
using System.Net;
using WebsiteValidator.BL.Classes;
using Xunit;

namespace WebsiteValidator.BL.Tests
{
    [Collection("ConsoleOutput")]
    public class JsonConsoleOutputHelperTests
    {
        [Fact]
        public void Write_string_array_gibt_JSON_auf_Konsole_aus()
        {
            var helper = new JsonConsoleOutputHelper();
            var output = CaptureConsoleOutput(() => helper.Write("test", new[] { "a", "b" }));

            Assert.Contains("\"a\"", output);
            Assert.Contains("\"b\"", output);
        }

        [Fact]
        public void Write_UrlInformation_array_gibt_JSON_auf_Konsole_aus()
        {
            var helper = new JsonConsoleOutputHelper();
            var urlInfo = new UrlInformation("https://example.com", new[] { "https://link.com" }, HttpStatusCode.OK, "<html></html>", "text", 100);
            var output = CaptureConsoleOutput(() => helper.Write("test", new[] { urlInfo }));

            Assert.Contains("https://example.com", output);
            Assert.Contains("https://link.com", output);
        }

        private static string CaptureConsoleOutput(Action action)
        {
            var originalOut = Console.Out;
            using var sw = new StringWriter();
            Console.SetOut(sw);
            try
            {
                action();
                return sw.ToString();
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
    }

    [Collection("ConsoleOutput")]
    public class HumanReadableConsoleOutputHelperTests
    {
        [Fact]
        public void Write_string_array_gibt_nummerierte_Liste_aus()
        {
            var helper = new HumanReadableConsoleOutputHelper();
            var output = CaptureConsoleOutput(() => helper.Write("links", new[] { "https://a.com", "https://b.com" }));

            Assert.Contains("links:", output);
            Assert.Contains("1.", output);
            Assert.Contains("2.", output);
            Assert.Contains("https://a.com", output);
            Assert.Contains("https://b.com", output);
        }

        [Fact]
        public void Write_UrlInformation_array_gibt_verschachtelte_Liste_aus()
        {
            var helper = new HumanReadableConsoleOutputHelper();
            var urlInfo = new UrlInformation("https://example.com", new[] { "https://sub.com" }, HttpStatusCode.OK, "<html></html>", "text", 100);
            var output = CaptureConsoleOutput(() => helper.Write("result", new[] { urlInfo }));

            Assert.Contains("result:", output);
            Assert.Contains("https://example.com", output);
            Assert.Contains("https://sub.com", output);
        }

        private static string CaptureConsoleOutput(Action action)
        {
            var originalOut = Console.Out;
            using var sw = new StringWriter();
            Console.SetOut(sw);
            try
            {
                action();
                return sw.ToString();
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
    }

    public class JsonFileOutputHelperTests
    {
        [Fact]
        public void Write_string_array_schreibt_JSON_in_Datei()
        {
            var tempFile = Path.GetTempFileName();
            try
            {
                var helper = new JsonFileOutputHelper(tempFile);
                helper.Write("test", new[] { "a", "b" });

                var content = File.ReadAllText(tempFile);
                Assert.Contains("\"a\"", content);
                Assert.Contains("\"b\"", content);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void Write_UrlInformation_array_schreibt_JSON_in_Datei()
        {
            var tempFile = Path.GetTempFileName();
            try
            {
                var helper = new JsonFileOutputHelper(tempFile);
                var urlInfo = new UrlInformation("https://example.com", new[] { "https://link.com" }, HttpStatusCode.OK, "<html></html>", "text", 100);
                helper.Write("test", new[] { urlInfo });

                var content = File.ReadAllText(tempFile);
                Assert.Contains("https://example.com", content);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }
    }
}
