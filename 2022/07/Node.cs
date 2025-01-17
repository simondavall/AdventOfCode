using AocHelper;

namespace _07;

internal static class NodeExt
{
    internal static Node MoveToParent(this Node n)
    {
        return n.Parent ?? n;
    }

    internal static Node MoveToDirectory(this Node n, string newDir)
    {
        if (!n.Children.Select(x => x.Name).Contains(newDir))
            n.Children.Add(new Node(newDir, n));

        return n.Children.First(x => x.Name == newDir);
    }

    internal static void AddDirectory(this Node n, string newDir)
    {
        if (!n.Children.Select(x => x.Name).Contains(newDir))
            n.Children.Add(new Node(newDir, n));
    }

    internal static void AddFile(this Node n, (string size, string name) f)
    {
        if (!n.Files.Select(x => x.name).Contains(f.name))
            n.Files.Add((f.name, f.size.ToInt()));
    }
}

internal class Node(string name, Node? parent)
{
    private long _size;
    internal string Name { get; } = name;
    internal Node? Parent { get; } = parent;
    internal List<(string name, int size)> Files { get; } = [];
    internal List<Node> Children { get; } = [];

    internal long Size
    {
        get
        {
            if (_size == 0)
                _size = Files.Select(x => x.size).Sum() + Children.Select(x => x.Size).Sum();
        
            return _size;
        }
    }
}