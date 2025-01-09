namespace _20;

internal abstract class Module
{
    public required string Name { get; init; }
    public string[] Targets { get; init; } = [];
    public bool State { get; set; }
    public Dictionary<string, bool> States { get; set; } = [];
}

internal class FlipFlip : Module
{ }

internal class Conjunction : Module
{ }

internal class Broadcast : Module
{ }