namespace _06;

internal static partial class Program
{
    private static char[][] _map = null!;
    private static short _mapWidth;
    private static short _mapHeight;
    private static readonly Dictionary<(short row, short col), (short deltaRow, short deltaCol)> Visited = [];
    private static (short row, short col) _initialPosition = (0, 0);
    
    internal static void Main()
    {
        var input = File.ReadAllText("input.txt").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        _mapWidth = (short)input[0].Length;
        _mapHeight = (short)input.Length;

        _map = new char[_mapHeight][];
        for (short i = 0; i < _mapHeight; i++)
            _map[i] = input[i].ToCharArray();

        for (short i = 0; i < _mapHeight; i++)
        {
            if (input[i].IndexOf('^') == -1)
                continue;
            _initialPosition = (i, (short)input[i].IndexOf('^'));
        }
        
        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }

    private static long PartOne()
    {
        var row = _initialPosition.row;
        var col = _initialPosition.col;
        short deltaRow = -1;
        short deltaCol = 0;
        
        while (true)
        {
            Visited.TryAdd((row, col), (deltaRow, deltaCol));
            (short row, short col) nextPosition = ((short)(row + deltaRow), (short)(col + deltaCol));
            if (!IsInBounds(nextPosition))
                break;
            if (_map[nextPosition.row][nextPosition.col] == '#')
                (deltaCol, deltaRow) = ((short)-deltaRow, deltaCol);
            else
            {
                row += deltaRow;
                col += deltaCol;
            }
        }
        return Visited.Count;
    }

    private static readonly List<(short row, short col, short deltaRow, short deltaCol)> Revisited = [];
    
    private static long PartTwo()
    {
        var row = _initialPosition.row;
        var col = _initialPosition.col;
        short deltaRow = -1;
        short deltaCol = 0;

        long tally = 0;
        
        Revisited.Add((row, col, deltaRow, deltaCol));
        
        foreach (var ((nextRow, nextCol), (nextDeltaRow, nextDeltaCol)) in Visited)
        {
            if ((row, col) == (nextRow, nextCol))
                continue;

            _map[nextRow][nextCol] = '#';
            if (HasInfiniteLoop(row, col, deltaRow, deltaCol))
            {
                tally++;
            }

            _map[nextRow][nextCol] = '.';
            (row, col, deltaRow, deltaCol) = (nextRow, nextCol, nextDeltaRow, nextDeltaCol);
            Revisited.Add((row, col, deltaRow, deltaCol));
        }
        
        return tally;
    }

    private static readonly HashSet<(short row, short col, short deltaRow, short deltaCol)> NewVisited = [];
    
    private static bool HasInfiniteLoop(short row, short col, short deltaRow, short deltaCol)
    {
        NewVisited.Clear();
        
        while (true)
        {
            NewVisited.Add((row, col, deltaRow, deltaCol));
            (short row, short col) nextPosition = ((short)(row + deltaRow), (short)(col + deltaCol));
            if (!IsInBounds(nextPosition))
                return false;
            if (_map[nextPosition.row][nextPosition.col] == '#')
                (deltaCol, deltaRow) = ((short)-deltaRow, deltaCol);
            else
            {
                row += deltaRow;
                col += deltaCol;
            }

            if (Revisited.Contains((row, col, deltaRow, deltaCol)) || NewVisited.Contains((row, col, deltaRow, deltaCol)))
                return true;
        }
    }
    
    private static bool IsInBounds((short row, short col) position)
    {
        return position.row >= 0
               && position.row < _mapHeight
               && position.col >= 0
               && position.col < _mapWidth;
    }
}