using Library;
using static Library.Lib;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using static Compiler.FeelParser;
using static Compiler.Compiler_static;

namespace Compiler
{
public partial class ErrorListener:BaseErrorListener{
public ErrorListener(string FileDir  = ""){this.FileDir = FileDir;
}
public string FileDir;
public  override  void SyntaxError( IRecognizer recognizer ,  IToken? offendingSymbol ,  int line ,  int charPositionInLine ,  string msg ,  RecognitionException? e ){
base.SyntaxError(recognizer, offendingSymbol, line, charPositionInLine, msg, e);
Print("------Syntax Error------");
Print((new System.Text.StringBuilder().Append("File: ").Append(this.FileDir)).To_Str());
Print((new System.Text.StringBuilder().Append("Line: ").Append(line).Append("  Column: ").Append(charPositionInLine)).To_Str());
Print((new System.Text.StringBuilder().Append("OffendingSymbol: ").Append(offendingSymbol.Text)).To_Str());
Print((new System.Text.StringBuilder().Append("Message: ").Append(msg)).To_Str());
}
}
}
