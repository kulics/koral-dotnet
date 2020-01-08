using Library;
using static Library.Lib;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using static Compiler.LiteParser;
using static Compiler.Compiler_static;

namespace Compiler
{
public partial class Namespace{
public string name;
public string imports;
}
public partial class LiteLangVisitor{
public  override  object VisitStatement( StatementContext context ){
var obj = "";
var ns = (Namespace)(Visit(context.exportStatement()));
obj+=(new System.Text.StringBuilder("using Library;").Append(Wrap).Append("using static Library.Lib;").Append(Wrap).Append("")).to_str();
obj+=ns.imports+Wrap;
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
obj+=(new System.Text.StringBuilder("namespace ").Append(ns.name+Wrap+BlockLeft+Wrap).Append("")).to_str();
var content = "";
var contentStatic = "";
this.add_current_set();
foreach (var item in context.namespaceSupportStatement()){
var type = item.GetChild(0).GetType();
if ( type==@typeof<NamespaceVariableStatementContext>()||type==@typeof<NamespaceFunctionStatementContext>()||type==@typeof<NamespaceConstantStatementContext>() ) {
contentStatic+=Visit(item);
}
else {
content+=Visit(item);
}
}
obj+=content;
if ( contentStatic!="" ) {
obj+=(new System.Text.StringBuilder("public partial class ").Append(ns.name.sub_str(ns.name.last_index_of(".")+1)).Append("_static")).to_str()+BlockLeft+Wrap+contentStatic+BlockRight+Wrap;
}
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitExportStatement( ExportStatementContext context ){
var name = context.TextLiteral().GetText();
name = name.sub_str(1, name.len()-2);
name = name.replace("/", ".");
var obj = (new Namespace(){name = name});
foreach (var item in context.importStatement()){
obj.imports+=(string)(Visit(item));
}
return obj;
}
public  override  object VisitImportStatement( ImportStatementContext context ){
var obj = "";
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
var ns = context.TextLiteral().GetText();
ns = ns.sub_str(1, ns.len()-2);
ns = ns.replace("/", ".");
obj+=run(()=>{if ( context.call()!=null ) {
return "using static "+ns+"."+((Result)(Visit(context.id()))).text;}
else if ( context.id()!=null ) {
return "using "+ns+"."+((Result)(Visit(context.id()))).text;}
else {
return "using "+ns;}
});
obj+=Terminate+Wrap;
return obj;
}
public  override  object VisitNameSpaceItem( NameSpaceItemContext context ){
var obj = "";
foreach (var i in range(0,context.id().Length-1,1,true,true)){
var id = (Result)(Visit(context.id(i)));
obj+=run(()=>{if ( i==0 ) {
return ""+id.text;}
else {
return "."+id.text;}
});
}
return obj;
}
public  override  object VisitName( NameContext context ){
var obj = "";
foreach (var i in range(0,context.id().Length-1,1,true,true)){
var id = (Result)(Visit(context.id(i)));
obj+=run(()=>{if ( i==0 ) {
return ""+id.text;}
else {
return "."+id.text;}
});
}
return obj;
}
public  override  object VisitEnumStatement( EnumStatementContext context ){
var obj = "";
var id = (Result)(Visit(context.id()));
var header = "";
var typ = "int";
if ( context.annotationSupport()!=null ) {
header+=Visit(context.annotationSupport());
}
header+=id.permission+" enum "+id.text+":"+typ;
header+=Wrap+BlockLeft+Wrap;
foreach (var i in range(0,context.enumSupportStatement().Length-1,1,true,true)){
obj+=Visit(context.enumSupportStatement(i));
}
obj+=BlockRight+Terminate+Wrap;
obj = header+obj;
return obj;
}
public  override  object VisitEnumSupportStatement( EnumSupportStatementContext context ){
var id = (Result)(Visit(context.id()));
if ( context.integerExpr()!=null ) {
var op = "";
if ( context.add()!=null ) {
op = (string)(Visit(context.add()));
}
id.text+=" = "+op+Visit(context.integerExpr());
}
return id.text+",";
}
public  override  object VisitNamespaceFunctionStatement( NamespaceFunctionStatementContext context ){
var id = (Result)(Visit(context.id()));
var obj = "";
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
var pout = "";
if ( context.parameterClauseOut()!=null ) {
pout = (string)(Visit(context.parameterClauseOut()));
}
if ( context.t.Type==Right_Flow ) {
pout = run(()=>{if ( context.Discard()!=null ) {
return "void";}
else if ( pout!="void" ) {
return (new System.Text.StringBuilder("").Append(Task).Append("<").Append(pout).Append(">")).to_str();}
else {
return Task;}
});
obj+=(new System.Text.StringBuilder("").Append(id.permission).Append(" async static ").Append(pout).Append(" ").Append(id.text).Append("")).to_str();
}
else {
if ( context.y!=null ) {
if ( pout!="void" ) {
pout = (new System.Text.StringBuilder("").Append(IEnum).Append("<").Append(pout).Append(">")).to_str();
}
}
obj+=(new System.Text.StringBuilder("").Append(id.permission).Append(" static ").Append(pout).Append(" ").Append(id.text).Append("")).to_str();
}
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = (TemplateItem)(Visit(context.templateDefine()));
obj+=template.Template;
templateContract = template.Contract;
}
this.add_current_set();
obj+=Visit(context.parameterClauseIn())+templateContract+BlockLeft+Wrap;
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitNamespaceConstantStatement( NamespaceConstantStatementContext context ){
var id = (Result)(Visit(context.id()));
var expr = (Result)(Visit(context.expression()));
var typ = run(()=>{if ( context.typeType()!=null ) {
return (string)(Visit(context.typeType()));}
else {
return (string)(expr.data);}
});
var obj = "";
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
obj+=(new System.Text.StringBuilder("").Append(id.permission).Append(" const ").Append(typ).Append(" ").Append(id.text).Append(" = ").Append(expr.text).Append(" ").Append(Terminate+Wrap).Append("")).to_str();
return obj;
}
public  override  object VisitNamespaceVariableStatement( NamespaceVariableStatementContext context ){
var r1 = (Result)(Visit(context.id()));
this.add_id(r1.text);
var isMutable = r1.isVirtual;
var typ = "";
Result r2 = null;
if ( context.expression()!=null ) {
r2 = (Result)(Visit(context.expression()));
typ = (string)(r2.data);
}
if ( context.typeType()!=null ) {
typ = (string)(Visit(context.typeType()));
}
var obj = "";
if ( context.annotationSupport()!=null ) {
this.selfPropertyID=r1.text;
obj+=Visit(context.annotationSupport());
}
if ( this.selfPropertyContent.len>0 ) {
var pri = "";
if ( this.selfPropertyVariable ) {
pri = (new System.Text.StringBuilder("private static ").Append(typ).Append(" _").Append(r1.text).Append("")).to_str();
if ( r2!=null ) {
pri+=" = "+r2.text;
}
pri+=Terminate+Wrap;
}
obj = pri+obj;
obj+=(new System.Text.StringBuilder("").Append(r1.permission).Append(" static ").Append(typ).Append(" ").Append(r1.text).Append("").Append(BlockLeft).Append("")).to_str();
foreach (var v in this.selfPropertyContent){
obj+=v;
}
obj+=BlockRight+Wrap;
this.selfPropertyContent.clear();
this.selfPropertyID="";
this.selfPropertyVariable=false;
}
else {
obj+=(new System.Text.StringBuilder("").Append(r1.permission).Append(" static ").Append(typ).Append(" ").Append(r1.text).Append("")).to_str();
obj+=run(()=>{if ( r2!=null ) {
return (new System.Text.StringBuilder(" = ").Append(r2.text).Append(" ").Append(Terminate+Wrap).Append("")).to_str();}
else {
return Terminate+Wrap;}
});
}
return obj;
}
}
}
