namespace _17;

internal static partial class Program
{
    private static List<T> List<T>(List<T> list)
    {
        return list;
    }

    private static IEnumerable<int> Range(int n)
    {
        return Enumerable.Range(0, n);
    }

    private static IEnumerable<int> Range(int start, int n)
    {
        return Enumerable.Range(start, n);
    }

    private static int[] ToIntArray(this string[] array)
    {
        var intArray = new int[array.Length];
        for (var i = 0; i < array.Length; i++)
        {
            intArray[i] = int.Parse(array[i]);
        }

        return intArray;
    }
    
    private static int[] ToIntArray(this string array)
    {
        var intArray = new int[array.Length];
        for (var i = 0; i < array.Length; i++)
        {
            intArray[i] = array[i] - '0';
        }

        return intArray;
    }
}