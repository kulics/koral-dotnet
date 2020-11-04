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
public partial class Namespace{
public string name;
public string imports;
}
public partial class FeelLangVisitor{
public  override  object VisitStatement( StatementContext context ){
var obj = "";
var imports = "";
var ns = (Namespace)(Visit(context.exportStatement()));
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
obj+=(new System.Text.StringBuilder().Append("namespace ").Append(ns.name).Append(Wrap).Append(BlockLeft).Append(Wrap)).To_Str();
var content = "";
var content_static = "";
this.Add_current_set();
foreach (var item in context.namespaceSupportStatement()){
var child = item.GetChild(0);
var type = child.GetType();
if ( type==typeof(PackageStatementContext) ) {
var childContext = (PackageStatementContext)(child);
var id = (Result)(Visit(childContext.id()));
this.Add_type(id.text);
}
}
foreach (var item in context.namespaceSupportStatement()){
var type = item.GetChild(0).GetType();
if ( type==typeof(NamespaceVariableStatementContext)||type==typeof(NamespaceFunctionStatementContext) ) {
content_static+=Visit(item);
}
else if ( type==typeof(ImportStatementContext) ) {
imports+=Visit(item);
}
else {
content+=Visit(item);
}
}
obj+=content;
if ( content_static!="" ) {
obj+=(new System.Text.StringBuilder().Append("public partial class ").Append(ns.name.Sub_Str(ns.name.Last_index_of(".")+1)).Append("_static ").Append(BlockLeft).Append(Wrap).Append(content_static).Append(BlockRight).Append(Wrap)).To_Str();
}
this.Delete_current_set();
obj+=BlockRight+Wrap;
obj = (new System.Text.StringBuilder().Append("using Library;").Append(Wrap).Append("using static Library.Lib;").Append(Wrap).Append(imports).Append(Wrap)).To_Str()+obj;
return obj;
}
public  override  object VisitExportStatement( ExportStatementContext context ){
var name = (string)(Visit(context.nameSpaceItem()));
var obj = (new Namespace(){name = name});
return obj;
}
public  override  object VisitImportStatement( ImportStatementContext context ){
var obj = "";
foreach (var item in context.importSubStatement()){
obj+=(string)(Visit(item));
}
return obj;
}
public  override  object VisitImportSubStatement( ImportSubStatementContext context ){
var obj = "";
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
var ns = (string)(Visit(context.nameSpaceItem()));
if ( context.Dot()!=null ) {
obj+=(new System.Text.StringBuilder().Append("using static ").Append(ns)).To_Str();
}
else if ( context.id()!=null ) {
obj+=(new System.Text.StringBuilder().Append("using ").Append(ns).Append(".").Append(((Result)(Visit(context.id()))).text)).To_Str();
}
else {
obj+=(new System.Text.StringBuilder().Append("using ").Append(ns)).To_Str();
}
obj+=Terminate+Wrap;
return obj;
}
public  override  object VisitNameSpaceItem( NameSpaceItemContext context ){
var obj = "";
foreach (var i in Range(0, context.id().Length, 1)){
var id = (Result)(Visit(context.id(i)));
if ( i==0 ) {
obj+=id.text;
}
else {
obj+="."+id.text;
}
}
return obj;
}
public  override  object VisitName( NameContext context ){
var obj = "";
foreach (var i in Range(0, context.id().Length, 1)){
var id = (Result)(Visit(context.id(i)));
if ( i==0 ) {
obj+=id.text;
}
else {
obj+="."+id.text;
}
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
header+=(new System.Text.StringBuilder().Append(id.permission).Append(" enum ").Append(id.text).Append(":").Append(typ)).To_Str();
header+=Wrap+BlockLeft+Wrap;
foreach (var i in Range(0, context.enumSupportStatement().Length, 1)){
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
if ( context.t==null ) {
pout = "void";
}
else {
pout = (string)(Visit(context.parameterClauseOut()));
if ( context.t.Type==Right_Flow ) {
if ( pout!="void" ) {
pout = (new System.Text.StringBuilder().Append(Task).Append("<").Append(pout).Append(">")).To_Str();
}
else {
pout = Task;
}
}
}
obj+=(new System.Text.StringBuilder().Append(pout).Append(" ").Append(id.text)).To_Str();
var template_contract = "";
if ( context.templateDefine()!=null ) {
var template = (TemplateItem)(Visit(context.templateDefine()));
obj+=template.template;
template_contract = template.contract;
}
this.Add_current_set();
this.Add_func_stack();
obj+=Visit(context.parameterClauseIn())+template_contract+BlockLeft+Wrap;
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.Delete_current_set();
obj+=BlockRight+Wrap;
if ( Get_func_async() ) {
obj = " async "+obj;
}
this.Delete_func_stack();
obj = id.permission+" static "+obj;
return obj;
}
public  override  object VisitNamespaceVariableStatement( NamespaceVariableStatementContext context ){
var r1 = (Result)(Visit(context.id()));
this.Add_ID(r1.text);
var is_mutable = r1.is_virtual;
var typ = "";
Result? r2 = null;
if ( context.expression()!=null ) {
r2 = (Result)(Visit(context.expression()));
typ = (string)(r2.data);
}
if ( context.typeType()!=null ) {
typ = (string)(Visit(context.typeType()));
}
var isMutable = true;
if ( !r1.isMutable ) {
switch (typ) {
case "int" :
{ isMutable = false;
} break;case "uint" :
{ isMutable = false;
} break;case "long" :
{ isMutable = false;
} break;case "ulong" :
{ isMutable = false;
} break;case "ushort" :
{ isMutable = false;
} break;case "short" :
{ isMutable = false;
} break;case "byte" :
{ isMutable = false;
} break;case "sbyte" :
{ isMutable = false;
} break;case "float" :
{ isMutable = false;
} break;case "double" :
{ isMutable = false;
} break;case "bool" :
{ isMutable = false;
} break;case "char" :
{ isMutable = false;
} break;case "string" :
{ isMutable = false;
} break;
}
}
var obj = "";
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
if ( this.self_property_content.Size()>0 ) {
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" static ").Append(typ).Append(" ").Append(r1.text).Append(BlockLeft)).To_Str();
foreach (var v in this.self_property_content){
obj+=v;
}
obj+=BlockRight+Wrap;
this.self_property_content.Clear();
}
else if ( isMutable||r2==null ) {
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" static ").Append(typ).Append(" ").Append(r1.text)).To_Str();
if ( r2!=null ) {
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).To_Str();
}
else {
obj+=Terminate+Wrap;
}
}
else {
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" const ").Append(typ).Append(" ").Append(r1.text).Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).To_Str();
}
return obj;
}
}
}
