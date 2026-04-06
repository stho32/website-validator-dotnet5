using System.IO;
using System.Text.Json;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes;

public class JsonFileOutputHelper : IOutputHelper
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    private readonly string _outputFilename;

    public JsonFileOutputHelper(string outputFilename)
    {
        _outputFilename = outputFilename;
    }

    public void Write(string name, string[] arrayOfThings)
    {
        var json = JsonSerializer.Serialize(arrayOfThings, JsonOptions);
        File.WriteAllText(_outputFilename, json);
    }

    public void Write(string name, IUrlInformation[] arrayOfThings)
    {
        var json = JsonSerializer.Serialize(arrayOfThings, JsonOptions);
        File.WriteAllText(_outputFilename, json);
    }
}
