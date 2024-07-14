using static Compiler.KoralParser;
using Compiler.Library;
using Antlr4.Runtime.Misc;
using Compiler.AstNodes;
using System.Security.Cryptography.X509Certificates;
using Compiler.Types;
using System;
using System.Linq;

namespace Compiler.Sema
{
    public partial class KoralLangVisitor
    {
        public override StatementNode VisitStatement(StatementContext context)
        {
            var stat = context.GetChild(0);
            return stat switch
            {
                VariableDeclarationContext it => VisitVariableDeclaration(it),
                ExpressionStatementContext it => VisitExpressionStatement(it),
                BreakStatementContext it => VisitBreakStatement(it),
                ContinueStatementContext it => VisitContinueStatement(it),
                ReturnStatementContext it => VisitReturnStatement(it),
                _ => throw new CompilingCheckException()
            };
        }
        public override VariableStatementNode VisitVariableDeclaration(VariableDeclarationContext context)
        {
            var idName = VisitIdentifier(context.variableIdentifier());
            if (IsRedefineId(idName))
            {
                throw new CompilingCheckException($"identifier: '{idName}' is redefined");
            }
            var expr = VisitExpressionWithTerminator(context.expressionWithTerminator());
            KoralType typeInfo;
            if (context.type() is null)
            {
                typeInfo = expr.Type;
            }
            else
            {
                var targetTypeInfo = CheckTypeNode(VisitType(context.type()));
                if (!CanAssign(expr.Type, targetTypeInfo))
                {
                    throw new CompilingCheckException($"the type of init value '{expr.Type.Name}' is not confirm '{targetTypeInfo.Name}'");
                }
                typeInfo = targetTypeInfo;
            }
            var id = new Identifier(idName, typeInfo, context.Mut() != null ? IdentifierKind.Mutable : IdentifierKind.Immutable);
            PushId(id);
            return new(id, expr);
        }

        public override ExpressionStatementNode VisitExpressionStatement(ExpressionStatementContext context)
        {
            ExpressionNode node;
            if (context.expression() is var e and not null)
            {
                node = VisitExpression(e);
            }
            else
            {
                node = VisitExpressionWithBlock(context.expressionWithBlock());
            }
            return new(node);
        }

        public override BreakStatementNode VisitBreakStatement(BreakStatementContext context)
        {
            if (scopes.Any(i => i.IsLoop))
            {
                return new BreakStatementNode();
            }
            throw new CompilingCheckException("here is not in a loop scope");
        }
        public override ContinueStatementNode VisitContinueStatement(ContinueStatementContext context)
        {
            if (scopes.Any(i => i.IsLoop))
            {
                return new ContinueStatementNode();
            }
            throw new CompilingCheckException("here is not in a loop scope");
        }

        public override ReturnStatementNode VisitReturnStatement(ReturnStatementContext context)
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
                    return new ReturnStatementNode(retValue);
                }
                // return value block
                if (context.expressionWithBlock() is var b and not null)
                {
                    var retValue = VisitExpressionWithBlock(b);
                    if (!CanAssign(retValue.Type, type))
                    {
                        throw new CompilingCheckException($"the type of return value {retValue.Type} is not {type}");
                    }
                    return new ReturnStatementNode(retValue);
                }
                // return void
                if (!CanAssign(BuiltinTypes.Void, type))
                {
                    throw new CompilingCheckException($"the type of return value {BuiltinTypes.Void} is not {type}");
                }
                return new ReturnStatementNode(null);
            }
            throw new CompilingCheckException("here is not in a function body scope");
        }
    }
}
