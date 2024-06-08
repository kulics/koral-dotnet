using Compiler.AstNodes;
using Compiler.Library;
using Compiler.Types;
using LLVMSharp.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.CodeGenerator
{
    public partial class LLVMGeneratorVisitor : NodeVisitor
    {
        private static readonly LLVMValueRef NullValue = new(IntPtr.Zero);

        private readonly LLVMModuleRef module;

        private readonly LLVMBuilderRef builder;

        private readonly Dictionary<Identifier, IdentifierValue> namedValues = [];

        private readonly Stack<LLVMValueRef> valueStack = [];

        private readonly Dictionary<string, LLVMTypeRef> funcTypes = [];

        private string? currentFunctionName;

        private Stack<(LLVMBasicBlockRef loopIn, LLVMBasicBlockRef loopOut)> loopStack = [];

        private int basicBlockCount = 0;

        private static LLVMValueRef GetVoidValue() => LLVMValueRef.CreateConstInt(LLVMTypeRef.Int1, 0);

        public LLVMGeneratorVisitor(LLVMModuleRef module, LLVMBuilderRef builder)
        {
            this.module = module;
            this.builder = builder;
        }

        private struct IdentifierValue(bool isPtr, LLVMValueRef value)
        {
            public bool IsPtr = isPtr;
            public LLVMValueRef Value = value;
        }

        public override void Visit(ProgramNode node)
        {
            // todo: 还没处理 module
            // pre load global
            foreach (var item in node.Declarations)
            {
                if (item is GlobalFunctionDeclarationNode globalF)
                {
                    var funcName = globalF.Id.Name;
                    var paramTypes = new List<LLVMTypeRef>();
                    foreach (var t in globalF.ParameterTypes)
                    {
                        paramTypes.Add(FindType(t.ParamType));
                    }
                    var returnType = FindType(globalF.ReturnType);
                    var functype = LLVMTypeRef.CreateFunction(returnType, [.. paramTypes]);
                    funcTypes[funcName] = functype;
                    module.AddFunction(funcName, functype);
                }
                if (item is GlobalVariableDeclarationNode globalV)
                {
                    var id = globalV.Id;
                    module.AddGlobal(FindType(id.Type), id.Name);
                }
            }
            foreach (var item in node.Declarations)
            {
                item.Accept(this);
            }
        }

        public override void Visit(ParameterDeclarationNode node) => throw new NotImplementedException();
        public override void Visit(MemberNode node) => throw new NotImplementedException();
        public override void Visit(MethodNode node) => throw new NotImplementedException();
        public override void Visit(VirtualMethodNode node) => throw new NotImplementedException();

    }
}
