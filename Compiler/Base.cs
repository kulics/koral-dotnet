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
public partial class Result{
public object data;
public string text;
public string permission;
public bool isVirtual;
public bool isDefine;
public string rootID = "";
}
public partial class FeelLangVisitor:FeelParserBaseVisitor<object>{
public string selfID = "";
public string superID = "";
public string setID = "";
public string getID = "";
public string selfPropertyID = "";
public list<string> selfPropertyContent = (new list<string>());
public bool selfPropertyVariable = false;
public hashset<string> AllIDSet = (new hashset<string>());
public stack<hashset<string>> CurrentIDSet = (new stack<hashset<string>>());
public hashset<string> TypeIdSet = (new hashset<string>());
public stack<bool> FuncAsyncStack = (new stack<bool>());
public  virtual  bool has_id( string id ){
return this.AllIDSet.contains(id)||this.CurrentIDSet.peek().contains(id);
}
public  virtual  void add_id( string id ){
this.CurrentIDSet.peek().add(id);
}
public  virtual  void add_current_set(){
foreach (var item in CurrentIDSet.peek()){
AllIDSet.add(item);
}
this.CurrentIDSet.push((new hashset<string>()));
}
public  virtual  void delete_current_set(){
this.AllIDSet.except_with(this.CurrentIDSet.peek());
this.CurrentIDSet.pop();
}
public  virtual  bool is_type( string id ){
return this.TypeIdSet.contains(id);
}
public  virtual  void add_type( string id ){
this.TypeIdSet.add(id);
}
public  virtual  void add_func_stack(){
FuncAsyncStack.push(false);
}
public  virtual  void delete_func_stack(){
FuncAsyncStack.pop();
}
public  virtual  bool get_func_async(){
return FuncAsyncStack.peek();
}
public  virtual  void set_func_async(){
if ( FuncAsyncStack.peek() ) {
return;
}
FuncAsyncStack.pop();
FuncAsyncStack.push(true);
}
}
public partial class FeelLangVisitor{
public FeelLangVisitor (){this.CurrentIDSet.push((new hashset<string>()));
FuncAsyncStack.push(false);
}
}
public partial class FeelLangVisitor{
public  override  object VisitProgram( ProgramContext context ){
var StatementList = context.statement();
var Result = "";
foreach (var item in StatementList){
Result+=VisitStatement(item);
}
return Result;
}
public  override  object VisitId( IdContext context ){
var r = (new Result(){data = "var"});
var first = (Result)(Visit(context.GetChild(0)));
r.permission=first.permission;
r.text=first.text;
r.isVirtual=first.isVirtual;
if ( context.ChildCount>=2 ) {
foreach (var i in range(1, context.ChildCount-1, 1, true, true)){
var other = (Result)(Visit(context.GetChild(i)));
r.text+=(new System.Text.StringBuilder().Append("_").Append(other.text)).to_str();
}
}
if ( keywords.Exists((t)=>t==r.text) ) {
r.text=(new System.Text.StringBuilder().Append("@").Append(r.text)).to_str();
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
else if ( r.text==getID ) {
r.text="_"+selfPropertyID;
}
r.rootID=r.text;
return r;
}
public  override  object VisitIdItem( IdItemContext context ){
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
public  override  object VisitVarId( VarIdContext context ){
if ( context.Discard()!=null ) {
return "_";
}
else {
var id = ((Result)(Visit(context.id()))).text;
if ( !this.has_id(id) ) {
this.add_id(id);
}
return "var "+id;
}
}
public  override  object VisitVarIdType( VarIdTypeContext context ){
if ( context.Discard()!=null ) {
return "_";
}
else {
var id = ((Result)(Visit(context.id()))).text;
if ( !this.has_id(id) ) {
this.add_id(id);
}
return Visit(context.typeType())+" "+id;
}
}
public  override  object VisitConstId( ConstIdContext context ){
if ( context.Discard()!=null ) {
return "_";
}
else {
var id = ((Result)(Visit(context.id()))).text;
if ( this.has_id(id) ) {
return id;
}
else {
this.add_id(id);
return "var "+id;
}
}
}
public  override  object VisitConstIdType( ConstIdTypeContext context ){
if ( context.Discard()!=null ) {
return "_";
}
else {
var id = ((Result)(Visit(context.id()))).text;
if ( !this.has_id(id) ) {
this.add_id(id);
}
return Visit(context.typeType())+" "+id;
}
}
public  override  object VisitBoolExpr( BoolExprContext context ){
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
public  override  object VisitAnnotationStatement( AnnotationStatementContext context ){
return "";
}
public  override  object VisitAnnotationSupport( AnnotationSupportContext context ){
return (string)(Visit(context.annotation()));
}
public  override  object VisitAnnotation( AnnotationContext context ){
var obj = "";
var r = (string)(Visit(context.annotationList()));
if ( r!="" ) {
obj+=r;
}
return obj;
}
public  override  object VisitAnnotationList( AnnotationListContext context ){
var obj = "";
foreach (var (i, v) in range(context.annotationItem())){
var txt = (string)(this.Visit(v));
if ( txt!="" ) {
obj+=txt;
}
}
return obj;
}
public  override  object VisitAnnotationItem( AnnotationItemContext context ){
var obj = "";
var id = "";
if ( context.id().Length==2 ) {
id = (new System.Text.StringBuilder().Append(((Result)(Visit(context.id(0)))).text).Append(":")).to_str();
obj+=((Result)(this.Visit(context.id(1)))).text;
}
else {
obj+=((Result)(this.Visit(context.id(0)))).text;
}
switch (obj) {
case "get" :
{ if ( context.lambda()==null ) {
this.selfPropertyVariable=true;
this.selfPropertyContent+=(new System.Text.StringBuilder().Append("get{return _").Append(this.selfPropertyID).Append("; }")).to_str();
}
else {
this.selfPropertyContent+=(new System.Text.StringBuilder().Append("get{").Append(this.VisitPropertyLambda(context.lambda(), true)).Append("}")).to_str();
}
return "";
} break;
case "set" :
{ if ( context.lambda()==null ) {
this.selfPropertyVariable=true;
this.selfPropertyContent+=(new System.Text.StringBuilder().Append("set{_").Append(this.selfPropertyID).Append("=value;}")).to_str();
}
else {
this.selfPropertyContent+=(new System.Text.StringBuilder().Append("set{").Append(this.VisitPropertyLambda(context.lambda(), false)).Append("}")).to_str();
}
return "";
} break;
case "_get" :
{ if ( context.lambda()==null ) {
this.selfPropertyVariable=true;
this.selfPropertyContent+=(new System.Text.StringBuilder().Append("private get{return _").Append(this.selfPropertyID).Append("; }")).to_str();
}
else {
this.selfPropertyContent+=(new System.Text.StringBuilder().Append("private get{").Append(this.VisitPropertyLambda(context.lambda(), true)).Append("}")).to_str();
}
return "";
} break;
case "_set" :
{ if ( context.lambda()==null ) {
this.selfPropertyVariable=true;
this.selfPropertyContent+=(new System.Text.StringBuilder().Append("private set{_").Append(this.selfPropertyID).Append("=value;}")).to_str();
}
else {
this.selfPropertyContent+=(new System.Text.StringBuilder().Append("private set{").Append(this.VisitPropertyLambda(context.lambda(), false)).Append("}")).to_str();
}
return "";
} break;
case "add" :
{ todo("not yet");
return "";
} break;
case "remove" :
{ todo("not yet");
return "";
} break;
}
if ( context.tuple()!=null ) {
obj+=((Result)(this.Visit(context.tuple()))).text;
}
else if ( context.lambda()!=null ) {
obj+=(new System.Text.StringBuilder().Append("(").Append(((Result)(this.Visit(context.lambda()))).text).Append(")")).to_str();
}
else {
obj+="";
}
if ( id!="" ) {
obj = id+obj;
}
obj = "["+obj+"]";
return obj;
}
public  virtual  string VisitPropertyLambda( LambdaContext context ,  bool is_get ){
this.add_current_set();
var obj = "";
if ( context.lambdaIn()!=null ) {
this.VisitPropertyLambdaIn(context.lambdaIn(), is_get);
}
if ( context.tupleExpression()!=null ) {
obj+=((Result)(Visit(context.tupleExpression()))).text;
if ( is_get ) {
obj = "return "+obj;
}
obj+=Terminate;
}
else {
obj+=ProcessFunctionSupport(context.functionSupportStatement());
}
this.getID="";
this.setID="";
this.delete_current_set();
return obj;
}
public  virtual  void VisitPropertyLambdaIn( LambdaInContext context ,  bool is_get ){
switch (context.id().Length) {
case 1 :
{ var id0 = (Result)(this.Visit(context.id(0)));
this.add_id(id0.text);
if ( is_get ) {
this.selfPropertyVariable=true;
this.add_id("_"+this.selfPropertyID);
this.getID=id0.text;
}
else {
this.setID=id0.text;
}
} break;
case 2 :
{ this.selfPropertyVariable=true;
this.add_id("_"+this.selfPropertyID);
var id0 = (Result)(this.Visit(context.id(0)));
var id1 = (Result)(this.Visit(context.id(1)));
this.add_id(id0.text);
this.add_id(id1.text);
this.getID=id0.text;
this.setID=id1.text;
} break;
}
}
}
public partial class Compiler_static {
public const string Terminate = ";";
public const string Wrap = "\r\n";
public const string Any = "object";
public const string Int = "int";
public const string Num = "double";
public const string I8 = "sbyte";
public const string I16 = "short";
public const string I32 = "int";
public const string I64 = "long";
public const string U8 = "byte";
public const string U16 = "ushort";
public const string U32 = "uint";
public const string U64 = "ulong";
public const string F32 = "float";
public const string F64 = "double";
public const string Bool = "bool";
public const string T = "true";
public const string F = "false";
public const string Chr = "char";
public const string Str = "string";
public const string Lst = "list";
public const string Set = "hashset";
public const string Dic = "dict";
public const string Stk = "stack";
public const string Que = "queue";
public const string BlockLeft = "{";
public const string BlockRight = "}";
public const string Task = "System.Threading.Tasks.Task";
}
}
