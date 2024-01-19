using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace deeprockitems.Common.Types
{
    public class FixedQueue<T>
    {
        private List<T> _innerList;
        private int _maxCapacity;
        /// <summary>
        /// The max length of the queue.
        /// </summary>
        public int MaxCapacity { get => _maxCapacity; }

        /// <summary>
        /// The oldest item in the queue. If the queue is empty, returns the default item.
        /// </summary>
        public T OldestItem
        {
            get
            {
                if (_innerList.Count == 0)
                {
                    return default;
                }
                return _innerList[_innerList.Count - 1];
            }
        }
        /// <summary>
        /// Creates a new fixed queue with the given capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public FixedQueue(int capacity)
        {
            _innerList = new List<T>();
            _maxCapacity = capacity;
        }

        /// <summary>
        /// Clears the queue. Keeps the capacity the same.
        /// </summary>
        public void Clear()
        {
            _innerList.Clear();
        }
        /// <summary>
        /// Puts an item into the beginning of the queue (index 0). If the array is at max capacity, the oldest item (highest index) will be removed.
        /// </summary>
        /// <param name="item">The item to add to the queue.</param>
        public void Enqueue(T item)
        {
            _innerList.Insert(0, item);
            if (_innerList.Count < _maxCapacity)
            {
                _innerList.RemoveAt(_maxCapacity - 1);
            }
        }
        /// <summary>
        /// Removes an item from the queue.
        /// </summary>
        /// <param name="item"></param>
        public bool Dequeue(T item)
        {
            return _innerList.Remove(item);
        }
        /// <summary>
        /// Converts this queue into an array
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            return _innerList.ToArray();
        }
    }
}
