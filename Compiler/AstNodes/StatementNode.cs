using Compiler.AstNodes;
using Compiler.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.AstNodes
{
    public abstract record class StatementNode : Node
    {
        internal StatementNode() { }
    }

    public sealed record class VariableStatementNode(
        Identifier Id,
        ExpressionNode InitValue) : StatementNode()
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class ExpressionStatementNode(ExpressionNode Expression) : StatementNode()
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class BreakStatementNode() : StatementNode()
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class ContinueStatementNode() : StatementNode()
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }
}
