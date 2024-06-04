using Compiler.AstNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.AstNodes
{
    public abstract class StatementNode : Node
    {
        internal StatementNode() { }
    }

    public sealed class VariableStatementNode(
        Identifier id,
        ExpressionNode initValue) : StatementNode()
    {
        public Identifier Id { get; } = id;
        public ExpressionNode InitValue { get; } = initValue;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class ExpressionStatementNode(ExpressionNode expr) : StatementNode()
    {
        public ExpressionNode Expression { get; } = expr;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }
}
