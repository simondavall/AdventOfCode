namespace _07;

internal static class Program
{
    private static int _mapHeight;
    private static readonly List<(long result, long[] operands)> FailedEquations = [];
    private static long _partOneTally;

    internal static void Main()
    {
        var map = File.ReadAllText("input.txt").Split("\n", StringSplitOptions.RemoveEmptyEntries);
        
        _mapHeight = map.Length;

        var results = new long[_mapHeight];
        List<long[]> operands = [];
        for (var i = 0; i < _mapHeight; i++)
        {
            var line = map[i];
            var item = line.Split(":");
            results[i] = long.Parse(item[0]);
            operands.Add(item[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToLongArray());
        }

        Console.WriteLine($"Part 1: {PartOne(results, operands)}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }

    private static long PartOne(long[] results, List<long[]> operands)
    {
        long tally = 0;
        for (var i = 0; i < _mapHeight; i++)
        {
            var accumulatedResults = new List<long> { results[i] };
            for (var j = operands[i].Length - 1; j >= 0; j--)
            {
                var currentOperand = operands[i][j];
                var calculatedResults = new List<long>();
                foreach (var accumulatedResult in accumulatedResults)
                {
                    if (accumulatedResult.IsDivisibleBy(currentOperand))
                    {
                        calculatedResults.Add(accumulatedResult / currentOperand);
                    }
                    calculatedResults.Add(accumulatedResult - currentOperand);
                }
                accumulatedResults = calculatedResults;
            }

            if (accumulatedResults.Contains(0))
            {
                tally += results[i];
            }
            else
            {
                FailedEquations.Add((results[i], operands[i]));
            }
        }

        _partOneTally = tally;
        return tally;
    }
    
    private static long PartTwo()
    {
        long tally = 0;
        for (var i = 0; i < FailedEquations.Count; i++)
        {
            var accumulatedResults = new List<long> {FailedEquations[i].operands[0]};

            for (var j = 1; j < FailedEquations[i].operands.Length; j++)
            {
                var currentOperand = FailedEquations[i].operands[j];
                var calculatedResults = new List<long>();
                foreach (var accumulatedResult in accumulatedResults)
                {
                    if (accumulatedResult + currentOperand <= FailedEquations[i].result)
                        calculatedResults.Add(accumulatedResult + currentOperand);
                    
                    if (accumulatedResult * currentOperand <= FailedEquations[i].result)
                        calculatedResults.Add(accumulatedResult * currentOperand);
                    
                    if (ConcatNumbers(accumulatedResult, currentOperand) <= FailedEquations[i].result)
                    {
                        calculatedResults.Add(ConcatNumbers(accumulatedResult, currentOperand));
                    }
                }
                accumulatedResults = calculatedResults;
            }

            if (accumulatedResults.Contains(FailedEquations[i].result))
                tally += FailedEquations[i].result;
        }
        return tally + _partOneTally;
    }

    private static long ConcatNumbers(long first, long second)
    {
        var temp = second;
        while (temp / 10 > 0)
        {
            temp /= 10;
            first *= 10;
        }
        
        return (first * 10) + second;
    }
    
    private static bool IsDivisibleBy(this long result, long operand)
    {
        return result % operand == 0;
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