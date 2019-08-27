using Library;
using static Library.Lib;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using static Compiler.LiteParser;
using static Compiler.Compiler_static;

namespace Compiler
{
public partial class ErrorListener:BaseErrorListener{
public string FileDir;
public  override  void @base( IRecognizer recognizer ,  IToken offendingSymbol ,  int line ,  int charPositionInLine ,  string msg ,  RecognitionException e ){
@base.base(recognizer, offendingSymbol, line, charPositionInLine, msg, e);
print("------Syntax Error------");
print((new System.Text.StringBuilder("File: ").Append(this.FileDir).Append("")).to_str());
print((new System.Text.StringBuilder("Line: ").Append(line).Append("  Column: ").Append(charPositionInLine).Append("")).to_str());
print((new System.Text.StringBuilder("OffendingSymbol: ").Append(offendingSymbol.Text).Append("")).to_str());
print((new System.Text.StringBuilder("Message: ").Append(msg).Append("")).to_str());
}
}
}
