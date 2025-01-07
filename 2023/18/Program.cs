using System.Globalization;

namespace _18;

internal static partial class Program
{
    internal static void Main()
    {
        const string filename = "input.txt";
        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        Console.WriteLine($"Part 1: {PartOne(input)}");
        Console.WriteLine($"Part 2: {PartTwo(input)}");
    }
    
    private static long PartOne(string[] input)
    {
        var instructions = new (char direction, long length, string colour)[input.Length];
        for (var i = 0; i < input.Length; i++)
        {
            var line = input[i];
            var items = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            instructions[i] = (items[0][0], items[1].ToLong(), items[2]);
        }

        var (area, boundaryPoints) = CalcArea(instructions);
        
        // Using Pick's Theorem: Area = interior points + (boundary points/2) - 1;
        // 
        // or i = Area - (b/2) + 1
        
        var interiorPoints = area - (boundaryPoints / 2) + 1;
            
        return interiorPoints + boundaryPoints;
    }
    
    private static long PartTwo(string[] input)
    {
        List<char> directions = ['R', 'D', 'L', 'U'];
        var instructions = new (char direction, long length, string colour)[input.Length];
        for (var i = 0; i < input.Length; i++)
        {
            var line = input[i];
            var items = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            instructions[i].direction = directions[items[2][7] - '0'];
            instructions[i].length = int.Parse(items[2][2..7], NumberStyles.HexNumber);
        }
        
        var (area, boundaryPoints) = CalcArea(instructions);
        var interiorPoints = area - (boundaryPoints / 2) + 1;
            
        return interiorPoints + boundaryPoints;
    }

    private static (long area, long boundarypoints) CalcArea((char direction, long length, string colour)[] instructions)
    {
        Dictionary<char, (int dr, int dc)> direction = new()
        {
            { 'U', (-1, 0) }, { 'D', (1, 0) }, { 'R', (0, 1) }, { 'L', (0, -1) }
        };

        List<(long row, long col)> points = [(0, 0)];
        long boundaryPoints = 0;
        foreach (var (dir, length, _) in instructions)
        {
            var (dr, dc) = direction[dir];
            boundaryPoints += length;
            var (r, c) = points.Last();
            points.Add((r + dr * length, c + dc * length));
        }

        // Using the Gauss' (Shoelace) Area Formula: 
        long sum = 0;
        foreach (var current in Range(points.Count))
        {
            var prev = (points.Count + current - 1) % points.Count;
            var next = (current + 1) % points.Count;
            sum += points[current].row * (points[prev].col - points[next].col);
        }
        
        return (long.Abs(sum) / 2, boundaryPoints);
    }
}