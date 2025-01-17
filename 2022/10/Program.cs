using AocHelper;

namespace _10;

internal static class Program
{
    private const long ExpectedPartOne = 14420;
    private const string ExpectedPartTwo = "RGLRBZAU";
    private const string Day = "_10";

    public static int Main(string[] args)
    {
        var filename = "input_10.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        var resultPartOne = PartOne(input);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(input);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(string[] program)
    {
        int[] cyclePoints = [20, 60, 100, 140, 180, 220];
        
        long tally = 0;
        var cycles = 0;
        long registerX = 1;
        
        while (cycles < 220)
        {
            foreach (var line in program)
            {
                var item = line.Split(' ');
                if (item[0] == "noop")
                {
                    cycles++;
                    if (cyclePoints.Contains(cycles))
                        tally += cycles * registerX;
                    continue;
                }

                foreach (var _ in Helper.Range(2))
                {
                    cycles++;
                    if (cyclePoints.Contains(cycles))
                        tally += cycles * registerX;
                }

                registerX += item[1].ToInt();
            }
        }
        return tally;
    }

    private static string PartTwo(string[] program)
    {
        var cycles = 0;
        var registerX = 1;
        
        foreach (var line in program)
        {
            var item = line.Split(' ');
            if (item[0] == "noop")
            {
                DrawPixel(cycles, registerX);
                cycles++;
                continue;
            }

            foreach (var _ in Helper.Range(2))
            {
                DrawPixel(cycles, registerX);
                cycles++;
            }

            registerX += item[1].ToInt();

            if (cycles > 240)
                break;
        }
        Console.WriteLine();

        // Running the program shows:
        // ######      ####    ##        ######    ######    ########    ####    ##    ##  
        // ##    ##  ##    ##  ##        ##    ##  ##    ##        ##  ##    ##  ##    ##  
        // ##    ##  ##        ##        ##    ##  ######        ##    ##    ##  ##    ##  
        // ######    ##  ####  ##        ######    ##    ##    ##      ########  ##    ##  
        // ##  ##    ##    ##  ##        ##  ##    ##    ##  ##        ##    ##  ##    ##  
        // ##    ##    ######  ########  ##    ##  ######    ########  ##    ##    ####    

        return "RGLRBZAU";
    }

    private static void DrawPixel(int cycles, int registerX)
    {
        if (cycles % 40 == 0)
            Console.WriteLine();
        
        if (cycles % 40 >= registerX - 1 && cycles % 40 <= registerX + 1)
            Console.Write("##");
        else
            Console.Write("  ");
    }
    
}