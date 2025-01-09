namespace Helpers;

internal static partial class Program
{
    internal class FlexStack<T>
    {
        private readonly List<T> _list;
            
        public FlexStack()
        {
            _list = [];
        }
        
        /// <summary>
        /// The supplied list has the first element (element.First()) at the head, and the
        /// last element at the tail (bottom) of the stack
        /// </summary>
        /// <param name="list"></param>
        public FlexStack(IEnumerable<T> list)
        {
            _list = list.ToList();
            _list.Reverse();
        }
        
        /// <summary>
        /// The supplied array has the first element (element[0]) at the head, and the
        /// last element at the tail (bottom) of the stack
        /// </summary>
        /// <param name="list"></param>
        public FlexStack(T[] list) : this(list.ToList()) { }
        
        public bool IsEmpty()
        {
            return _list.Count == 0;
        }
        
        public T Peek()
        {
            return _list.Last();
        }
        
        /// <summary>
        /// Peek the item at the bottom of the stack
        /// </summary>
        /// <returns></returns>
        public T PeekBottom()
        {
            return  _list.First();
        }
            
        public T Pop()
        {
            if (IsEmpty()) throw new EmptyStackException();
            
            var item = _list.Last();
            _list.Remove(item);
            return item;
        }
        
        /// <summary>
        /// Pop an item from the bottom of the stack
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EmptyStackException"></exception>
        public T PopBottom()
        {
            if (IsEmpty()) throw new EmptyStackException();
                
            var item = _list.First();
            _list.RemoveAt(0);
            return item;
        }
        
        public void Push(T item)
        {
            _list.Add(item);
        }
        
        /// <summary>
        /// Push an item to the bottom of the stack
        /// </summary>
        /// <param name="item"></param>
        public void PushBottom(T item)
        {
            _list.Insert(0, item);
        }
            
        public List<T>.Enumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        private class EmptyStackException : Exception {}
    }
}