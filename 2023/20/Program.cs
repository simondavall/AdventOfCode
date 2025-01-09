using System.Diagnostics;
using System.Numerics;

namespace _20;

internal static partial class Program
{
    private static readonly Dictionary<string, Module> Modules = [];
    
    internal static void Main()
    {
        const string filename = "input.txt";
        var moduleConfig = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var m in moduleConfig)
        {
            var (module, targets) = m.Split(" -> ").ToTuplePair();
            var targetList = targets.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            switch (module[0])
            {
                case '%':
                    Modules.Add(module[1..], new FlipFlip {Name = module[1..], Targets = targetList});
                    break;
                case '&':
                    Modules.Add(module[1..], new Conjunction{Name = module[1..], Targets = targetList});
                    break;
                default:
                    Modules.Add(module, new Broadcast {Name = module, Targets = targetList});
                    break;
            }
        }

        foreach (var module in Modules.Values.Where(x => x is Conjunction))
            foreach (var target in Modules.Values.Where(x => x.Targets.Contains(module.Name)).Select(x => x.Name))
                module.States.Add(target, false);
        
        Console.WriteLine($"Part 1: {PartOne()}");
        Console.WriteLine($"Part 2: {PartTwo()}");
    }
    
    private static long PartOne()
    {
        (long low, long high) pulseCounts = (0, 0);
        
        foreach (var _ in Range(1000))
        {
            pulseCounts.low++;
            
            var q = new Queue<(string, string, bool)>();
            foreach (var target in Modules["broadcaster"].Targets)
                q.Enqueue(("broadcaster", target, false));

            while (q.Count > 0)
            {
                var (origin, target, pulse) = q.Dequeue();
                
                if (pulse)
                    pulseCounts.high++;
                else
                    pulseCounts.low++;
                
                if(!Modules.TryGetValue(target, out var module))
                    continue;
                
                if (module is FlipFlip)
                {
                    if (pulse) 
                        continue;
                    
                    module.State = !module.State;
                    var outgoing = module.State;
                    foreach (var moduleTarget in module.Targets)
                        q.Enqueue((module.Name, moduleTarget, outgoing));
                }
                else
                {
                    module.States[origin] = pulse;
                    var outgoing = module.States.Values.Any(x => !x);
                    foreach (var moduleTarget in module.Targets)
                        q.Enqueue((module.Name, moduleTarget, outgoing));
                }
            }
        }
        
        return pulseCounts.low * pulseCounts.high;
    }

    private static long PartTwo()
    {
        var modules = new Dictionary<string, Module>(Modules);
        var feed = modules.Values.First(x => x.Targets.Contains("rx")).Name;
        Dictionary<string, long> cycleLengths = [];
        Dictionary<string, long> firstSeenPresses = [];
        Dictionary<string, int> visited = [];

        foreach (var seenItem in modules.Values.Where(x => x.Targets.Contains(feed)).Select(x => x.Name).ToArray())
            visited.TryAdd(seenItem, 0);
        
        var buttonPresses = 0;
        var isDataCollected = false;

        while (true)
        {
            if (isDataCollected)
                break;
            
            buttonPresses++;
            
            var q = new Queue<(string, string, bool)>();
            foreach (var target in Modules["broadcaster"].Targets)
                q.Enqueue(("broadcaster", target, false));
        
            while (q.Count > 0)
            {
                var (origin, target, pulse) = q.Dequeue();
                
                if(!Modules.TryGetValue(target, out var module))
                    continue;
        
                if (module.Name == feed && pulse)
                {
                    visited[origin]++;
                    if (visited[origin] == 1)
                        firstSeenPresses.Add(origin, buttonPresses);
                    else
                    {
                        if (!cycleLengths.TryAdd(origin, buttonPresses - firstSeenPresses[origin]))
                            if (buttonPresses != (visited[origin] - 1) * cycleLengths[origin] + firstSeenPresses[origin])
                                throw new UnreachableException("Ooops, something went wrong");
                    }
                    
                    if (visited.All(x => x.Value > 2))
                        isDataCollected = true;
                }
        
                if (module is FlipFlip)
                {
                    if (pulse) 
                        continue;
                    
                    module.State = !module.State;
                    var outgoing = module.State;
                    foreach (var moduleTarget in module.Targets)
                        q.Enqueue((module.Name, moduleTarget, outgoing));
                }
                else
                {
                    module.States[origin] = pulse;
                    var outgoing = module.States.Values.Any(x => !x);
                    foreach (var moduleTarget in module.Targets)
                        q.Enqueue((module.Name, moduleTarget, outgoing));
                }
            }
        }

        long tally = 1;
        foreach (var cycleLength in cycleLengths.Values)
            tally *= cycleLength / (long)BigInteger.GreatestCommonDivisor(tally, cycleLength);
        
        return tally;
    }
}