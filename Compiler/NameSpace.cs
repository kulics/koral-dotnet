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
obj+=(new System.Text.StringBuilder().Append("namespace ").Append(ns.name).Append(Wrap).Append(BlockLeft).Append(Wrap)).to_str();
var content = "";
var contentStatic = "";
this.add_current_set();
foreach (var item in context.namespaceSupportStatement()){
var child = item.GetChild(0);
var type = child.GetType();
if ( type==typeof(PackageStatementContext) ) {
var childContext = (PackageStatementContext)(child);
var id = (Result)(Visit(childContext.id()));
this.add_type(id.text);
}
else if ( type==typeof(TypeTagStatementContext) ) {
var childContext = (TypeTagStatementContext)(child);
this.add_type(childContext.Comment_Tag().GetText().sub_str(2));
}
}
foreach (var item in context.namespaceSupportStatement()){
var type = item.GetChild(0).GetType();
if ( type==typeof(NamespaceVariableStatementContext)||type==typeof(NamespaceFunctionStatementContext)||type==typeof(NamespaceConstantStatementContext) ) {
contentStatic+=Visit(item);
}
else if ( type==typeof(ImportStatementContext) ) {
imports+=Visit(item);
}
else {
content+=Visit(item);
}
}
obj+=content;
if ( contentStatic!="" ) {
obj+=(new System.Text.StringBuilder().Append("public partial class ").Append(ns.name.sub_str(ns.name.last_index_of(".")+1)).Append("_static ").Append(BlockLeft).Append(Wrap).Append(contentStatic).Append(BlockRight).Append(Wrap)).to_str();
}
this.delete_current_set();
obj+=BlockRight+Wrap;
obj = (new System.Text.StringBuilder().Append("using Library;").Append(Wrap).Append("using static Library.Lib;").Append(Wrap).Append(imports).Append(Wrap)).to_str()+obj;
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
obj+=(new System.Text.StringBuilder().Append("using static ").Append(ns)).to_str();
}
else if ( context.id()!=null ) {
obj+=(new System.Text.StringBuilder().Append("using ").Append(ns).Append(".").Append(((Result)(Visit(context.id()))).text)).to_str();
}
else {
obj+=(new System.Text.StringBuilder().Append("using ").Append(ns)).to_str();
}
obj+=Terminate+Wrap;
return obj;
}
public  override  object VisitNameSpaceItem( NameSpaceItemContext context ){
var obj = "";
foreach (var i in range(0, context.id().Length-1, 1, true, true)){
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
foreach (var i in range(0, context.id().Length-1, 1, true, true)){
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
header+=(new System.Text.StringBuilder().Append(id.permission).Append(" enum ").Append(id.text).Append(":").Append(typ)).to_str();
header+=Wrap+BlockLeft+Wrap;
foreach (var i in range(0, context.enumSupportStatement().Length-1, 1, true, true)){
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
pout = (new System.Text.StringBuilder().Append(Task).Append("<").Append(pout).Append(">")).to_str();
}
else {
pout = Task;
}
}
}
obj+=(new System.Text.StringBuilder().Append(pout).Append(" ").Append(id.text)).to_str();
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = (TemplateItem)(Visit(context.templateDefine()));
obj+=template.Template;
templateContract = template.Contract;
}
this.add_current_set();
this.add_func_stack();
obj+=Visit(context.parameterClauseIn())+templateContract+BlockLeft+Wrap;
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
if ( get_func_async() ) {
obj = " async "+obj;
}
this.delete_func_stack();
obj = id.permission+" static "+obj;
return obj;
}
public  override  object VisitNamespaceConstantStatement( NamespaceConstantStatementContext context ){
var id = (Result)(Visit(context.id()));
var expr = (Result)(Visit(context.expression()));
var typ = "";
if ( context.typeType()!=null ) {
typ = (string)(Visit(context.typeType()));
}
else {
typ = (string)(expr.data);
}
var obj = "";
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
obj+=(new System.Text.StringBuilder().Append(id.permission).Append(" const ").Append(typ).Append(" ").Append(id.text).Append(" = ").Append(expr.text).Append(Terminate).Append(Wrap)).to_str();
return obj;
}
public  override  object VisitNamespaceVariableStatement( NamespaceVariableStatementContext context ){
var r1 = (Result)(Visit(context.id()));
this.add_id(r1.text);
var isMutable = r1.isVirtual;
var typ = "";
Result? r2 = null;
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
pri = (new System.Text.StringBuilder().Append("private static ").Append(typ).Append(" _").Append(r1.text)).to_str();
if ( r2!=null ) {
pri+=" = "+r2.text;
}
pri+=Terminate+Wrap;
}
obj = pri+obj;
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" static ").Append(typ).Append(" ").Append(r1.text).Append(BlockLeft)).to_str();
foreach (var v in this.selfPropertyContent){
obj+=v;
}
obj+=BlockRight+Wrap;
this.selfPropertyContent.clear();
this.selfPropertyID="";
this.selfPropertyVariable=false;
}
else {
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" static ").Append(typ).Append(" ").Append(r1.text)).to_str();
if ( r2!=null ) {
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).to_str();
}
else {
obj+=Terminate+Wrap;
}
}
return obj;
}
public  override  object VisitTypeTagStatement( TypeTagStatementContext context ){
return "";
}
}
}
