using Compiler.AstNodes;
using Compiler.Library;
using LLVMSharp.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.CodeGenerator
{
    public partial class LLVMGeneratorVisitor(LLVMModuleRef module, LLVMBuilderRef builder) : NodeVisitor
    {
        private static readonly LLVMValueRef NullValue = new(IntPtr.Zero);

        private readonly LLVMModuleRef module = module;

        private readonly LLVMBuilderRef builder = builder;

        private readonly Dictionary<Identifier, IdentifierValue> namedValues = [];

        private readonly Stack<LLVMValueRef> valueStack = [];

        private readonly Dictionary<string, LLVMTypeRef> funcTypes = [];

        private struct IdentifierValue(bool isPtr, LLVMValueRef value)
        {
            public bool IsPtr = isPtr;
            public LLVMValueRef Value = value;
        }

        public override void Visit(ProgramNode node)
        {
            // 还没处理 module
            foreach (var item in node.Declarations)
            {
                item.Accept(this);
            }
        }

        public override void Visit(ParameterDeclarationNode node) => throw new NotImplementedException();
    }
}
