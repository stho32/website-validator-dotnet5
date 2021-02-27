using System;
using WebsiteValidator.BL.Interfaces;

namespace WebsiteValidator.BL.Classes
{
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
    }
}