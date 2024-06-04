using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Library
{
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

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
