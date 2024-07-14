using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Types;

namespace Compiler.AstNodes
{
    public sealed class Identifier(string name, KoralType type, IdentifierKind kind = IdentifierKind.Immutable)
    {
        public string Name { get; } = name;
        public KoralType Type { get; } = type;
        public IdentifierKind Kind { get; } = kind;
    }

    public enum IdentifierKind
    {
        Immutable, Mutable
    }
}
