namespace _13;

internal class Machine
{
    public Button A { get; set; } = null!;
    public Button B { get; set; } = null!;
    public Prize Prize { get; set; } = null!;
    
    internal static Machine CreateMachine(string input)
    {
        var machine = new Machine();
        var lines = input.Split("\n");
        
        machine.A = new Button(GetX(lines[0]), GetY(lines[0]));
        machine.B = new Button(GetX(lines[1]), GetY(lines[1]));
        machine.Prize = new Prize(GetX(lines[2]), GetY(lines[2]));

        return machine;
    }

    private static long GetX(string input)
    {
        var i = input.IndexOf('X') + 2;
        var j = input.IndexOf(',');
        return long.Parse(input.Substring(i, j - i));
    }
    
    private static long GetY(string input)
    {
        var i = input.IndexOf('Y') + 2;
        return long.Parse(input[i..]);
    }
}

internal class Prize(long x, long y)
{
    public long X { get; set; } = x;
    public long Y { get; set; } = y;
}

internal class Button(long x, long y)
{
    public long X { get; } = x;
    public long Y { get; } = y;
}