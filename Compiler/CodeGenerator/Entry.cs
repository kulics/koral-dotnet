using Compiler.AstNodes;
using LLVMSharp;
using LLVMSharp.Interop;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Compiler.CodeGenerator
{
    public static class Entry
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate int Int32Delegate();
        public static void Generate(ProgramNode node, string path)
        {
            using var module = LLVMModuleRef.CreateWithName("LLVMSharpIntro");
            using var builder = module.Context.CreateBuilder();

            //var functype = LLVMTypeRef.CreateFunction(LLVMTypeRef.Int32, []);
            //var def = module.AddFunction("main", functype);
            //var bb = def.AppendBasicBlock("entry");
            //builder.PositionAtEnd(bb);
            //builder.BuildRet(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 123));

            var visitor = new LLVMGeneratorVisitor(module, builder);
            visitor.Visit(node);

            module.Verify(LLVMVerifierFailureAction.LLVMPrintMessageAction);
            module.WriteBitcodeToFile(path);
        }
    }
}
