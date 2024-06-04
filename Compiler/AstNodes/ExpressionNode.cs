using Compiler.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.AstNodes
{
    public class ExpressionNode(KoralType typeInfo) : Node()
    {
        public KoralType Type { get; } = typeInfo;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class IdentifierExpressionNode(Identifier id) : ExpressionNode(id.Type)
    {
        public Identifier Id { get; } = id;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public abstract class LiteralExpressionNode(KoralType type) : ExpressionNode(type)
    {
    }

    public sealed class IntegerLiteralExpressionNode(long value) : LiteralExpressionNode(BuiltinTypes.Int)
    {
        public long Value { get; } = value;

        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class BooleanLiteralExpressionNode(bool value) : LiteralExpressionNode(BuiltinTypes.Bool)
    {
        public bool Value { get; } = value;

        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public enum CalculativeOperator
    {
        Add, Sub, Mul, Div, Mod
    }

    public sealed class CalculativeExpressionNode(
        ExpressionNode lhs,
        ExpressionNode rhs,
        CalculativeOperator operatorName,
        KoralType type
    ) : ExpressionNode(type)
    {
        public ExpressionNode Lhs { get; } = lhs;
        public ExpressionNode Rhs { get; } = rhs;
        public CalculativeOperator operatorName { get; } = operatorName;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public enum CompareOperator
    {
        Equal, NotEqual, Less, LessEqual, Greater, GreaterEqual
    }

    public sealed class CompareExpressionNode(
        ExpressionNode lhs,
        ExpressionNode rhs,
        CompareOperator operatorName
    ) : ExpressionNode(BuiltinTypes.Bool)
    {
        public ExpressionNode Lhs { get; } = lhs;
        public ExpressionNode Rhs { get; } = rhs;
        public CompareOperator OperatorName { get; } = operatorName;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public enum LogicOperator
    {
        And, Or
    }

    public sealed class LogicExpressionNode(
        ExpressionNode lhs,
        ExpressionNode rhs,
        LogicOperator operatorName
    ) : ExpressionNode(BuiltinTypes.Bool)
    {
        public ExpressionNode Lhs { get; } = lhs;
        public ExpressionNode Rhs { get; } = rhs;
        public LogicOperator operatorName { get; } = operatorName;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class BlockExpressionNode(
        List<StatementNode> stats,
        ExpressionNode? expr) : ExpressionNode(expr?.Type ?? BuiltinTypes.Void)
    {
        public List<StatementNode> Stats { get; } = stats;
        public ExpressionNode? Expr { get; } = expr;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class FunctionCallExpressionNode(
        Identifier id,
        ExpressionNode expr,
        List<KoralType> types,
        List<ExpressionNode> args,
        KoralType type) : ExpressionNode(type)
    {
        public Identifier Id { get; } = id;
        public ExpressionNode Expr { get; } = expr;
        public List<KoralType> Types { get; } = types;
        public List<ExpressionNode> Args { get; } = args;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class AssignmentExpressionNode(
        Identifier id,
        ExpressionNode newValue) : ExpressionNode(BuiltinTypes.Void)
    {
        public Identifier Id { get; } = id;
        public ExpressionNode Value { get; } = newValue;
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }
}
