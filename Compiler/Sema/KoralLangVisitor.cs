using System.Collections.Generic;
using Compiler.Types;
using System.Linq;
using Compiler.AstNodes;
using static Compiler.KoralParser;

namespace Compiler.Sema
{
    public partial class KoralLangVisitor : KoralParserBaseVisitor<object>
    {
        readonly Stack<Scope> scopes = [];

        public KoralLangVisitor()
        {
            var rootScope = new Scope();
            rootScope.PushType(BuiltinTypes.Void);
            rootScope.PushType(BuiltinTypes.Int);
            rootScope.PushType(BuiltinTypes.Bool);
            scopes.Push(rootScope);
        }

        public override ProgramNode VisitProgram(ProgramContext context) =>
            new(VisitModuleDeclaration(context.moduleDeclaration()),
                context.globalDeclaration().Select(VisitGlobalDeclaration).ToList());

        private static string VisitIdentifier(VariableIdentifierContext context) => context.LowerIdentifier().GetText();

        private static string VisitIdentifier(TypeIdentifierContext context) => context.UpperIdentifier().GetText();

        void PushScope() => scopes.Push(new());

        void PopScope() => scopes.Pop();

        bool IsRedefineId(string id) => scopes.Peek().GetId(id) is not null;

        void PushId(Identifier id) => scopes.Peek().PushId(id);

        Identifier? GetId(string id)
        {
            foreach (var scope in scopes)
            {
                var target = scope.GetId(id);
                if (target is not null)
                {
                    return target;
                }
            }
            return null;
        }

        void PushKoralType(KoralType typeInfo) => scopes.Peek().PushType(typeInfo);

        bool IsRedefineType(string type) => scopes.Peek().GetTypeInfo(type) is not null;

        KoralType? GetKoralType(string id)
        {
            foreach (var scope in scopes)
            {
                var target = scope.GetTypeInfo(id);
                if (target is not null)
                {
                    return target;
                }
            }
            return null;
        }
    }
}