using System.Drawing;

namespace _16;

public struct Vector
{
    public Vector(Point point, Direction direction, long tally)
    {
        Point = point;
        Direction = direction;
        Tally = tally;
    }
    
    public Vector(Vector v)
    {
        Point = v.Point;
        Direction = v.Direction;
        Tally = v.Tally;
    }

    public Point Point { get; set; }
    public Direction Direction { get; set; }
    public long Tally { get; set; }
}

public enum Direction
{
    East,
    South,
    West,
    North
}