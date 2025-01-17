using AocHelper;

namespace _08;

internal static class Program
{
    private const long ExpectedPartOne = 1794;
    private const long ExpectedPartTwo = 199272;
    private const string Day = "_08";

    private static readonly (int dr, int dc)[] Directions = [(-1, 0), (0, 1), (1, 0), (0, -1)];
    private static int _mapHeight;
    private static int _mapWidth;
    
    public static int Main(string[] args)
    {
        var filename = "input_08.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries).To2DIntArray();

        _mapHeight = input.Length;
        _mapWidth = input[0].Length;

        var resultPartOne = PartOne(input);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(input);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(int[][] map)
    {
        long interiorTrees = 0;
        
        for (var r = 1; r < _mapHeight - 1; r++)
            for (var c = 1; c < _mapWidth - 1; c++)
                if (CanBeSeen(r, c, map))
                    interiorTrees++;

        var edgeTrees = _mapHeight * 2 + (_mapWidth - 2) * 2;
        return edgeTrees + interiorTrees;
    }
    
    private static long PartTwo(int[][] map)
    {
        var maxScenicScore = 0;
        
        for (var r = 0; r < _mapHeight; r++)
            for (var c = 0; c < _mapWidth; c++)
            {
                var dist = 1;
                foreach (var (dr, dc) in Directions)
                    dist *= GetViewDistance(r + dr, c + dc, dr, dc, map[r][c], 0, map);

                maxScenicScore = Math.Max(maxScenicScore, dist);
            }

        return maxScenicScore;
    }

    private static int GetViewDistance(int r, int c, int dr, int dc, int height, int dist, int[][] map)
    {
        while (true)
        {
            if (!IsInBounds(r, c)) 
                return dist;

            if (map[r][c] >= height) 
                return dist + 1;

            r += dr;
            c += dc;
            dist++;
        }
    }

    private static bool IsInBounds(int r, int c)
    {
        return r >= 0 && r < _mapHeight && c >= 0 && c < _mapWidth;
    }
    
    private static bool CanBeSeen(int r, int c, int[][] map)
    {
        foreach (var (dir, dist) in DirectionsByLeastDistance(r, c))
        {
            var (dr, dc) = Directions[dir];
            var position = 0;
            while (position++ < dist)
            {
                var nr = r + (dr * position);
                var nc = c + (dc * position);
                if (map[r][c] <= map[nr][nc])
                    break;
            }

            if (position > dist)
                return true;
        }

        return false;
    }
    
    private static List<(int dir, int dist)> DirectionsByLeastDistance(int r, int c)
    {
        IEnumerable<(int dir, int dist)> edgeDistance = [(0, r), (1, _mapWidth - c - 1), (2, _mapHeight - r - 1), (3, c)];
        return edgeDistance.OrderBy(x => x.dist).ToList();
    }
}