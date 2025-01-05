using System.Drawing;

namespace _10;

internal static class Program
{
    private static int _mapHeight;
    private static int _mapWidth;
    private static int[,] _intMap = null!;
    private static readonly Dictionary<Point, List<Point>> CachedTrailHeads = [];
    private static readonly Dictionary<Point, int> CachedTrails = [];
    
    internal static void Main()
    {
        var map = File.ReadAllText("input.txt").Split("\n", StringSplitOptions.RemoveEmptyEntries);
        
        CreateIntMap(map);
        
        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }

    private static long PartOne()
    {
        long tally = 0;

        var startingPoints = FindStartingPoints();

        foreach (var startingPoint in startingPoints)
        {
            var headsReached = FindTrailHeads(startingPoint, 0);
            tally += headsReached.Length;
        }
        
        return tally;
    }

    private static long PartTwo()
    {
        long tally = 0;

        var startingPoints = FindStartingPoints();

        foreach (var startingPoint in startingPoints)
        {
            var trails = FindDistinctTrails(startingPoint, 0);
            tally += trails;
        }
        
        return tally;
    }

    private static int FindDistinctTrails(Point location, int number)
    {
        var count = 0;

        if (number == 9)
        {
            return 1;
        }
        
        if (CachedTrails.TryGetValue(location, out var score))
        {
            return score;
        }
        
        var neighbours = GetNeighbours(location, number);

        if (neighbours.Length > 0)
        {
            count += neighbours.Sum(neighbour => FindDistinctTrails(neighbour, number + 1));
        }

        CachedTrails.Add(location, count);

        return count;
    }
    
    private static Point[] FindTrailHeads(Point location, int number)
    {
        List<Point> heads = [];

        if (number == 9)
        {
            return [location];
        }
        
        if (CachedTrailHeads.TryGetValue(location, out var cachedHeads))
        {
            return cachedHeads.ToArray();
        }
        
        var neighbours = GetNeighbours(location, number);

        if (neighbours.Length > 0)
        {
            foreach (var neighbour in neighbours)
            {
                var reachableHeads = FindTrailHeads(neighbour, number + 1);
                foreach (var head in reachableHeads)
                {
                    if (!heads.Contains(head))
                        heads.Add(head);
                }
            }
        }

        CachedTrailHeads.Add(location, heads);

        return heads.ToArray();
    }
    
    private static Point[] FindStartingPoints()
    {
        List<Point> points = [];
        for (var row = 0; row < _mapHeight; row++)
        {
            for (var col = 0; col < _mapWidth; col++)
            {
                if (_intMap[row, col] == 0)
                {
                    points.Add(new Point(col, row));
                }
            }
        }

        return points.ToArray();
    }
    
    private static Point[] GetNeighbours(Point p, int number)
    {
        List<Point> points = [];
        
        if (p.X > 0 && _intMap[p.Y, p.X - 1] == number + 1)
            points.Add(p with { X = p.X - 1 });
        if (p.X < _mapWidth - 1 && _intMap[p.Y, p.X + 1] == number + 1)
            points.Add(p with { X = p.X + 1 });
        if (p.Y > 0 && _intMap[p.Y - 1, p.X] == number + 1)
            points.Add(p with { Y = p.Y - 1 });
        if (p.Y < _mapHeight - 1 && _intMap[p.Y + 1, p.X] == number + 1)
            points.Add(p with { Y = p.Y + 1 });
        
        return points.ToArray();
    }
    
    private static void CreateIntMap(string[] map)
    {
        _mapHeight = map.Length;
        _mapWidth = map[0].Length;
        _intMap = new int[_mapHeight, _mapWidth];
        for (var row = 0; row < _mapHeight; row++)
        {
            for (var col = 0; col < _mapWidth; col++)
            {
                _intMap[row, col] = map[row][col] - '0';
            }
        }
    }
}