using System;
using System.IO;
using System.Net;
using NUnit.Framework;
using WebsiteValidator.BL.Classes;

namespace WebsiteValidator.BL.Tests;

[TestFixture]
[NonParallelizable]
public class JsonConsoleOutputHelperTests
{
    [Test]
    public void Write_string_array_gibt_JSON_auf_Konsole_aus()
    {
        var helper = new JsonConsoleOutputHelper();
        var output = CaptureConsoleOutput(() => helper.Write("test", new[] { "a", "b" }));

        Assert.That(output, Does.Contain("\"a\""));
        Assert.That(output, Does.Contain("\"b\""));
    }

    [Test]
    public void Write_UrlInformation_array_gibt_JSON_auf_Konsole_aus()
    {
        var helper = new JsonConsoleOutputHelper();
        var urlInfo = new UrlInformation("https://example.com", new[] { "https://link.com" }, HttpStatusCode.OK, "<html></html>", "text", 100);
        var output = CaptureConsoleOutput(() => helper.Write("test", new[] { urlInfo }));

        Assert.That(output, Does.Contain("https://example.com"));
        Assert.That(output, Does.Contain("https://link.com"));
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

[TestFixture]
[NonParallelizable]
public class HumanReadableConsoleOutputHelperTests
{
    [Test]
    public void Write_string_array_gibt_nummerierte_Liste_aus()
    {
        var helper = new HumanReadableConsoleOutputHelper();
        var output = CaptureConsoleOutput(() => helper.Write("links", new[] { "https://a.com", "https://b.com" }));

        Assert.That(output, Does.Contain("links:"));
        Assert.That(output, Does.Contain("1."));
        Assert.That(output, Does.Contain("2."));
        Assert.That(output, Does.Contain("https://a.com"));
        Assert.That(output, Does.Contain("https://b.com"));
    }

    [Test]
    public void Write_UrlInformation_array_gibt_verschachtelte_Liste_aus()
    {
        var helper = new HumanReadableConsoleOutputHelper();
        var urlInfo = new UrlInformation("https://example.com", new[] { "https://sub.com" }, HttpStatusCode.OK, "<html></html>", "text", 100);
        var output = CaptureConsoleOutput(() => helper.Write("result", new[] { urlInfo }));

        Assert.That(output, Does.Contain("result:"));
        Assert.That(output, Does.Contain("https://example.com"));
        Assert.That(output, Does.Contain("https://sub.com"));
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

[TestFixture]
public class JsonFileOutputHelperTests
{
    [Test]
    public void Write_string_array_schreibt_JSON_in_Datei()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            var helper = new JsonFileOutputHelper(tempFile);
            helper.Write("test", new[] { "a", "b" });

            var content = File.ReadAllText(tempFile);
            Assert.That(content, Does.Contain("\"a\""));
            Assert.That(content, Does.Contain("\"b\""));
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Test]
    public void Write_UrlInformation_array_schreibt_JSON_in_Datei()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            var helper = new JsonFileOutputHelper(tempFile);
            var urlInfo = new UrlInformation("https://example.com", new[] { "https://link.com" }, HttpStatusCode.OK, "<html></html>", "text", 100);
            helper.Write("test", new[] { urlInfo });

            var content = File.ReadAllText(tempFile);
            Assert.That(content, Does.Contain("https://example.com"));
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}
