using System.Diagnostics;

namespace _06;

internal static class Program
{
    private static char[,] _charMap = null!;
    private static int _mapWidth;
    private static int _mapHeight;
    private static readonly List<(int row, int col)> PreviouslyVisited = [];
    private static (int row, int col) _initialPosition = (0, 0);
    private static Direction _initialDirection = Direction.Down;
    
    internal static void Main()
    {
        const string aocDay = "06";
        const string filename = "input.txt";
        const string path = $"/home/sdv/Documents/Projects/Aoc/2024/{aocDay}/";
        var map = File.ReadAllLines($"{path}{filename}").ToArray();
        
        _mapWidth = map[0].Length;
        _mapHeight = map.Length;

        _charMap = new char[_mapHeight, _mapWidth];
        for (var i = 0; i < _mapHeight; i++)
            for (var j = 0; j < _mapWidth; j++)
            {
                _charMap[i, j] = map[i][j];
            }

        for (var i = 0; i < _mapHeight; i++)
        {
            var j = map[i].IndexOfAny(['^', '>', '<', 'V']);
            if (j <= -1) continue;
            _initialPosition = (i, j);
            _initialDirection = GetDirection(map[i][j]);
            break;
        }
        
        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }

    private static long PartOne()
    {
        long tally = 1;
        
        var guardPosition = _initialPosition;
        var guardDirection = _initialDirection;
        
        PreviouslyVisited.Add(guardPosition);
        
        while(GuardMove(ref guardPosition, ref guardDirection))
        {
            if (!PreviouslyVisited.Contains(guardPosition))
            {
                PreviouslyVisited.Add(guardPosition);
                tally++;
            }
        }
        
        return tally;
    }

    private static long PartTwo()
    {
        long tally = 0;
        
        foreach (var visitedPosition in PreviouslyVisited)
        {
            if (visitedPosition == _initialPosition)
                continue;
            
            var guardPosition = _initialPosition;
            var guardDirection = _initialDirection;
            List<(int row, int col, Direction direction)> previouslyVisited = [(guardPosition.row, guardPosition.col, guardDirection)];
            
            _charMap[visitedPosition.row, visitedPosition.col] = '#';
            
            while(GuardMove(ref guardPosition, ref guardDirection))
            {
                if (previouslyVisited.Contains((guardPosition.row, guardPosition.col, guardDirection)))
                {
                    tally++;
                    break;
                }
                
                previouslyVisited.Add((guardPosition.row, guardPosition.col, guardDirection));
            }
            
            _charMap[visitedPosition.row, visitedPosition.col] = '.';
        }
        
        return tally;
    }

    private static bool GuardMove(ref (int row, int col) guardPosition, ref Direction guardDirection)
    {
        var row = guardPosition.row;
        var col = guardPosition.col;
        
        switch (guardDirection)
        {
            case Direction.Up: row--; break;
            case Direction.Down: row++; break;
            case Direction.Right: col++; break;
            case Direction.Left: col--; break;
            default: throw new UnreachableException();
        }

        if (!IsGuardOnMap((row, col)))
        {
            return false;
        }
        
        if (_charMap[row, col] == '#')
        {
            guardDirection = (Direction)((int)++guardDirection % 4);
            return GuardMove(ref guardPosition, ref guardDirection);
        }

        guardPosition = (row, col);
        return true;
    }
    
    private static bool IsGuardOnMap((int row, int col) guardPosition)
    {
        return guardPosition.row >= 0
               && guardPosition.row < _mapHeight
               && guardPosition.col >= 0
               && guardPosition.col < _mapWidth;
    }
    
    private static Direction GetDirection(char c)
    {
        return c switch
        {
            '^' => Direction.Up,
            '>' => Direction.Right,
            'V' => Direction.Down,
            '<' => Direction.Left,
            _ => throw new InvalidCastException(c.ToString())
        };
    }
    
    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}