namespace _11;

internal static class Program
{
    private static readonly Dictionary<long, long[]> CachedRulesResult = [];
    
    internal static void Main()
    {
        const string aocDay = "11";
        const string filename = "input.txt";
        const string path = $"/home/sdv/Documents/Projects/Aoc/2024/{aocDay}/";
        var source = File.ReadAllText($"{path}{filename}");
        
        Console.WriteLine($"Part 1: {PartOne(source)}");
        Console.WriteLine($"Part 2: {PartTwo(source)}");
    }

    private static long PartOne(string source)
    {
        var list = source.ToLongDictionary();
        long tally = 0;
        var counter = 0;
        while (counter++ < 25)
        {
            list = Blink(list);
        }

        foreach (var item in list)
        {
            tally += item.Value;
        }
        
        return tally;
    }

    private static long PartTwo(string source)
    {
        var list = source.ToLongDictionary();
        long tally = 0;
        var counter = 0;
        while (counter++ < 75)
        {
            list = Blink(list);
        }

        foreach (var item in list)
        {
            tally += item.Value;
        }
        
        return tally;
    }
    
    private static Dictionary<long, long> Blink(Dictionary<long, long> list)
    {
        Dictionary<long, long> nextResults = [];

        foreach (var listItem in list)
        {
            long[] results;
            if (CachedRulesResult.TryGetValue(listItem.Key, out var value))
            {
                results = value;
            }
            else
            {
                results = ApplyRules(listItem.Key);
                CachedRulesResult.Add(listItem.Key, results);
            }

            foreach (var result in results)
            {
                if (!nextResults.TryAdd(result, 1 * listItem.Value))
                {
                    nextResults[result] += listItem.Value;
                }
            }
        }

        return nextResults;
    }
    
    private static long[] ApplyRules(long i)
    {
        if (i == 0)
            return [1];
        
        var digitsCount = GetDigitsCount(i);
        if (digitsCount % 2 == 0)
        {
            var multiplier = 10;
            for (var j = 1; j < digitsCount / 2; j++)
                multiplier *= 10;
            
            return [i / multiplier, i % multiplier];
        }
        
        return [i * 2024];
    }
    
    private static int GetDigitsCount(long i)
    {
        var count = 0;
            
        while (i > 0)
        {
            count++;
            i /= 10;
        }

        return count;
    }

    private static Dictionary<long, long> ToLongDictionary(this string map)
    {
        var dict = new Dictionary<long, long>();
        
        var numbers = map.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        foreach (var i in numbers)
        {
            if (!dict.TryAdd(long.Parse(i), 1))
            {
                dict[long.Parse(i)]++;
            }
        }

        return dict;
    }
}