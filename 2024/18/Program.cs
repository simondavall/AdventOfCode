using System.Drawing;

namespace _18;

internal static class Program
{
    private static int _mapDimension;
    private static int _corruptBytes;
    private static char[,] _map = null!;
    private static string _filename = null!;
    private static readonly Dictionary<Point, int> Visited = [];
    private static readonly List<List<Point>> Routes = [];

    internal static void Main()
    {
        SetConfig("input.txt");
        var input = File.ReadAllText($"{_filename}")
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .ToPointArray();
        
        Console.WriteLine($"Part 1: {PartOne(input)}");
        Console.WriteLine($"Part 2: {PartTwo(input)}");
    }

    private static long PartOne(Point[] bytes)
    {
        InitializeMap();
        CorruptMemory(_corruptBytes, bytes);
        PrintMap();

        var startPoint = new Point(0, 0);
        var shortestPath = FindShortestPath(startPoint);

        MapPath(shortestPath);
        PrintMap();

        return shortestPath.Length - 1;
    }

    private static Point PartTwo(Point[] bytes)
    {
        InitializeMap();
        CorruptMemory(_corruptBytes, bytes);

        var startPoint = new Point(0, 0);
        var shortestPath = FindShortestPath(startPoint);

        foreach (var corruptedByte in bytes)
        {
            _map[corruptedByte.X, corruptedByte.Y] = '#';
            
            if (shortestPath.Contains(corruptedByte))
                shortestPath = FindShortestPath(startPoint);

            if (shortestPath.Length == 0)
                return corruptedByte;
        }

        return new Point(-1,-1);
    }

    private static Point[] FindShortestPath(Point startPoint)
    {
        Point[] shortestPath = [];
        Visited.Clear();
        Routes.Clear();
        
        var finishPoint = new Point(_mapDimension - 1, _mapDimension - 1);

        Visited.Add(startPoint, 0);
        Routes.Add([startPoint]);
        
        while (Routes.Count > 0)
        {
            var route = Routes[0];
            Routes.RemoveAt(0);
            
            var validNeighbours = FindNextValidNeighbours(route[0], route.Count);
            foreach (var neighbour in validNeighbours)
            {
                List<Point> newRoute = [neighbour];
                newRoute.AddRange(route);
                Routes.Add(newRoute);
                
                if (neighbour == finishPoint)
                    shortestPath = newRoute.ToArray();
            }
        }
        
        return shortestPath;
    }

    private static Point[] FindNextValidNeighbours(Point p, int pathLength)
    {
        List<Point> validNeighbours = [];

        var pUp = p with { Y = p.Y - 1 };
        if (pUp.IsValid(pathLength))
            validNeighbours.Add(pUp);
        
        var pRight = p with { X = p.X + 1 };
        if (pRight.IsValid(pathLength))
            validNeighbours.Add(pRight);
        
        var pDown = p with { Y = p.Y + 1 };
        if (pDown.IsValid(pathLength))
            validNeighbours.Add(pDown);
        
        var pLeft = p with { X = p.X - 1 };
        if (pLeft.IsValid(pathLength))
            validNeighbours.Add(pLeft);
        
        return validNeighbours.ToArray();
    }
    
    private static bool IsValid(this Point p, int n)
    {
        if (!p.IsInBounds() || _map[p.X, p.Y] == '#')
            return false;
        
        if (Visited.TryGetValue(p, out var value))
        {
            if (value <= n)
                return false;

            // remove existing paths with point p in
            for (var index = Routes.Count - 1; index >= 0; index--)
            {
                var route = Routes[index];
                if (route.Contains(p))
                    Routes.RemoveAt(index--);
            }

            Visited[p] = n;
        }
        else
        {
            Visited.Add(p, n);
        }
        
        return true;
    }

    private static bool IsInBounds(this Point p)
    {
        return p.X >= 0 && p.X < _mapDimension
                        && p.Y >= 0 && p.Y < _mapDimension;
    }

    private static void CorruptMemory(int numberToCorrupt, Point[] bytes)
    {
        for (var i = 0; i < numberToCorrupt; i++)
            _map[bytes[i].X, bytes[i].Y] = '#';
    }
    
    private static void MapPath(Point[] path)
    {
        foreach (var p in path)
            _map[p.X, p.Y] = 'O';
    }

    private static void SetConfig(string filename)
    {
        if (filename == "sample.txt")
        {
            _filename = filename;
            _mapDimension = 7;
            _corruptBytes = 12;
            return;
        }

        _filename = "input.txt";
        _mapDimension = 71;
        _corruptBytes = 1024;
    }

    private static void InitializeMap()
    {
        _map = new char[_mapDimension, _mapDimension];

        for (var y = 0; y < _mapDimension; y++)
            for (var x = 0; x < _mapDimension; x++)
                _map[x, y] = '.';
    }

    private static void PrintMap()
    {
        for (var y = 0; y < _mapDimension; y++)
        {
            for (var x = 0; x < _mapDimension; x++)
            {
                Console.Write(_map[x, y]);
            }

            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private static Point[] ToPointArray(this string[] array)
    {
        var points = new Point[array.Length];
        for (var i = 0; i < points.Length; i++)
        {
            var coords = array[i].Split(',');
            points[i] = new Point(int.Parse(coords[0]), int.Parse(coords[1]));
        }

        return points;
    }
}