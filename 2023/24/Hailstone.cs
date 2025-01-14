// ReSharper disable InconsistentNaming
namespace _24;

internal static partial class Program
{
    internal readonly struct Hailstone
    {
        private Hailstone((long x, long y, long z) p, (long x, long y, long z) v) : this()
        {
            x = p.x;
            y = p.y;
            z = p.z;
            vx = v.x;
            vy = v.y;
            vz = v.z;
        }

        public long x { get; }
        public long y { get; }
        private long z { get; }

        public long vx { get; }
        public long vy { get; }
        private long vz { get; }

        public double Gradient => (double)vy / vx;
        
        public static Hailstone Create(string position, string velocity)
        {
            var p = position.Split(',').ToLongTupleTriple();
            var v = velocity.Split(',').ToLongTupleTriple();
            return new Hailstone(p, v);
        }
        
        public override string ToString() => $"{(x, y, z)}-{(vx, vy, vz)}";
        
        public ProjectedHailstone Projected(int component)
        {
            // Take all components except the specified component
            return component switch
            {
                0 => new ProjectedHailstone((y, z), (vy, vz)),
                1 => new ProjectedHailstone((x, z), (vx, vz)),
                2 => new ProjectedHailstone((x, y), (vx, vy)),
                _ => throw new ArgumentException("component", $"Invalid component: {component}")
            };
        }
    }
    
    internal readonly struct ProjectedHailstone((long x, long y) position, (long x, long y)  velocity)
    {
        public long a { get; } = velocity.y;
        public long b { get; } = -velocity.x;
        public long c { get; } = velocity.y * position.x - velocity.x * position.y;

        public readonly (long x, long y) Position = position;
        
        public readonly (long x, long y) Velocity = velocity;
        
        /**
         * Adds the specified velocity to this projected stone
         */
        public ProjectedHailstone AddingVelocity((long x, long y) delta)
        {
            return new ProjectedHailstone(Position,  (Velocity.x + delta.x, Velocity.y + delta.y));
        }

        public override string ToString()
        {
            return $"{Position} @ {Velocity}";
        }
    }

}