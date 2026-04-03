using System;
using System.Text.Json;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes
{
    public class JsonConsoleOutputHelper : IOutputHelper
    {
        private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

        public void Write(string name, string[] arrayOfThings)
        {
            var json = JsonSerializer.Serialize(arrayOfThings, JsonOptions);
            Console.WriteLine(json);
        }

        public void Write(string name, IUrlInformation[] arrayOfThings)
        {
            var json = JsonSerializer.Serialize(arrayOfThings, JsonOptions);
            Console.WriteLine(json);
        }
    }
}