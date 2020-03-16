/**********************************************************************************************
 * Merge ICS Files to a single file that can be imported to Outlook as a new calendar or
 * emerged into an existing one
 *
 * Enjoy,
 * Alon Fliess
 *
 *
 **********************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MergeIcsFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2 || Directory.Exists(args[0]) == false)
                {
                    var oldColor = Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please provide a folder that contains ics files:");
                    Console.WriteLine(@"MergeIcsFiles C:\temp\icsFiles mergedFile.ics");
                    Console.ForegroundColor = oldColor;
                    return;
                }

                var calendarFileLines = new List<string> {"BEGIN:VCALENDAR"};

                foreach (var file in Directory.EnumerateFiles(args[0]))
                {
                    File.ReadAllLines(file)
                        .Select(line =>
                            line.Contains("BEGIN:VCALENDAR") || line.Contains("END:VCALENDAR")
                                ? Environment.NewLine
                                : line).
                        Select(line=>line.Contains("METHOD:REQUEST") ? "METHOD:PUBLISH" : line).ToList()
                        .ForEach(line => calendarFileLines.Add(line));
                }

                calendarFileLines.Add("END:VCALENDAR");
                File.WriteAllLines(args[1], calendarFileLines);

            }
            catch (Exception e)
            {
                var oldColor = Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error has occured: {e.Message}");
                Console.ForegroundColor = oldColor;
            }
        }
    }
}
