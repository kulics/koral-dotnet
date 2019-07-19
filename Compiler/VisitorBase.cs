using Library;
using static Library.Lib;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using static Compiler.LiteParser;
using static Compiler.Compiler_static;

namespace Compiler
{
public partial class Result
{
public object data;
public string text;
public string permission;
public bool isVirtual;
public bool isDefine;
}
public partial class LiteLangVisitor:LiteParserBaseVisitor<object>
{
public string selfID = "" ; 
public string superID = "" ; 
public string setID = "" ; 
public hashset<string> AllIDSet = (new hashset<string>()) ; 
public Stack<hashset<string>> CurrentIDSet = (new Stack<hashset<string>>()) ; 
}
public partial class LiteLangVisitor{
public LiteLangVisitor (){CurrentIDSet.Push((new hashset<string>()));
}
}
public partial class LiteLangVisitor{
public  virtual  bool has_id( string id )
{
return this.AllIDSet.contains(id)||this.CurrentIDSet.Peek().contains(id);
}
public  virtual  void add_id( string id )
{
this.CurrentIDSet.Peek().add(id);
}
public  virtual  void add_current_set()
{
foreach (var item in CurrentIDSet.Peek()){
AllIDSet.Add(item);
}
this.CurrentIDSet.Push((new hashset<string>()));
}
public  virtual  void delete_current_set()
{
this.AllIDSet.except_with(this.CurrentIDSet.Peek());
this.CurrentIDSet.Pop();
}
}
public partial class LiteLangVisitor{
public  override  object VisitProgram( ProgramContext context )
{
var StatementList = context.statement();
var Result = "";
foreach (var item in StatementList){
Result+=VisitStatement(item);
}
return Result;
}
public  override  object VisitId( IdContext context )
{
var r = (new Result(){data = "var"});
var first = ((Result)(Visit(context.GetChild(0))));
r.permission=first.permission;
r.text=first.text;
r.isVirtual=first.isVirtual;
if ( context.ChildCount>=2 ) {
foreach (var i in range(1,context.ChildCount,1,true,false)){
var other = ((Result)(Visit(context.GetChild(i))));
r.text+=(new System.Text.StringBuilder("_").Append(other.text).Append("")).to_str();
}
}
if ( keywords.Exists((t)=>t==r.text) ) {
r.text=(new System.Text.StringBuilder("@").Append(r.text).Append("")).to_str();
}
if ( r.text==selfID ) {
r.text="this";
}
else if ( r.text==superID ) {
r.text="base";
} 
else if ( r.text==setID ) {
r.text="value";
} 
return r;
}
public  override  object VisitIdItem( IdItemContext context )
{
var r = (new Result(){data = "var"});
if ( context.typeBasic()!=null ) {
r.permission="public";
r.text+=context.typeBasic().GetText();
r.isVirtual=true;
}
else if ( context.typeAny()!=null ) {
r.permission="public";
r.text+=context.typeAny().GetText();
r.isVirtual=true;
} 
else if ( context.linqKeyword()!=null ) {
r.permission="public";
r.text+=Visit(context.linqKeyword());
r.isVirtual=true;
} 
else if ( context.op.Type==IDPublic ) {
r.permission="public";
r.text+=context.op.Text;
r.isVirtual=true;
} 
else if ( context.op.Type==IDPrivate ) {
r.permission="protected";
r.text+=context.op.Text;
r.isVirtual=true;
} 
return r;
}
public  override  object VisitIdExpression( IdExpressionContext context )
{
var r = (new Result(){data = "var"});
if ( context.idExprItem().Length>1 ) {
r.text="(";
foreach (var (i,v) in range(context.idExprItem())){
var subID = ((Result)(Visit(v))).text;
if ( i!=0 ) {
r.text+=", "+subID;
}
else {
r.text+=subID;
}
if ( this.has_id(subID) ) {
r.isDefine=true;
}
else {
this.add_id(subID);
}
}
r.text+=")";
}
else {
r = ((Result)(Visit(context.idExprItem(0))));
if ( this.has_id(r.text) ) {
r.isDefine=true;
}
else {
this.add_id(r.text);
}
}
return r;
}
public  override  object VisitIdExprItem( IdExprItemContext context )
{
return Visit(context.GetChild(0));
}
public  override  object VisitBoolExpr( BoolExprContext context )
{
var r = (new Result());
if ( context.t.Type==TrueLiteral ) {
r.data=Bool;
r.text=T;
}
else if ( context.t.Type==FalseLiteral ) {
r.data=Bool;
r.text=F;
} 
return r;
}
public  override  object VisitAnnotationSupport( AnnotationSupportContext context )
{
return ((string)(Visit(context.annotation())));
}
public  override  object VisitAnnotation( AnnotationContext context )
{
var obj = "";
var id = "";
if ( context.id()!=null ) {
id = (new System.Text.StringBuilder("").Append(((Result)(Visit(context.id()))).text).Append(":")).to_str();
}
var r = ((string)(Visit(context.annotationList())));
obj+=(new System.Text.StringBuilder("[").Append(id).Append("").Append(r).Append("]")).to_str();
return obj;
}
public  override  object VisitAnnotationList( AnnotationListContext context )
{
var obj = "";
foreach (var i in range(0,context.annotationItem().Length,1,true,false)){
if ( i>0 ) {
obj+=(new System.Text.StringBuilder(",").Append(Visit(context.annotationItem(i))).Append("")).to_str();
}
else {
obj+=Visit(context.annotationItem(i));
}
}
return obj;
}
public  override  object VisitAnnotationItem( AnnotationItemContext context )
{
var obj = "";
obj+=((Result)(Visit(context.id()))).text;
foreach (var i in range(0,context.annotationAssign().Length,1,true,false)){
if ( i>0 ) {
obj+=(new System.Text.StringBuilder(",").Append(Visit(context.annotationAssign(i))).Append("")).to_str();
}
else {
obj+=(new System.Text.StringBuilder("(").Append(Visit(context.annotationAssign(i))).Append("")).to_str();
}
}
if ( context.annotationAssign().Length>0 ) {
obj+=")";
}
return obj;
}
public  override  object VisitAnnotationAssign( AnnotationAssignContext context )
{
var obj = "";
var id = "";
if ( context.id()!=null ) {
id = (new System.Text.StringBuilder("").Append(((Result)(Visit(context.id()))).text).Append("=")).to_str();
}
var r = ((Result)(Visit(context.expression())));
obj = id+r.text;
return obj;
}
}
public partial class Compiler_static{
public const string Terminate = ";" ;
public const string Wrap = "\r\n" ;
public const string Any = "object" ;
public const string Int = "int" ;
public const string Num = "double" ;
public const string I8 = "sbyte" ;
public const string I16 = "short" ;
public const string I32 = "int" ;
public const string I64 = "long" ;
public const string U8 = "byte" ;
public const string U16 = "ushort" ;
public const string U32 = "uint" ;
public const string U64 = "ulong" ;
public const string F32 = "float" ;
public const string F64 = "double" ;
public const string Bool = "bool" ;
public const string T = "true" ;
public const string F = "false" ;
public const string Chr = "char" ;
public const string Str = "string" ;
public const string Lst = "list" ;
public const string Set = "hashset" ;
public const string Dic = "dictionary" ;
public const string Stk = "stack" ;
public const string BlockLeft = "{" ;
public const string BlockRight = "}" ;
public const string Task = "System.Threading.Tasks.Task" ;
}
}
