namespace _23;

internal static class Program
{
    private static Dictionary<string, List<string>> _connections = [];
    private static readonly List<string> LanParty = [];

    internal static void Main()
    {
        const string aocDay = "23";
        const string aocYear = "2024";
        const string path = $"/home/sdv/Documents/Projects/Aoc/{aocYear}/{aocDay}/";

        const string filename = "input.txt";
        var input = File.ReadAllText($"{path}{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        _connections = GetConnections(input);
        
        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }

    private static long PartOne()
    {
        var connected = (from x in _connections.Keys
            .Where(x => x[0] == 't') from y in _connections[x] from z in _connections[y] where x != z && _connections[z]
            .Contains(x) select (x, y, z)
            .SortedTriple<string>()).ToList();

        return connected.ToList().Distinct().Count();
    }
    
    private static long PartTwo()
    {
        foreach (var computer in _connections.Keys)
        {
            FindLongestChain(computer, [computer]);
        }
        
        var max = 0;
        var lanParty = string.Empty;
        foreach (var party in LanParty.Where(party => party.Length > max))
        {
            max = party.Length;
            lanParty = party;
        }
        
        return lanParty.Split(',', StringSplitOptions.RemoveEmptyEntries).Length;
    }
    
    private static void FindLongestChain(string computer, List<string> connectedComputers)
    {
        connectedComputers.Sort();
        var key = string.Join(',', connectedComputers);
        if (LanParty.Contains(key))
            return;
        LanParty.Add(key);
        
        foreach (var neighbour in _connections[computer])
        {
            if (connectedComputers.Contains(neighbour))
                continue;
            
            if (connectedComputers.All(connectedComputer => _connections[connectedComputer].Contains(neighbour)))
            {
                connectedComputers.Add(neighbour);
                FindLongestChain(neighbour, connectedComputers);
            }
        }
    }
    
    private static Dictionary<string, List<string>> GetConnections(string[] input)
    {
        Dictionary<string, List<string>> connections = [];
        
        foreach (var c in input)
        {
            var (x, y) = c.Split('-').ToPairedTuple();

            if (!connections.ContainsKey(x))
                connections.Add(x, []);
            if (!connections.ContainsKey(y))
                connections.Add(y, []);

            connections[x].Add(y);
            connections[y].Add(x);
        }
        
        return connections;
    }
    
    private static (T, T) ToPairedTuple<T>(this T[] array)
    {
        return (array[0], array[1]);
    }
    
    private static (T, T, T) SortedTriple<T>(this (T, T, T) tuple) where T: IComparable<T>
    {
        List<T> list = [tuple.Item1, tuple.Item2, tuple.Item3];
        list.Sort();
        return (list[0], list[1], list[2]);
    }
}