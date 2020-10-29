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
foreach (var i in Range(1, extend.Size(), 1)){
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
