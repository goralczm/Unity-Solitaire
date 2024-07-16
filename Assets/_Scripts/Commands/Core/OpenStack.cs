using System.Collections.Generic;

namespace Utilities.CommandPattern
{
    public class OpenStack<T>
    {
        private LinkedList<T> _items = new LinkedList<T>();

        public int Count => _items.Count;

        public void PushOnTop(T item)
        {
            _items.AddLast(item);
        }

        public T PopFirst()
        {
            if (_items.Count == 0)
                return default(T);

            T it = _items.Last.Value;
            _items.RemoveLast();

            return it;
        }

        public T PopLast()
        {
            if (_items.Count == 0)
                return default(T);

            T it = _items.First.Value;
            _items.RemoveFirst();

            return it;
        }
    }
}
