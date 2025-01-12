// ReSharper disable InconsistentNaming
namespace _22;

internal static partial class Program
{
    private static readonly List<Brick> Bricks = [];
    
    internal static void Main()
    {
        const string filename = "input.txt";
        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in input)
        {
            var (start, end) = line.Split('~').ToTuplePair();
            Bricks.Add(Brick.Create(start, end));
        }
        Bricks.Sort(new SortBrickByElevation());
        
        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }
    
    private static long PartOne()
    {
        long tally = 0;

        MakeFall();
        
        HashSet<(int x, int y, int z)> brickSet = [];
        foreach (var b in Bricks)
            foreach (var x in b.xValues)
                foreach (var y in b.yValues)
                    brickSet.Add((x, y, b.ez));
        
        for (var i = 0; i < Bricks.Count; i++)
        {
            List<Brick> bricksCopy = [..Bricks];
            var b = bricksCopy[i];
            bricksCopy.RemoveAt(i);
            RemoveFromHashset(b, brickSet);
            if (!HasFall(bricksCopy, brickSet))
            {
                tally++;
            }
            AddToHashset(b, brickSet);
        }
  
        return tally;
    }
    
    private static long PartTwo()
    {
        HashSet<(int x, int y, int z)> brickSet = [];
        foreach (var b in Bricks)
            foreach (var x in b.xValues)
                foreach (var y in b.yValues)
                    brickSet.Add((x, y, b.ez));
        
        long tally = 0;
        
        for (var i = 0; i < Bricks.Count; i++)
        {
            List<Brick> bricksCopy = [];
            foreach (var brick in Bricks)
            {
                bricksCopy.Add(brick.Clone());
            }
            var b = bricksCopy[i];
            bricksCopy.RemoveAt(i);
            
            HashSet<(int x, int y, int z)> bricksSetCopy = [..brickSet];
            RemoveFromHashset(b, bricksSetCopy);
            
            var fallen = HowManyFall(bricksCopy, bricksSetCopy);
            
            //Console.WriteLine($"Brick {i} causes {fallen} to fall.");
            tally += fallen;
        }

        return tally;
    }

    private static void MakeFall()
    {
        HashSet<(int x, int y, int z)> brickSet = [];
        foreach (var b in Bricks)
            foreach (var x in b.xValues)
                foreach (var y in b.yValues)
                    brickSet.Add((x, y, b.ez));
        
        var fell = true;
        var currentBrick = 0;
        while (fell)
        {
            fell = false;

            for (var i = currentBrick; i < Bricks.Count; i++)
            {
                currentBrick = i;
                var b = Bricks[i];
                if (IsSupported(b, brickSet)) 
                    continue;
                
                fell = true;
                UpdateHashset(brickSet, b);
                b.MoveDown();
                Bricks[i] = b;
                break;
            }
        }
    }
    
    private static int HowManyFall(List<Brick> bricks, HashSet<(int x, int y, int z)> brickSet)
    {
        HashSet<int> fallenBricks = [];
        
        var fell = true;
        var currentBrick = 0;
        while (fell)
        {
            fell = false;

            for (var i = currentBrick; i < bricks.Count; i++)
            {
                currentBrick = i;
                var b = bricks[i];
                if (IsSupported(b, brickSet)) 
                    continue;
                
                fell = true;
                UpdateHashset(brickSet, b);
                b.MoveDown();
                bricks[i] = b;
                fallenBricks.Add(currentBrick);
                break;
            }
        }

        return fallenBricks.Count;
    }
    
    private static bool HasFall(List<Brick> bricks, HashSet<(int x, int y, int z)> brickSet)
    {
        foreach (var b in bricks)
        {
            var supported = false;
            foreach (var x in b.xValues)
            {
                foreach (var y in b.yValues)
                {
                    if (!IsSupportedBy(brickSet, x, y, b.sz - 1)) 
                        continue;
                    
                    supported = true;
                    break;
                }
                if (supported)
                    break;
            }
            if (!supported)
                return true;
        }

        return false;
    }

    private static bool IsSupported(Brick b, HashSet<(int x, int y, int z)> brickSet)
    {
        var supported = false;
        foreach (var x in b.xValues)
        {
            foreach (var y in b.yValues)
            {
                if (IsSupportedBy(brickSet, x, y, b.sz - 1))
                {
                    supported = true;
                    break;
                }
            }

            if (supported)
                break;
        }

        return supported;
    }
    
    private static bool IsSupportedBy(HashSet<(int x, int y, int z)> bricksSet, int x, int y, int z)
    {
        return z <= 0 || bricksSet.Contains((x, y, z));
    }

    private static void UpdateHashset(HashSet<(int x, int y, int z)> set, Brick b)
    {
        foreach (var x in b.xValues)
            foreach (var y in b.yValues)
            {
                set.Remove((x, y, b.ez));
                set.Add((x, y, b.ez - 1));
            }
    }

    private static void RemoveFromHashset(Brick b, HashSet<(int x, int y, int z)> set)
    {
        foreach (var x in b.xValues)
            foreach (var y in b.yValues)
            {
                set.Remove((x, y, b.ez));
            }
    }
    
    private static void AddToHashset(Brick b, HashSet<(int x, int y, int z)> set)
    {
        foreach (var x in b.xValues)
            foreach (var y in b.yValues)
            {
                set.Add((x, y, b.ez));
            }
    }
    
    internal struct Brick() : IEquatable<Brick>
    {
        private Brick((int x, int y, int z) s, (int x, int y, int z) e) : this()
        {
            sx = s.x;
            sy = s.y;
            sz = s.z;
            ex = e.x;
            ey = e.y;
            ez = e.z;
            (sx, ex) = (Math.Min(sx, ex), Math.Max(sx, ex));
            (sy, ey) = (Math.Min(sy, ey), Math.Max(sy, ey));
            (sz, ez) = (Math.Min(sz, ez), Math.Max(sz, ez));
            xValues = Range(sx, ex - sx + 1).ToArray();
            yValues = Range(sy, ey - sy + 1).ToArray();
        }

        private int sx { get; init;  }
        private int sy { get; init;  }
        public int sz { get; private set;  }

        private int ex { get; init;  }
        private int ey { get; init;  }
        public int ez { get; private set;  }
        
        public int[] xValues { get; private init; }
        public int[] yValues { get; private init; }

        public void MoveDown()
        {
            sz -= 1;
            ez -= 1;
        }

        public Brick Clone()
        {
            return new Brick
            {
                sx = ex, sy = sy, sz = sz,
                ex = ex, ey = ey, ez = ez,
                xValues = xValues, yValues = yValues
            };
        }
        
        public static Brick Create(string start, string end)
        {
            var s = start.Split(',').ToIntTupleTriple();
            var e = end.Split(',').ToIntTupleTriple();
            return new Brick(s, e);
        }
        
        public override string ToString() => $"{(sx, sy, sz)}-{(ex, ey, ez)}";

        public bool Equals(Brick other)
        {
            return sx == other.sx && sy == other.sy && sz == other.sz 
                   && ex == other.ex && ey == other.ey && ez == other.ez;
        }

        public override bool Equals(object? obj)
        {
            return obj is Brick other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(sx, sy, sz, ex, ey, ez);
        }
    }

    private class SortBrickByElevation : IComparer<Brick>
    {
        public int Compare(Brick a, Brick b)
        {
            return a.sz.CompareTo(b.sz);
        }
    }
}