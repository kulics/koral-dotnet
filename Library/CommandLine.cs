using System;

namespace Library
{
    public static class Cmd
    {
        public static void Print(params object[] paramList)
        {
            var context = string.Join("", paramList);
            if (paramList.Length > 0 && paramList[paramList.Length - 1] as string == "")
            {
                Console.Write(context);
                return;
            }
            Console.WriteLine(context);
        }

        public static string Read() => Console.ReadLine();

        public static void Clear() => Console.Clear();
    }
}
