using WebsiteValidator.BL.Classes;
using Xunit;

namespace WebsiteValidator.BL.Tests
{
    public class OutputHelperFactoryTests
    {
        private readonly OutputHelperFactory _factory = new();

        [Fact]
        public void Get_mit_human_true_liefert_HumanReadableConsoleOutputHelper()
        {
            var result = _factory.Get(true, null);
            Assert.IsType<HumanReadableConsoleOutputHelper>(result);
        }

        [Fact]
        public void Get_mit_human_true_und_outputFilename_liefert_HumanReadableConsoleOutputHelper()
        {
            var result = _factory.Get(true, "output.json");
            Assert.IsType<HumanReadableConsoleOutputHelper>(result);
        }

        [Fact]
        public void Get_ohne_human_ohne_outputFilename_liefert_JsonConsoleOutputHelper()
        {
            var result = _factory.Get(false, null);
            Assert.IsType<JsonConsoleOutputHelper>(result);
        }

        [Fact]
        public void Get_ohne_human_mit_leerem_outputFilename_liefert_JsonConsoleOutputHelper()
        {
            var result = _factory.Get(false, "");
            Assert.IsType<JsonConsoleOutputHelper>(result);
        }

        [Fact]
        public void Get_ohne_human_mit_outputFilename_liefert_JsonFileOutputHelper()
        {
            var result = _factory.Get(false, "output.json");
            Assert.IsType<JsonFileOutputHelper>(result);
        }
    }
}
