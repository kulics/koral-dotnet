using Compiler.AstNodes;
using Compiler.Types;
using LLVMSharp.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.CodeGenerator
{
    public partial class LLVMGeneratorVisitor
    {
        public override void Visit(ExpressionNode node) => node.Accept(this);
        public override void Visit(IdentifierExpressionNode node)
        {
            var idValue = namedValues[node.Id];
            if (idValue.IsPtr)
            {
                var loadValue = builder.BuildLoad2(FindType(node.Id.Type), idValue.Value);
                valueStack.Push(loadValue);
            }
            else
            {
                valueStack.Push(idValue.Value);
            }
        }
        public override void Visit(LiteralExpressionNode node)
        {
            if (node is IntegerLiteralExpressionNode i)
            {
                valueStack.Push(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, (ulong)i.Value));
            }
            else if (node is BooleanLiteralExpressionNode b)
            {
                valueStack.Push(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int1, (ulong)(b.Value ? 1 : 0)));
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        public override void Visit(CalculativeExpressionNode node)
        {
            node.Lhs.Accept(this);
            var lhs = valueStack.Pop();
            node.Rhs.Accept(this);
            var rhs = valueStack.Pop();
            valueStack.Push(node.OperatorName switch
            {
                CalculativeOperator.Add =>
                    builder.BuildAdd(lhs, rhs),
                CalculativeOperator.Sub =>
                    builder.BuildSub(lhs, rhs),
                CalculativeOperator.Mul =>
                    builder.BuildMul(lhs, rhs),
                CalculativeOperator.Div =>
                    builder.BuildSDiv(lhs, rhs),
                _ =>
                    throw new NotImplementedException()
            });
        }
        public override void Visit(CompareExpressionNode node) => throw new NotImplementedException();
        public override void Visit(LogicExpressionNode node) => throw new NotImplementedException();
        public override void Visit(BlockExpressionNode node)
        {
            foreach (var item in node.Stats)
            {
                item.Accept(this);
            }
            if (node.Expr is not null)
            {
                node.Expr.Accept(this);
            }
            else
            {
                valueStack.Push(NullValue);
            }
        }
        public override void Visit(FunctionCallExpressionNode node)
        {
            var fn = module.GetNamedFunction(node.Id.Name);
            var functype = funcTypes[node.Id.Name];

            var args = new List<LLVMValueRef>();
            foreach (var arg in node.Args)
            {
                arg.Accept(this);
                args.Add(valueStack.Pop());
            }
            var retValue = builder.BuildCall2(functype, fn, [.. args]);
            valueStack.Push(retValue);
        }
        public override void Visit(AssignmentExpressionNode node)
        {
            var variable = namedValues[node.Id];
            node.NewValue.Accept(this);
            var newValue = valueStack.Pop();
            builder.BuildStore(newValue, variable.Value);
            valueStack.Push(NullValue);
        }
        public override void Visit(ConditionNode node)
        {
            node.Expr.Accept(this);
        }
        public override void Visit(IfThenElseExpressionNode node)
        {
            if (currentFunctionName == null)
            {
                throw new NotImplementedException();
            }
            var def = module.GetNamedFunction(currentFunctionName);
            basicBlockCount++;
            var thenBlock = def.AppendBasicBlock(basicBlockCount.ToString());
            basicBlockCount++;
            var elseBlock = def.AppendBasicBlock(basicBlockCount.ToString());
            basicBlockCount++;
            var endBlock = def.AppendBasicBlock(basicBlockCount.ToString());

            var retType = FindType(node.Type);
            var tempValue = builder.BuildAlloca(retType);

            node.Condition.Accept(this);
            var conditionValue = valueStack.Pop();
            builder.BuildCondBr(conditionValue, thenBlock, elseBlock);

            builder.PositionAtEnd(thenBlock);
            node.ThenBranch.Accept(this);
            builder.BuildStore(valueStack.Pop(), tempValue);
            builder.BuildBr(endBlock);

            builder.PositionAtEnd(elseBlock);
            node.ElseBranch.Accept(this);
            builder.BuildStore(valueStack.Pop(), tempValue);
            builder.BuildBr(endBlock);

            builder.PositionAtEnd(endBlock);
            var ret = builder.BuildLoad2(retType, tempValue);
            valueStack.Push(ret);
        }
        public override void Visit(IfThenExpressionNode node)
        {
            if (currentFunctionName == null)
            {
                throw new NotImplementedException();
            }
            var def = module.GetNamedFunction(currentFunctionName);
            basicBlockCount++;
            var thenBlock = def.AppendBasicBlock(basicBlockCount.ToString());
            basicBlockCount++;
            var endBlock = def.AppendBasicBlock(basicBlockCount.ToString());

            node.Condition.Accept(this);
            var conditionValue = valueStack.Pop();
            builder.BuildCondBr(conditionValue, thenBlock, endBlock);

            builder.PositionAtEnd(thenBlock);
            node.ThenBranch.Accept(this);
            builder.BuildBr(endBlock);

            builder.PositionAtEnd(endBlock);
        }
    }
}
