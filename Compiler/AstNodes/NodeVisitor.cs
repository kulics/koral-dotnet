using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.AstNodes
{
    public abstract class NodeVisitor
    {
        protected NodeVisitor() { }

        public abstract void Visit(ProgramNode node);

        public abstract void Visit(ModuleDeclarationNode node);
        public abstract void Visit(GlobalRecordDeclarationNode node);
        public abstract void Visit(GlobalFunctionDeclarationNode node);
        public abstract void Visit(ParameterDeclarationNode node);
        public abstract void Visit(GlobalVariableDeclarationNode node);
        public abstract void Visit(GlobalInterfaceDeclarationNode node);

        public abstract void Visit(MemberNode node);
        public abstract void Visit(MethodNode node);
        public abstract void Visit(VirtualMethodNode node);

        public abstract void Visit(StatementNode node);
        public abstract void Visit(VariableStatementNode node);
        public abstract void Visit(ExpressionStatementNode node);
        public abstract void Visit(BreakStatementNode node);
        public abstract void Visit(ContinueStatementNode node);
        public abstract void Visit(ReturnStatementNode node);

        public abstract void Visit(ExpressionNode node);
        public abstract void Visit(IdentifierExpressionNode node);
        public abstract void Visit(LiteralExpressionNode node);
        public abstract void Visit(CalculativeExpressionNode node);
        public abstract void Visit(CompareExpressionNode node);
        public abstract void Visit(LogicExpressionNode node);
        public abstract void Visit(BlockExpressionNode node);
        public abstract void Visit(FunctionCallExpressionNode node);
        public abstract void Visit(MemberExpressionNode node);
        public abstract void Visit(ConstructCallExpressionNode node);
        public abstract void Visit(AssignmentExpressionNode node);

        public abstract void Visit(ConditionNode node);
        public abstract void Visit(IfThenElseExpressionNode node);
        public abstract void Visit(IfThenExpressionNode node);
        public abstract void Visit(WhileThenExpressionNode node);
        public abstract void Visit(WhileThenElseExpressionNode node);
    }
}
