using System.Diagnostics;
using System.Drawing;

namespace _20;

internal static class Program
{
    private static string _filename = null!;
    private static int _mapHeight;
    private static int _mapWidth;
    private static char[,] _map = null!;
    private static Point _start;
    private static Point _finish;
    private static Dictionary<Point, int> _racePath = [];
    private static int _psSaved;
    private static int _maxCheatLenPart1;
    private static int _maxCheatLenPat2;
    private static readonly Dictionary<int, int> Cheats = [];

    internal static void Main()
    {
        SetConfig("input.txt");
        _map = File.ReadAllText($"{_filename}")
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .To2DCharArray();
        
        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }
    
    private static long PartOne()
    {
        // run race in reverse
        var currentPoint = _finish;
        _racePath.Add(currentPoint, 0);
        while (currentPoint != _start)
        {
            currentPoint = FindNext(currentPoint);
        }
        // correct the direction of the results
        _racePath = _racePath.Reverse().ToDictionary();
        
        foreach (var index in Enumerable.Range(0, _racePath.Count - 1))
        {
            FindCheats(index, _maxCheatLenPart1);
        }

        return Cheats.Sum(x => x.Value);
    }
    
    private static long PartTwo()
    {
        Cheats.Clear();

        foreach (var index in Enumerable.Range(0, _racePath.Count - 1))
        {
            FindCheats(index, _maxCheatLenPat2);
        }
        
        return Cheats.Sum(x => x.Value);
    }
    
    private static void FindCheats(int index, int maxCheatLen)
    {
        var currentPoint = _racePath.Keys.Skip(index).First();
        foreach (var destPoint in _racePath.Skip(index + 1))
        {
            var dx = currentPoint.X - destPoint.Key.X;
            var dy = currentPoint.Y - destPoint.Key.Y;

            var cheatLen = int.Abs(dx) + int.Abs(dy);
            if (cheatLen > maxCheatLen) 
                continue;
            
            var saving = _racePath[currentPoint] - (destPoint.Value + cheatLen);
            if (saving < _psSaved) 
                continue;
                
            if (!Cheats.TryAdd(saving, 1))
                Cheats[saving]++;
        }
    }
    
    private static Point FindNext(Point p)
    {
        var pUp = p with { X = p.X - 1 };
        if (pUp.IsNextPoint())
        {
            return pUp;
        }
        var pRight = p with { Y = p.Y + 1 };
        if (pRight.IsNextPoint())
        {
            return pRight;
        }
        var pDown = p with { X = p.X + 1 };
        if (pDown.IsNextPoint())
        {
            return pDown;
        }
        var pLeft = p with { Y = p.Y - 1 };
        if (pLeft.IsNextPoint())
        {
            return pLeft;
        }

        throw new UnreachableException("Did not find next point in race");
    }
    
    private static bool IsNextPoint(this Point p)
    {
        if ((_map[p.X, p.Y] == '.' || _map[p.X, p.Y] == 'S') && !_racePath.ContainsKey(p))
        {
            _racePath.Add(p, _racePath.Count);
            return true;
        }
            
        return false;
    }

    private static void SetConfig(string filename)
    {
        if (filename == "sample.txt")
        {
            _filename = filename;
            _psSaved = 12;
            _maxCheatLenPart1 = 2;
            _maxCheatLenPat2 = 6;
            return;
        }

        _filename = "input.txt";
        _psSaved = 100;
        _maxCheatLenPart1 = 2;
        _maxCheatLenPat2 = 20;
    }
    
    private static char[,] To2DCharArray(this string[] input)
    {
        var charArray = new char[input[0].Length, input.Length];
        _mapHeight = input.Length;
        _mapWidth = input[0].Length;
        for(var y = 0; y < _mapHeight; y++)
        {
            for (var x = 0; x < _mapWidth; x++)
            {
                var ch = input[y][x];
                if (ch == 'S')
                    _start = new Point(x, y);
                if (ch == 'E')
                    _finish = new Point(x, y);
                charArray[x, y] = ch;
            }
        }

        return charArray;
    }
}