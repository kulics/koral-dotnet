using Compiler.AstNodes;
using Compiler.Types;
using System.Linq;
using static Compiler.KoralParser;

namespace Compiler.Sema
{
    public partial class KoralLangVisitor
    {
        public override TypeNode VisitType(TypeContext context)
        {
            if (context.functionType() is not null)
            {
                throw new CompilingCheckException();
            }
            return new NominalTypeNode(VisitIdentifier(context.typeIdentifier()));
        }

        KoralType CheckTypeNode(TypeNode node) => node switch
        {
            NominalTypeNode it => CheckNominalTypeNode(it),
            _ => throw new CompilingCheckException(),
        };

        KoralType CheckNominalTypeNode(NominalTypeNode node)
        {
            var targetType = GetType(node.Id);
            return targetType switch
            {
                null => throw new CompilingCheckException($"type: '{node.Id}' is undefined"),
                _ => targetType,
            };
        }
        bool CanAssign(KoralType rhs, KoralType lhs)
        {
            return rhs.TypeEquals(lhs);
        }
    }
}
