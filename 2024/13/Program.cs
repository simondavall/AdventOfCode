// ReSharper disable CompareOfFloatsByEqualityOperator
namespace _13;

internal static class Program
{
    internal static void Main()
    {
        var machines = File.ReadAllText("input.txt")
            .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
            .ToMachineList();
        
        Console.WriteLine($"Part 1: {PartOne(machines)}");
        Console.WriteLine($"Part 2: {PartTwo(machines)}");
    }

    private static long PartOne(Machine[] machines)
    {
        long tally = 0;
        foreach(var m in machines)
        {
            var result = CalcAb(m);
            tally += result.a * 3 + result.b * 1;
        }
        
        return tally;
    }

    private static long PartTwo(Machine[] machines)
    {
        long tally = 0;

        foreach(var m in machines)
        {
            m.Prize.X += 10_000_000_000_000;
            m.Prize.Y += 10_000_000_000_000;

            var result = CalcAb(m);
            tally += result.a * 3 + result.b * 1;
        }
        
        return tally;
    }
    
    private static (long a, long b) CalcAb(Machine m)
    {
        var top = (m.Prize.X * m.B.Y) - (m.B.X * m.Prize.Y);
        var bottom = m.A.X * m.B.Y - m.A.Y * m.B.X;
        if (bottom == 0)
            throw new DivideByZeroException();
        
        var a = (double)top / bottom;
        
        if (a != (long)a) 
            return (0, 0);

        var b = (m.Prize.X - m.A.X * a) / m.B.X;
        return b == (long)b ? ((long)a, (long)b) : (0, 0);
    }
    
    private static Machine[] ToMachineList(this string[] map)
    {
        List<Machine> machines = [];
        machines.AddRange(map.Select(Machine.CreateMachine));

        return machines.ToArray();
    }
}