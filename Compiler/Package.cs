using Library;
using static Library.Lib;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using static Compiler.LiteParser;
using static Compiler.Compiler_static;

namespace Compiler
{
public partial class LiteLangVisitor{
public  override  object @base( IncludeStatementContext context ){
return Visit(context.typeType());
}
public  override  object @base( PackageStatementContext context ){
var id = ((Result)(Visit(context.id(0))));
if ( context.id(1)!=null ) {
var Self = ((Result)(Visit(context.id(1))));
this.selfID=Self.text;
}
var obj = "";
var extend = "";
foreach (var item in context.packageSupportStatement()){
if ( item.GetChild(0).GetType()==@typeof<IncludeStatementContext>() ) {
extend+=run(()=>{if ( extend=="" ) {
return Visit(item);}
else {
return ","+Visit(item);}
});
}
else {
obj+=Visit(item);
}
}
foreach (var item in context.packageImplementStatement()){
var r = ((Result)(Visit(item)));
extend+=run(()=>{if ( extend=="" ) {
return r.data;}
else {
return ","+r.data;}
});
obj+=r.text;
}
foreach (var item in context.packageNewStatement()){
var r = ((string)(Visit(item)));
obj+=(new System.Text.StringBuilder("public ").Append(id.text).Append("").Append(r).Append("")).to_str();
}
obj+=BlockRight+Wrap;
var header = "";
if ( context.annotationSupport()!=null ) {
header+=Visit(context.annotationSupport());
}
header+=(new System.Text.StringBuilder("").Append(id.permission).Append(" partial class ").Append(id.text).Append("")).to_str();
var template = "";
var templateContract = "";
if ( context.templateDefine()!=null ) {
var item = ((TemplateItem)(Visit(context.templateDefine())));
template+=item.Template;
templateContract = item.Contract;
header+=template;
}
if ( extend.Length>0 ) {
header+=":"+extend;
}
header+=templateContract+BlockLeft+Wrap;
obj = header+obj;
this.selfID="";
return obj;
}
public  override  object @base( PackageVariableStatementContext context ){
var r1 = ((Result)(Visit(context.id())));
var isMutable = r1.isVirtual;
var typ = "";
Result r2 = null;
if ( context.expression()!=null ) {
r2 = ((Result)(Visit(context.expression())));
typ = ((string)(r2.data));
}
if ( context.typeType()!=null ) {
typ = ((string)(Visit(context.typeType())));
}
var obj = "";
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
obj+=(new System.Text.StringBuilder("").Append(r1.permission).Append(" ").Append(typ).Append(" ").Append(r1.text).Append("")).to_str();
obj+=run(()=>{if ( r2!=null ) {
return (new System.Text.StringBuilder(" = ").Append(r2.text).Append(" ").Append(Terminate).Append(" ").Append(Wrap).Append("")).to_str();}
else {
return Terminate+Wrap;}
});
return obj;
}
public  override  object @base( PackageControlSubStatementContext context ){
var obj = "";
var id = "";
var typ = "";
this.add_current_set();
(id, typ) = GetControlSub(context.id(0).GetText());
if ( context.id(1)!=null ) {
this.setID=context.id(1).GetText();
}
if ( context.functionSupportStatement().Length>0 ) {
obj+=id+BlockLeft;
foreach (var item in context.functionSupportStatement()){
obj+=Visit(item);
}
obj+=BlockRight+Wrap;
}
else {
obj+=id+Terminate;
}
this.delete_current_set();
this.setID="";
return (new Result(){text = obj,data = typ});
}
public  override  object @base( PackageNewStatementContext context ){
var text = "";
this.add_current_set();
text+=((string)(Visit(context.parameterClauseIn())));
if ( context.expressionList()!=null ) {
text+=(new System.Text.StringBuilder(":base(").Append(((Result)(Visit(context.expressionList()))).text).Append(")")).to_str();
}
text+=BlockLeft+ProcessFunctionSupport(context.functionSupportStatement())+BlockRight+Wrap;
this.delete_current_set();
return text;
}
public  override  object @base( PackageEventStatementContext context ){
var obj = "";
var id = ((Result)(Visit(context.id())));
var nameSpace = Visit(context.nameSpaceItem());
obj+=(new System.Text.StringBuilder("public event ").Append(nameSpace).Append(" ").Append(id.text+Terminate+Wrap).Append("")).to_str();
return obj;
}
public  override  object @base( ProtocolStatementContext context ){
var id = ((Result)(Visit(context.id())));
var obj = "";
var interfaceProtocol = "";
var ptclName = id.text;
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
foreach (var item in context.protocolSupportStatement()){
var r = ((Result)(Visit(item)));
interfaceProtocol+=r.text;
}
obj+="public partial interface "+ptclName;
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = ((TemplateItem)(Visit(context.templateDefine())));
obj+=template.Template;
templateContract = template.Contract;
}
obj+=templateContract+BlockLeft+Wrap;
obj+=interfaceProtocol;
obj+=BlockRight+Wrap;
return obj;
}
public  override  object @base( ProtocolControlStatementContext context ){
var id = ((Result)(Visit(context.id())));
var isMutable = id.isVirtual;
var r = (new Result());
if ( context.annotationSupport()!=null ) {
r.text+=Visit(context.annotationSupport());
}
r.permission="public";
var type = ((string)(Visit(context.typeType())));
r.text+=type+" "+id.text;
r.text+=BlockLeft;
if ( context.protocolControlSubStatement().Length>0 ) {
foreach (var item in context.protocolControlSubStatement()){
r.text+=Visit(item);
}
}
else {
r.text+="get;set;";
}
r.text+=BlockRight+Wrap;
return r;
}
public  override  object @base( ProtocolControlSubStatementContext context ){
var obj = "";
obj = GetControlSub(context.id().GetText()).id+Terminate;
return obj;
}
public  override  object @base( ProtocolFunctionStatementContext context ){
var id = ((Result)(Visit(context.id())));
var r = (new Result());
if ( context.annotationSupport()!=null ) {
r.text+=Visit(context.annotationSupport());
}
r.permission="public";
var pout = ((string)(Visit(context.parameterClauseOut())));
if ( context.t.Type==Right_Flow ) {
pout = run(()=>{if ( pout!="void" ) {
return (new System.Text.StringBuilder("").Append(Task).Append("<").Append(pout).Append(">")).to_str();}
else {
return Task;}
});
r.text+=pout+" "+id.text;
}
else {
if ( context.y!=null ) {
if ( pout!="void" ) {
pout = (new System.Text.StringBuilder("").Append(IEnum).Append("<").Append(pout).Append(">")).to_str();
}
}
r.text+=pout+" "+id.text;
}
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = ((TemplateItem)(Visit(context.templateDefine())));
r.text+=template.Template;
templateContract = template.Contract;
}
r.text+=Visit(context.parameterClauseIn())+templateContract+Terminate+Wrap;
return r;
}
public  override  object @base( PackageImplementStatementContext context ){
var obj = "";
var extends = ((string)(Visit(context.typeType())));
foreach (var item in context.implementSupportStatement()){
obj+=Visit(item);
}
return (new Result(){text = obj,data = extends});
}
}
}
