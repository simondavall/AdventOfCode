using System.Numerics;

namespace _24;

internal static partial class Program
{
    private static string _filename = null!;
    private static double _lowerLimit;
    private static double _upperLimit;
    private static readonly List<Hailstone> Hailstones = [];

    internal static void Main()
    {
        SetConfig("input.txt");
        var input = File.ReadAllText($"{_filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in input)
        {
            var (position, velocity) = line.Split('@').ToTuplePair();
            Hailstones.Add(Hailstone.Create(position, velocity));
        }

        Console.WriteLine($"Part 1: {PartOne()}");
        
        Console.WriteLine($"Part 2: {PartTwo(Hailstones)}");
    }

    private static long PartOne()
    {
        long tally = 0;
        
        for (var i = 0; i < Hailstones.Count - 1; i++)
            for (var j = i + 1; j < Hailstones.Count; j++)
            {
                if (Intersects2D(Hailstones[i], Hailstones[j]))
                    tally++;
            }

        return tally;
    }
    
    private static long PartTwo(List<Hailstone> stones)
    {
        var (x, y, z) = FindRockStartingPosition(stones);
        return x + y + z;
    }

    private static List<(long, long)> SolveEquations(ProjectedHailstone first, ProjectedHailstone second)
    {
        return SolveEquations(first.a, first.b, first.c, second.a, second.b, second.c);
    }
    
    // Solve two linear equations for x and y
    // Equations of the form: ax + by = c
    private static List<(long, long)> SolveEquations(BigInteger a1, BigInteger b1, BigInteger c1, BigInteger a2, BigInteger b2, BigInteger c2)
    {
        // a1*x + b1*y = c1
        // a2*x + b2*y = c2

        // b2*a1*x + b2*b1*y = b2*c1
        // b1*a2*x + b2*b1*y = b1*c2

        // x(b2*a1 - b1*a2) = b2*c1 - b1*c2

        // x = (b2*c1 - b1*c2) / (b2*a1 - b1*a2)

        // let d = (b2*a1 - b1*a2)

        // if d == 0: lines are parallel and there is no solution
        
        var d = b2 * a1 - b1 * a2;
        if (d == BigInteger.Zero) 
            return [];
        var x = (b2 * c1 - b1 * c2) / d;
        var y = (c2 * a1 - c1 * a2) / d;
        return [((long)x, (long)y)];
    }

    private static (long, long, long) FindRockStartingPosition(List<Hailstone> stones)
    {
        var result1 = FindRockPositionAndVelocity(stones, component: 2);
        var result2 = FindRockPositionAndVelocity(stones, component: 0);
        var result3 = FindRockPositionAndVelocity(stones, component: 1);
    
        if (result1.Count == 0 || result2.Count == 0 || result3.Count == 0)
            throw new Exception("Missing some results");
        
        var (x1, y1) = result1[0].position;
        var (y2, z1) = result2[0].position;
        var (x2, z2) = result3[0].position;
        var (vx1, vy1) = result1[0].velocity;
        var (vy2, vz1) = result2[0].velocity;
        var (vx2, vz2) = result3[0].velocity;
    
        if (y1 != y2 || x1 != x2 || z1 != z2)
        {
            throw new Exception("Expected positions to match");
        }
        
        if (vy1 != vy2 || vx1 != vx2 || vz1 != vz2)
        {
            throw new Exception("Expected velocities to match");
        }
        
        Console.WriteLine($"Found rock position and velocity: {x1},{y1},{z1} @ {vx1},{vy1},{vz1}");
        
        return (x1, y1, z1);
    }

    private static List<((long, long) position, (int, int) velocity)> FindRockPositionAndVelocity(List<Hailstone> stones, int component)
    {
        var maxValue = 300;
        var minResultCount = 3;
        foreach (var vx in Range(-maxValue, maxValue * 2))
        {
            foreach (var vy in Range(-maxValue, maxValue * 2))
            {
                var deltaV = (vx, vy);
                var matchingPositions = new HashSet<(long, long)>();
                var resultCount = 0;
                
                IEnumerable<(long x, long y)> intersections = ProcessEquations(stones, component, deltaV);

                foreach (var intersection in intersections)
                {
                    matchingPositions.Add(intersection);
                    resultCount++;
                    if (resultCount >= minResultCount)
                        break;
                }
                
                // We need exactly 1 position with at least minResultCount matches
                if (matchingPositions.Count == 1 && resultCount >= Math.Min(minResultCount, stones.Count / 2)) 
                {
                    return [(matchingPositions.First(), (-deltaV.vx, -deltaV.vy))];
                }
            }
        }
    
        return [];
    }
    
    private static IEnumerable<(long, long)> ProcessEquations(List<Hailstone> stones, int projectedComponent, (int, int) deltaSpeed)
    {
        for (var i = 0; i < stones.Count - 1; i++)
        {
            for (var j = i + 1; j < stones.Count; j++)
            {
                var firstStone = stones[i].Projected(projectedComponent).AddingVelocity(deltaSpeed);
                var secondStone = stones[j].Projected(projectedComponent).AddingVelocity(deltaSpeed);
                var intersection = SolveEquations(firstStone, secondStone);
                if (intersection.Count == 0)
                    continue;
    
                var (x, y) = intersection[0];
                
                if (List([firstStone, secondStone]).All(p => IsConvergent(p.Position.y, y, p.Velocity.y)))
                    yield return (x, y);
            }
        }
    }
    
    private static bool Intersects2D(Hailstone h1, Hailstone h2)
    {
        if (h1.x == h2.x && h1.y == h2.y) // already at same spot
            return true;

        if (Math.Abs(h1.Gradient - h2.Gradient) < 0.01) // are parallel
            return false;
        
        // intersection
        var y1 = h1.y - h1.x * h1.Gradient;
        var y2 = h2.y - h2.x * h2.Gradient;
        var x = (y2 - y1) / (h1.Gradient - h2.Gradient);
        var y = y1 + h1.Gradient * x;

        if (!IsConvergent(h1, x, y) || !IsConvergent(h2, x, y)) // crossed in the past
            return false;
        
        return IsInBounds(x, y); 
    }

    private static bool IsInBounds(double x, double y)
    {
        return x >= _lowerLimit && x <= _upperLimit && y >= _lowerLimit && y <= _upperLimit;
    }
    
    private static bool IsConvergent(Hailstone h, double x, double y)
    {
        return IsConvergent(h.x, x, h.vx) && IsConvergent(h.y, y, h.vy);
    }

    private static bool IsConvergent(long a, double b, double grad)
    {
        return a > b && grad < 0 || a < b && grad > 0;
    }
    
    private static void SetConfig(string filename)
    {
        _filename = filename;
        if (filename == "input.txt")
        {
            _lowerLimit = 200000000000000;
            _upperLimit = 400000000000000;
            return;
        }

        _lowerLimit = 7;
        _upperLimit = 27;
    }
}