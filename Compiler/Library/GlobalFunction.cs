namespace Compiler.Library
{
    public static class GlobalFunction
    {
        public static IntRangeClose UpTo(this int begin, int end) => new(begin, end, true);

        public static IntRangeClose DownTo(this int begin, int end) => new(begin, end, false);

        public static IntRange UpUntil(this int begin, int end) => new(begin, end, true);

        public static IntRange DownUntil(this int begin, int end) => new(begin, end, false);

        

        public static IntRange Range(int begin, int end, int step)
        {
            if (step > 0)
            {
                return new IntRange(begin, end, true).Step(step);
            }
            return new IntRange(begin, end, false).Step(-step);
        }

        public static IntRangeClose RangeClose(int begin, int end, int step)
        {
            if (step > 0)
            {
                return new IntRangeClose(begin, end, true).Step(step);
            }
            return new IntRangeClose(begin, end, false).Step(-step);
        }

        public static void Todo(string it) => throw new(it);
    }
}
