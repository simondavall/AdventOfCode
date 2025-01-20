namespace _11;

internal static class Program
{
    private const long ExpectedPartOne = 66124;
    private const long ExpectedPartTwo = 19309892877;
    private const string Day = "_11";

    public static int Main(string[] args)
    {
        var filename = "input_11.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var blocks = File.ReadAllText($"{filename}").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        
        var resultPartOne = PartOne(blocks);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(blocks);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(string[] blocks)
    {
        var monkeys = GetMonkeys(blocks);
        var count = 0;
        while (count++ < 20)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.HoldingItems.Count > 0)
                {
                    var item = monkey.HoldingItems.Dequeue();
                    monkey.InspectionCount++;
                    
                    item = monkey.WorryOperation(item);
                    item /= 3;
                    
                    if (item % monkey.DivisibleBy == 0)
                        monkeys[monkey.TruthDestination].HoldingItems.Enqueue(item);
                    else
                        monkeys[monkey.FalseDestination].HoldingItems.Enqueue(item);
                }
            }
        }

        var mostActiveMonkeys = monkeys.OrderByDescending(x => x.InspectionCount).Take(2).ToList();
        return mostActiveMonkeys[0].InspectionCount * mostActiveMonkeys[1].InspectionCount;
    }

    private static long PartTwo(string[] blocks)
    {
        var monkeys = GetMonkeys(blocks);

        var reliefValue = 1;
        foreach (var monkey in monkeys)
            reliefValue *= monkey.DivisibleBy;
        
        var count = 0;
        while (count++ < 10000)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.HoldingItems.Count > 0)
                {
                    var item = monkey.HoldingItems.Dequeue();
                    monkey.InspectionCount++;
                    
                    item = monkey.WorryOperation(item);
                    item %= reliefValue;
                    
                    if (item % monkey.DivisibleBy == 0)
                        monkeys[monkey.TruthDestination].HoldingItems.Enqueue(item);
                    else
                        monkeys[monkey.FalseDestination].HoldingItems.Enqueue(item);
                }
            }
        }

        var mostActiveMonkeys = monkeys.OrderByDescending(x => x.InspectionCount).Take(2).ToList();
        return mostActiveMonkeys[0].InspectionCount * mostActiveMonkeys[1].InspectionCount;
    }
    
    private static List<Monkey> GetMonkeys(string[] blocks)
    {
        List<Monkey> monkeys = [];
        
        foreach (var block in blocks)
            monkeys.Add(Monkey.Create(block));

        return monkeys;
    }
}