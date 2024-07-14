using System;
using System.Linq;
using System.Text;

namespace Compiler.Library
{
    public static class ExpressionExtension
    {
        // char
        public static char ToLower(this char it) => char.ToLower(it);
        public static char ToUpper(this char it) => char.ToUpper(it);

        public static bool IsLower(this char it) => char.IsLower(it);
        public static bool IsUpper(this char it) => char.IsUpper(it);

        public static bool IsLetter(this char it) => char.IsLetter(it);
        public static bool IsDigit(this char it) => char.IsDigit(it);
        public static bool IsLetterOrDigit(this char it) => char.IsLetterOrDigit(it);

        public static bool IsNumber(this char it) => char.IsNumber(it);
        public static bool IsSymbol(this char it) => char.IsSymbol(it);
        public static bool IsWhiteSpace(this char it) => char.IsWhiteSpace(it);
        public static bool IsControl(this char it) => char.IsControl(it);

        // String
        public static bool IsNotEmpty(this string it) => !it.IsNullOrEmpty();
        public static bool IsNullOrEmpty(this string it) => string.IsNullOrEmpty(it);

        public static int LastIndex(this string it) => it.Length - 1;

        public static int FindFirst(this string it, Func<char, bool> fn)
        {
            for (int i = 0; i < it.Length; i++)
            {
                if (fn(it[i]))
                {
                    return i;
                }
            }
            return 0;
        }

        public static int FirstIndexOf(this string it, string value, StringComparison comparisonType = StringComparison.Ordinal) => it.IndexOf(value, comparisonType);
        public static int FirstIndexOf(this string it, string value, int startIndex, StringComparison comparisonType = StringComparison.Ordinal) => it.IndexOf(value, startIndex, comparisonType);
        public static int FirstIndexOf(this string it, string value, int startIndex, int count, StringComparison comparisonType = StringComparison.Ordinal) => it.IndexOf(value, startIndex, count, comparisonType);

        public static int LastIndexOf(this string it, string value, StringComparison comparisonType = StringComparison.Ordinal) => it.LastIndexOf(value, comparisonType);
        public static int LastIndexOf(this string it, string value, int startIndex, StringComparison comparisonType = StringComparison.Ordinal) => it.LastIndexOf(value, startIndex, comparisonType);
        public static int LastIndexOf(this string it, string value, int startIndex, int count, StringComparison comparisonType = StringComparison.Ordinal) => it.LastIndexOf(value, startIndex, count, comparisonType);


        public static string[] Split(this string it, string[] separator, StringSplitOptions options = StringSplitOptions.None) => it.Split(separator, options);

        public static byte[] ToBytes(this string it) => Encoding.UTF8.GetBytes(it);

        public static sbyte ToInt8FromBase(this string it, int fromBase) => Convert.ToSByte(it, fromBase);
        public static short ToInt16FromBase(this string it, int fromBase) => Convert.ToInt16(it, fromBase);
        public static int ToInt32FromBase(this string it, int fromBase) => Convert.ToInt32(it, fromBase);
        public static long ToInt64FromBase(this string it, int fromBase) => Convert.ToInt64(it, fromBase);
        public static byte ToUInt8FromBase(this string it, int fromBase) => Convert.ToByte(it, fromBase);
        public static ushort ToUInt16FromBase(this string it, int fromBase) => Convert.ToUInt16(it, fromBase);
        public static uint ToUInt32FromBase(this string it, int fromBase) => Convert.ToUInt32(it, fromBase);
        public static ulong ToUInt64FromBase(this string it, int fromBase) => Convert.ToUInt64(it, fromBase);
        public static byte[] ToBytesByBase64(this string it) => Convert.FromBase64String(it);

        public static string ToHexString(this byte[] it) => BitConverter.ToString(it, 0).Replace("-", string.Empty);

        public static string ToLowerHexString(this byte[] it) => it.ToHexString().ToLower();
        public static string ToUpperHexString(this byte[] it) => it.ToHexString();
        public static string ToBase64String(this byte[] it) => Convert.ToBase64String(it, 0, it.Length);
        public static byte[] FromBase64String(this string it) => Convert.FromBase64String(it);

        public static byte[] SubBytes(this byte[] it, int start, int length) => it.Skip(start).Take(length).ToArray();
    }
}
