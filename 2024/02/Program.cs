namespace _02;

internal static class Program
{
    private static bool _isLevelIncreasing;

    internal static void Main()
    {
        var input = File.ReadAllText("input.txt").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        Console.WriteLine($"Part 1: {PartOne(input)}");
        Console.WriteLine($"Part 2: {PartTwo(input.ToList())}");
    }
    
    private static long PartOne(string[] reports)
    {
        long tally = 0;
        foreach (var report in reports)
        {
            var levels = report.Split(' ').Select(int.Parse).ToList();
            tally += IsReportSafe(levels) ? 1 : 0;
        }
        
        return tally;
    }

    private static long PartTwo(List<string> reports)
    {
        var tally = 0;

        foreach (var report in reports)
        {
            var levels = report.Split(' ').Select(int.Parse).ToList();
            if (IsReportSafe(levels))
            {
                tally++;
                continue;
            }
            
            for (var i = 0; i < levels.Count; i++)
            {
                List<int> amendedLevels = [];
                amendedLevels.AddRange(levels.Where((_, j) => i != j));

                if (!IsReportSafe(amendedLevels))
                    continue;
                
                tally++;
                break;
            }
        }
        
        return tally;
    }
    
    private static bool IsReportSafe(List<int> levels)
    {
        _isLevelIncreasing = levels[0] > levels[1];
        foreach (var _ in levels)
        {
            for (var j = 1; j < levels.Count; j++)
            {
                if (levels[j - 1] > levels[j] != _isLevelIncreasing
                    || levels[j - 1] == levels[j]
                    || int.Abs(levels[j - 1] - levels[j]) > 3)
                    return false;
            }
        }

        return true;
    }
}