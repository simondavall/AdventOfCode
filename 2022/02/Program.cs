namespace _02;

internal static class Program
{    
    private const long ExpectedPartOne = 13268;
    private const long ExpectedPartTwo = 15508;
    private const string Day = "02";
    private static readonly Dictionary<(char, char), int> Outcomes = new()
    {
        { ('A', 'X'), 4 },
        { ('A', 'Y'), 8 },
        { ('A', 'Z'), 3 },
        { ('B', 'X'), 1 },
        { ('B', 'Y'), 5 },
        { ('B', 'Z'), 9 },
        { ('C', 'X'), 7 },
        { ('C', 'Y'), 2 },
        { ('C', 'Z'), 6 },
    };
    
    private static readonly Dictionary<(char, char), int> RevisedOutcomes = new()
    {
        { ('A', 'X'), 3 },
        { ('A', 'Y'), 4 },
        { ('A', 'Z'), 8 },
        { ('B', 'X'), 1 },
        { ('B', 'Y'), 5 },
        { ('B', 'Z'), 9 },
        { ('C', 'X'), 2 },
        { ('C', 'Y'), 6 },
        { ('C', 'Z'), 7 },
    };
    
    public static int Main(string[] args)
    {
        var filename = "input02.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];
        
        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        var resultPartOne = PartOne(input);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(input);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");
        
        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(string [] gamePlays)
    {
        long tally = 0;
        foreach (var round in gamePlays)
        {
            tally += Outcomes[(round[0], round[2])];
        }
        return tally;
    }

    private static long PartTwo(string [] gamePlays)
    {
        long tally = 0;
        foreach (var round in gamePlays)
        {
            tally += RevisedOutcomes[(round[0], round[2])];
        }
        return tally;
    }
}