using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    public static partial class CollectionsExtension
    {
        public static int Size<T>(this ICollection<T> it) => it.Count;
        public static bool Is_empty<T>(this ICollection<T> it) => it.Count == 0;
        public static int Append<T>(this List<T> it, T element)
        {
            it.Add(element);
            return it.Count - 1;
        }
        public static int Append_all<T>(this List<T> it, IList<T> elements)
        {
            it.AddRange(elements);
            return it.Count - 1;
        }
        public static int Last_index<T>(this List<T> it) => it.Count == 0 ? 0 : it.Count - 1;

        public static void Put<TKey, TValue>(this Dictionary<TKey, TValue> it, TKey key, TValue value) => it.Add(key, value);

        public static List<T> Slice<T>(this List<T> it, int? startIndex, int? endIndex)
        {
            if (startIndex == null && endIndex == null)
            {
                return Sub_List(it, 0, it.Last_index());
            }
            else if (endIndex == null)
            {
                return Sub_List(it, startIndex ?? 0, it.Last_index());
            }
            else
            { // (startIndex == null)
                return Sub_List(it, 0, endIndex ?? 0);
            }
        }

        public static List<T> Sub_List<T>(this List<T> it, int startIndex, int endIndex) //=> GetRange(startIndex, count) as lst<T>;
        {
            var temp = new List<T>();
            int currIndex = startIndex;
            while (currIndex <= endIndex)
            {
                temp.Append(it[currIndex]);
                currIndex++;
            }
            return temp;
        }
    }

    public class IntRange : IEnumerable<int>
    {
        public readonly int begin;
        public readonly int end;
        public readonly int step;
        public readonly bool order;

        public IntRange(int begin, int end, bool order) : this(begin, end, 1, order)
        {
        }

        private IntRange(int begin, int end, int step, bool order)
        {
            this.begin = begin;
            this.end = end;
            this.step = step;
            this.order = order;
        }

        public IEnumerator<int> GetEnumerator()
        {
            var next = begin;
            if (order)
            {
                while (next < end)
                {
                    yield return next;
                    next += step;
                }
            }
            else
            {
                while (next > end)
                {
                    yield return next;
                    next -= step;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IntRange Reversed()
        {
            if (order)
            {
                return new(end - 1, begin - 1, step, !order);
            }
            return new(end + 1, begin + 1, step, !order);
        }

        public IntRange Step(int step)
        {
            if (step <= 0)
            {
                throw new("Step must be bigger than zero.");
            }
            return new(begin, end, step, order);
        }
    }

    public class IntRangeClose : IEnumerable<int>
    {
        public readonly int begin;
        public readonly int end;
        public readonly int step;
        public readonly bool order;

        public IntRangeClose(int begin, int end, bool order) : this(begin, end, 1, order)
        {
        }

        private IntRangeClose(int begin, int end, int step, bool order)
        {
            this.begin = begin;
            this.end = end;
            this.step = step;
            this.order = order;
        }

        public IEnumerator<int> GetEnumerator()
        {
            var next = begin;
            if (order)
            {
                while (next <= end)
                {
                    yield return next;
                    next += step;
                }
            }
            else
            {
                while (next >= end)
                {
                    yield return next;
                    next -= step;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IntRangeClose Reversed()
        {
            return new(end, begin, !order);
        }

        public IntRangeClose Step(int step)
        {
            if (step <= 0)
            {
                throw new("Step must be bigger than zero.");
            }
            return new(begin, end, step, order);
        }
    }
}
