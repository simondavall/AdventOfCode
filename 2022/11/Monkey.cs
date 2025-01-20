using AocHelper;

namespace _11;

internal class Monkey
{
    public Queue<long> HoldingItems { get; private set; } = [];
    public long InspectionCount { get; set; }
    public Func<long, long> WorryOperation { get; private set; } = i => i;
    public int DivisibleBy { get; private set; }
    public int TruthDestination { get; private set; }
    public int FalseDestination { get; private set; }

    internal static Monkey Create(string block)
    {
        var monkey = new Monkey();
        var lines = block.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length != 6)
            throw new ArgumentException("block", $"Invalid format. Should have 6 lines, has {lines.Length}");

        // starting items
        var line1 = lines[1].Split(':');
        var startingItems = line1[1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToLongArray();
        monkey.HoldingItems = new Queue<long>(startingItems);

        // operation
        var line2 = lines[2].Split(": new = old ");
        var operation = line2[1].Split(' ');
        monkey.WorryOperation = GetOperation(operation[0], operation[1]);

        // test
        var line3 = lines[3].Split("divisible by ");
        monkey.DivisibleBy = line3[1].ToInt();

        // true
        var line4 = lines[4].Split("throw to monkey ");
        monkey.TruthDestination = line4[1].ToInt();

        // false
        var line5 = lines[5].Split("throw to monkey ");
        monkey.FalseDestination = line5[1].ToInt();

        return monkey;
    }

    private static Func<long, long> GetOperation(string op, string operand)
    {
        if (operand == "old")
        {
            return op switch
            {
                "*" => i => i * i,
                "/" => i => i / i,
                "+" => i => i + i,
                "-" => i => i - i,
                _ => throw new NotSupportedException($"Unsupported operator. {op}")
            };
        }

        return op switch
        {
            "*" => i => i * operand.ToInt(),
            "/" => i => i / operand.ToInt(),
            "+" => i => i + operand.ToInt(),
            "-" => i => i - operand.ToInt(),
            _ => throw new NotSupportedException($"Unsupported operator. {op}")
        };
    }
}



