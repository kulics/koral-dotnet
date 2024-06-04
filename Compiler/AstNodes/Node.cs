using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Compiler.AstNodes
{
    public abstract class Node
    {
        internal Node() { }
        public virtual void Accept(NodeVisitor visitor) => throw new CompilingCheckException();
    }
    public class CompilingCheckException(string msg = "not implemention") : Exception(msg) {}

    public sealed class ProgramNode(ModuleDeclarationNode module, List<DeclarationNode> declarations) : Node
    {
        public ModuleDeclarationNode Module { get; } = module;
        public List<DeclarationNode> Declarations { get; } = declarations;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }
}
