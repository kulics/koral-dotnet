using Compiler.AstNodes;

namespace Compiler.CodeGenerator
{
    public partial class LLVMGeneratorVisitor
    {
        public override void Visit(StatementNode node) => node.Accept(this);
        public override void Visit(VariableStatementNode node)
        {
            node.InitValue.Accept(this);
            var initValue = valueStack.Pop();
            if (node.Id.Kind == IdentifierKind.Immutable)
            {
                namedValues[node.Id] = new IdentifierValue(false, initValue);
            }
            else
            {
                var variable = builder.BuildAlloca(FindType(node.Id.Type));
                builder.BuildStore(initValue, variable);
                namedValues[node.Id] = new IdentifierValue(true, variable);
            }
        }
        public override void Visit(ExpressionStatementNode node)
        {
            node.Expression.Accept(this);
            valueStack.Pop();
        }
    }
}
