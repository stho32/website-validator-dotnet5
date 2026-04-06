using NUnit.Framework;
using WebsiteValidator.BL.Classes;

namespace WebsiteValidator.BL.Tests;

[TestFixture]
public class OutputHelperFactoryTests
{
    private readonly OutputHelperFactory _factory = new();

    [Test]
    public void Get_mit_human_true_liefert_HumanReadableConsoleOutputHelper()
    {
        var result = _factory.Get(true, null);
        Assert.That(result, Is.TypeOf<HumanReadableConsoleOutputHelper>());
    }

    [Test]
    public void Get_mit_human_true_und_outputFilename_liefert_HumanReadableConsoleOutputHelper()
    {
        var result = _factory.Get(true, "output.json");
        Assert.That(result, Is.TypeOf<HumanReadableConsoleOutputHelper>());
    }

    [Test]
    public void Get_ohne_human_ohne_outputFilename_liefert_JsonConsoleOutputHelper()
    {
        var result = _factory.Get(false, null);
        Assert.That(result, Is.TypeOf<JsonConsoleOutputHelper>());
    }

    [Test]
    public void Get_ohne_human_mit_leerem_outputFilename_liefert_JsonConsoleOutputHelper()
    {
        var result = _factory.Get(false, "");
        Assert.That(result, Is.TypeOf<JsonConsoleOutputHelper>());
    }

    [Test]
    public void Get_ohne_human_mit_outputFilename_liefert_JsonFileOutputHelper()
    {
        var result = _factory.Get(false, "output.json");
        Assert.That(result, Is.TypeOf<JsonFileOutputHelper>());
    }
}
