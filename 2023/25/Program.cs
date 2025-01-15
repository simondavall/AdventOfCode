namespace _25;

internal static partial class Program
{
    private static readonly Dictionary<string, List<string>> Connections = [];
    private static string[] _components = [];
    
    internal static void Main()
    {
        const string filename = "input.txt";
        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);

        HashSet<string> components = [];
        foreach (var line in input)
        {
            var (main, rest) = line.Split(':').ToTuplePair();
            components.Add(main);
            foreach (var comp in rest.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                components.Add(comp);
                if (!Connections.TryAdd(main, [comp]))
                    Connections[main].Add(comp);
                if (!Connections.TryAdd(comp, [main]))
                    Connections[comp].Add(main);
            }
        }
        _components = components.ToArray();
        
        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }
    
    private static long PartOne()
    {
        var loop = 0;

        List<(string, string)> wiresToIgnore = [];

        // Find three most active wire connections.
        // These will be ignored when calculating the final connected networks
        // which should result in only two networks
        while (loop++ < 3)
        {
            var pathLengths = GetPathLengths(wiresToIgnore);
            wiresToIgnore.Add(GetMostActiveWire(pathLengths));
        }
        
        var connectedPaths = GetConnectedNetworks(wiresToIgnore);

        if (connectedPaths.Count != 2)
            throw new Exception($"Expected two networks, found {connectedPaths.Count}");
        
        long tally = 1;
        foreach (var path in connectedPaths)
        {
            tally *= path.Count;
        }
        
        return tally;
    }

    private static HashSet<List<string>>  GetConnectedNetworks(List<(string, string)> wiresToIgnore)
    {
        HashSet<List<string>> connectedPaths = [];
        HashSet<string> seen = [];
        foreach (var component in _components)
        {
            HashSet<string> connectedComponents = [];
            var q = new PriorityQueue<string, int>();
            q.Enqueue(component, 0);
        
            while (q.Count > 0)
            {
                if (!q.TryDequeue(out var next, out var priority))
                    continue;
                
                connectedComponents.Add(next);
                seen.Add(next);
        
                foreach (var c in Connections[next])
                {
                    if (!seen.Contains(c) && !wiresToIgnore.Contains((next, c).Sorted()))
                        q.Enqueue(c, priority + 1);
                }
            }
        
            if (connectedComponents.Count > 1)
                connectedPaths.Add(connectedComponents.ToSortedList());
        }

        return connectedPaths;
    }
    
    private static Dictionary<string, List<(string, string)>> GetPathLengths(List<(string, string)> wiresToIgnore)
    {
        Dictionary<string, List<(string, string)>> pathLengths = [];
        HashSet<string> seen = [];
        
        foreach (var component in _components)
        {
            List<(string, string)> maxPath = [];
            seen.Clear();

            var q = new PriorityQueue<(string, List<(string, string)>), int>();
            q.Enqueue((component, []), 0);

            while (q.Count > 0)
            {
                var (next, path) = q.Dequeue();
                if (!seen.Add(next))
                    continue;
                
                if (path.Count > maxPath.Count)
                    maxPath = path;

                foreach (var c in Connections[next])
                {
                    var wire = (next, c).Sorted();
                    if (!seen.Contains(c) && !wiresToIgnore.Contains(wire))
                    {
                        q.Enqueue((c, [..path, wire]), path.Count + 1);
                    }
                }
            }
            
            pathLengths.Add(component, maxPath);
        }
        
        return pathLengths;
    }
    
    private static (string, string) GetMostActiveWire(Dictionary<string, List<(string, string)>> pathLengths)
    {
        Dictionary<(string, string), int> wiresCount = [];
        
        foreach (var wires in pathLengths.Values)
            foreach (var wire in wires)
                if (!wiresCount.TryAdd(wire, 1))
                {
                    wiresCount[wire]++;
                }
        
        var maxCount = wiresCount.Select(x => x.Value).Max();
        
        return wiresCount.First(x => x.Value == maxCount).Key;
    }
    
    private static (string, string) Sorted(this (string a, string b) t)
    {
        return string.Compare(t.a, t.b, StringComparison.Ordinal) > 0 ? (t.b, t.a) : t;
    }
    
    private static long PartTwo()
    {
        long tally = 0;
        return tally;
    }
}