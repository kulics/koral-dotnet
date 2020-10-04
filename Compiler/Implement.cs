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
var extend = (new list<string>());
if ( context.packageFieldStatement()!=null ) {
var item = context.packageFieldStatement();
var r = (Result)(Visit(item));
obj+=r.text;
}
if ( context.packageNewStatement()!=null ) {
var item = context.packageNewStatement();
var r = (string)(Visit(item));
obj+=(new System.Text.StringBuilder().Append("public ").Append(id.text).Append(" ").Append(r)).to_str();
}
obj+=BlockRight+Wrap;
var header = "";
header+=(new System.Text.StringBuilder().Append(id.permission).Append(" partial class ").Append(id.text)).to_str();
var template = "";
var templateContract = "";
if ( context.templateDefine()!=null ) {
var item = (TemplateItem)(Visit(context.templateDefine()));
template+=item.Template;
templateContract = item.Contract;
header+=template;
}
if ( extend.length>0 ) {
var temp = extend[0];
foreach (var i in range(1, extend.length, 1, true, false)){
temp+=","+extend[i];
}
header+=":"+temp;
}
header+=templateContract+BlockLeft+Wrap;
obj = header+obj;
this.selfID="";
this.superID="";
return obj;
}
public  override  object VisitOverrideVariableStatement( OverrideVariableStatementContext context ){
var r1 = (Result)(Visit(context.id()));
var isMutable = r1.isVirtual;
var isVirtual = " override ";
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
pri = (new System.Text.StringBuilder().Append("private ").Append(typ).Append(" _").Append(r1.text)).to_str();
if ( r2!=null ) {
pri+=" = "+r2.text;
}
pri+=Terminate+Wrap;
}
obj = pri+obj;
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" ").Append(isVirtual).Append(" ").Append(typ).Append(" ").Append(r1.text).Append(BlockLeft)).to_str();
foreach (var v in this.selfPropertyContent){
obj+=v;
}
obj+=BlockRight+Wrap;
this.selfPropertyContent.clear();
this.selfPropertyID="";
this.selfPropertyVariable=false;
}
else {
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" ").Append(typ).Append(" ").Append(r1.text)).to_str();
if ( r2!=null ) {
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).to_str();
}
else {
obj+=Terminate+Wrap;
}
}
return obj;
}
public  override  object VisitOverrideFunctionStatement( OverrideFunctionStatementContext context ){
var id = (Result)(Visit(context.id()));
var isVirtual = " override ";
var obj = "";
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
obj+=(new System.Text.StringBuilder().Append(isVirtual).Append(" ").Append(pout).Append(" ").Append(id.text)).to_str();
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
if ( context.n!=null ) {
obj = "protected "+obj;
}
else {
obj = (new System.Text.StringBuilder().Append(id.permission).Append(" ")).to_str()+obj;
}
return obj;
}
}
}
