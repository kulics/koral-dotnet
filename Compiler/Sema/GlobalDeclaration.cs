using Antlr4.Runtime.Misc;
using Compiler.Library;
using Compiler.AstNodes;
using Compiler.Types;
using System.Collections.Generic;
using static Compiler.KoralParser;

namespace Compiler.Sema
{
    public partial class KoralLangVisitor
    {
        public override ModuleDeclarationNode VisitModuleDeclaration(ModuleDeclarationContext context) =>
            new(VisitIdentifier(context.variableIdentifier()));

        public override DeclarationNode VisitGlobalDeclaration(GlobalDeclarationContext context)
        {
            var declaration = context.GetChild(0);
            return declaration switch
            {
                GlobalVariableDeclarationContext it => VisitGlobalVariableDeclaration(it),
                GlobalFunctionDeclarationContext it => VisitGlobalFunctionDeclaration(it),
                GlobalRecordDeclarationContext => throw new CompilingCheckException(),
                GlobalInterfaceDeclarationContext => throw new CompilingCheckException(),
                GlobalExtensionDeclarationContext => throw new CompilingCheckException(),
                GlobalSumTypeDeclarationContext => throw new CompilingCheckException(),
                _ => throw new CompilingCheckException()
            };
        }

        public override GlobalVariableDeclarationNode VisitGlobalVariableDeclaration(GlobalVariableDeclarationContext context)
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
                if (CannotAssign(expr.Type, targetTypeInfo))
                {
                    throw new CompilingCheckException($"the type of init value '{expr.Type.Name}' is not confirm '{targetTypeInfo.Name}'");
                }
                typeInfo = targetTypeInfo;
            };
            var id = new Identifier(idName, typeInfo, context.Mut() != null ? IdentifierKind.Mutable : IdentifierKind.Immutable);
            PushId(id);
            return new(id, expr);
        }

        public override GlobalFunctionDeclarationNode VisitGlobalFunctionDeclaration(GlobalFunctionDeclarationContext context)
        {
            var idName = VisitIdentifier(context.variableIdentifier());
            if (IsRedefineId(idName))
            {
                throw new CompilingCheckException($"identifier: '{idName}' is redefined");
            }
            if (context.typeParameterList() is not null)
            {
                throw new CompilingCheckException();
            }
            if (context.type() is null)
            {
                var parameters = VisitParameterList(context.parameterList());
                PushScope();
                foreach (var item in parameters)
                {
                    if (IsRedefineId(item.Name))
                    {
                        throw new CompilingCheckException($"identifier: '{idName}' is redefined");
                    }
                    PushId(item);
                }
                var expr = VisitExpressionWithTerminator(context.expressionWithTerminator());
                var returnType = expr.Type;
                PopScope();
                var type = new FunctionType(parameters.Map(i => i.Type), returnType);
                var id = new Identifier(idName, type, IdentifierKind.Immutable);
                PushId(id);
                return new(
                    id,
                    [],
                    parameters.Map(i => new ParameterDeclarationNode(i, i.Type)),
                    returnType,
                    expr);
            }
            else
            {
                var returnType = CheckTypeNode(VisitType(context.type()));
                var parameters = VisitParameterList(context.parameterList());
                var type = new FunctionType(parameters.Map(i => i.Type), returnType);
                var id = new Identifier(idName, type, IdentifierKind.Immutable);
                PushId(id);
                PushScope();
                foreach (var item in parameters)
                {
                    if (IsRedefineId(item.Name))
                    {
                        throw new CompilingCheckException($"identifier: '{idName}' is redefined");
                    }
                    PushId(item);
                }
                var expr = VisitExpressionWithTerminator(context.expressionWithTerminator());
                if (CannotAssign(expr.Type, returnType))
                {
                    throw new CompilingCheckException($"the return is '{returnType.Name}', but find '{expr.Type.Name}'");
                }
                PopScope();
                return new(
                    id,
                    [],
                    parameters.Map(i => new ParameterDeclarationNode(i, i.Type)),
                    returnType,
                    expr);
            }
        }

        public override List<Identifier> VisitParameterList(ParameterListContext context)
        {
            var list = new List<Identifier>(context.parameter().Length);
            foreach (var item in context.parameter())
            {
                list.Add(VisitParameter(item));
            }
            return list;
        }

        public override Identifier VisitParameter(ParameterContext context)
        {
            var id = VisitIdentifier(context.variableIdentifier());
            var type = CheckTypeNode(VisitType(context.type()));
            return new(id, type);
        }
    }
}
