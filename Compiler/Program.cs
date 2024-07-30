using Compiler.Library;
using static Compiler.Library.GlobalFunction;
using Antlr4.Runtime;
using System;
using System.IO;
using System.Text;
using Compiler.Sema;
using Compiler.CodeGenerator;
using System.Collections.Generic;

namespace Compiler
{
    public class Compiler
    {
        public static void Main(string[] args)
        {
            Compiled(args[0]);
            Console.WriteLine("Completed");
        }

        public static void Compiled(string path)
        {
            var files = Directory.GetFiles(path, "*.koral");
            foreach (var file in files)
            {
                using var fsRead = new FileStream(file, FileMode.Open);
                var stream = new AntlrInputStream(fsRead);
                var lexer = new Parser.KoralLexer(stream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new Parser.KoralParser(tokens)
                {
                    BuildParseTree = true
                };
                parser.RemoveErrorListeners();
                parser.AddErrorListener(new ErrorListener(file));
                var ast = parser.program();
                var visitor = new KoralLangVisitor();
                var programNode = visitor.VisitProgram(ast);
                var bcFilePath = $"{file.Replace(".koral", ".bc")}";
                Entry.Generate(programNode, bcFilePath);
            }
            foreach (var folder in Directory.GetDirectories(path))
            {
                Compiled(folder);
            }
        }
    }
}
