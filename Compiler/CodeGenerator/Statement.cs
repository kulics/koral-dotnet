using Compiler.AstNodes;
using LLVMSharp.Interop;

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

        public override void Visit(BreakStatementNode node)
        {
            if (loopStack.TryPeek(out var block) && currentFunctionName != null)
            {
                var def = module.GetNamedFunction(currentFunctionName);
                basicBlockCount++;
                var breakBlock = def.AppendBasicBlock(basicBlockCount.ToString());
                basicBlockCount++;
                var endBlock = def.AppendBasicBlock(basicBlockCount.ToString());

                builder.BuildBr(breakBlock);

                builder.PositionAtEnd(breakBlock);
                builder.BuildBr(block.loopOut);

                builder.PositionAtEnd(endBlock);
            }
            else
            {
                throw new CompilingCheckException("internal error");
            }
        }

        public override void Visit(ContinueStatementNode node)
        {
            if (loopStack.TryPeek(out var block) && currentFunctionName != null)
            {
                var def = module.GetNamedFunction(currentFunctionName);
                basicBlockCount++;
                var continueBlock = def.AppendBasicBlock(basicBlockCount.ToString());
                basicBlockCount++;
                var endBlock = def.AppendBasicBlock(basicBlockCount.ToString());

                builder.BuildBr(continueBlock);

                builder.PositionAtEnd(continueBlock);
                builder.BuildBr(block.loopIn);

                builder.PositionAtEnd(endBlock);
            }
            else
            {
                throw new CompilingCheckException("internal error");
            }
        }

        public override void Visit(ReturnStatementNode node)
        {
            if (currentFunctionName != null)
            {
                var def = module.GetNamedFunction(currentFunctionName);
                basicBlockCount++;
                var endBlock = def.AppendBasicBlock(basicBlockCount.ToString());

                if (node.Expression is var e and not null)
                {
                    e.Accept(this);
                    var retValue = valueStack.Pop();
                    builder.BuildRet(retValue);
                }
                else
                {
                    builder.BuildRet(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int1, 0));
                }

                builder.PositionAtEnd(endBlock);
            }
            else
            {
                throw new CompilingCheckException("internal error");
            }
        }
    }
}
