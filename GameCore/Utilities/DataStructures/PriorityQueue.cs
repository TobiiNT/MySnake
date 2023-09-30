using System;
using System.Collections.Generic;

namespace GameCore.Utilities.DataStructures
{
    public class PriorityQueue<TItem, TPriority> where TPriority : IComparable<TPriority>
    {
        private SortedSet<Tuple<TPriority, TItem>> queue = new SortedSet<Tuple<TPriority, TItem>>(new Comparer());

        private class Comparer : IComparer<Tuple<TPriority, TItem>>
        {
            public int Compare(Tuple<TPriority, TItem> x, Tuple<TPriority, TItem> y)
            {
                int result = x.Item1.CompareTo(y.Item1);
                if (result == 0)
                {
                    return x.Item2.GetHashCode().CompareTo(y.Item2.GetHashCode());
                }
                return result;
            }
        }

        public void Enqueue(TItem item, TPriority priority)
        {
            queue.Add(new Tuple<TPriority, TItem>(priority, item));
        }

        public TItem Dequeue()
        {
            if (queue.Count == 0)
                throw new InvalidOperationException("The priority queue is empty");

            var minItem = queue.Min;
            queue.Remove(minItem);
            return minItem.Item2;
        }

        public int Count => queue.Count;
    }
}