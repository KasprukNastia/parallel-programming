using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab4.Task1
{
    public class Collection
    {
        private readonly object _locker = new object();

        private readonly int _maxSize;
        private readonly Queue<Item> _items;

        public Collection(int maxSize = 20)
        {
            _maxSize = maxSize;
            _items = new Queue<Item>(maxSize);
        }

        public void Add(Item item)
        {
            bool wasEntered = false;
            try
            {
                Monitor.Enter(_locker, ref wasEntered);

                while (_items.Count >= _maxSize)
                    Monitor.Wait(_locker);

                _items.Enqueue(item);

                Console.WriteLine($"The number of elements in the collection {_items.Count}");
                Console.WriteLine();

                if(_items.Count == 1)
                    Monitor.Pulse(_locker);
            }
            finally
            {
                if (wasEntered) 
                    Monitor.Exit(_locker);
            }
        }

        public Item Remove()
        {
            bool wasEntered = false;
            try
            {
                Monitor.Enter(_locker, ref wasEntered);

                while (_items.Count == 0)
                    Monitor.Wait(_locker);

                Item toReturn = _items.Dequeue();

                Console.WriteLine($"The number of elements in the collection {_items.Count}");
                Console.WriteLine();

                if (_items.Count == _maxSize - 1)
                    Monitor.Pulse(_locker);

                return toReturn;
            }
            finally
            {
                if (wasEntered)
                    Monitor.Exit(_locker);
            }
        }
    }
}
