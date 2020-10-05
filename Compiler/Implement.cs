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
public partial class FeelLangVisitor{
public  override  object VisitImplementStatement( ImplementStatementContext context ){
var id = (Result)(Visit(context.id()));
var obj = "";
var extend = (new List<string>());
if ( context.packageFieldStatement()!=null ) {
var item = context.packageFieldStatement();
var r = (Result)(Visit(item));
obj+=r.text;
}
if ( context.packageNewStatement()!=null ) {
var item = context.packageNewStatement();
var r = (string)(Visit(item));
obj+=(new System.Text.StringBuilder().Append("public ").Append(id.text).Append(" ").Append(r)).To_Str();
}
obj+=BlockRight+Wrap;
var header = "";
header+=(new System.Text.StringBuilder().Append(id.permission).Append(" partial class ").Append(id.text)).To_Str();
var template = "";
var template_contract = "";
if ( context.templateDefine()!=null ) {
var item = (TemplateItem)(Visit(context.templateDefine()));
template+=item.template;
template_contract = item.contract;
header+=template;
}
if ( extend.Size()>0 ) {
var temp = extend[0];
foreach (var i in range(1, extend.Size(), 1, true, false)){
temp+=","+extend[i];
}
header+=":"+temp;
}
header+=template_contract+BlockLeft+Wrap;
obj = header+obj;
this.self_ID="";
this.super_ID="";
return obj;
}
public  override  object VisitOverrideVariableStatement( OverrideVariableStatementContext context ){
var r1 = (Result)(Visit(context.id()));
var is_mutable = r1.is_virtual;
var is_virtual = " override ";
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
this.self_property_ID=r1.text;
obj+=Visit(context.annotationSupport());
}
if ( this.self_property_content.Size()>0 ) {
var pri = "";
if ( this.self_property_variable ) {
pri = (new System.Text.StringBuilder().Append("private ").Append(typ).Append(" _").Append(r1.text)).To_Str();
if ( r2!=null ) {
pri+=" = "+r2.text;
}
pri+=Terminate+Wrap;
}
obj = pri+obj;
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" ").Append(is_virtual).Append(" ").Append(typ).Append(" ").Append(r1.text).Append(BlockLeft)).To_Str();
foreach (var v in this.self_property_content){
obj+=v;
}
obj+=BlockRight+Wrap;
this.self_property_content.Clear();
this.self_property_ID="";
this.self_property_variable=false;
}
else {
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" ").Append(typ).Append(" ").Append(r1.text)).To_Str();
if ( r2!=null ) {
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).To_Str();
}
else {
obj+=Terminate+Wrap;
}
}
return obj;
}
public  override  object VisitOverrideFunctionStatement( OverrideFunctionStatementContext context ){
var id = (Result)(Visit(context.id()));
var is_virtual = " override ";
var obj = "";
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
obj+=(new System.Text.StringBuilder().Append(is_virtual).Append(" ").Append(pout).Append(" ").Append(id.text)).To_Str();
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
if ( context.n!=null ) {
obj = "protected "+obj;
}
else {
obj = (new System.Text.StringBuilder().Append(id.permission).Append(" ")).To_Str()+obj;
}
return obj;
}
}
}
