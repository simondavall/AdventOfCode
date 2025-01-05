using System.Diagnostics;
using System.Drawing;

namespace _16;

internal static class Program
{
    private static int _mapHeight;
    private static int _mapWidth;
    private static Point _startPoint;
    private static Point _finishPoint;
    private static readonly Dictionary<long, List<List<Vector>>> Paths = [];
    private static readonly List<List<Vector>> ValidPaths = [];
    private static readonly Dictionary<(Point, Direction), long> Visited = [];
    
    internal static void Main()
    {
        var map = File.ReadAllText("input.txt").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        _mapHeight = map.Length;
        _mapWidth = map[0].Length;
        
        var charMap = map.To2DCharArray();
        _startPoint = FindChar(charMap, 'S');
        _finishPoint = FindChar(charMap, 'E');
        
        Console.WriteLine($"Part 1: {PartOne(charMap)}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }

    private static long PartOne(char[,] charMap)
    {
        var tally = CalculateShortestRoute(charMap);

        // foreach (var path in _validPaths)
        // {
        //     PrintMap(charMap, path);
        // }
        
        return tally;
    }
    
    private static long PartTwo()
    {
        List<Point> bestPathPoints = [];
        foreach (var path in ValidPaths)
        {
            bestPathPoints.AddRange(path.Select(v => v.Point));
        }
        
        return bestPathPoints.Distinct().Count();
    }
    
    private static long CalculateShortestRoute(char[,] map)
    {
        long score = 0;
        long index = 0;
        
        var v = new Vector(_startPoint, Direction.East, 0);
        var startPointExits = GetValidExits(map, v.Point, v.Direction);
        
        foreach (var nextPoint in startPointExits)
            ProcessNextPoint([v], nextPoint.point, nextPoint.direction);
        
        while (Paths.Count > 0 && (score > 0 || index >= score))
        {
            if (!Paths.Remove(index++, out var activePaths)) 
                continue;
            
            foreach (var path in activePaths)
            {
                if (path.Last().Point == _finishPoint)
                {
                    ValidPaths.Add(path);
                    if (score == 0 || score > path.Last().Tally)
                        score = path.Last().Tally;
                }
                
                var validExits = GetValidExits(map, path.Last().Point, path.Last().Direction);
                    
                foreach (var nextPoint in validExits)
                    ProcessNextPoint(path, nextPoint.point, nextPoint.direction);
            }
        }
        
        return score;
    }
    
    private static void ProcessNextPoint(List<Vector> v, Point p, Direction d)
    {
        var newVector = new Vector(p, d, v.Last().Tally);
        
        if (d == v.Last().Direction) 
            newVector.Tally += 1;
        else
            newVector.Tally += 1001;

        if (Visited.TryGetValue((p, d), out var currentTally))
        {
            if (currentTally >= newVector.Tally)
                Visited[(p, d)] = newVector.Tally;
            else
                return;
        }
        else
        {
            Visited.Add((p, d), newVector.Tally);
        }

        var clone = new List<Vector>(v) { newVector };

        if(!Paths.TryAdd(newVector.Tally, [clone]))
            Paths[newVector.Tally].Add(clone);
    }
    
    private static (Point point, Direction direction)[] GetValidExits(char[,] map, Point p, Direction d)
    {
        List<(Point, Direction)> points = [];
        char[] validChars = ['.', 'E'];
        if (d != Direction.West && validChars.Contains(map[p.X + 1, p.Y]))
        {
            points.Add((p with { X = p.X + 1 }, Direction.East));
        }

        if (d != Direction.East && validChars.Contains(map[p.X - 1, p.Y]))
        {
            points.Add((p with { X = p.X - 1 }, Direction.West));
        }
        
        if (d != Direction.North && validChars.Contains(map[p.X, p.Y + 1]))
        {
            points.Add((p with { Y = p.Y + 1 }, Direction.South));
        }
        
        if (d != Direction.South && validChars.Contains(map[p.X, p.Y - 1]))
        {
            points.Add((p with { Y = p.Y - 1 }, Direction.North));
        }
        
        return points.ToArray();
    }

    private static char[,] To2DCharArray(this string[] input)
    {
        var charArray = new char[input[0].Length, input.Length];
        for(var y = 0; y < input.Length; y++)
            for (var x = 0; x < input[0].Length; x++)
                charArray[x, y] = input[y][x];

        return charArray;
    }

    private static Point FindChar(char[,] map, char c)
    {
        for (var y = 0; y < _mapHeight; y++)
            for (var x = 0; x < _mapWidth; x++)
                if (map[x, y] == c)
                    return new Point(x, y);

        return new Point(0, 0);
    }
    
    // ReSharper disable once UnusedMember.Local
    private static void PrintMap(char[,] map, List<Vector> path)
    {
        Console.WriteLine($"Tally {path.Last().Tally}");
        for (var y = 0; y < _mapHeight; y++)
        {
            for (var x = 0; x < _mapWidth; x++)
            {
                Console.Write(map[x, y] == '.' ? PrintPathChar(x, y, path) : map[x, y]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private static char PrintPathChar(int x, int y, List<Vector> path)
    {
        foreach (var vector in path.Where(vector => vector.Point.X == x && vector.Point.Y == y))
        {
            return GetPathChar(vector.Direction);
        }

        return '.';
    }

    private static char GetPathChar(Direction d)
    {
        return d switch
        {
            Direction.East => '>',
            Direction.North => '^',
            Direction.West => '<',
            Direction.South => 'v',
            _ => throw new UnreachableException()
        };
    }
}