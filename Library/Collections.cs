using System;
using System.Collections.Generic;
using System.Linq;

namespace Library {
    public static partial class CollectionsExtension {
        public static int Size<T>(this ICollection<T> it) => it.Count;
        public static bool Is_empty<T>(this ICollection<T> it) => it.Count == 0;
        public static int Append<T>(this List<T> it, T element) {
            it.Add(element);
            return it.Count - 1;
        }
        public static int Append_all<T>(this List<T> it, IList<T> elements) {
            it.AddRange(elements);
            return it.Count - 1;
        }
        public static int Last_index<T>(this List<T> it) => it.Count == 0 ? 0 : it.Count - 1;

        public static void Put<TKey, TValue>(this Dictionary<TKey, TValue> it, TKey key, TValue value) => it.Add(key, value);

        public static List<T> Slice<T>(this List<T> it, int? startIndex, int? endIndex, bool order = true, bool close = true) {
            if (startIndex == null && endIndex == null) {
                return Sub_List(it, 0, it.Last_index());
            } else if (endIndex == null) {
                return Sub_List(it, startIndex ?? 0, it.Last_index());
            } else { // (startIndex == null)
                return Sub_List(it, 0, endIndex ?? 0);
            }
        }

        public static List<T> Sub_List<T>(this List<T> it, int startIndex, int endIndex) //=> GetRange(startIndex, count) as lst<T>;
        {
            var temp = new List<T>();
            int currIndex = startIndex;
            while (currIndex <= endIndex) {
                temp.Append(it[currIndex]);
                currIndex++;
            }
            return temp;
        }
    }
}
