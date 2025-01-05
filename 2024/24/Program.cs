using System.Diagnostics;

namespace _24;

internal static class Program
{
    private static readonly Dictionary<string, int> Wires = [];
    private static Dictionary<string, (string input1, string input2, string gate)> _connections = [];
    
    internal static void Main()
    {
        var inputs = File.ReadAllText($"input.txt").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var wire in inputs[0].Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var items = wire.Split(": ", StringSplitOptions.RemoveEmptyEntries);
            Wires.Add(items[0], int.Parse(items[1]));
        }
        
        var connArray = inputs[1].Split('\n', StringSplitOptions.RemoveEmptyEntries);
        _connections = connArray
            .Select(connection => connection.Split(' '))
            .ToDictionary(output => output[4], formula => (formula[0], formula[2], formula[1]));

        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }

    private static long PartOne()
    {
        var zWires = new List<int>();
        var i = 0;
        
        while (true)
        {
            var key = Wire('z', i);
            if (!_connections.ContainsKey(key))
                break;
            zWires.Add(GetOutput(key));
            i++;
        }

        zWires.Reverse();
        return Convert.ToInt64(string.Join("", zWires), 2);
    }
    
    private static long PartTwo()
    {
        List<string> swappedWires = [];

        var repeat = 0;
        while (repeat++ < 4)
        {
            var foundSwap = false;
            var errorLine = FindError();
            foreach (var x in _connections.Keys)
            {
                foreach (var y in _connections.Keys)
                {
                    if (x == y)
                        continue;
                    (_connections[x], _connections[y]) = (_connections[y], _connections[x]);
                    if (FindError() > errorLine)
                    {
                        swappedWires.Add(x);
                        swappedWires.Add(y);
                        foundSwap = true;
                        break;
                    }
                    (_connections[x], _connections[y]) = (_connections[y], _connections[x]);
                }

                if (foundSwap)
                    break;
            }
        }
        
        swappedWires.Sort();
        Console.WriteLine(string.Join(",", swappedWires));
            
        return 0;
    }

    private static int GetOutput(string wire)
    {
        if (Wires.TryGetValue(wire, out var value))
            return value;

        var (input1, input2, gate) = _connections[wire];
        Wires[wire] = GetOutput(gate, GetOutput(input1), GetOutput(input2));

        return Wires[wire];
    }

    private static int GetOutput(string gate, int input1, int input2)
    {
        var output = gate switch
        {
            "AND" => input1 & input2,
            "OR" => input1 | input2,
            "XOR" => input1 ^ input2,
            _ => throw new UnreachableException($"Oops, unknown gate {gate}")
        };
        
        return output;
    }
    
    private static int FindError()
    {
        var i = 0;

        while (true)
        {
            if (!CheckGraph(i))
                break;
            i++;
        }

        return i;
    }
    
    private static bool CheckGraph(int n)
    {
        return CheckZWires(Wire('z', n), n);
    }
    
    private static bool CheckZWires(string wire, int n)
    {
        if (!_connections.TryGetValue(wire, out var value))
            return false;

        var (input1, input2, gate) = value;
        if (gate != "XOR")
            return false;
        if (n == 0)
            return SortedPair(input1, input2) == ("x00", "y00");

        return (CheckXor(input1, n) && CheckCarryBit(input2, n)) 
               || (CheckXor(input2, n) && CheckCarryBit(input1, n));
    }
    
    private static bool CheckXor(string wire, int n)
    {
        if (!_connections.TryGetValue(wire, out var value))
            return false;
        
        var (input1, input2, gate) = value;
        if (gate != "XOR")
            return false;

        return SortedPair(input1, input2) == (Wire('x', n), Wire('y', n));
    }

    private static bool CheckCarryBit(string wire, int n)
    {
        if (!_connections.TryGetValue(wire, out var value))
            return false;
        
        var (input1, input2, gate) = value;
        if (n == 1)
        {
            if (gate != "AND")
                return false;
            return SortedPair(input1, input2) == ("x00", "y00");
        }

        if (gate != "OR")
            return false;
        
        return (CheckCarry(input1, n - 1) && CheckCarryOver(input2, n - 1)) 
               || (CheckCarry(input2, n - 1) && CheckCarryOver(input1, n -1));
    }

    private static bool CheckCarry(string wire, int n)
    {
        if (!_connections.TryGetValue(wire, out var value))
            return false;
        
        var (input1, input2, gate) = value;
        if (gate != "AND")
            return false;

        return SortedPair(input1, input2) == (Wire('x', n), Wire('y', n));
    }

    private static bool CheckCarryOver(string wire, int n)
    {
        if (!_connections.TryGetValue(wire, out var value))
            return false;
        
        var (input1, input2, gate) = value;
        if (gate != "AND")
            return false;

        return (CheckXor(input1, n) && CheckCarryBit(input2, n))
               || (CheckXor(input2, n) && CheckCarryBit(input1, n));
    }
    
    private static string Wire(char ch, int n)
    {
        return ch + n.ToString("00");
    }

    private static (T, T) SortedPair<T>(T item1, T item2) where T: IComparable<T>
    {
        return item1.CompareTo(item2) <= 0 ? (item1, item2) : (item2, item1);
    }
 }