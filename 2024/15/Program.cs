using System.Drawing;

namespace _15;

internal static class Program
{
    private static int _mapHeight;
    private static int _mapWidth;

    internal static void Main()
    {
        const string aocDay = "15";
        const string aocYear = "2024";
        const string path = $"/home/sdv/Documents/Projects/Aoc/{aocYear}/{aocDay}/";
        
        const string filename = "input.txt";
        var input = File.ReadAllText($"{path}{filename}").Split("\n\n");
        
        var map = input[0].Split('\n');
        _mapHeight = map.Length;
        _mapWidth = map[0].Length;
        
        var instructions = GetInstructionSet(input[1]);
        
        Console.WriteLine($"Part 1: {PartOne(map, instructions)}");
        Console.WriteLine($"Part 2: {PartTwo(map, instructions)}");
    }

    private static long PartOne(string[] map, char[] instructions)
    {
        var charMap = map.To2DCharArray();
        var currentPosition = FindStartingPosition(charMap);
        var index = 0;
        
        while (index < instructions.Length)
        {
            var nextInstruction = instructions[index];
            currentPosition = Robot.Move(charMap, nextInstruction, currentPosition);
            index++;
        }
        
        return CalcTally(charMap);
    }
    
    private static long PartTwo(string[] map, char[] instructions)
    {
        var charMap = map.ToExpanded2DCharArray();
        var currentPosition = FindStartingPosition(charMap);
        var index = 0;

        //PrintMap(charMap, 0, ' ');
        while (index < instructions.Length)
        {
            var nextInstruction = instructions[index];
            var nextPosition = Robot.Move2(charMap, nextInstruction, currentPosition);
                
            currentPosition = nextPosition;
            index++;
        }
        
        PrintMap(charMap, index, ' ');
        return CalcTally(charMap);
    }

    private static long CalcTally(char[,] map)
    {
        long tally = 0;
        for (var y = 0; y < _mapHeight; y++)
            for (var x = 0; x < _mapWidth; x++)
                if (map[x, y] is 'O' or '[')
                {
                    tally += (100 * y + x);
                }

        return tally;
    }

    private static Point FindStartingPosition(char[,] map)
    {
        for (var y = 0; y < _mapHeight; y++)
        {
            for (var x = 0; x < _mapWidth; x++)
                if (map[x, y] == '@')
                    return new Point(x, y);
        }
        
        return new Point(-1, -1);
    }
    
    private static char[] GetInstructionSet(string input)
    {
        List<char> instructions = [];
        foreach (var line in input.Split('\n'))
        {
            instructions.AddRange(line.ToCharArray());
        }
        return instructions.ToArray();
    }

    private static char[,] To2DCharArray(this string[] input)
    {
        var charArray = new char[input[0].Length, input.Length];
        for(var y = 0; y < input.Length; y++)
        {
            for (var x = 0; x < input[0].Length; x++)
            {
                charArray[x, y] = input[y][x];
            }
        }

        return charArray;
    }
    
    private static char[,] ToExpanded2DCharArray(this string[] input)
    {
        _mapWidth = input[0].Length * 2;
        var charArray = new char[_mapWidth, input.Length];
        for(var y = 0; y < input.Length; y++)
        {
            var newString = input[y].Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.");
            for (var x = 0; x < _mapWidth; x++)
            {
                charArray[x, y] = newString[x];
            }
        }

        return charArray;
    }
    
    private static void PrintMap(char[,] map, int count, char c)
    {
        Console.WriteLine($"Map at {count} secs with instruction {c}");
        for (var y = 0; y < _mapHeight; y++)
        {
            Console.Write($"{y}: ");
            for (var x = 0; x < _mapWidth; x++)
            {
                Console.Write(map[x,y]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}