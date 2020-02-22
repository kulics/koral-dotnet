using Library;
using static Library.Lib;
using Antlr4.Runtime;
using System;
using System.IO;
using System.Text;

namespace Compiler
{
public partial class Compiler_static {
protected static string _ReadPath;
protected static string _PathLine;
public static void Main( string[] args ){
var os = Environment.OSVersion.Platform;
if ( os==PlatformID.Unix||os==PlatformID.MacOSX ) {
_ReadPath="./";
_PathLine="/";
}
else {
_ReadPath=".\\";
_PathLine="\\";
}
Compiled(_ReadPath);
print("Completed");
}
public static void Compiled( string path ){
var Files = Directory.GetFiles(path, "*.k");
foreach (var file in Files){
using (var fsRead = (new FileStream(file, FileMode.Open))) {
try {
var FSLength = (int)(fsRead.Length);
var ByteBlock = array<byte>(FSLength);
var r = fsRead.Read(ByteBlock, 0, ByteBlock.Length);
var Input = Encoding.UTF8.GetString(ByteBlock);
var Stream = (new AntlrInputStream(Input));
var Lexer = (new KLexer(Stream));
var Tokens = (new CommonTokenStream(Lexer));
var Parser = (new KParser(Tokens));
Parser.BuildParseTree=true;
Parser.RemoveErrorListeners();
Parser.AddErrorListener((new ErrorListener(){FileDir = file}));
var AST = Parser.program();
var Visitor = (new KLangVisitor());
var Result = Visitor.Visit(AST);
var ByteResult = Encoding.UTF8.GetBytes(Result.to_str());
using (var fsWrite = (new FileStream("_ReadPath; file.sub_str(0, file.Length - 2)}.cs", FileMode.Create))) {
fsWrite.Write(ByteResult, 0, ByteResult.Length);
}}catch( Exception err )
{
print(err);
return;
}
}}
var Folders = Directory.GetDirectories(path);
foreach (var folder in Folders){
Compiled(folder);
}
}
}
}
