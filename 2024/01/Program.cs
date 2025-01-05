// ReSharper disable ClassNeverInstantiated.Global
namespace _01;

internal class Program
{
    private static int Main()
    {
        var input = File.ReadAllText("input.txt").Split("\n", StringSplitOptions.RemoveEmptyEntries);

        List<int> firstList = [];
        List<int> secondList = [];
        
        foreach (var line in input)
        {
            var pair = line.Split("   ");
            firstList.Add(int.Parse(pair[0]));
            secondList.Add(int.Parse(pair[1]));
        }

        Console.WriteLine($"Part 1: {PartOne(firstList, secondList)}");

        Console.WriteLine($"Part 2: {PartTwo(firstList, secondList)}");
        return 0;
    }
    
    private static long PartOne(List<int> firstList, List<int> secondList)
    {
        firstList.Sort();
        secondList.Sort();

        long tally = 0;
        for (var i = 0; i < firstList.Count; i++)
        {
            tally += long.Abs(firstList[i] - secondList[i]);
        }
        
        return tally;
    }
    
    private static long PartTwo(List<int> firstList, List<int> secondList)
    {
        long tally = 0; 
        foreach (var locationId in firstList)
        {
            var count = secondList.Count(x => x == locationId);
            tally += locationId * count;
        }

        return tally;
    }
}