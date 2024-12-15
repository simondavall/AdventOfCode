using System.Diagnostics;
using System.Drawing;

namespace _15;

public static class Robot
{
    public static Point Move(char[,] map, char instruction, Point p)
    {
        switch (instruction)
        {
            case '^':
                if (CanMove(map, p, '@', 0, -1))
                    return p with { Y = p.Y - 1 };
                break;
                
            case '>':
                if (CanMove(map, p, '@', 1, 0))
                    return p with { X = p.X + 1 };
                break;
            
            case 'v':
                if (CanMove(map, p, '@', 0, 1))
                    return p with { Y = p.Y + 1 };
                break;
            
            case '<':
                if (CanMove(map, p, '@', -1, 0))
                    return p with { X = p.X - 1 };
                break;
            
            default:
                throw new UnreachableException();
        }

        return p;
    }
    
    private static bool CanMove(char[,] map, Point p, char currentChar, int dx, int dy)
    {
        var neighbour = map[p.X + dx, p.Y + dy];
        
        if (neighbour == 'O')
            if (CanMove(map, p with { X = p.X + dx, Y = p.Y + dy }, neighbour, dx, dy))
            {
                map[p.X + dx, p.Y + dy] = currentChar;
                map[p.X, p.Y] = '.';
                return true;
            }
        
        if (neighbour == '#')
            return false;
        
        if (neighbour == '.')
        {
            map[p.X + dx, p.Y + dy] = currentChar;
            map[p.X, p.Y] = '.';
            return true;
        }

        return false;
    }
    
    public static Point Move2(char[,] map, char instruction, Point p)
    {
        switch (instruction)
        {
            case '^':
                if (CanMoveVertical(map, p, -1))
                {
                    MoveVertical(map, p, '@', -1);
                    return p with { Y = p.Y - 1 };
                }
                    
                break;
                
            case '>':
                if (CanMoveHorizontal(map, p, 1))
                {
                    MoveHorizontal(map, p, '@', 1);
                    return p with { X = p.X + 1 };
                }
                break;
            
            case 'v':
                if (CanMoveVertical(map, p, 1))
                {
                    MoveVertical(map, p, '@', 1);
                    return p with { Y = p.Y + 1 };
                }
                    
                break;
            
            case '<':
                if (CanMoveHorizontal(map, p, -1))
                {
                    MoveHorizontal(map, p, '@', -1);
                    return p with { X = p.X - 1 };
                }
                break;
            
            default:
                throw new UnreachableException();
        }

        return p;
    }
    
    private static bool CanMoveHorizontal(char[,] map, Point p, int dx)
    {
        var neighbour = map[p.X + dx, p.Y];

        return neighbour switch
        {
            '[' or ']' when CanMoveHorizontal(map, p with { X = p.X + dx, Y = p.Y }, dx) => true,
            '#' => false,
            '.' => true,
            _ => false
        };
    }
    
    private static void MoveHorizontal(char[,] map, Point p, char currentChar, int dx)
    {
        var neighbour = map[p.X + dx, p.Y];

        switch (neighbour)
        {
            case '[' or ']':
                MoveHorizontal(map, p with { X = p.X + dx, Y = p.Y }, neighbour, dx);
                map[p.X + dx, p.Y] = currentChar;
                map[p.X, p.Y] = '.';
                break;
            case '.':
                map[p.X + dx, p.Y] = currentChar;
                map[p.X, p.Y] = '.';
                break;
        }
    }
    
    private static bool CanMoveVertical(char[,] map, Point p, int dy)
    {
        var neighbour = map[p.X, p.Y + dy];

        return neighbour switch
        {
            '[' => CanMoveBoth(map, p, 1, dy),
            ']' => CanMoveBoth(map, p, -1, dy),
            '#' => false,
            '.' => true,
            _ => false
        };
    }
    
    private static void MoveVertical(char[,] map, Point p, char currentChar, int dy)
    {
        var neighbour = map[p.X, p.Y + dy];

        switch (neighbour)
        {
            case '[':
                MoveBoth(map, p, 1, dy, currentChar, '[', ']');
                break;
            case ']':
                MoveBoth(map, p, -1, dy, currentChar, ']', '[');
                break;
            case '.':
                map[p.X, p.Y + dy] = currentChar;
                map[p.X, p.Y] = '.';
                break;
        }
    }
    
    private static bool CanMoveBoth(char[,] map, Point p, int dx, int dy)
    {
        return CanMoveVertical(map, p with { Y = p.Y + dy }, dy)
               && CanMoveVertical(map, p with { X = p.X + dx, Y = p.Y + dy }, dy);
    }
    
    private static void MoveBoth(char[,] map, Point p, int dx, int dy, char currentChar, char first, char second)
    {
        MoveVertical(map, p with { Y = p.Y + dy }, first, dy);
        MoveVertical(map, p with { X = p.X + dx, Y = p.Y + dy }, second, dy);

        map[p.X, p.Y + dy] = currentChar;
        map[p.X, p.Y] = '.';
    }
}