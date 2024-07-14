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

        private readonly Dictionary<string, LLVMTypeRef> structTypes = [];

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
                        var type = MapArcType(FindType(t.ParamType));
                        paramTypes.Add(type);
                    }
                    var returnType = MapArcType(FindType(globalF.ReturnType));
                    var functype = LLVMTypeRef.CreateFunction(returnType, [.. paramTypes]);
                    funcTypes[funcName] = functype;
                    module.AddFunction(funcName, functype);
                }
                else if (item is GlobalVariableDeclarationNode globalV)
                {
                    var id = globalV.Id;
                    var type = FindType(id.Type);
                    var mapType = MapArcType(type);
                    module.AddGlobal(mapType, id.Name);
                }
                else if (item is GlobalRecordDeclarationNode globalR)
                {
                    var list = new List<LLVMTypeRef>
                    {
                        // add ref count field
                        LLVMTypeRef.Int32
                    };
                    foreach (var v in globalR.Fields)
                    {
                        list.Add(FindType(v.Type));
                    }
                    structTypes[globalR.Type.Name] = LLVMTypeRef.CreateStruct(list.ToArray(), false);
                }
            }
            foreach (var item in node.Declarations)
            {
                item.Accept(this);
            }
        }

        private static LLVMTypeRef MapArcType(LLVMTypeRef t)
        {
            if (t.Kind == LLVMTypeKind.LLVMStructTypeKind)
            {
                return LLVMTypeRef.CreatePointer(t, 0);
            }
            else
            {
                return t;
            }
        }

        private void ArcInc(LLVMTypeRef argType, LLVMValueRef argValue)
        {
            if (argType.Kind == LLVMTypeKind.LLVMStructTypeKind)
            {
                var elementPtr = builder.BuildStructGEP2(argType, argValue, 0);
                var refCount = builder.BuildLoad2(LLVMTypeRef.Int32, elementPtr);
                var newCount = builder.BuildAdd(refCount, LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 1));
                builder.BuildStore(newCount, elementPtr);
            }
        }

        public override void Visit(ParameterDeclarationNode node) => throw new NotImplementedException();
        public override void Visit(MemberNode node) => throw new NotImplementedException();
        public override void Visit(MethodNode node) => throw new NotImplementedException();
        public override void Visit(VirtualMethodNode node) => throw new NotImplementedException();

    }
}
