// ReSharper disable ClassNeverInstantiated.Global
using System.Text.RegularExpressions;


namespace _03;

internal partial class Program
{
    internal static void Main()
    {
        var input = File.ReadAllText("input.txt");
        
        Console.WriteLine($"Part 1: {PartOne(input)}");
        Console.WriteLine($"Part 2: {PartTwo(input)}");
    }

    private static long PartOne(string memory)
    {
        long tally = 0;
        
        var items = MyRegex().Matches(memory);
        foreach (Match item in items)
        {
            tally += long.Parse(item.Groups[1].Value) * long.Parse(item.Groups[2].Value);
        }
        
        return tally;
    }
    
    private static long PartTwo(string memory)
    {
        long tally = 0;
        var instructions = memory.Split("do()");
        foreach (var instructionSet in instructions)
        {
            var activeInstructions = instructionSet.Split("don't()").FirstOrDefault();
            if (activeInstructions == null) 
                continue;
            
            var items = MyRegex().Matches(activeInstructions);
            foreach (Match item in items)
            {
                tally += long.Parse(item.Groups[1].Value) * long.Parse(item.Groups[2].Value);
            }
        }
        
        return tally;
    }

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex MyRegex();
}