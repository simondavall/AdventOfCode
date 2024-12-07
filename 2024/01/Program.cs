// ReSharper disable ClassNeverInstantiated.Global
namespace _01;

internal class Program
{
    private static int Main()
    {
        const string aocDay = "01";
        const string filename = "input.txt";
        const string path = $"/home/sdv/Documents/Projects/Aoc/2024/{aocDay}/";
        var lines = File.ReadLines($"{path}{filename}").ToArray();

        List<int> firstList = [];
        List<int> secondList = [];
        
        foreach (var line in lines)
        {
            var pair = line.Split("   ");
            firstList.Add(int.Parse(pair[0]));
            secondList.Add(int.Parse(pair[1]));
        }

        Console.WriteLine($"Part 1: {PartOne(firstList, secondList)}");

        Console.WriteLine($"Part 1: {PartTwo(firstList, secondList)}");
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