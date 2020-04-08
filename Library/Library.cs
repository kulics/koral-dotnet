using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library {

    public static partial class Lib {

        public static System.Type @typeof<T>() => typeof(T);

        public static void @throw(Exception it) => throw it;

        public static T[] array_of<T>(params T[] item) => item;

        public static T[] array<T>(int cap, params T[] item) {
            var arr = new T[cap];
            for (int i = 0; i < item.Length; i++) {
                arr[i] = item[i];
            }
            return arr;
        }

        public static list<T> list_of<T>(params T[] item) => new list<T>(item);

        public static T empty<T>() => default(T);

        public static T to<T>(object it) => (T)it;

        public static bool @is<T>(object it) => it is T;
        public static bool is_not<T>(object it) => !(it is T);

        public static T @as<T>(object it) where T : class => it as T;

        public static void print(params object[] paramList) => Cmd.print(paramList);

        public static string read() => Cmd.read();

        public static void clear() => Cmd.clear();

        public static T run<T>(Func<T> rn) => rn();

        public static list<T2> runloop<T1, T2>(IEnumerable<T1> source, Func<T1, T2> fn) {
            var temp = new list<T2>();
            foreach (var item in source) {
                temp.add(fn(item));
            }
            return temp;
        }

        public static list<T2> runloop<T1, T2>(IEnumerable<T1> source, Func<T1, T2> fn, Func<list<T2>> elseFn) {
            if (can_range(source)) {
                var temp = new list<T2>();
                foreach (var item in source) {
                    temp.add(fn(item));
                }
                return temp;
            } else {
                return elseFn();
            }
        }

        public static Task<T> go<T>(Func<Task<T>> fn) => Task.Run(fn);

        public static Task go(Func<Task> fn) => fn();

        public static Task go(Action fn) => Task.Run(fn);

        public static void wait(params Task[] tasks) => Task.WaitAll(tasks);

        public static void sleep(int milliseconds) => Thread.Sleep(milliseconds);

        public static Task delay(int milliseconds) => Task.Delay(milliseconds);

        public static double pow(double a, double b) => Math.Pow(a, b);

        public static double root(double a, double b) => Math.Pow(a, 1 / b);

        public static double log(double a, double b) => Math.Log(a, b);

        public static int len<T>(T[] it) => it.Length;
        public static int length<T>(T[] it) => it.Length;
        public static int len<T>(ICollection<T> it) => it.Count;
        public static int length<T>(ICollection<T> it) => it.Count;
        public static int cap<T>(List<T> it) => it.Capacity;
        public static int capacity<T>(List<T> it) => it.Capacity;

        public static IEnumerable<int> range(int begin, int end, int step = 1, bool order = true) {
            if (order) {
                for (int index = begin; index <= end; index += step) {
                    yield return index;
                }
            } else {
                for (int index = begin; index >= end; index -= step) {
                    yield return index;
                }
            }
        }

        public static IEnumerable<(int index, T item)> range<T>(IEnumerable<T> self)
=> self.Select((item, index) => (index, item));

        public static IEnumerable<(TKey, TValue)> range<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> self)
   => self.Select((item) => (item.Key, item.Value));

        public static bool can_range<T>(IEnumerable<T> self) => self.GetEnumerator().MoveNext();

        public static bool can_range<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> self) => self.GetEnumerator().MoveNext();

        public static void todo(string it) => throw new Exception(it);

        public static decimal abs(decimal it) => Math.Abs(it);
        public static sbyte abs(sbyte it) => Math.Abs(it);
        public static short abs(short it) => Math.Abs(it);
        public static int abs(int it) => Math.Abs(it);
        public static long abs(long it) => Math.Abs(it);
        public static float abs(float it) => Math.Abs(it);
        public static double abs(double it) => Math.Abs(it);

        public static decimal max(params decimal[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Max(it[i], it[i - 1]);
            }
            return x;
        }

        public static sbyte max(params sbyte[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Max(it[i], it[i - 1]);
            }
            return x;
        }

        public static byte max(params byte[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Max(it[i], it[i - 1]);
            }
            return x;
        }

        public static short max(params short[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Max(it[i], it[i - 1]);
            }
            return x;
        }

        public static ushort max(params ushort[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Max(it[i], it[i - 1]);
            }
            return x;
        }

        public static int max(params int[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Max(it[i], it[i - 1]);
            }
            return x;
        }

        public static uint max(params uint[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Max(it[i], it[i - 1]);
            }
            return x;
        }

        public static long max(params long[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Max(it[i], it[i - 1]);
            }
            return x;
        }

        public static ulong max(params ulong[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Max(it[i], it[i - 1]);
            }
            return x;
        }

        public static float max(params float[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Max(it[i], it[i - 1]);
            }
            return x;
        }

        public static double max(params double[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Max(it[i], it[i - 1]);
            }
            return x;
        }

        public static decimal min(params decimal[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Min(it[i], it[i - 1]);
            }
            return x;
        }

        public static sbyte min(params sbyte[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Min(it[i], it[i - 1]);
            }
            return x;
        }

        public static byte min(params byte[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Min(it[i], it[i - 1]);
            }
            return x;
        }

        public static short min(params short[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Min(it[i], it[i - 1]);
            }
            return x;
        }

        public static ushort min(params ushort[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Min(it[i], it[i - 1]);
            }
            return x;
        }

        public static int min(params int[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Min(it[i], it[i - 1]);
            }
            return x;
        }

        public static uint min(params uint[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Min(it[i], it[i - 1]);
            }
            return x;
        }

        public static long min(params long[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Min(it[i], it[i - 1]);
            }
            return x;
        }

        public static ulong min(params ulong[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Min(it[i], it[i - 1]);
            }
            return x;
        }

        public static float min(params float[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Min(it[i], it[i - 1]);
            }
            return x;
        }

        public static double min(params double[] it) {
            if (it.Length == 0) {
                return 0;
            }
            var x = it[0];
            for (int i = 1; i < it.Length; i++) {
                x = Math.Min(it[i], it[i - 1]);
            }
            return x;
        }

        //编码
        public static string encode_base64(string code) {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(code);
            try {
                encode = Convert.ToBase64String(bytes);
            } catch {
                encode = code;
            }
            return encode;
        }
        //解码
        public static string decode_base64(string code) {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try {
                decode = Encoding.GetEncoding("utf-8").GetString(bytes);
            } catch {
                decode = code;
            }
            return decode;
        }
    }
}
