namespace _23;

internal static partial class Program
{
    private static char[][] _map = [];
    private static int _mapHeight;
    private static int _mapWidth;
    private static (int r, int c) _startingPoint;
    private static (int r, int c) _finishPoint;
    
    private static readonly HashSet<(int r, int c)> Seen = [];
    private static readonly Dictionary<(int, int), Dictionary<(int, int), int>> Graph = [];
    

    private static readonly Dictionary<char, (int, int)[]> Directions = new()
    {
        { '.', [(-1, 0), (0, 1), (1, 0), (0, -1)] },
        { '^', [(-1, 0)] },
        { '>', [(0, 1)] },
        { 'v', [(1, 0)] },
        { '<', [(0, -1)] }
    };
    
    internal static void Main()
    {
        const string filename = "input.txt";
        var map = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        _map = map.ToCharArray();
        _mapHeight = _map.Length;
        _mapWidth = _map[0].Length;

        _startingPoint = (0, map[0].IndexOf('.'));
        _finishPoint = (_mapHeight - 1, map.Last().IndexOf('.'));
        
        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }

    private static long PartOne()
    {
        BuildGraph();
        return FindLongestPath(_startingPoint);
    }

    private static long PartTwo()
    {
        Seen.Clear();
        Graph.Clear();
        foreach (var ch in "^>v<")
        {
            Directions[ch] = [(-1, 0), (0, 1), (1, 0), (0, -1)];
        }
        
        BuildGraph();
        return FindLongestPath(_startingPoint);
    }
    
    private static int FindLongestPath((int r, int c) p)
    {
        if (p == _finishPoint)
            return 0;

        var maxLength = -1;

        Seen.Add(p);
        foreach (var next in Graph[p].Keys)
        {
            if (Seen.Contains(next)) 
                continue;
            
            var length = FindLongestPath(next);
            if (length < 0) 
                continue;
            
            maxLength = Math.Max(maxLength, length + Graph[p][next]);
        }
        Seen.Remove(p);
        
        return maxLength;
    }
    
    private static void BuildGraph()
    {
        HashSet<(int, int)> points = [_startingPoint, _finishPoint];
        foreach (var r in Range(_mapHeight))
            foreach (var c in Range(_mapWidth))
            {
                var ch = _map[r][c];
                if (ch == '#')
                    continue;
                
                var neighbours = 0;
                foreach (var (dr, dc) in Directions['.'])
                {
                    var nr = r + dr;
                    var nc = c + dc;
                    if (IsInBounds(nr, nc) && _map[nr][nc] != '#')
                        neighbours++;
                    if (neighbours > 2)
                        points.Add((r, c));
                }
            }
        
        foreach (var p in points)
            Graph.Add(p, []);

        foreach (var (sr, sc) in points)
        {
            var stack = new Stack<(int n, int r, int c)>([(0, sr, sc)]);
            HashSet<(int r, int c)> seen = [(sr, sc)];

            while (stack.Count > 0)
            {
                var (n, r, c) = stack.Pop();

                if (n != 0 && points.Contains((r, c)))
                {
                    Graph[(sr, sc)][(r, c)] = n;
                    continue;
                }

                foreach (var (dr, dc) in Directions[_map[r][c]])
                {
                    var nr = r + dr;
                    var nc = c + dc;
                    
                    if (!IsInBounds(nr, nc) || _map[nr][nc] == '#' || seen.Contains((nr, nc))) 
                        continue;
                    
                    stack.Push((n + 1, nr, nc));
                    seen.Add((nr, nc));
                }
            }
        }
    }
    
    private static bool IsInBounds(int r, int c)
    {
        return r >= 0 && r < _mapHeight && c >= 0 && c < _mapWidth;
    }
}