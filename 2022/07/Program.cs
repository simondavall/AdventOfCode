using AocHelper;

namespace _07;

internal static class Program
{
    private const long ExpectedPartOne = 1543140;
    private const long ExpectedPartTwo = 1117448;
    private const string Day = "_07";

    public static int Main(string[] args)
    {
        var filename = "input_07.txt";
        if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            filename = args[1];

        var input = File.ReadAllText($"{filename}").Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        var resultPartOne = PartOne(input);
        Console.WriteLine($"Day{Day} Part 1: {resultPartOne}");
        var resultPartTwo = PartTwo(input);
        Console.WriteLine($"Day{Day} Part 2: {resultPartTwo}");

        return resultPartOne == ExpectedPartOne && resultPartTwo == ExpectedPartTwo ? 0 : 1;
    }

    private static long PartOne(string[] terminalOutput)
    {
        var root = new Node("/", null);

        BuildDirectoryTree(root, terminalOutput);

        //PrintDirectoryTree(root);
            
        TallyLargeDirectories(root, 100_000); 
        
        return _runningTotal;
    }

    private static long PartTwo(string[] terminalOutput)
    {
        var root = new Node("/", null);

        BuildDirectoryTree(root, terminalOutput);

        var currentAvailable = 70_000_000 - root.Size;
        var requiredSpace = 30_000_000 - currentAvailable;
        
        FindDirectoriesLargerThan(root, requiredSpace);
        
        return QualifyingDirectories.OrderBy(x => x.Size).First().Size;
    }

    private static readonly List<Node> QualifyingDirectories = [];
    private static void FindDirectoriesLargerThan(Node current, long minSize)
    {
        if (current.Size >= minSize)
            QualifyingDirectories.Add(current);
        
        if (current.Children.Count == 0)
            return;
        
        foreach (var child in current.Children)
        {
            FindDirectoriesLargerThan(child, minSize);
        }
    }
    
    private static long _runningTotal;
    private static void TallyLargeDirectories(Node current, int minSize)
    {
        if (current.Size < minSize)
            _runningTotal += current.Size;
        
        if (current.Children.Count == 0)
            return;
        
        foreach (var child in current.Children)
        {
            TallyLargeDirectories(child, minSize);
        }
    }

    private static void BuildDirectoryTree(Node root, string[] terminalOutput)
    {
        var currentNode = root;

        foreach (var line in terminalOutput.Skip(1))
        {
            if (line == "$ ls")
                continue;
            if (line.StartsWith("$ cd .."))
            {
                currentNode = currentNode.MoveToParent();
                continue;
            }
            if (line.StartsWith("$ cd"))
            {
                currentNode = currentNode.MoveToDirectory(line[5..]);
                continue;
            }
            if (line.StartsWith("dir"))
            {
                currentNode.AddDirectory(line[4..]);
                continue;
            }
            
            currentNode.AddFile(line.Split(' ').ToTuplePair());
        }
    }
    
    private static void PrintDirectoryTree(Node node, string tabs = "")
    {
        Console.WriteLine($"{tabs}- {node.Name} (dir)");
        foreach (var child in node.Children)
        {
            PrintDirectoryTree(child, tabs + "  ");
        }

        foreach (var file in node.Files)
        {
            Console.WriteLine($"{tabs + "  "}- {file.name} (file, size={file.size})");
        }
    }
}