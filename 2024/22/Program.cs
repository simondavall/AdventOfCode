namespace _22;

internal static class Program
{
    private static readonly Dictionary<(int, int, int, int), long> Sequences = [];
    private static Dictionary<(int, int, int, int), int> _contributed = [];
    
    internal static void Main()
    {
        const string filename = "input.txt";
        var secretNumbers = File.ReadAllText($"{filename}")
            .Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .ToLongArray();
        
        Console.WriteLine($"Part 1: {PartOne(secretNumbers)}");
        
        secretNumbers = File.ReadAllText($"{filename}")
            .Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .ToLongArray();
        Console.WriteLine($"Part 2: {PartTwo(secretNumbers)}");
    }

    private static long PartOne(long[] initialNumbers)
    {
        long tally = 0;
        
        for (var index = 0; index < initialNumbers.Length; index++)
        {
            var counter = 0;
            while (counter++ < 2000)
            {
                initialNumbers[index] = Next(initialNumbers[index]);
            }

            tally += initialNumbers[index];
            //Console.WriteLine(secretNumbers[index]);
        }
        
        return tally;
    }

    private static long PartTwo(long[] initialNumbers)
    {
        long tally = 0;
        var secretNumbers = new List<SecretNumber>();
        
        foreach (var n in initialNumbers)
        {
            _contributed = [];
            secretNumbers.Clear();
            var initial = new SecretNumber
            {
                Number = n,
                Price = (int)n % 10
            };
            secretNumbers.Add(initial);
            
            var counter = 0;
            while (counter++ < 2000)
            {
                secretNumbers = NextSecret(secretNumbers);
            }
        }

        tally = Sequences.Values.Max();

        // foreach (var (key, _) in Sequences.Where(x => x.Value == tally))
        // {
        //     Console.WriteLine(key);
        // }
        
        return tally;
    }

    private static long Next(long current)
    {
        current = Prune(Mix(current * 64, current));
        current = Prune(Mix(current / 32, current));
        current = Prune(Mix(current * 2048, current));
        return current;
    }
    
    private static List<SecretNumber> NextSecret(List<SecretNumber> secrets)
    {
        if (secrets.Count > 3)
        {
            secrets.RemoveAt(0);
        }
        
        var next = new SecretNumber(Next(secrets.Last().Number));
        next.Price = (int)(next.Number % 10);
        next.Diff =  next.Price - secrets.Last().Price;
        secrets.Add(next);
        
        if (secrets.Count > 3)
        {
            if (_contributed.TryAdd((secrets[0].Diff, secrets[1].Diff, secrets[2].Diff, secrets[3].Diff), next.Price))
            {
                if (!Sequences.TryAdd((secrets[0].Diff, secrets[1].Diff, secrets[2].Diff, secrets[3].Diff), next.Price))
                    Sequences[(secrets[0].Diff, secrets[1].Diff, secrets[2].Diff, secrets[3].Diff)] += next.Price;
            }
        }
        
        return secrets;
    }
    
    private static long Mix(long current, long orig)
    {
        return current ^ orig;
    }

    private static long Prune(long current)
    {
        return current % 16777216;
    }
    
    private struct SecretNumber(long number)
    {
        public long Number { get; init; } = number;
        public int Price { get; set; }
        public int Diff { get; set; }
    }

    private static long[] ToLongArray(this string[] array)
    {
        var longArray = new long[array.Length];
        for (var i = 0; i < array.Length; i++)
        {
            longArray[i] = long.Parse(array[i]);
        }
    
        return longArray;
    }
}