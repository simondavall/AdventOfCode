namespace _02;

internal static partial class Program
{
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
    
    internal static void Main()
    {
        const string filename = "input.txt";
        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        Console.WriteLine($"Part 1: {PartOne(input)}");
        Console.WriteLine($"Part 2: {PartTwo(input)}");
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