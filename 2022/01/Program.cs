namespace _01;

internal static partial class Program
{
    internal static void Main()
    {
        const string filename = "input.txt";
        var blocks = File.ReadAllText($"{filename}").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        List<int[]> elves = [];
        foreach (var block in blocks)
        {
            elves.Add(block.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToIntArray());
        }
        
        Console.WriteLine($"Part 1: {PartOne(elves)}");
        Console.WriteLine($"Part 2: {PartTwo(elves)}");
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