using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library
{
    public static partial class ExpressionExtension
    {
        // object
        public static string To_Str(this object it) => it.ToString();

        public static T To<T>(this object it) => (T)it;
        public static int To_Int(this object it) => Convert.ToInt32(it);
        public static double To_Num(this object it) => Convert.ToDouble(it);
        public static sbyte To_I8(this object it) => Convert.ToSByte(it);
        public static short To_I16(this object it) => Convert.ToInt16(it);
        public static int To_I32(this object it) => Convert.ToInt32(it);
        public static long To_I64(this object it) => Convert.ToInt64(it);
        public static byte To_U8(this object it) => Convert.ToByte(it);
        public static ushort To_U16(this object it) => Convert.ToUInt16(it);
        public static uint To_U32(this object it) => Convert.ToUInt32(it);
        public static ulong To_U64(this object it) => Convert.ToUInt64(it);
        public static float To_F32(this object it) => Convert.ToSingle(it);
        public static double To_F64(this object it) => Convert.ToDouble(it);

        public static char To_lower(this char it) => char.ToLower(it);
        public static char To_upper(this char it) => char.ToUpper(it);

        public static bool Is_lower(this char it) => char.IsLower(it);
        public static bool Is_upper(this char it) => char.IsUpper(it);

        public static bool Is_letter(this char it) => char.IsLetter(it);
        public static bool Is_digit(this char it) => char.IsDigit(it);
        public static bool Is_letter_or_digit(this char it) => char.IsLetterOrDigit(it);

        public static bool Is_number(this char it) => char.IsNumber(it);
        public static bool Is_symbol(this char it) => char.IsSymbol(it);
        public static bool Is_white_space(this char it) => char.IsWhiteSpace(it);
        public static bool Is_control(this char it) => char.IsControl(it);

        // String
        public static bool Not_empty(this string it) => !it.Is_empty();
        public static bool Is_empty(this string it) => string.IsNullOrEmpty(it);
        public static string Sub_Str(this string it, int startIndex, int length) => it.Substring(startIndex, length);
        public static string Sub_Str(this string it, int startIndex) => it.Substring(startIndex);
        public static int Size(this string it) => it.Length;
        public static int Last_index(this string it) => it.Length - 1;

        public static int Find_first(this string it, Func<char, bool> fn)
        {
            for (int i = 0; i < it.Size(); i++)
            {
                if (fn(it[i]))
                {
                    return i;
                }
            }
            return 0;
        }

        public static int First_index_of(this string it, string value, StringComparison comparisonType = StringComparison.Ordinal) => it.IndexOf(value, comparisonType);
        public static int First_index_of(this string it, string value, int startIndex, StringComparison comparisonType = StringComparison.Ordinal) => it.IndexOf(value, startIndex, comparisonType);
        public static int First_index_of(this string it, string value, int startIndex, int count, StringComparison comparisonType = StringComparison.Ordinal) => it.IndexOf(value, startIndex, count, comparisonType);

        public static int Last_index_of(this string it, string value, StringComparison comparisonType = StringComparison.Ordinal) => it.LastIndexOf(value, comparisonType);
        public static int Last_index_of(this string it, string value, int startIndex, StringComparison comparisonType = StringComparison.Ordinal) => it.LastIndexOf(value, startIndex, comparisonType);
        public static int Last_index_of(this string it, string value, int startIndex, int count, StringComparison comparisonType = StringComparison.Ordinal) => it.LastIndexOf(value, startIndex, count, comparisonType);


        public static string[] Split(this string it, string[] separator, StringSplitOptions options = StringSplitOptions.None) => it.Split(separator, options);
        public static string Slice(this string it, int? startIndex, int? endIndex)
        {
            if (startIndex == null && endIndex == null)
            {
                return it;
            }
            else if (endIndex == null)
            {
                return it.Sub_Str(startIndex ?? 0, it.Last_index() - startIndex ?? 0);
            }
            else // (startIndex == null)
            {
                return it.Sub_Str(0, it.Last_index() - endIndex ?? 0);
            }
        }

        public static string Join(this string it, string j) => string.Join(j, it);

        public static string To_Str(this string it, string format) => it;
        public static byte[] To_Bytes(this string it) => Encoding.UTF8.GetBytes(it);

        public static sbyte To_I8_from_Base(this string it, int fromBase) => Convert.ToSByte(it, fromBase);
        public static short To_I16_from_Base(this string it, int fromBase) => Convert.ToInt16(it, fromBase);
        public static int To_I32_from_Base(this string it, int fromBase) => Convert.ToInt32(it, fromBase);
        public static long To_I64_from_Base(this string it, int fromBase) => Convert.ToInt64(it, fromBase);
        public static byte To_U8_from_Base(this string it, int fromBase) => Convert.ToByte(it, fromBase);
        public static ushort To_U16_from_Base(this string it, int fromBase) => Convert.ToUInt16(it, fromBase);
        public static uint To_U32_from_Base(this string it, int fromBase) => Convert.ToUInt32(it, fromBase);
        public static ulong To_U64_from_Base(this string it, int fromBase) => Convert.ToUInt64(it, fromBase);
        public static byte[] To_Bytes_by_Base64(this string it) => Convert.FromBase64String(it);

        public static string To_Str(this byte[] it) => Encoding.UTF8.GetString(it);
        public static string To_hex(this byte[] it) => BitConverter.ToString(it, 0).Replace("-", string.Empty);

        public static string To_lower_hex(this byte[] it) => it.To_hex().ToLower();
        public static string To_upper_hex(this byte[] it) => it.To_hex();
        public static string To_Base64_Str(this byte[] it) => Convert.ToBase64String(it, 0, it.Length);
        public static byte[] From_Base64_Str(this string it) => Convert.FromBase64String(it);

        public static byte[] Sub_Bytes(this byte[] it, int start, int length) => it.Skip(start).Take(length).ToArray();
    }
}
