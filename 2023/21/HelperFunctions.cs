namespace _21;

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

    private static int ToInt(this string str)
    {
        if (int.TryParse(str, out var value))
            return value;

        throw new InvalidCastException($"Not a valid integer: {str}");
    }

    private static long ToLong(this string str)
    {
        if (long.TryParse(str, out var value))
            return value;

        throw new InvalidCastException($"Not a valid integer: {str}");
    }

    private static int[] ToIntArray(this string[] array)
    {
        var intArray = new int[array.Length];
        for (var i = 0; i < array.Length; i++)
        {
            intArray[i] = array[i].ToInt();
        }

        return intArray;
    }
    
    private static long[] ToLongArray(this IEnumerable<int> list)
    {
        var array = list.ToArray();
        var intArray = new long[array.Length];
        for (var i = 0; i < array.Length; i++)
        {
            intArray[i] = array[i];
        }

        return intArray;
    }

    private static (T first, T second) ToTuplePair<T>(this T[] array)
    {
        return array.Length switch
        {
            > 2 => throw new ArgumentException(
                $" Too many array members.{array.Length} This method requires an array of length 2."),
            < 2 => throw new ArgumentException(
                $" Too few array members.{array.Length} This method requires an array of length 2."),
            _ => (array[0], array[1])
        };
    }

    private static char[][] ToCharArray(this string[] array)
    {
        var charArr = new char[array.Length][];
        for (var i = 0; i < array.Length; i++)
            charArr[i] = array[i].ToCharArray();
        
        return charArr;
    }

    private static long Squared(this long n)
    {
        return n * n;
    }
}