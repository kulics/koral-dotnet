using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Library
{
    public static class CollectionsExtension
    {
        public static IEnumerable<(int index, T item)> WithIndex<T>(this IEnumerable<T> self) => self.Select((item, index) => (index, item));

        public static IEnumerable<R> Map<T, R>(this IEnumerable<T> self, Func<T, R> fn) => self.Select(fn);
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> self, Func<T, bool> fn) => self.Where(fn);
        public static int Size<T>(this ICollection<T> it) => it.Count;
        public static bool IsEmpty<T>(this ICollection<T> it) => it.Count == 0;
        public static int PushBack<T>(this List<T> it, T element)
        {
            it.Add(element);
            return it.Count - 1;
        }

        public static List<R> Map<T, R>(this List<T> list, Func<T, R> transform)
        {
            var newList = new List<R>(list.Count);
            foreach (var item in list)
            {
                newList.Add(transform(item));
            }
            return newList;
        }

        public static List<R> Map<T, R>(this T[] array, Func<T, R> transform)
        {
            var newList = new List<R>(array.Length);
            foreach (var item in array)
            {
                newList.Add(transform(item));
            }
            return newList;
        }

        public static int PushBackAll<T>(this List<T> it, IList<T> elements)
        {
            it.AddRange(elements);
            return it.Count - 1;
        }
        public static int LastIndex<T>(this List<T> it) => it.Count == 0 ? 0 : it.Count - 1;
    }
}
