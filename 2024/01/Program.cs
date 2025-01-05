namespace _01;

internal static class Program
{
    private static void Main()
    {
        var input = File.ReadAllText("input.txt").Split("\n", StringSplitOptions.RemoveEmptyEntries);

        List<int> firstList = [];
        List<int> secondList = [];
        
        foreach (var line in input)
        {
            var pair = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            firstList.Add(int.Parse(pair[0]));
            secondList.Add(int.Parse(pair[1]));
        }

        Console.WriteLine($"Part 1: {PartOne(firstList, secondList)}");
        Console.WriteLine($"Part 2: {PartTwo(firstList, secondList)}");
    }
    
    private static long PartOne(List<int> firstList, List<int> secondList)
    {
        firstList.Sort();
        secondList.Sort();

        return firstList.Select((t, i) => long.Abs(t - secondList[i])).Sum();
    }
    
    private static long PartTwo(List<int> firstList, List<int> secondList)
    {
        long tally = 0; 
        foreach (var locationId in firstList)
        {
            tally += locationId * secondList.Count(x => x == locationId);
        }

        return tally;
    }
}