using Antlr4.Runtime.Tree;
using Compiler.AstNodes;
using Compiler.Types;
using System.Collections.Generic;
using System.Linq;
using static Compiler.KoralParser;
using Compiler.Library;
using Antlr4.Runtime.Misc;
using System.Xml.Linq;
using System;

namespace Compiler.Sema
{
    public partial class KoralLangVisitor
    {
        public override ExpressionNode VisitExpressionWithTerminator(ExpressionWithTerminatorContext context)
        {
            if (context.expression() is not null)
            {
                return VisitExpression(context.expression());
            }
            else
            {
                return VisitExpressionWithBlock(context.expressionWithBlock());
            }
        }

        public override ExpressionNode VisitExpression(ExpressionContext context) => context.ChildCount switch
        {
            1 => VisitSingleExpression(context.GetChild(0)),
            2 => VisitAccess(context),
            3 => VisitBinaryExpression(
                context.expression(0),
                context.GetChild(1),
                context.expression(1)),
            _ => throw new CompilingCheckException()
        };

        ExpressionNode VisitAccess(ExpressionContext context)
        {
            if (context.callSuffix() is not null)
            {
                return VisitFunctionCallExpression(context.expression(0), context.callSuffix());
            }
            else
            {
                throw new CompilingCheckException();
            }
        }

        ExpressionNode VisitSingleExpression(IParseTree expr) => expr switch
        {
            PrimaryExpressionContext it => VisitPrimaryExpression(it),
            BlockExpressionContext it => VisitBlockExpression(it),
            IfThenElseExpressionContext it => VisitIfThenElseExpression(it),
            IfThenExpressionContext it => VisitIfThenExpression(it),
            WhileThenExpressionContext it => VisitWhileThenExpression(it),
            WhileThenElseExpressionContext it => VisitWhileThenElseExpression(it),
            ExpressionWithBlockContext it => VisitExpressionWithBlock(it),
            AssignmentExpressionContext it => VisitAssignmentExpression(it),
            _ => throw new CompilingCheckException()
        };

        public override ExpressionNode VisitExpressionWithBlock(ExpressionWithBlockContext context)
        {
            var expr = context.GetChild(0);
            return expr switch
            {
                BlockExpressionContext it => VisitBlockExpression(it),
                IfThenExpressionWithBlockContext it => VisitIfThenExpressionWithBlock(it),
                IfThenElseExpressionWithBlockContext it => VisitIfThenElseExpressionWithBlock(it),
                WhileThenExpressionWithBlockContext it => VisitWhileThenExpressionWithBlock(it),
                WhileThenElseExpressionWithBlockContext it => VisitWhileThenElseExpressionWithBlock(it),
                AssignmentExpressionWithBlockContext it => VisitAssignmentExpressionWithBlock(it),
                _ => throw new CompilingCheckException()
            };
        }

        FunctionCallExpressionNode VisitFunctionCallExpression(ExpressionContext context, CallSuffixContext callContext)
        {
            var expr = VisitExpression(context);
            var exprType = expr.Type;
            if (exprType is FunctionType it)
            {
                //return ProcessFunctionCallExpression(expr, callContext.expression().Select(VisitExpression).ToList(), it);
                throw new NotImplementedException();
            }
            else
            {
                throw new CompilingCheckException("the type of expression is not a function");
            }
        }

        ExpressionNode VisitBinaryExpression(ExpressionContext lhs, IParseTree op, ExpressionContext rhs)
        {
            var lhsExpr = VisitExpression(lhs);
            var rhsExpr = VisitExpression(rhs);
            switch (op)
            {
                case AdditiveOperatorContext it:
                    {
                        CheckCalculateExpressionType(lhsExpr, rhsExpr);
                        CalculativeOperator symbol;
                        if (it.Add() is not null)
                        {
                            symbol = CalculativeOperator.Add;
                        }
                        else
                        {
                            symbol = CalculativeOperator.Sub;
                        }
                        return new CalculativeExpressionNode(lhsExpr, rhsExpr, symbol, lhsExpr.Type);
                    }
                case MultiplicativeOperatorContext it:
                    {
                        CheckCalculateExpressionType(lhsExpr, rhsExpr);
                        CalculativeOperator symbol;
                        if (it.Mul() is not null)
                        {
                            symbol = CalculativeOperator.Mul;
                        }
                        else if (it.Div() is not null)
                        {
                            symbol = CalculativeOperator.Div;
                        }
                        else
                        {
                            symbol = CalculativeOperator.Mod;
                        }
                        return new CalculativeExpressionNode(lhsExpr, rhsExpr, symbol, lhsExpr.Type);
                    }
                case CompareOperatorContext it:
                    {
                        CheckCompareExpressionType(lhsExpr, rhsExpr);
                        CompareOperator symbol;
                        if (it.EqualEqual() is not null)
                        {
                            symbol = CompareOperator.Equal;
                        }
                        else if (it.NotEqual() is not null)
                        {
                            symbol = CompareOperator.NotEqual;
                        }
                        else if (it.Less() is not null)
                        {
                            symbol = CompareOperator.Less;
                        }
                        else if (it.LessEqual() is not null)
                        {
                            symbol = CompareOperator.LessEqual;
                        }
                        else if (it.Greater() is not null)
                        {
                            symbol = CompareOperator.Greater;
                        }
                        else
                        {
                            symbol = CompareOperator.GreaterEqual;
                        }
                        return new CompareExpressionNode(lhsExpr, rhsExpr, symbol);
                    }

                case LogicAndOperatorContext it:
                    {
                        CheckLogicExpressionType(lhsExpr, rhsExpr);
                        return new LogicExpressionNode(lhsExpr, rhsExpr, LogicOperator.And);
                    }
                case LogicOrOperatorContext it:
                    {
                        CheckLogicExpressionType(lhsExpr, rhsExpr);
                        return new LogicExpressionNode(lhsExpr, rhsExpr, LogicOperator.And);
                    }
                default:
                    throw new CompilingCheckException();
            }
        }

        static void CheckCalculateExpressionType(ExpressionNode lhs, ExpressionNode rhs)
        {
            if (lhs.Type != BuiltinTypes.Int || rhs.Type != BuiltinTypes.Int)
            {
                throw new CompilingCheckException($"the type of value is not '{BuiltinTypes.Int.Name}'");
            }
        }
        static void CheckCompareExpressionType(ExpressionNode lhs, ExpressionNode rhs)
        {
            if (lhs.Type != BuiltinTypes.Int || rhs.Type != BuiltinTypes.Int)
            {
                throw new CompilingCheckException($"the type of value is not '{BuiltinTypes.Int.Name}'");
            }
        }
        static void CheckLogicExpressionType(ExpressionNode lhs, ExpressionNode rhs)
        {
            if (lhs.Type != BuiltinTypes.Bool || rhs.Type != BuiltinTypes.Bool)
            {
                throw new CompilingCheckException($"the type of value is not '{BuiltinTypes.Bool.Name}'");
            }
        }

        public override ExpressionNode VisitPrimaryExpression(PrimaryExpressionContext context)
        {
            if (context.literalExpression() is var it and not null)
            {
                return VisitLiteralExpression(it);
            }
            else if (context.variableIdentifier() is var it2 and not null)
            {
                var name = VisitIdentifier(it2);
                var id = GetId(name);
                if (id is null)
                {
                    throw new CompilingCheckException($"the identifier '{name}' is not define");
                }
                else
                {
                    return new IdentifierExpressionNode(id);
                }
            }
            else if (context.functionCallExpression() is var it3 and not null)
            {
                var name = VisitIdentifier(it3.variableIdentifier());
                var id = GetId(name) ?? throw new CompilingCheckException($"the identifier '{name}' is not define");
                if (id.Type is FunctionType type)
                {
                    return ProcessFunctionCall(new IdentifierExpressionNode(id), it3.expression().Map(VisitExpression), type);
                }
                throw new CompilingCheckException("the type of expression is not a function");
            }
            else
            {
                throw new CompilingCheckException();
            }
        }

        FunctionCallExpressionNode ProcessFunctionCall(IdentifierExpressionNode expr, List<ExpressionNode> callArgs, FunctionType type)
        {
            if (type.ParamTypes.Count != callArgs.Count)
            {
                throw new CompilingCheckException($"the size of args is {callArgs.Count}, but need ${type.ParamTypes.Count}");
            }
            var argList = new List<ExpressionNode>();
            foreach (var (i, v) in type.ParamTypes.WithIndex())
            {
                if (!CanAssign(callArgs[i].Type, v))
                {
                    throw new CompilingCheckException($"the type of args{i}: '{callArgs[i].Type.Name}' is not '${v.Name}'");
                }
                argList.Add(callArgs[i]);
            }
            return new(expr.Id, expr, [], argList, type.ReturnType);
        }

        public override LiteralExpressionNode VisitLiteralExpression(LiteralExpressionContext context)
        {
            if (context.integerExpression() is var i and not null)
            {
                return new IntegerLiteralExpressionNode(Convert.ToInt64(i.GetText()));
            }
            else if (context.boolExpression() is var b and not null)
            {
                return new BooleanLiteralExpressionNode(b.True() is not null);
            }
            else
            {
                throw new CompilingCheckException();
            }
        }

        public override BlockExpressionNode VisitBlockExpression(BlockExpressionContext context)
        {
            PushScope();
            var stats = new List<StatementNode>();
            foreach (var item in context.statement())
            {
                stats.Add(VisitStatement(item));
            }
            ExpressionNode? expr = null;
            if (context.expression() is var e and not null)
            {
                expr = VisitExpression(e);
            }
            var node = new BlockExpressionNode(stats, expr);
            PopScope();
            return node;
        }

        public override AssignmentExpressionNode VisitAssignmentExpression(AssignmentExpressionContext context) =>
            ProcessAssignmentExpression(context.variableIdentifier(), () => VisitExpression(context.expression()));

        public override AssignmentExpressionNode VisitAssignmentExpressionWithBlock(AssignmentExpressionWithBlockContext context) =>
            ProcessAssignmentExpression(context.variableIdentifier(), () => VisitExpressionWithBlock(context.expressionWithBlock()));
        AssignmentExpressionNode ProcessAssignmentExpression(VariableIdentifierContext context, Func<ExpressionNode> exprFunc)
        {
            var idName = VisitIdentifier(context);
            var id = GetId(idName) ?? throw new CompilingCheckException($"the identifier '{idName}' is not defined");
            if (id.Kind == IdentifierKind.Immutable)
            {
                throw new CompilingCheckException($"the identifier '{idName}' is not mutable");
            }
            var expr = exprFunc();
            if (!CanAssign(expr.Type, id.Type))
            {
                throw new CompilingCheckException($"the type of assign value '{expr.Type.Name}' is not confirm '${id.Type.Name}'");
            }
            return new(id, expr);
        }

        public override IfThenExpressionNode VisitIfThenExpression(IfThenExpressionContext context) =>
            ProcessIfThen(context.condition(), () => VisitExpressionOrControl(context.expressionOrControl()));

        public override IfThenExpressionNode VisitIfThenExpressionWithBlock(IfThenExpressionWithBlockContext context) =>
            ProcessIfThen(context.condition(), () => VisitExpressionWithBlock(context.expressionWithBlock()));

        public override IfThenElseExpressionNode VisitIfThenElseExpression(IfThenElseExpressionContext context) =>
            ProcessIfThenEsle(context.condition(), context.expression(0), () => VisitExpression(context.expression(1)));

        public override IfThenElseExpressionNode VisitIfThenElseExpressionWithBlock(IfThenElseExpressionWithBlockContext context) =>
            ProcessIfThenEsle(context.condition(), context.expression(), () => VisitExpressionWithBlock(context.expressionWithBlock()));

        public override ExpressionNode VisitExpressionOrControl(ExpressionOrControlContext context)
        {
            if (context.expression() is var e and not null)
            {
                return VisitExpression(e);
            }
            else if (context.breakExpression() is var b and not null)
            {
                return VisitBreakExpression(b);
            }
            else if (context.continueExpression() is var c and not null)
            {
                return VisitContinueExpression(c);
            }
            else if (context.returnExpression() is var r and not null)
            {
                return VisitReturnExpression(r);
            }
            throw new NotImplementedException();
        }

        public override ConditionNode VisitCondition(ConditionContext context)
        {
            if (context.pattern() is not null || context.And() is not null || context.Or() is not null)
            {
                throw new NotImplementedException();
            }
            var expr = VisitExpression(context.expression());
            if (expr.Type != BuiltinTypes.Bool)
            {
                throw new CompilingCheckException($"the type of if condition is '{expr.Type.Name}', but want '${BuiltinTypes.Bool.Name}'");
            }
            return new(expr);
        }

        IfThenExpressionNode ProcessIfThen(ConditionContext condContext, Func<ExpressionNode> thenFunc)
        {
            PushScope();
            var condition = VisitCondition(condContext);
            var thenBranch = thenFunc();
            PopScope();
            return new(condition, thenBranch);
        }

        IfThenElseExpressionNode ProcessIfThenEsle(ConditionContext condContext, ExpressionContext thenContext, Func<ExpressionNode> elseFunc)
        {
            PushScope();
            var condition = VisitCondition(condContext);
            var thenBranch = VisitExpression(thenContext);
            PopScope();
            var elseBranch = elseFunc();
            if (thenBranch.Type != elseBranch.Type)
            {
                throw new CompilingCheckException($"the type of then branch is '{thenBranch.Type.Name}', and the type of else branch is '${elseBranch.Type.Name}', they are not equal");
            }
            return new(condition, thenBranch, elseBranch, thenBranch.Type);
        }

        public override WhileThenExpressionNode VisitWhileThenExpression(WhileThenExpressionContext context) =>
            ProcessWhileThen(context.condition(), () => VisitExpressionOrControl(context.expressionOrControl()));
        public override WhileThenExpressionNode VisitWhileThenExpressionWithBlock(WhileThenExpressionWithBlockContext context) =>
            ProcessWhileThen(context.condition(), () => VisitExpressionWithBlock(context.expressionWithBlock()));

        WhileThenExpressionNode ProcessWhileThen(ConditionContext condContext, Func<ExpressionNode> thenFunc)
        {
            PushScope();
            var condition = VisitCondition(condContext);
            PushScope(isLoop: true);
            var thenBranch = thenFunc();
            PopScope();
            PopScope();
            return new(condition, thenBranch);
        }

        public override WhileThenElseExpressionNode VisitWhileThenElseExpression(WhileThenElseExpressionContext context) =>
            ProcessWhileThenElse(context.condition(), context.expression(0), () => VisitExpression(context.expression(1)));

        public override WhileThenElseExpressionNode VisitWhileThenElseExpressionWithBlock(WhileThenElseExpressionWithBlockContext context) =>
            ProcessWhileThenElse(context.condition(), context.expression(), () => VisitExpressionWithBlock(context.expressionWithBlock()));

        WhileThenElseExpressionNode ProcessWhileThenElse(ConditionContext condContext, ExpressionContext thenContext, Func<ExpressionNode> elseFunc)
        {
            PushScope();
            var condition = VisitCondition(condContext);
            PushScope(isLoop: true);
            var thenBranch = VisitExpression(thenContext);
            PopScope();
            var elseBranch = elseFunc();
            PopScope();
            return new(condition, thenBranch, elseBranch);
        }

        public override BlockExpressionNode VisitBreakExpression(BreakExpressionContext context)
        {
            if (scopes.Any(i => i.IsLoop))
            {
                return new BlockExpressionNode([new BreakStatementNode()], null);
            }
            throw new CompilingCheckException("here is not in a loop scope");
        }
        public override BlockExpressionNode VisitContinueExpression(ContinueExpressionContext context)
        {
            if (scopes.Any(i => i.IsLoop))
            {
                return new BlockExpressionNode([new ContinueStatementNode()], null);
            }
            throw new CompilingCheckException("here is not in a loop scope");
        }

        public override BlockExpressionNode VisitReturnExpression(ReturnExpressionContext context)
        {
            if (scopes.Map(i => i.IsFuncBody).First(i => i != null) is var type and not null)
            {
                // return value
                if (context.expression() is var e and not null)
                {
                    var retValue = VisitExpression(e);
                    if (!CanAssign(retValue.Type, type))
                    {
                        throw new CompilingCheckException($"the type of return value {retValue.Type} is not {type}");
                    }
                    return new BlockExpressionNode([new ReturnStatementNode(retValue)], null);
                }
                // return void
                if (!CanAssign(BuiltinTypes.Void, type))
                {
                    throw new CompilingCheckException($"the type of return value {BuiltinTypes.Void} is not {type}");
                }
                return new BlockExpressionNode([new ReturnStatementNode(null)], null);
            }
            throw new CompilingCheckException("here is not in a function body scope");
        }
    }
}
