namespace _19;

internal static class Program
{
    private static readonly Dictionary<string, bool> CachedMatches = [];
    private static readonly Dictionary<(string design, string towel), long> CachedMatchCounts = [];
    private static readonly Dictionary<string, long> CachedCounts = [];
    
    internal static void Main()
    {
        var input = File.ReadAllText("input.txt").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        
        var towels = input[0].Split(", ");
        var designs = input[1].Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        Console.WriteLine($"Part 1: {PartOne(towels, designs)}");
        Console.WriteLine($"Part 2: {PartTwo(towels, designs)}");
    }

    private static long PartOne(string[] towels, string[] designs)
    {
        long tally = 0;
        
        foreach (var design in designs)
        {
            CachedMatches.Clear();
            tally += HasMatch(towels, design) ? 1 : 0;
        }
        
        return tally;
    }

    private static long PartTwo(string[] towels, string[] designs)
    {
        long tally = 0;
        
        foreach (var design in designs)
        {
            CachedMatchCounts.Clear();
            var count = CountMatches(towels, design);
            tally += count;
        }
        
        return tally;
    }
    
    private static bool HasMatch(string[] towels, string design)
    {
        if (towels.Contains(design))
            return true;

        if (CachedMatches.TryGetValue(design, out var match))
            return match;
        
        var hasMatch = false;
        
        foreach (var towel in towels.Where(design.StartsWith))
        {
            if (design.StartsWith(towel))
            {
                hasMatch = HasMatch(towels, design[towel.Length..]);
            }

            if (hasMatch)
            {
                CachedMatches.TryAdd(design, true);
                return true;
            }
        }

        CachedMatches.Add(design, false);
        return false;
    }

    private static long CountMatches(string[] towels, string design)
    {
        if (design.Length == 0)
            return 1;

        if (CachedCounts.TryGetValue(design, out var cachedMatches))
            return cachedMatches;
        
        long count = 0;
        
        foreach (var towel in towels.Where(design.StartsWith))
        {
            if (CachedMatchCounts.TryGetValue((design, towel), out var cachedCount))
            {
                count+= cachedCount;
                continue;
            }
            
            var newDesign = design[towel.Length..];
            var matches = CountMatches(towels, newDesign);
            CachedMatchCounts.Add((newDesign, towel), matches);
            count += matches;

            CachedMatchCounts.TryAdd((newDesign, towel), count);
        }

        CachedCounts.Add(design, count);
        return count;
    }
}