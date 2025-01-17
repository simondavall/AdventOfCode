﻿namespace _05;

internal static partial class Program
{
    private static readonly List<int[]> FailedSets = [];
    
    internal static void Main()
    {
        var input = File.ReadAllText("input.txt").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        
        var ruleSets = input[0].Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var updates = input[1].Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        Dictionary<int, List<int>> rules = [];
        foreach (var ruleSet in ruleSets)
        {
            var key = ruleSet[..2].ToInt();
            var value = ruleSet[3..].ToInt();
            if (!rules.TryAdd(key, [value]))
                rules[key].Add(value);
        }

        List<int[]> pageSets = [];
        pageSets.AddRange(updates.Select(update => update.Split(",").ToIntArray()));

        Console.WriteLine($"Part 1: {PartOne(rules, pageSets)}");
        Console.WriteLine($"Part 2: {PartTwo(rules)}");
    }

    private static long PartOne(Dictionary<int, List<int>> rules, List<int[]> pageSets)
    {
        long tally = 0;

        foreach (var pages in pageSets)
        {
            var pageCount = pages.Length;
            var currentPage = 0;
            var isValid = true;
            while (currentPage < pageCount - 1 && isValid)
            {
                for (var i = currentPage + 1; i < pageCount; i++)
                {
                    if (SatisfiesRules(rules, pages[currentPage], pages[i])) 
                        continue;
                    
                    FailedSets.Add(pages);
                    isValid = false;
                    break;
                }

                if (isValid)
                    currentPage++;
            }

            if (isValid)
                tally += pages[pageCount / 2];
        }
        
        return tally;
    }
    
    private static long PartTwo(Dictionary<int, List<int>> rules)
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
                    if (SatisfiesRules(rules, pages[currentPage], pages[i])) 
                        continue;
                    
                    (pages[i], pages[currentPage]) = (pages[currentPage], pages[i]);
                    // reset and start pageSet again
                    currentPage = 0;
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
}