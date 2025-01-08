namespace _19;

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

    internal class FlexStack<T>
    {
        private readonly List<T> _list;
        
        public FlexStack()
        {
            _list = [];
        }
        
        public FlexStack(IEnumerable<T> list)
        {
            _list = list.ToList();
            _list.Reverse();
        }
        
        public FlexStack(T[] list)
        {
            _list = list.ToList();
            _list.Reverse();
        }

        public T Peek()
        {
            return _list.Last();
        }
        
        public T PeekLeft()
        {
            return Peek();
        }
        
        public T PeekRight()
        {
            return  _list.First();
        }
        
        public void Push(T item)
        {
            _list.Add(item);
        }
        
        public void PushLeft(T item)
        {
            Push(item);
        }
        
        public void PushRight(T item)
        {
            _list.Insert(0, item);
        }
        
        public T Pop()
        {
            if (_list.Count > 0)
            {
                var item = _list.Last();
                _list.Remove(item);
                return item;
            }

            throw new InvalidOperationException("Stack Empty");
        }
        
        public T PopLeft()
        {
            return Pop();
        }
        
        public T PopRight()
        {
            if (_list.Count <= 0) 
                throw new InvalidOperationException("Stack Empty");
            
            var item = _list.First();
            _list.RemoveAt(0);
            return item;
        }

        public bool IsEmpty()
        {
            return _list.Count == 0;
        }
        
        public List<T>.Enumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}