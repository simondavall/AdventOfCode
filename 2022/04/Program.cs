using AocHelper;

namespace _04;

internal static class Program
{
    private const long ExpectedPartOne = 534;
    private const long ExpectedPartTwo = 841;
    private const string Day = "_04";

    public static int Main(string[] args)
    {
        var filename = "input_04.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);

        List<(int, int, int, int)> elfPairs = [];
        foreach (var line in input)
        {
            var (left, right) = line.Split(',').ToTuplePair();
            var (s1, e1) = left.Split('-').ToIntArray().ToTuplePair();
            var (s2, e2) = right.Split('-').ToIntArray().ToTuplePair();
            elfPairs.Add((s1, e1, s2, e2));
        }

        var resultPartOne = PartOne(elfPairs);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(elfPairs);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(List<(int, int, int, int)> elfPairs)
    {
        long tally = 0;

        foreach (var (s1, e1, s2, e2) in elfPairs)
        {
            if ((s1 >= s2 && e1 <= e2) || (s2 >= s1 && e2 <= e1))
                tally++;
        }

        return tally;
    }

    private static long PartTwo(List<(int, int, int, int)> elfPairs)
    {
        long tally = 0;
        foreach (var (s1, e1, s2, e2) in elfPairs)
        {
            if (!(e1 < s2 || e2 < s1))
                tally++;
        }

        return tally;
    }
}