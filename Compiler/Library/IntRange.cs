using System.Collections;
using System.Collections.Generic;

namespace Compiler.Library
{
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

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
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
}
