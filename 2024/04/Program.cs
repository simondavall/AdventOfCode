namespace _04;

internal static partial class Program
{
    private static string[] _map = [];
    private static int _mapWidth;
    private static int _mapHeight;
    
    internal static void Main()
    {
        _map = File.ReadAllText("input.txt").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        _mapWidth = _map[0].Length;
        _mapHeight = _map.Length;
        
        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }
    
    private static long PartOne()
    {
        long tally = 0;
        
        for (var row = 0; row < _mapHeight - 1; row++)
            for (var col = 0; col < _mapWidth - 1; col++)
            {
                if (_map[row][col] != 'X')
                    continue;
                
                foreach (var deltaRow in List([-1, 0, 1]))
                    foreach (var deltaCol in List([-1, 0, 1]))
                    {
                        if (deltaRow == 0 && deltaCol == 0)
                            continue;

                        if (!IsInBounds(row, deltaRow, _map) || !IsInBounds(col, deltaCol, _map))
                            continue;

                        if (_map[row + deltaRow][col + deltaCol] == 'M'
                            && _map[row + 2 * deltaRow][col + 2 * deltaCol] == 'A'
                            && _map[row + 3 * deltaRow][col + 3 * deltaCol] == 'S')
                            tally++;
                    }
            }

        return tally;
    }

    private static long PartTwo()
    {
        long tally = 0;
        
        foreach (var row in Range(1, _mapHeight - 2))
            foreach (var col in Range(1, _mapWidth - 2))
                if (_map[row][col] == 'A' && Has_Xmas(row, col))
                {
                    tally++;
                }
        
        return tally;
    }

    private static bool Has_Xmas(int row, int col)
    {
        return ((AboveLeft(row, col, 'M') && BelowRight(row, col, 'S'))
                || (AboveLeft(row, col, 'S') && BelowRight(row, col, 'M')))
               && ((AboveRight(row, col, 'M') && BelowLeft(row, col, 'S'))
                   || (AboveRight(row, col, 'S') && BelowLeft(row, col, 'M')));
    }

    private static bool AboveLeft(int row, int col, char ch)
    {
        return _map[row - 1][col - 1] == ch;
    }
    
    private static bool AboveRight(int row, int col, char ch)
    {
        return _map[row - 1][col + 1] == ch;
    }
    
    private static bool BelowLeft(int row, int col, char ch)
    {
        return _map[row + 1][col - 1] == ch;
    }
    
    private static bool BelowRight(int row, int col, char ch)
    {
        return _map[row + 1][col + 1] == ch;
    }
    
    private static bool IsInBounds(int value, int delta, string[] map)
    {
        var bound = value + 3 * delta;
        return bound >= 0 && bound < map.Length;
    }
}