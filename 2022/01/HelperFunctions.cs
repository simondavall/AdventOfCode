namespace _01;

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

    private static long[] ToLongArray(this string[] array)
    {
        var longArray = new long[array.Length];
        for (var i = 0; i < array.Length; i++)
        {
            longArray[i] = array[i].ToLong();
        }

        return longArray;
    }

    private static char[][] ToCharArray(this string[] array)
    {
        var charArr = new char[array.Length][];
        for (var i = 0; i < array.Length; i++)
            charArr[i] = array[i].ToCharArray();

        return charArr;
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

    private static (long first, long second, long third) ToLongTupleTriple(this string[] array)
    {
        return array.Length switch
        {
            > 3 => throw new ArgumentException(
                $" Too many array members.{array.Length} This method requires an array of length 2."),
            < 3 => throw new ArgumentException(
                $" Too few array members.{array.Length} This method requires an array of length 2."),
            _ => (array[0].ToLong(), array[1].ToLong(), array[2].ToLong())
        };
    }

    private static List<T> ToSortedList<T>(this HashSet<T> set)
    {
        var list = set.ToList();
        list.Sort();
        return list;
    }

    private static List<T> Sorted<T>(this List<T> list)
    {
        list.Sort();
        return list;
    }
    
    private static List<T> SortedDesc<T>(this List<T> list)
    {
        return list.OrderDescending().ToList();
    }
}