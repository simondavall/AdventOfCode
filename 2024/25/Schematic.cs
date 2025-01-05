using System.Text;

namespace _25;

public struct Schematic
{
    public Schematic()
    {
    }

    public int Size { get; private set; } = -5;
    public readonly int[] Tumblers = [-1, -1, -1, -1, -1];
        
    public void AddRow(string row)
    {
        for (var i = 0; i < Tumblers.Length; i++)
        {
            Tumblers[i] += row[i] == '#' ? 1 : 0;
        }

        Size += row.Sum(ch => ch == '#' ? 1 : 0);
    }
        
    public override string ToString()
    {
        var str = new StringBuilder($"Size: {Size:00}: (");
        foreach (var t in Tumblers)
        {
            str.Append($"{t},");
        }
        str.Replace(',', ')', str.Length - 1, 1);
        return str.ToString();
    }
}