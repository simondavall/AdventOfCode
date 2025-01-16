using System.Reflection;

namespace Aoc;

internal static class Aoc
{
    /// <summary>
    /// Discovers all of the days' solutions from the referenced projects. Activates the Main
    /// method of each and returns a result.
    /// Main project must be public (wasn't able to discover internal Main methods)
    /// Does make the assumption that the only other referenced assemblies are System assemblies.
    /// </summary>
    /// <exception cref="Exception"></exception>
    internal static void Main()
    {
        var result = 0;
        
        foreach (var assemblyName in Enumerable.Range(1, 25).ToString("00"))
        {
            Assembly? assembly;
            
            try { assembly = Assembly.Load(assemblyName); }
            catch (FileNotFoundException) { continue; } // this program check all 25 days, some of which
                                                           // will not have been started yet. So skip failed finds.
            var type = assembly.GetTypes().First(x => x.Name.Contains("Program"));
            var main = type.GetMethod("Main");
            var current = (int)main?.Invoke(null, [new[]{"",$"input_{assemblyName}.txt"}])!;
            
            Console.WriteLine(current != 0 ? "Oops, Failed!!!" : "Success");
            result += current;
        }

        Console.WriteLine();
        
        if (result > 0)
            Console.Write($"Incorrect results found for {result} days.");
        else
            Console.Write("All days passed successfully!.");
    }

    private static IEnumerable<string> ToString(this IEnumerable<int> list, string format)
    {
        foreach (var item in list)
        {
            yield return item.ToString(format);
        }
    }
    
}