using Library;
using static Library.Lib;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using static Compiler.KParser;
using static Compiler.Compiler_static;

namespace Compiler
{
public partial class ErrorListener:BaseErrorListener{
public string FileDir;
public  override  void SyntaxError( IRecognizer recognizer ,  IToken? offendingSymbol ,  int line ,  int charPositionInLine ,  string msg ,  RecognitionException? e ){
base.SyntaxError(recognizer, offendingSymbol, line, charPositionInLine, msg, e);
print("------Syntax Error------");
print((new System.Text.StringBuilder().Append("File: ").Append(this.FileDir)).to_str());
print((new System.Text.StringBuilder().Append("Line: ").Append(line).Append("  Column: ").Append(charPositionInLine)).to_str());
print((new System.Text.StringBuilder().Append("OffendingSymbol: ").Append(offendingSymbol.Text)).to_str());
print((new System.Text.StringBuilder().Append("Message: ").Append(msg)).to_str());
}
}
}
