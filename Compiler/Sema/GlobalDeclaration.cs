using Antlr4.Runtime.Misc;
using Compiler.Library;
using Compiler.AstNodes;
using Compiler.Types;
using System.Collections.Generic;
using static Compiler.Parser.KoralParser;
using System;

namespace Compiler.Sema
{
    public partial class KoralLangVisitor
    {
        public override ModuleDeclarationNode VisitModuleDeclaration(ModuleDeclarationContext context) =>
            new(VisitIdentifier(context.variableIdentifier()));

        void PreVisitGlobalDeclaration(GlobalDeclarationContext context)
        {
            var declaration = context.GetChild(0);
            switch (declaration)
            {
                case GlobalVariableDeclarationContext it:
                    if (it.type() is not null)
                    {
                        var idName = VisitIdentifier(it.variableIdentifier());
                        if (IsRedefineId(idName))
                        {
                            throw new CompilingCheckException($"identifier: '{idName}' is redefined");
                        }
                        var type = CheckTypeNode(VisitType(it.type()));
                        var id = new Identifier(idName, type, it.Mut() != null ? IdentifierKind.Mutable : IdentifierKind.Immutable);
                        PushId(id);
                    }
                    break;
                case GlobalFunctionDeclarationContext it:
                    if (it.type() is not null)
                    {
                        var idName = VisitIdentifier(it.variableIdentifier());
                        if (IsRedefineId(idName))
                        {
                            throw new CompilingCheckException($"identifier: '{idName}' is redefined");
                        }
                        if (it.typeParameterList() is not null)
                        {
                            throw new CompilingCheckException();
                        }
                        var returnType = CheckTypeNode(VisitType(it.type()));
                        var parameters = VisitParameterList(it.parameterList());
                        var type = new FunctionType(parameters.Map(i => i.Type), returnType);
                        var id = new Identifier(idName, type, IdentifierKind.Immutable);
                        PushId(id);
                    }
                    break;
                case GlobalRecordDeclarationContext it:
                    {
                        var idName = VisitIdentifier(it.typeIdentifier());
                        if (IsRedefineId(idName) && IsRedefineType(idName))
                        {
                            throw new CompilingCheckException($"identifier: '{idName}' is redefined");
                        }
                        if (it.typeParameterList() is not null)
                        {
                            throw new NotImplementedException();
                        }
                        var type = new RecordType(idName, [], idName);
                        PushType(type);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public override DeclarationNode VisitGlobalDeclaration(GlobalDeclarationContext context)
        {
            var declaration = context.GetChild(0);
            return declaration switch
            {
                GlobalVariableDeclarationContext it => VisitGlobalVariableDeclaration(it),
                GlobalFunctionDeclarationContext it => VisitGlobalFunctionDeclaration(it),
                GlobalRecordDeclarationContext it => VisitGlobalRecordDeclaration(it),
                GlobalInterfaceDeclarationContext => throw new NotImplementedException(),
                GlobalExtensionDeclarationContext => throw new NotImplementedException(),
                GlobalSumTypeDeclarationContext => throw new NotImplementedException(),
                _ => throw new CompilingCheckException()
            };
        }

        public override GlobalVariableDeclarationNode VisitGlobalVariableDeclaration(GlobalVariableDeclarationContext context)
        {
            var expr = VisitExpressionWithTerminator(context.expressionWithTerminator());
            var idName = VisitIdentifier(context.variableIdentifier());
            if (context.type() is null)
            {
                if (IsRedefineId(idName))
                {
                    throw new CompilingCheckException($"identifier: '{idName}' is redefined");
                }

                var typeInfo = expr.Type;
                var id = new Identifier(idName, typeInfo, context.Mut() != null ? IdentifierKind.Mutable : IdentifierKind.Immutable);
                PushId(id);
                return new(id, expr);
            }
            else
            {
                var id = GetId(idName) ?? throw new CompilingCheckException("internal error");
                if (!CanAssign(expr.Type, id.Type))
                {
                    throw new CompilingCheckException($"the type of init value '{expr.Type.Name}' is not confirm '{id.Type.Name}'");
                }
                return new(id, expr);
            }
        }

        public override GlobalFunctionDeclarationNode VisitGlobalFunctionDeclaration(GlobalFunctionDeclarationContext context)
        {
            var idName = VisitIdentifier(context.variableIdentifier());
            if (context.typeParameterList() is not null)
            {
                throw new CompilingCheckException();
            }
            if (context.type() is null)
            {
                if (IsRedefineId(idName))
                {
                    throw new CompilingCheckException($"identifier: '{idName}' is redefined");
                }
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
                // todo: 没有声明返回类型怎么支持 内部的 return 语句？
                PushScope();
                var expr = VisitExpressionWithTerminator(context.expressionWithTerminator());
                var returnType = expr.Type;
                PopScope();
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
                var id = GetId(idName) ?? throw new CompilingCheckException("internal error");

                PushScope();
                foreach (var item in parameters)
                {
                    if (IsRedefineId(item.Name))
                    {
                        throw new CompilingCheckException($"identifier: '{idName}' is redefined");
                    }
                    PushId(item);
                }
                PushScope(isFuncBody: returnType);
                var expr = VisitExpressionWithTerminator(context.expressionWithTerminator());
                if (!CanAssign(expr.Type, returnType))
                {
                    throw new CompilingCheckException($"the return is '{returnType.Name}', but find '{expr.Type.Name}'");
                }
                PopScope();
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

        public override GlobalRecordDeclarationNode VisitGlobalRecordDeclaration(GlobalRecordDeclarationContext context)
        {
            var idName = VisitIdentifier(context.typeIdentifier());
            if (GetType(idName) is not null and RecordType type)
            {
                var fieldList = VisitFieldList(context.fieldList());
                type.Fields.AddRange(fieldList);
                return new(type, [], fieldList, [], null);
            }
            throw new CompilingCheckException("internal error");
        }

        public override List<Identifier> VisitFieldList(FieldListContext context) => context.field().Map(VisitField);
        public override Identifier VisitField(FieldContext context)
        {
            var id = VisitIdentifier(context.variableIdentifier());
            var type = CheckTypeNode(VisitType(context.type()));
            return new(id, type, context.Mut() is null ? IdentifierKind.Immutable : IdentifierKind.Mutable);
        }
    }
}
