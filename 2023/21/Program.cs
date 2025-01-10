using System.Diagnostics;

namespace _21;

internal static partial class Program
{
    private static char[][] _map = null!;
    private static int _mapHeight;
    private static int _mapWidth;
    private static (int row, int col) _startPoint = (0, 0);
    private static readonly (int dr, int dc)[] Directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];


    internal static void Main()
    {
        const string filename = "input.txt";
        _map = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries).ToCharArray();
        
        _mapHeight = _map.Length;
        _mapWidth = _map[0].Length;

        for (var r = 0; r < _mapHeight; r++)
            for (var c = 0; c < _mapWidth; c++)
            {
                if (_map[r][c] != 'S')
                    continue;
                _startPoint = (r, c);
            }
        
        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }

    private static long PartOne()
    {
        return GetGardenPlots(_startPoint.row, _startPoint.col, 64);
    }

    private static long PartTwo()
    {
        var size = _mapHeight;
        const int steps = 26501365;
        
        CheckAssumptions(size, steps);

        long gridWidth = steps / size - 1;
        var oddGrids = (gridWidth / 2 * 2 + 1).Squared();
        var evenGrids = ((gridWidth + 1) / 2 * 2).Squared();

        var oddPoints = GetGardenPlots(_startPoint.row, _startPoint.col, size * 2 + 1);
        var evenPoints = GetGardenPlots(_startPoint.row, _startPoint.col, size * 2);
        var gridsTotal = oddGrids * oddPoints + evenGrids * evenPoints;
        
        var cornerTop = GetGardenPlots(size - 1, _startPoint.col, size - 1);
        var cornerRight = GetGardenPlots(_startPoint.row, 0, size - 1);
        var cornerBottom = GetGardenPlots(0, _startPoint.col, size - 1);
        var cornerLeft = GetGardenPlots(_startPoint.row, size - 1, size - 1);
        var cornersTotal = cornerTop + cornerRight + cornerBottom + cornerLeft;
        
        var smallTopRight = GetGardenPlots(size - 1, 0, size / 2 - 1);
        var smallTopLeft = GetGardenPlots(size - 1, size - 1, size / 2 - 1);
        var smallBottomRight = GetGardenPlots(0, 0, size / 2 - 1);
        var smallBottomLeft = GetGardenPlots(0, size - 1, size / 2 - 1);
        var smallTriangles = (gridWidth + 1) * (smallTopRight + smallTopLeft + smallBottomRight + smallBottomLeft); 
        
        var largeTopRight = GetGardenPlots(size - 1, 0, size * 3 / 2 - 1);
        var largeTopLeft = GetGardenPlots(size - 1, size - 1, size * 3 / 2 - 1);
        var largeBottomRight = GetGardenPlots(0, 0, size * 3 / 2 - 1);
        var largeBottomLeft = GetGardenPlots(0, size - 1, size * 3 / 2 - 1);
        var largeTriangles = gridWidth * (largeTopRight + largeTopLeft + largeBottomRight + largeBottomLeft);
        
        return gridsTotal + cornersTotal + smallTriangles + largeTriangles;
    }

    private static void CheckAssumptions(int size, int steps)
    {
        // grid is square
        if (_mapHeight != _mapWidth)
            throw new UnreachableException("Gird is not square");
        // start in centre
        if (_startPoint.row != _startPoint.col || _startPoint.row != size / 2)
            throw new UnreachableException("Start point not in centre");
        // steps take us to the edge of an exact number of grids
        if (steps % size != size / 2)
            throw new UnreachableException("Start point not in centre");
        // no rocks on centre row or column
        var rocks = 0;
        foreach (var i in Range(_mapHeight))
        {
            rocks += _map[i][size / 2] == '#' ? 1 : 0;
            rocks += _map[size / 2][i] == '#' ? 1 : 0;
        }
        if (rocks > 0)
            throw new UnreachableException("Rocks appear on centre lines");
    }
    private static long GetGardenPlots(int sr, int sc, int steps)
    {
        HashSet<(int, int)> positions = [];
        HashSet<(int, int)> visited = [];

        var q = new Queue<(int, int, int)>();
        q.Enqueue((sr, sc, steps));

        while (q.Count > 0)
        {
            var (r, c, step) = q.Dequeue();

            if (step % 2 == 0)
                positions.Add((r, c));

            if (step == 0)
                continue;

            foreach (var (dr, dc) in Directions)
            {
                var (nr, nc) = (r + dr, c + dc);
                if (!IsInBounds(nr, nc) || _map[nr][nc] == '#' || !visited.Add((nr, nc)))
                    continue;

                q.Enqueue((nr, nc, step - 1));
            }
        }

        return positions.Count;
    }
    
    private static bool IsInBounds(long r, long c)
    {
        return r >= 0 && r < _mapHeight && c >= 0 && c < _mapWidth;
    }
}