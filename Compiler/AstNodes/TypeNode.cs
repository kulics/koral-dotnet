using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.AstNodes
{
    public abstract class TypeNode
    {
        internal TypeNode() { }
    }

    public sealed class NominalTypeNode(string id): TypeNode
    {
        public string Id { get; } = id;
    }
}
