using Compiler.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.AstNodes
{
    public record class ExpressionNode(KoralType Type) : Node()
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class IdentifierExpressionNode(Identifier Id) : ExpressionNode(Id.Type)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public abstract record class LiteralExpressionNode(KoralType Type) : ExpressionNode(Type)
    {
    }

    public sealed record class IntegerLiteralExpressionNode(long Value) : LiteralExpressionNode(BuiltinTypes.Int)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class BooleanLiteralExpressionNode(bool Value) : LiteralExpressionNode(BuiltinTypes.Bool)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public enum CalculativeOperator
    {
        Add, Sub, Mul, Div, Mod
    }

    public sealed record class CalculativeExpressionNode(
        ExpressionNode Lhs,
        ExpressionNode Rhs,
        CalculativeOperator OperatorName,
        KoralType Type
    ) : ExpressionNode(Type)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public enum CompareOperator
    {
        Equal, NotEqual, Less, LessEqual, Greater, GreaterEqual
    }

    public sealed record class CompareExpressionNode(
        ExpressionNode Lhs,
        ExpressionNode Rhs,
        CompareOperator OperatorName
    ) : ExpressionNode(BuiltinTypes.Bool)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public enum LogicOperator
    {
        And, Or
    }

    public sealed record class LogicExpressionNode(
        ExpressionNode Lhs,
        ExpressionNode Rhs,
        LogicOperator OperatorName
    ) : ExpressionNode(BuiltinTypes.Bool)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class BlockExpressionNode(
        List<StatementNode> Stats,
        ExpressionNode? Expr) : ExpressionNode(Expr?.Type ?? BuiltinTypes.Void)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class FunctionCallExpressionNode(
        Identifier Id,
        ExpressionNode Expr,
        List<KoralType> Types,
        List<ExpressionNode> Args,
        KoralType Type) : ExpressionNode(Type)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class MemberExpressionNode(
        ExpressionNode Expr,
        Identifier Member) : ExpressionNode(Member.Type)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class ConstructCallExpressionNode(
        RecordType Id,
        List<KoralType> Types,
        List<ExpressionNode> Args, KoralType Type) : ExpressionNode(Type)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class AssignmentExpressionNode(
            Identifier Id,
            ExpressionNode NewValue) : ExpressionNode(BuiltinTypes.Void)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class ConditionNode(ExpressionNode Expr) : Node
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class IfThenElseExpressionNode(
        ConditionNode Condition,
        ExpressionNode ThenBranch,
        ExpressionNode ElseBranch,
        KoralType Type) : ExpressionNode(Type)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class IfThenExpressionNode(
        ConditionNode Condition,
        ExpressionNode ThenBranch) : ExpressionNode(BuiltinTypes.Void)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed record class WhileThenExpressionNode(ConditionNode Condition,
        ExpressionNode ThenBranch) : ExpressionNode(BuiltinTypes.Void)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }
    public sealed record class WhileThenElseExpressionNode(ConditionNode Condition,
        ExpressionNode ThenBranch,
        ExpressionNode ElseBranch
        ) : ExpressionNode(BuiltinTypes.Void)
    {
        public override void Accept(NodeVisitor visitor) => visitor.Visit(this);
    }
}
