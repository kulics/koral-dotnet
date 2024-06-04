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
        public override void Visit(GlobalVariableDeclarationNode node) => throw new NotImplementedException();

        public override void Visit(GlobalFunctionDeclarationNode node)
        {
            var funcName = node.Id.Name;
            var paramTypes = new List<LLVMTypeRef>();
            foreach (var item in node.Parameters)
            {
                paramTypes.Add(FindType(item.ParameterType));
            }
            var returnType = FindType(node.returnType);
            var functype = LLVMTypeRef.CreateFunction(returnType, [.. paramTypes]);
            funcTypes[funcName] = functype;
            var def = module.AddFunction(funcName, functype);
            var bb = def.AppendBasicBlock("entry");
            builder.PositionAtEnd(bb);
            foreach (var (index, item) in node.Parameters.WithIndex())
            {
                namedValues[item.Id] = new IdentifierValue(false, def.GetParam((uint)index));
            }
            node.Body.Accept(this);
            builder.BuildRet(valueStack.Pop());
        }

        public override void Visit(GlobalInterfaceDeclarationNode node) => throw new NotImplementedException();
        public override void Visit(GlobalRecordDeclarationNode node) => throw new NotImplementedException();

        LLVMTypeRef FindType(KoralType type)
        {
            if (type == BuiltinTypes.Int)
            {
                return LLVMTypeRef.Int32;
            }
            else if (type == BuiltinTypes.Void)
            {
                return LLVMTypeRef.Void;
            }
            throw new NotImplementedException();
        }
    }
}
