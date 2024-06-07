using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Types
{
    public static class BuiltinTypes
    {
        public static readonly RecordType Void = new("Void", [], "Void");
        public static readonly RecordType Bool = new("Bool", [], "Bool");
        public static readonly RecordType Int = new("Int", [], "Int");
        public static readonly RecordType Nothing = new("Nothing", [], "Nothing");

        public static IEnumerable<RecordType> Enumerate()
        {
            yield return Void;
            yield return Bool;
            yield return Int;
            yield return Nothing;
        }
    }
}
