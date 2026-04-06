using System;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes;

public class HumanReadableConsoleOutputHelper : IOutputHelper
{
    public void Write(string name, string[] arrayOfThings)
    {
        Console.WriteLine(name + ":");

        var position = 0;
        foreach (var thing in arrayOfThings)
        {
            position += 1;
            Console.WriteLine($" - {position,4}. {thing}");
        }
    }

    public void Write(string name, IUrlInformation[] arrayOfThings)
    {
        Console.WriteLine(name + ":");

        var position = 0;
        foreach (var thing in arrayOfThings)
        {
            position += 1;
            Console.WriteLine($" - {position,4}. {thing.Url}");
            Console.WriteLine($"          HTTP: {(int)thing.HttpResponseCode} | HTML Valid: {(thing.IsHtmlValid ? "Yes" : "No")}");

            if (!thing.IsHtmlValid && thing.HtmlErrors.Length > 0)
            {
                foreach (var error in thing.HtmlErrors)
                {
                    Console.WriteLine($"          ERROR: {error}");
                }
            }

            var subposition = 0;
            foreach (var link in thing.Links)
            {
                subposition += 1;
                Console.WriteLine($"   - {subposition,4}. {link}");
            }
        }
    }
}
