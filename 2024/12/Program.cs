using System.Drawing;

namespace _12;

internal static class Program
{
    private static int _mapHeight;
    private static int _mapWidth;
    private static string[] _map = [];
    private static List<Point> _visited = [];
    
    internal static void Main()
    {
        _map = File.ReadAllText("input.txt").Split("\n", StringSplitOptions.RemoveEmptyEntries);
        
        _mapHeight = _map.Length;
        _mapWidth = _map[0].Length;
        
        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }

    private static long PartOne()
    {
        long tally = 0;
        
        for (var y = 0; y < _mapWidth; y++)
        {
            for (var x = 0; x < _mapHeight; x++)
            {
                var point = new Point(x, y);
                if (!_visited.Contains(point))
                {
                    var c = _map[y][x];
                    var result = CalcPerimeter(c, point);
                    tally += result.area * result.perimeter;
                }
            }
        }

        return tally;
    }
    
    private static long PartTwo()
    {
        long tally = 0;
        _visited = [];
        for (var y = 0; y < _mapWidth; y++)
        {
            for (var x = 0; x < _mapHeight; x++)
            {
                var point = new Point(x, y);
                if (!_visited.Contains(point))
                {
                    var c = _map[y][x];
                    var result = CalcBulkPerimeter(c, point);
                    tally += result.area * (result.corners);
                }
            }
        }

        return tally;
    }

    private static (int area, int corners) CalcBulkPerimeter(char plant, Point p)
    {
        var area = 1;
        var corners = 0;
        
        if (_visited.Contains(p))
            return (0, 0);
        _visited.Add(p);

        corners += CountCorners(plant, p);

        if (p.X > 0 && _map[p.Y][p.X - 1] == plant)
        {
            var result = CalcBulkPerimeter(plant, p with { X = p.X - 1 });
            area += result.area;
            corners += result.corners;
        }
        
        if (p.Y > 0 && _map[p.Y - 1][p.X] == plant)
        {
            var result = CalcBulkPerimeter(plant, p with { Y = p.Y - 1 });
            area += result.area;
            corners += result.corners;
        }
        
        if (p.X < _mapWidth - 1 && _map[p.Y][p.X + 1] == plant)
        {
            var result = CalcBulkPerimeter(plant, p with { X = p.X + 1 });
            area += result.area;
            corners += result.corners;
        }
        
        if (p.Y < _mapHeight - 1 && _map[p.Y + 1][p.X] == plant)
        {
            var result = CalcBulkPerimeter(plant, p with { Y = p.Y + 1 });
            area += result.area;
            corners += result.corners;
        }
        
        return (area, corners);
    }
    
    private static (int area, int perimeter) CalcPerimeter(char plant, Point p)
    {
        var area = 1;
        var perimeter = 0;
        if (_visited.Contains(p))
            return (0, 0);
        _visited.Add(p);
        
        if (p.X == 0 || _map[p.Y][p.X - 1] != plant)
        {
            perimeter++;
        }
        else if (_map[p.Y][p.X - 1] == plant)
        {
            var result = CalcPerimeter(plant, p with { X = p.X - 1 });
            area += result.area;
            perimeter += result.perimeter;
        }
        
        if (p.Y == 0 || _map[p.Y - 1][p.X] != plant)
        {
            perimeter++;
        }
        else if (_map[p.Y - 1][p.X] == plant)
        {
            var result = CalcPerimeter(plant, p with { Y = p.Y - 1 });
            area += result.area;
            perimeter += result.perimeter;
        }
        
        if (p.X == _mapWidth - 1 || _map[p.Y][p.X + 1] != plant)
        {
            perimeter++;
        }
        else if (_map[p.Y][p.X + 1] == plant)
        {
            var result = CalcPerimeter(plant, p with { X = p.X + 1 });
            area += result.area;
            perimeter += result.perimeter;
        }
        
        if (p.Y == _mapHeight - 1 || _map[p.Y + 1][p.X] != plant)
        {
            perimeter++;
        }
        else if (_map[p.Y + 1][p.X] == plant)
        {
            var result = CalcPerimeter(plant, p with { Y = p.Y + 1 });
            area += result.area;
            perimeter += result.perimeter;
        }
        
        return (area, perimeter);
    }

    private static int CountCorners(char c, Point p)
    {
        var corners = 0;
        //check left and up
        if ((p.X == 0 || _map[p.Y][p.X - 1] != c) && (p.Y == 0 || _map[p.Y - 1][p.X] != c)
            || (p.X > 0 && _map[p.Y][p.X - 1] == c && p.Y > 0 && _map[p.Y - 1][p.X] == c && _map[p.Y - 1][p.X - 1] != c))
        {
            corners++;
        }
        
        //check right and up
        if ((p.X == _mapWidth - 1 || _map[p.Y][p.X + 1] != c) && (p.Y == 0 || _map[p.Y - 1][p.X] != c)
            || (p.X < _mapWidth - 1 && _map[p.Y][p.X + 1] == c && p.Y > 0 && _map[p.Y - 1][p.X] == c && _map[p.Y - 1][p.X + 1] != c))
        {
            corners++;
        }

        //check right and down
        if ((p.X == _mapWidth - 1 || _map[p.Y][p.X + 1] != c) && (p.Y == _mapHeight - 1 || _map[p.Y + 1][p.X] != c)
            || (p.X < _mapWidth - 1 && _map[p.Y][p.X + 1] == c && p.Y < _mapHeight - 1 && _map[p.Y + 1][p.X] == c && _map[p.Y + 1][p.X + 1] != c))
        {
            corners++;
        }
        
        //check left and down
        if ((p.X == 0 || _map[p.Y][p.X - 1] != c) && (p.Y == _mapHeight - 1 || _map[p.Y + 1][p.X] != c)
            || (p.X > 0 && _map[p.Y][p.X - 1] == c && p.Y < _mapHeight - 1 && _map[p.Y + 1][p.X] == c && _map[p.Y + 1][p.X - 1] != c))
        {
            corners++;
        }
        
        return corners;
    }
}