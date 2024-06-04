using Antlr4.Runtime;
using System;

namespace Compiler
{
    public class ErrorListener(string FileDir = "") : BaseErrorListener
    {
        public string FileDir = FileDir;

        public override void SyntaxError(
            IRecognizer recognizer,
            IToken offendingSymbol,
            int line,
            int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            base.SyntaxError(recognizer, offendingSymbol, line, charPositionInLine, msg, e);
            Console.WriteLine("------Syntax Error------");
            Console.WriteLine($"File: {this.FileDir}");
            Console.WriteLine($"Line: {line}  Column: {charPositionInLine}");
            Console.WriteLine($"OffendingSymbol: {offendingSymbol.Text}");
            Console.WriteLine($"Message: {msg}");
        }
    }
}
