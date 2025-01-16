namespace _06;

internal static class Program
{
    private const long ExpectedPartOne = 1093;
    private const long ExpectedPartTwo = 3534;
    private const string Day = "_06";

    public static int Main(string[] args)
    {
        var filename = "input_06.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        var resultPartOne = PartOne(input[0]);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(input[0]);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(string signalInput)
    {
        var signal = signalInput.ToList();
        return FindMarkerLocation(4, signal);
    }

    private static long PartTwo(string signalInput)
    {
        var signal = signalInput.ToList();
        return FindMarkerLocation(14, signal);
    }

    private static long FindMarkerLocation(int markerLength, List<char> signal)
    {
        var count = 0;
        while (signal.Count >= markerLength)
        {
            var marker = signal.Take(markerLength);
            if (marker.Distinct().ToArray().Length == markerLength)
            {
                return count + markerLength;
            }
            signal.RemoveAt(0);
            count++;
        }

        return 0;
    }
}