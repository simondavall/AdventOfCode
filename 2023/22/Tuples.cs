using System.Runtime.CompilerServices;

namespace _22;

internal static partial class Program
{
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
    
    private static (T first, T second, T third) ToTupleTriple<T>(this T[] array)
    {
        return array.Length switch
        {
            > 3 => throw new ArgumentException(
                $" Too many array members.{array.Length} This method requires an array of length 2."),
            < 3 => throw new ArgumentException(
                $" Too few array members.{array.Length} This method requires an array of length 2."),
            _ => (array[0], array[1], array[2])
        };
    }
    
    private static (int first, int second, int third) ToIntTupleTriple(this string[] array)
    {
        return array.Length switch
        {
            > 3 => throw new ArgumentException(
                $" Too many array members.{array.Length} This method requires an array of length 2."),
            < 3 => throw new ArgumentException(
                $" Too few array members.{array.Length} This method requires an array of length 2."),
            _ => (array[0].ToInt(), array[1].ToInt(), array[2].ToInt())
        };
    }
}