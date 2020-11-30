using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library
{

    public static partial class Lib
    {
        public static Type Typeof<T>() => typeof(T);

        public static void Throw(Exception it) => throw it;

        public static T[] Array_of<T>(params T[] item) => item;

        public static T[] Array<T>(int size, Func<int, T> initElement)
        {
            var arr = new T[size];
            for (int i = 0; i < size; i++)
            {
                arr[i] = initElement(i);
            }
            return arr;
        }

        public static List<T> List_of<T>(params T[] item)
        {
            return new List<T>(item);
        }

        public static KeyValuePair<TKey, TValue> Pair_of<TKey, TValue>(TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }

        public static Dictionary<TKey, TValue> Dict_of<TKey, TValue>(params KeyValuePair<TKey, TValue>[] item)
        {
            return new Dictionary<TKey, TValue>(item);
        }

        public static T Empty<T>() => default;

        public static T To<T>(object it) => (T)it;

        public static bool Is<T>(object it) => it is T;

        public static bool Is_not<T>(object it) => !(it is T);

        public static T As<T>(object it) where T : class => it as T;

        public static void Print(params object[] paramList) => Cmd.Print(paramList);

        public static string Read() => Cmd.Read();

        public static void Clear() => Cmd.Clear();

        public static T Run<T>(Func<T> fn) => fn();

        public static Task<T> go<T>(Func<Task<T>> fn) => Task.Run(fn);

        public static Task go(Func<Task> fn) => fn();

        public static Task go(Action fn) => Task.Run(fn);

        public static void Wait(params Task[] tasks) => Task.WaitAll(tasks);

        public static void Sleep(int milliseconds) => Thread.Sleep(milliseconds);

        public static Task Delay(int milliseconds) => Task.Delay(milliseconds);

        public static double Pow(double a, double b) => Math.Pow(a, b);

        public static double Root(double a, double b) => Math.Pow(a, 1 / b);

        public static double Log(double a, double b) => Math.Log(a, b);

        public static IntRange Range(int begin, int end, int step)
        {
            return new IntRange(begin, end, step);
        }

        public static IEnumerable<int> Range_close(int begin, int end, int step)
        {
            return new IntRangeClose(begin, end, step);
        }

        public static IEnumerable<(int index, T item)> Range<T>(IEnumerable<T> self)
=> self.Select((item, index) => (index, item));

        public static IEnumerable<(TKey, TValue)> Range<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> self)
   => self.Select((item) => (item.Key, item.Value));

        public static IEnumerable<(int index, T item)> WithIndex<T>(this IEnumerable<T> self)
=> self.Select((item, index) => (index, item));

        public static IEnumerable<(TKey, TValue)> WithIndex<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> self)
   => self.Select((item) => (item.Key, item.Value));

        public static bool Can_range<T>(IEnumerable<T> self) => self.GetEnumerator().MoveNext();

        public static bool Can_range<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> self) => self.GetEnumerator().MoveNext();

        public static void Todo(string it) => throw new Exception(it);

        //编码
        public static string encode_base64(string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }
        //解码
        public static string decode_base64(string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding("utf-8").GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }
    }
}
