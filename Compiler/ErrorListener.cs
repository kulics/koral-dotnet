using Antlr4.Runtime;
using System;
using System.IO;

namespace Compiler
{
    public class ErrorListener(string FileDir = "") : BaseErrorListener
    {
        public string FileDir = FileDir;

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);
            output.WriteLine("------Syntax Error------");
            output.WriteLine($"File: {this.FileDir}");
            output.WriteLine($"Line: {line}  Column: {charPositionInLine}");
            output.WriteLine($"OffendingSymbol: {offendingSymbol.Text}");
            output.WriteLine($"Message: {msg}");
        }
    }
}
