using System.Drawing;

namespace _14;

internal static class Program
{
    private static int _mapHeight = 7;
    private static int _mapWidth = 11;
    private static int[,] _map = new int[11,7];
    private static string _filename = "sample.txt";

    internal static void Main()
    {
        DataSetup("input.txt");
        var robotData = File.ReadAllText($"{_filename}").Split("\n", StringSplitOptions.RemoveEmptyEntries);
        
        Console.WriteLine($"Part 1: {PartOne(robotData)}");
        Console.WriteLine($"Part 2: {PartTwo(robotData)}");
    }

    private static long PartOne(string[] robotData)
    {
        List<Robot> robots = [];
        robots.AddRange(robotData.Select(line => line.Split(" "))
            .Select(point => new Robot(GetPoint(point[0]), GetPoint(point[1]))));
        var quads = new int[4];
        
        foreach (var robot in robots)
        {
            var x = (robot.Position.X + (robot.Velocity.X + _mapWidth) * 100) % _mapWidth;
            var y = (robot.Position.Y + (robot.Velocity.Y + _mapHeight) * 100) % _mapHeight;
            robot.Position = robot.Position with { X = x, Y = y };
            
            var index = GetQuad(robot.Position);
            if (index > -1)
                quads[index]++;
        }
        
        return quads.Aggregate(1, (current, quad) => current * quad);
    }
    
    private static long PartTwo(string[] robotData)
    {
        List<Robot> robots = [];
        robots.AddRange(robotData.Select(line => line.Split(" "))
            .Select(point => new Robot(GetPoint(point[0]), GetPoint(point[1]))));
        var secs = 0;
        var points = new Point[500];

        while (secs++ < 10403)
        {
            var index = 0;
            
            foreach (var robot in robots)
            {
                Move(robot);
                points[index++] = robot.Position;
            }

            if (HasUnusualSpread(points) > 300)
            {
                //PrintMap(robots, secs);
                return secs;
            }
        }
        
        return 0;
    }

    private static int HasUnusualSpread(Point[] points)
    {
        const int weight = 20;
        var midHeight = _mapHeight / 2;
        var midWidth = _mapWidth / 2;

        var count = 0;
        
        foreach (var point in points)
        {
            if (point.X > midWidth - weight && point.X < midWidth + weight
            && point.Y > midHeight - weight && point.Y < midHeight + weight)
                count++;
        }

        return count;
    }

    private static int GetQuad(Point p)
    {
        var halfWidth = _mapWidth / 2;
        var halfHeight = _mapHeight / 2;
        
        if (p.X < halfWidth && p.Y < halfHeight)
            return 0;
        if (p.X > halfWidth && p.Y < halfHeight)
            return 1;
        if (p.X < halfWidth && p.Y > halfHeight)
            return 2;
        if (p.X > halfWidth && p.Y > halfHeight)
            return 3;

        return -1;
    }
    private static void Move(Robot robot)
    {
        var x = (robot.Position.X + robot.Velocity.X + _mapWidth) % _mapWidth;
        var y = (robot.Position.Y + robot.Velocity.Y + _mapHeight) % _mapHeight;
        robot.Position = robot.Position with { X = x, Y = y };
    }
    
    // ReSharper disable once UnusedMember.Local
    private static void PrintMap(List<Robot> robots, int secs)
    {
        Console.WriteLine($"Map at: {secs} secs");
        _map = new int[_mapWidth, _mapHeight];
        
        foreach (var robot in robots)
        {
            _map[robot.Position.X, robot.Position.Y]++;
        }

        for (var y = 0; y < _mapHeight; y++)
        {
            for (var x = 0; x < _mapWidth; x++)
            {
                Console.Write(_map[x, y]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    
    private static Point GetPoint(string input)
    {
        var p = input.Split("=");
        var x = int.Parse(p[1].Split(",")[0]);
        var y = int.Parse(p[1].Split(",")[1]);
        return new Point(x, y);
    }
    
    private static void DataSetup(string filename)
    {
        if (filename != "input.txt") return;
        _mapHeight = 103;
        _mapWidth = 101;
        _map = new int[101,103];
        _filename = filename;
    }
    
    private class Robot(Point position, Point velocity)
    {
        public Point Position { get; set; } = position;
        public Point Velocity { get; set; } = velocity;
    }
}