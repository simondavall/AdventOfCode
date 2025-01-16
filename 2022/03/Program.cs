namespace _03;

internal static class Program
{
    private const long ExpectedPartOne = 7817;
    private const long ExpectedPartTwo = 2444;
    private const string Day = "_03";

    public static int Main(string[] args)
    {
        var filename = "input03.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        var resultPartOne = PartOne(input);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(input);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(string[] input)
    {
        long tally = 0;
        foreach (var rucksack in input)
        {
            var midPoint = rucksack.Length / 2;
            foreach (var ch in rucksack)
            {
                var first = rucksack.IndexOf(ch);
                var second = rucksack.LastIndexOf(ch);
                if (first < second && first < midPoint && second >= midPoint)
                {
                    tally += GetValue(ch);
                    break;
                }
            }
        }
        return tally;
    }

    private static long PartTwo(string[] input)
    {
        long tally = 0;
        for (var i = 0; i < input.Length; i += 3)
        {
            var rucksack = input[i];
            foreach (var ch in rucksack)
            {
                if (input[i + 1].IndexOf(ch) > -1 && input[i + 2].IndexOf(ch) > -1)
                {
                    tally += GetValue(ch);;
                    break;
                }
            }
        }
        
        return tally;
    }
    
    private static int GetValue(char ch)
    {
        return ch switch
        {
            >= 'a' and <= 'z' => ch - 'a' + 1,
            >= 'A' and <= 'Z' => ch - 'A' + 27,
            _ => int.MinValue
        };
    }
    
}