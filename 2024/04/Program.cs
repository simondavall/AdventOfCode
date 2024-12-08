using System.Text.RegularExpressions;
// ReSharper disable ClassNeverInstantiated.Global

namespace _04;

internal partial class Program
{
    private static string[] _map = [];
    private static int _mapWidth;
    private static int _mapHeight;
    
    internal static void Main()
    {
        const string aocDay = "04";
        const string filename = "input.txt";
        const string path = $"/home/sdv/Documents/Projects/Aoc/2024/{aocDay}/";
        _map = File.ReadAllLines($"{path}{filename}").ToArray();

        _mapWidth = _map[0].Length;
        _mapHeight = _map.Length;
        
        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }

    private static long PartOne()
    {
        long tally = 0;
        var combinations = new []
        {
            _map,
            ReverseChars(_map),
            Down(_map),
            ReverseChars(Down(_map)),
            LeftDiagonal(_map),
            ReverseChars(LeftDiagonal(_map)),
            RightDiagonal(_map),
            ReverseChars(RightDiagonal(_map))
        };
        
        foreach (var combination in combinations)
        {
            foreach (var line in combination)
            {
                tally += XmasRegex().Matches(line).Count;
            }
        }
        
        return tally;
    }

    private static long PartTwo()
    {
        long tally = 0;

        for (var i = 1; i < _mapHeight - 1; i++)
        {
            for (var j = 1; j < _mapWidth - 1; j++)
            {
                if (_map[i][j] == 'A' && HasX_Mas(i, j))
                {
                    tally++;
                }
            }
        }
        
        return tally;
    }

    private static bool HasX_Mas(int row, int col)
    {
        return ((AboveLeft(row, col, 'M') && BelowRight(row, col, 'S'))
                || (AboveLeft(row, col, 'S') && BelowRight(row, col, 'M')))
               && ((AboveRight(row, col, 'M') && BelowLeft(row, col, 'S'))
                   || (AboveRight(row, col, 'S') && BelowLeft(row, col, 'M')));
    }

    private static bool AboveLeft(int row, int col, char c)
    {
        return _map[row - 1][col - 1] == c;
    }
    
    private static bool AboveRight(int row, int col, char c)
    {
        return _map[row - 1][col + 1] == c;
    }
    
    private static bool BelowLeft(int row, int col, char c)
    {
        return _map[row + 1][col - 1] == c;
    }
    
    private static bool BelowRight(int row, int col, char c)
    {
        return _map[row + 1][col + 1] == c;
    }

    private static string[] Down(string[] map)
    {
        List<string> down = [];
        for (var i = 0; i < _mapWidth; i++)
        {
            var newRow = new char[_mapHeight];
            for (var j = 0; j < _mapHeight; j++)
            {
                newRow[j] = map[j][i];
            }
            down.Add(new string(newRow));
        }
        return down.ToArray();
    }

    private static string[] LeftDiagonal(string[] map)
    {
        List<string> diagonal = [];
        for (var i = 0; i < _mapHeight + _mapWidth; i++)
        {
            List<char> diagChar = [];
            var row = i;
            var col = 0;
            if (i >= _mapHeight)
            {
                row = _mapHeight - 1;
                col = i - _mapHeight + 1;
            }

            while (col < _mapWidth && row >= 0)
            {
                diagChar.Add(map[row--][col++]);
            }
            diagonal.Add(new string(diagChar.ToArray()));
        }

        return diagonal.ToArray();
    }
    
    private static string[] RightDiagonal(string[] map)
    {
        List<string> diagonal = [];
        for (var i = 0; i < _mapHeight + _mapWidth; i++)
        {
            List<char> diagChar = [];
            int row; var col = _mapWidth - 1;
            if (i >= _mapHeight)
            {
                row = _mapHeight - 1;
                col = _mapWidth - 2 - (i - _mapHeight);
            }
            else
            {
                row = i;
            }

            while (col >= 0 && row >= 0)
            {
                diagChar.Add(map[row--][col--]);
            }
            diagonal.Add(new string(diagChar.ToArray()));
        }

        return diagonal.ToArray();
    }
    
    private static string[] ReverseChars(string[] array)
    {
        List<string> reverse = [];
        foreach (var row in array)
        {
            reverse.Add(new string(row.Reverse().ToArray()));
        }

        return reverse.ToArray();
    }

    [GeneratedRegex("XMAS")]
    private static partial Regex XmasRegex();
}