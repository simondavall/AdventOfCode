using AocHelper;
namespace _01;

internal static class Program
{
    private const long ExpectedPartOne = 72602;
    private const long ExpectedPartTwo = 207410;
    private const string Day = "01";
    
    public static int Main(string[] args)
    {
        var filename = "input_01.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];
        
        var blocks = File.ReadAllText($"{filename}").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        List<int[]> elves = [];
        foreach (var block in blocks)
        {
            elves.Add(block.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToIntArray());
        }

        var resultPartOne = PartOne(elves);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(elves);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");
        
        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }
    
    private static long PartOne(List<int[]> elves)
    {
        long tally = 0;
        foreach (var elf in elves)
        {
            tally = Math.Max(tally, elf.Sum(x => x));
        }
        return tally;
    }

    private static long PartTwo(List<int[]> elves)
    {
        List<int> elfTotals = [];
        foreach (var elf in elves)
        {
            elfTotals.Add(elf.Sum(x => x));
        }

        return elfTotals.SortedDesc().GetRange(0, 3).Sum(x => x);
    }
}