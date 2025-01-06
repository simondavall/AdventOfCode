namespace _17;

internal static partial class Program
{
    private static int[][] _map = [];
    private static int _rows;
    private static int _cols;

    internal static void Main()
    {
        const string filename = "input.txt";
        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _rows = input.Length;
        _cols = input[0].Length;

        _map = new int[_rows][];
        for (var i = 0; i < _rows; i++)
        {
            _map[i] = input[i].ToIntArray();
        }

        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }

    private static readonly HashSet<(int row, int col, int dRow, int dCol, int n)> Visited = [];
    private static readonly (int dRow, int dCol)[] Directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    
    private static long PartOne()
    {
        var endPoint = (_rows - 1, _cols - 1);
        
        var q = new PriorityQueue<(int row, int col, int dRow, int dCol, int n), int>();
        q.Enqueue((0,0,0,0,0), 0);
        while (q.Count > 0)
        {
            q.TryDequeue(out var item, out var heatloss);
            var (row, col, dRow, dCol, n) = item;

            if ((row, col) == endPoint)
            {
                return heatloss;
            }
            
            if (!Visited.Add((row, col, dRow, dCol, n)))
                continue;

            if (n < 3 && (dRow, dCol) != (0, 0))
            {
                var nextRow = row + dRow;
                var nextCol = col + dCol;
                if (IsInBounds(nextRow, nextCol))
                {
                    q.Enqueue((nextRow, nextCol, dRow, dCol, n + 1), heatloss + _map[nextRow][nextCol]);
                }
            }

            foreach (var (nextDRow, nextDCol) in Directions)
            {
                if ((nextDRow, nextDCol) != (dRow, dCol) && (nextDRow, nextDCol) != (-dRow, -dCol))
                {
                    var nextRow = row + nextDRow;
                    var nextCol = col + nextDCol;
                    if (IsInBounds(nextRow, nextCol))
                        q.Enqueue((nextRow, nextCol, nextDRow, nextDCol, 1), heatloss + _map[nextRow][nextCol]);
                }
            }
        }

        return 0;
    }
    
    private static long PartTwo()
    {
        Visited.Clear();
        var endPoint = (_rows - 1, _cols - 1);
        
        var q = new PriorityQueue<(int row, int col, int dRow, int dCol, int n), int>();
        q.Enqueue((0,0,0,0,0), 0);
        while (q.Count > 0)
        {
            q.TryDequeue(out var item, out var heatloss);
            var (row, col, dRow, dCol, n) = item;

            if ((row, col) == endPoint && n >= 4)
            {
                return heatloss;
            }
            
            if (!Visited.Add((row, col, dRow, dCol, n)))
                continue;

            if (n < 10 && (dRow, dCol) != (0, 0))
            {
                var nextRow = row + dRow;
                var nextCol = col + dCol;
                if (IsInBounds(nextRow, nextCol))
                {
                    q.Enqueue((nextRow, nextCol, dRow, dCol, n + 1), heatloss + _map[nextRow][nextCol]);
                }
            }

            if (n >= 4 || (dRow, dCol) == (0, 0))
            {
                foreach (var (nextDRow, nextDCol) in Directions)
                {
                    if ((nextDRow, nextDCol) != (dRow, dCol) && (nextDRow, nextDCol) != (-dRow, -dCol))
                    {
                        var nextRow = row + nextDRow;
                        var nextCol = col + nextDCol;
                        if (IsInBounds(nextRow, nextCol))
                            q.Enqueue((nextRow, nextCol, nextDRow, nextDCol, 1), heatloss + _map[nextRow][nextCol]);
                    }
                }
            }
        }

        return 0;
    }
    
    private static bool IsInBounds(int row, int col)
    {
        return row >= 0 && row < _rows && col >= 0 && col < _cols;
    }
}