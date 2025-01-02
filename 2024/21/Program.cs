namespace _21;

internal static class Program
{
    private static readonly char[][] NumericKeypad =  [['7', '8', '9' ], ['4', '5', '6'], ['1', '2', '3'], ['\0', '0', 'A']];
    private static readonly char[][] DirectionalKeypad =  [['\0', '^', 'A'], ['<', 'v', '>']];

    private static Dictionary<(int, int), string[]> _numberSequences = null!;
    private static Dictionary<(int, int), string[]> _directionalSequences = null!;
    private static readonly Dictionary<(int, int), int> DirectionalSequenceLengths = new();
    private static readonly Dictionary<(string, int), long> CachedLengths = [];
    
    internal static void Main()
    {
        const string aocDay = "21";
        const string aocYear = "2024";
        const string path = $"/home/sdv/Documents/Projects/Aoc/{aocYear}/{aocDay}/";

        const string filename = "input.txt";
        var codes = File.ReadAllText($"{path}{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        _numberSequences = ComputeSequences(NumericKeypad);
        _directionalSequences = ComputeSequences(DirectionalKeypad);
        
        foreach (var seq in _directionalSequences)
        {
            DirectionalSequenceLengths[seq.Key] = seq.Value[0].Length;
        }
        
        Console.WriteLine($"Part 1: {PartOne(codes, 2)}");
        Console.WriteLine($"Part 2: {PartTwo(codes, 25)}");
    }

    private static long PartOne(string[] doorCodes, int depth)
    {
        long tally = 0;
        
        foreach (var doorCode in doorCodes)
        {
            var inputs = GetSequences(doorCode, _numberSequences);
            
            // Console.Write($"Key Path: ");
            // foreach (var seq in inputs)
            // {
            //     Console.Write($"{seq} [{seq.Length}], ");
            // }
            // Console.WriteLine();

            var length = inputs
                .Select(input => ComputeLength(input, depth))
                .Min();

            tally += length * doorCode.FromDigits();
        }
        
        return tally;
    }
    
    private static long PartTwo(string[] doorCodes, int depth)
    {
        long tally = 0;
        
        foreach (var doorCode in doorCodes)
        {
            var inputs = GetSequences(doorCode, _numberSequences);
            
            var length = inputs
                .Select(input => ComputeLength(input, depth))
                .Min();
            
            tally += length * doorCode.FromDigits();
        }
        
        return tally;
    }
    
    private static string[] GetSequences(string code, Dictionary<(int, int), string[]> sequences)
    {
        List<string[]> options = [];

        foreach (var (from, to) in ('A' + code).Zip(code))
        {
            options.Add(sequences[(from, to)]);
        }

        return CartesianProduct(options);
    }
    
    private static Dictionary<(int, int), string[]> ComputeSequences(char[][] keypad)
    {
        var position = new Dictionary<char, (int row, int col)>();
        for (var row = 0; row < keypad.Length; row++)
        {
            for (var col = 0; col < keypad[0].Length; col++)
            {
                if (keypad[row][col] != '\0')
                    position[keypad[row][col]] = (row, col);
            }
        }
        var sequences = new Dictionary<(int, int), string[]>();
        foreach (var from in position.Keys)
        {
            foreach (var to in position.Keys)
            {
                if (from == to)
                {
                    sequences[(from, to)] = ["A"];
                    continue;
                }

                var possibilities = new List<string>();
                var q = new Queue<((int row, int col), string)>();
                q.Enqueue((position[from], ""));
                var optimal = int.MaxValue;
                var foundOptimal = false;
                while (q.Count > 0 && !foundOptimal)
                {
                    var (item, moves) = q.Dequeue();
                    foreach (var (nextRow, nextCol, nextMove) in PossibleNextMoves(item.row, item.col))
                    {
                        if (nextRow < 0 || nextCol < 0 || nextRow >= keypad.Length || nextCol >= keypad[0].Length)
                            continue;
                        if (keypad[nextRow][nextCol] == '\0')
                            continue;
                        if (keypad[nextRow][nextCol] == to)
                        {
                            if (optimal < moves.Length)
                            {
                                foundOptimal = true;
                                break;
                            }
                            
                            optimal = moves.Length + 1;
                            possibilities.Add($"{moves}{nextMove}{'A'}");
                        }
                        else
                        {
                            q.Enqueue(((nextRow, nextCol), $"{moves}{nextMove}"));
                        }
                    }
                }
                sequences[(from, to)] = possibilities.ToArray();
            }
        }
        
        return sequences;
    }
    
    private static long ComputeLength(string sequence, int depth)
    {
        if (CachedLengths.ContainsKey((sequence, depth)))
            return CachedLengths[(sequence, depth)];
        
        long length = 0;
        
        if (depth == 1)
        {
            length = ('A' + sequence)
                .Zip(sequence)
                .Aggregate(length, (current, x) => (long)(current + DirectionalSequenceLengths[(x.First, x.Second)]));

            CachedLengths[(sequence, depth)] = length;
            
            return length;
        }
        
        foreach (var (from, to) in ('A' + sequence).Zip(sequence))
        {
            var min = _directionalSequences[(from, to)]
                .Select(subSequence => ComputeLength(subSequence, depth - 1))
                .Prepend(long.MaxValue)
                .Min();

            length += min;
        }
        
        CachedLengths[(sequence, depth)] = length;
        
        return length;
    }
    
    private static (int, int, char)[] PossibleNextMoves(int row, int col)
    {
        return [(row - 1, col, '^'), (row + 1, col, 'v'), (row, col - 1, '<'), (row, col + 1, '>')];
    }
    
    private static string[] CartesianProduct(List<string[]> input)
    {
        List<string> output = [string.Empty];

        while (input.Count > 0)
        {
            var currentArray = input[0];
            input.RemoveAt(0);
            var index = 0;
            var tempArray = new List<string>();
            foreach (var nextStr in currentArray)
            {
                var newArray = output.Select(str => str + nextStr).ToList();
                if (index++ == 0)
                {
                    tempArray = newArray;
                }
                else
                {
                    tempArray.AddRange(newArray);
                }
            }
            output = tempArray;
        }
        
        return output.ToArray();
    }
    
    private static int FromDigits(this string str)
    {
        var strDigits = str.Remove(str.IndexOf('A'));
        return int.Parse(strDigits);
    }
}