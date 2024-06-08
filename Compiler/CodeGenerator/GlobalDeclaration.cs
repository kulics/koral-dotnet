using Compiler.AstNodes;
using Compiler.Library;
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
        public override void Visit(ModuleDeclarationNode node) => throw new NotImplementedException();
        public override void Visit(GlobalVariableDeclarationNode node)
        {
            var id = node.Id;
            var def = module.GetNamedGlobal(id.Name);
            if (node.InitValue is IntegerLiteralExpressionNode i)
            {
                def.Initializer = LLVMValueRef.CreateConstInt(FindType(id.Type), (ulong)i.Value);
                namedValues[id] = new IdentifierValue(true, def);
                return;
            }
            throw new NotImplementedException();
        }

        public override void Visit(GlobalFunctionDeclarationNode node)
        {
            var funcName = node.Id.Name;
            var def = module.GetNamedFunction(funcName);
            currentFunctionName = funcName;
            basicBlockCount++;
            var bb = def.AppendBasicBlock(basicBlockCount.ToString());
            builder.PositionAtEnd(bb);
            foreach (var (index, item) in node.ParameterTypes.WithIndex())
            {
                namedValues[item.Id] = new IdentifierValue(false, def.GetParam((uint)index));
            }
            node.Body.Accept(this);
            builder.BuildRet(valueStack.Pop());
            currentFunctionName = null;
        }

        public override void Visit(GlobalInterfaceDeclarationNode node) => throw new NotImplementedException();
        public override void Visit(GlobalRecordDeclarationNode node) => throw new NotImplementedException();

        private LLVMTypeRef FindType(KoralType type)
        {
            if (type == BuiltinTypes.Int)
            {
                return LLVMTypeRef.Int32;
            }
            else if (type == BuiltinTypes.Void)
            {
                return LLVMTypeRef.Int1;
            }
            throw new NotImplementedException();
        }
    }
}
