using System.Drawing;

namespace _08;

internal static class Program
{
    private static int _mapHeight;
    private static int _mapWidth;

    internal static void Main()
    {
        var map = File.ReadAllText("input.txt").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        _mapHeight = map.Length;
        _mapWidth = map[0].Length;

        Dictionary<char, List<Point>> antennas = [];
            
        for (var y = 0; y < _mapHeight; y++)
        {
            var line = map[y].ToCharArray();
            for (var x = 0; x < _mapWidth; x++)
            {
                var c = line[x];
                if (c =='.')
                    continue;
                if (antennas.TryGetValue(c, out var antenna))
                {
                    antenna.Add(new Point(x, y));
                    continue;
                }
                antennas.Add(c, [new Point(x, y)]);
            }
        }
        
        Console.WriteLine($"Part 1: {PartOne(antennas)}");
        Console.WriteLine($"Part 2: {PartTwo(antennas)}");
    }

    private static long PartOne(Dictionary<char, List<Point>> antennas)
    {
        var antiNodeLocations = new List<Point>();
        foreach (var antenna in antennas)
        {
            for (var i = 0; i < antenna.Value.Count - 1; i++)
            {
                for (var j = i + 1; j < antenna.Value.Count; j++)
                {
                    var firstPoint = antenna.Value[i];
                    var secondPoint = antenna.Value[j];

                    var antiNodeDx = firstPoint.X - secondPoint.X;
                    var antiNodeDy = firstPoint.Y - secondPoint.Y;
                    
                    AddAntiNodeLocation(new Point(firstPoint.X + antiNodeDx, firstPoint.Y + antiNodeDy), antiNodeLocations);
                    AddAntiNodeLocation(new Point(secondPoint.X - antiNodeDx, secondPoint.Y - antiNodeDy), antiNodeLocations);
                }
            }
        }

        return antiNodeLocations.Count;
    }

    private static long PartTwo(Dictionary<char, List<Point>> antennas)
    {
        var antiNodeLocations = new List<Point>();
        foreach (var antenna in antennas)
        {
            for (var i = 0; i < antenna.Value.Count - 1; i++)
            {
                for (var j = i + 1; j < antenna.Value.Count; j++)
                {
                    var firstPoint = antenna.Value[i];
                    var secondPoint = antenna.Value[j];

                    if (!antiNodeLocations.Contains(firstPoint))
                        antiNodeLocations.Add(firstPoint);
                    if (!antiNodeLocations.Contains(secondPoint))
                        antiNodeLocations.Add(secondPoint);
                    
                    var antiNodeDx = firstPoint.X - secondPoint.X;
                    var antiNodeDy = firstPoint.Y - secondPoint.Y;

                    var newPoint1 = new Point(firstPoint.X + antiNodeDx, firstPoint.Y + antiNodeDy);
                    while (IsInBounds(newPoint1))
                    {
                        if (!antiNodeLocations.Contains(newPoint1))
                            antiNodeLocations.Add(newPoint1);

                        newPoint1.X += antiNodeDx;
                        newPoint1.Y += antiNodeDy;
                    }
                    
                    var newPoint2 = new Point(secondPoint.X - antiNodeDx, secondPoint.Y - antiNodeDy);
                    while (IsInBounds(newPoint2))
                    {
                        if (!antiNodeLocations.Contains(newPoint2))
                            antiNodeLocations.Add(newPoint2);

                        newPoint2.X -= antiNodeDx;
                        newPoint2.Y -= antiNodeDy;
                    }
                }
            }
        }

        return antiNodeLocations.Count;
    }

    private static void AddAntiNodeLocation(Point p, List<Point> list)
    {
        if (IsInBounds(p) && !list.Contains(p))
            list.Add(p);
    }
    
    private static bool IsInBounds(Point p)
    {
        return p.X >= 0 && p.X < _mapWidth && p.Y >= 0 && p.Y < _mapHeight;
    }
}