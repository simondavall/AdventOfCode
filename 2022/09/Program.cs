using AocHelper;

namespace _09;

internal static class Program
{
    private const long ExpectedPartOne = 6175;
    private const long ExpectedPartTwo = 2578;
    private const string Day = "_09";

    private static readonly Dictionary<char, (int dr, int dc)> Directions = new() {{'U', (-1, 0)}, {'R', (0,1)}, {'D', (1,0)}, {'L', (0,-1)}};
    
    public static int Main(string[] args)
    {
        var filename = "input_09.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        var resultPartOne = PartOne(input);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(input);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(string[] instructions)
    {
        (int r, int c) head = (0, 0);
        (int r, int c) tail = (0, 0);
        HashSet<(int, int)> visited = [(0, 0)];
        
        foreach (var line in instructions)
        {
            var (dir, dist) = line.Split(' ').ToTuplePair();
            var (dr, dc) = Directions[dir[0]];

            var steps = 0;
            while (steps++ < dist.ToInt())
            {
                head.r += dr;
                head.c += dc;

                var dtr = head.r - tail.r;
                var dtc = head.c - tail.c;

                (tail.r, tail.c) = CheckTailPosition(dtr, dtc, tail.r, tail.c);
                (tail.c, tail.r) = CheckTailPosition(dtc, dtr, tail.c, tail.r);
                
                visited.Add(tail);
            }
        }
        return visited.Count;
    }

    private static long PartTwo(string[] instructions)
    {
        var knots = new (int r, int c)[10];
        
        HashSet<(int, int)> visited = [(0, 0)];
        
        foreach (var line in instructions)
        {
            var (dir, dist) = line.Split(' ').ToTuplePair();
            var (dr, dc) = Directions[dir[0]];

            var steps = 0;
            while (steps++ < dist.ToInt())
            {
                knots[0].r += dr;
                knots[0].c += dc;
                
                for (var i = 0; i < knots.Length - 1; i++)
                {
                    var current = knots[i];
                    var next = knots[i + 1];
                    
                    var rowDiff = current.r - next.r;
                    var colDiff = current.c - next.c;
                    (next.r, next.c) = CheckTailPosition(rowDiff, colDiff, next.r, next.c);

                    rowDiff = current.r - next.r;
                    colDiff = current.c - next.c;
                    (next.c, next.r) = CheckTailPosition(colDiff, rowDiff, next.c, next.r);
                    
                    knots[i + 1] = next;
                }
                
                visited.Add(knots[9]);
            }
        }
        return visited.Count;
    }

    private static (int a, int b) CheckTailPosition(int a, int b, int tailA, int tailB)
    {
        switch (a)
        {
            case > 1:
            {
                tailA++;
                if (b > 0)
                    tailB++;
                else if (b < 0)
                    tailB--;
                break;
            }
            case < -1:
            {
                tailA--;
                if (b > 0)
                    tailB++;
                else if (b < 0)
                    tailB--;
                break;
            }
        }

        return (tailA, tailB);
    }
}