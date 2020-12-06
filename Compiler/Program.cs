using Library;
using static Library.Lib;
using Antlr4.Runtime;
using System;
using System.IO;
using System.Text;

namespace Compiler
{
public partial class Compiler_static {
protected internal static string _read_path;
protected internal static string _path_line;
public static void Main( string[] args ){
var os = Environment.OSVersion.Platform;
if ( os==PlatformID.Unix||os==PlatformID.MacOSX ) {
_read_path="./";
_path_line="/";
}
else {
_read_path=".\\";
_path_line="\\";
}
Compiled(_read_path);
Print("Completed");
}
public static void Compiled( string path ){
var Files = Directory.GetFiles(path, "*.feel");
foreach (var file in Files){
using (var fsRead = (new FileStream(file, FileMode.Open))) {
var FSLength = ((int)fsRead.Length);
var ByteBlock = Array<byte>(FSLength, (i)=>0);
var r = fsRead.Read(ByteBlock, 0, ByteBlock.Length);
var Input = Encoding.UTF8.GetString(ByteBlock);
var Stream = (new AntlrInputStream(Input));
var Lexer = (new FeelLexer(Stream));
var Tokens = (new CommonTokenStream(Lexer));
var Parser = (new FeelParser(Tokens));
Parser.BuildParseTree=true;
Parser.RemoveErrorListeners();
Parser.AddErrorListener((new ErrorListener(file)));
var AST = Parser.program();
var Visitor = (new FeelLangVisitor());
var Result = Visitor.Visit(AST);
var ByteResult = Encoding.UTF8.GetBytes(Result.To_Str());
using (var fsWrite = (new FileStream((new System.Text.StringBuilder().Append(_read_path).Append(file.Replace(".feel", ".cs"))).To_Str(), FileMode.Create))) {
fsWrite.Write(ByteResult, 0, ByteResult.Length);
}}}
foreach (var folder in Directory.GetDirectories(path)){
Compiled(folder);
}
}
}
}
