using System.Collections;
using AocHelper;

namespace _05;

internal static class Program
{
    private const string ExpectedPartOne = "PTWLTDSJV";
    private const string ExpectedPartTwo = "WZMFVGGZP";
    private const string Day = "_05";

    public static int Main(string[] args)
    {
        var filename = "input_05.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var blocks = File.ReadAllText($"{filename}").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        
        List<(int, int, int)> instructions = [];
        foreach (var line in blocks[1].Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var preparedLine = line.Replace("move ", "").Replace("from ", ",").Replace("to ", ",");
            var (move, from, to) = preparedLine.Split(',').ToIntTupleTriple();
            instructions.Add((move, from - 1, to - 1));
        }

        var resultPartOne = PartOne(blocks[0], instructions);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(blocks[0], instructions);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static string PartOne(string stackInput, List<(int, int, int)> instructions)
    {
        var stacks = InitializeStacks(stackInput);
        foreach (var (move, from, to) in instructions)
        {
            var count = 0;
            while (count++ < move)
            {
                stacks[to].Push(stacks[from].Pop());
            }
        }
        
        return GetTopCrates(stacks);
    }

    private static string PartTwo(string stackInput, List<(int, int, int)> instructions)
    {
        var stacks = InitializeStacks(stackInput);
        var holdingStack = new Stack();
        foreach (var (move, from, to) in instructions)
        {
            var count = 0;
            while (count++ < move)
                holdingStack.Push(stacks[from].Pop());

            while (holdingStack.Count > 0)
                stacks[to].Push(holdingStack.Pop());
        }
        
        return GetTopCrates(stacks);
    }

    private static Stack[] InitializeStacks(string stackInput)
    {
        var lines = stackInput.Split('\n', StringSplitOptions.RemoveEmptyEntries).Reverse().ToArray();
        var stacks = new Stack[lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length];
        for (var i = 0; i < stacks.Length; i++)
            stacks[i] = [];

        foreach (var line in lines.Skip(1))
        {
            var index = 0;
            for (var i = 1; i < line.Length; i += 4)
            {
                var ch = line[i];
                if (ch != ' ')
                    stacks[index].Push(ch);
                index++;
            }
        }

        return stacks;
    }
    
    private static string GetTopCrates(Stack[] stacks)
    {
        var output = "";
        foreach (var stack in stacks)
        {
            output += stack.Pop();
        }

        return output;
    }
}