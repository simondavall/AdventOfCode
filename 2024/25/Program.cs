namespace _25;

internal static class Program
{
    private static readonly List<Schematic> Locks = [];
    private static readonly List<Schematic> Keys = [];

    internal static void Main()
    {
        var input = File.ReadAllText("input.txt").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        foreach (var schematic in input)
        {
            var lines = schematic.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            
            var item = new Schematic();
            foreach (var row in lines)
            {
                item.AddRow(row);
            }
            
            if (lines[0][0] == '#')
                Locks.Add(item);
            else
                Keys.Add(item);
        }

        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: Merry Christmas! :-)");
    }

    private static long PartOne()
    {
        long tally = 0;
        foreach (var k in Keys)
        {
            foreach (var l in Locks)
            {
                if(k.Size + l.Size > 25)
                    continue;
                tally += MatchKey(l, k);
            }
        }
        return tally;
    }

    private static int MatchKey(Schematic l, Schematic k)
    {
        return l.Tumblers.Where((t, i) => t + k.Tumblers[i] > 5).Any() ? 0 : 1;
    }
    
    private static long PartTwo()
    {
        long tally = 0;
        return tally;
    }
}