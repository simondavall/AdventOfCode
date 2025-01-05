// ReSharper disable ClassNeverInstantiated.Global

namespace _05;

internal static class Program
{
    private static readonly List<int[]> FailedSets = [];
    
    internal static void Main()
    {
        var input = File.ReadAllText("input.txt").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        
        var ruleSets = input[0].Split("\n");
        var updates = input[1].Split("\n", StringSplitOptions.RemoveEmptyEntries);
        
        Dictionary<int, List<int>> rules = [];
        foreach (var ruleSet in ruleSets)
        {
            var rule = ruleSet.Split("|").ToIntArray();
            var key = rule[0];
            var value = rule[1];
            if (rules.TryGetValue(key, out var values))
            {
                values.Add(value);
                continue;
            }
            
            rules.Add(key, [value]);
        }

        List<int[]> pageSets = [];
        pageSets.AddRange(updates.Select(update => update.Split(",").ToIntArray()));

        Console.WriteLine($"Part 1: {PartOne(rules, pageSets)}");
        Console.WriteLine($"Part 2: {PartTwo(rules, pageSets)}");
    }

    private static long PartOne(Dictionary<int, List<int>> rules, List<int[]> pageSets)
    {
        long tally = 0;

        foreach (var pages in pageSets)
        {
            var pageCount = pages.Length;
            var currentPage = 0;
            var isValid = true;
            while (currentPage < pageCount - 1)
            {
                for (var i = currentPage + 1; i < pageCount; i++)
                {
                    if (!SatisfiesRules(rules, pages[currentPage], pages[i]))
                    {
                        FailedSets.Add(pages);
                        isValid = false;
                        break;
                    }
                }

                if (!isValid)
                    break;
                
                currentPage++;
            }

            if (isValid)
                tally += pages[pageCount / 2];
        }
        
        return tally;
    }
    
    private static long PartTwo(Dictionary<int, List<int>> rules, List<int[]> pageSets)
    {
        long tally = 0;

        foreach (var pages in FailedSets)
        {
            var pageCount = pages.Length;
            var currentPage = 0;
            
            while (currentPage < pageCount - 1)
            {
                for (var i = currentPage + 1; i < pageCount; i++)
                {
                    if (!SatisfiesRules(rules, pages[currentPage], pages[i]))
                    {
                        (pages[i], pages[currentPage]) = (pages[currentPage], pages[i]);
                        // reset and start pageSet again
                        currentPage = 0;
                    }
                }
                
                currentPage++;
            }
            
            tally += pages[pageCount / 2];
        }
        
        return tally;
    }
    
    private static bool SatisfiesRules(Dictionary<int, List<int>> orderings, int first, int second)
    {
        return !orderings.TryGetValue(second, out var value) || !value.Contains(first);
    }

    private static int[] ToIntArray(this string[] array)
    {
        var intArray = new int[array.Length];
        for (var i = 0; i < array.Length; i++)
        {
            intArray[i] = int.Parse(array[i]);
        }

        return intArray;
    }
}