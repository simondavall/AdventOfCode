namespace _19;

internal static partial class Program
{
    private static readonly Dictionary<string, (List<Condition> conditions, string fallback)> Workflows = [];

    internal static void Main()
    {
        const string filename = "input.txt";
        var input = File.ReadAllText($"{filename}").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        List<Dictionary<char, int>> parts = [];

        foreach (var line in input[0].Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var item = line.Split('{');
            var name = item[0];
            var rules = new FlexStack<string>(item[1][..^1].Split(','));
            Workflows[name] = ([], rules.PopBottom());
            while (!rules.IsEmpty())
            {
                var rule = rules.Pop();
                var elements = rule.Split(':', StringSplitOptions.RemoveEmptyEntries);
                var condition = Condition.Create(elements[0], elements[1]);
                Workflows[name].conditions.Add(condition);
            }
        }
        
        foreach (var part in input[1].Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var items = new Dictionary<char, int>();
            foreach (var elements in part[1..^1].Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var item = elements.Split('=', StringSplitOptions.RemoveEmptyEntries);
                items[item[0][0]] = item[1].ToInt();
            }
            parts.Add(items);
        }

        Console.WriteLine($"Part 1: {PartOne(parts)}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }

    private static long PartOne(List<Dictionary<char, int>> parts)
    {
        long tally = 0;

        foreach (var part in parts.Where(part => IsAccepted("in", part)))
            tally += part.Values.Sum();

        return tally;
    }

    private static long PartTwo()
    {
        var ranges = new Dictionary<char, (int low, int hi)>();
        foreach (var ch in "xmas")
        {
           ranges.Add(ch, (1, 4000)); 
        }

        return CountCombinations("in", ranges);
    }
    
    
    private static long CountCombinations(string workflow, Dictionary<char, (int low, int hi)> ranges)
    {
        if (workflow == "R")
            return 0;

        if (workflow == "A")
        {
            long product = 1;
            foreach (var (low, hi) in ranges.Values)
            {
                product *= hi - low + 1;
            }
            return product;
        }

        var (conditions, fallback) = Workflows[workflow];
        
        long tally = 0;

        foreach (var condition in conditions)
        {
            var (low, hi) = ranges[condition.Key];
            
            (int low, int hi) accepted;
            (int low, int hi) rejected;
            if (condition.Op == '<')
            {
                accepted = (low, Math.Min(condition.Value - 1, hi));
                rejected = (Math.Max(condition.Value, low), hi);
            }
            else
            {
                accepted = (Math.Max(condition.Value + 1, low), hi);
                rejected = (low, Math.Min(condition.Value, hi));
            }

            if (accepted.low <= accepted.hi)
            {
                var newRanges = new Dictionary<char, (int low, int hi)>(ranges)
                {
                    [condition.Key] = accepted
                };
                tally += CountCombinations(condition.Target, newRanges);
            }

            if (rejected.low <= rejected.hi)
            {
                ranges = new Dictionary<char, (int low, int hi)>(ranges)
                {
                    [condition.Key] = rejected
                };
            }
            else
                break;
        }
        
        tally += CountCombinations(fallback, ranges);
        
        return tally;
    }
    
    private static bool IsAccepted(string workflow, Dictionary<char, int> parts)
    {
        if (workflow == "A")
            return true;

        if (workflow == "R")
            return false;

        var (conditions, fallback) = Workflows[workflow];
        
        foreach (var condition in conditions.Where(condition => Rule(condition, parts)))
        {
            return IsAccepted(condition.Target, parts);
        }

        return IsAccepted(fallback, parts);
    }
    
    private static bool Rule(Condition rule, Dictionary<char, int> part)
    {
        var operand = part[rule.Key];
        
        return rule.Op == '<' ? operand < rule.Value : operand > rule.Value;
    }
    
    private struct Condition
    {
        public char Key { get; private init; }
        public char Op { get; private init; }
        public int Value { get; private init; }
        public string Target { get; private init; }

        public static Condition Create(string condition, string target)
        {
            return new Condition
            {
                Key = condition[0],
                Op = condition[1],
                Value = condition[2..].ToInt(),
                Target = target
            };
        }
    }
}