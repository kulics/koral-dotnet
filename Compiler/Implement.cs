using Library;
using static Library.Lib;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using static Compiler.KParser;
using static Compiler.Compiler_static;

namespace Compiler
{
public partial class LiteLangVisitor{
public  override  object VisitImplementStatement( ImplementStatementContext context ){
var id = (Result)(Visit(context.id()));
var obj = "";
var extend = (new list<string>());
foreach (var item in context.includeStatement()){
var r = (string)(Visit(item));
extend+=r;
}
foreach (var item in context.packageFieldStatement()){
var r = (Result)(Visit(item));
obj+=r.text;
}
foreach (var item in context.packageImplementStatement()){
var r = (Result)(Visit(item));
extend+=(string)(r.data);
obj+=r.text;
}
foreach (var item in context.packageNewStatement()){
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
foreach (var i in range(1, extend.length-1, 1, true, true)){
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
public  override  object VisitImplementVariableStatement( ImplementVariableStatementContext context ){
var r1 = (Result)(Visit(context.id()));
var isMutable = r1.isVirtual;
var isVirtual = "";
if ( r1.isVirtual ) {
isVirtual = " virtual ";
}
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
obj+=run(()=>{if ( r2!=null ) {
return (new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).to_str();}
else {
return Terminate+Wrap;}
});
}
return obj;
}
public  override  object VisitImplementFunctionStatement( ImplementFunctionStatementContext context ){
var id = (Result)(Visit(context.id()));
var isVirtual = "";
if ( id.isVirtual ) {
isVirtual = " virtual ";
}
var obj = "";
obj+=(new System.Text.StringBuilder().Append(id.permission).Append(" ")).to_str();
var pout = "";
if ( context.parameterClauseOut()!=null ) {
pout = (string)(Visit(context.parameterClauseOut()));
}
if ( context.t.Type==Right_Flow ) {
if ( context.Discard()!=null ) {
pout = "void";
}
else if ( pout!="void" ) {
if ( context.y!=null ) {
pout = (new System.Text.StringBuilder().Append(IEnum).Append("<").Append(pout).Append(">")).to_str();
}
pout = (new System.Text.StringBuilder().Append(Task).Append("<").Append(pout).Append(">")).to_str();
}
else {
pout = Task;
}
obj+=(new System.Text.StringBuilder().Append(isVirtual).Append(" async ").Append(pout).Append(" ").Append(id.text)).to_str();
}
else {
if ( context.y!=null ) {
if ( pout!="void" ) {
pout = (new System.Text.StringBuilder().Append(IEnum).Append("<").Append(pout).Append(">")).to_str();
}
}
obj+=(new System.Text.StringBuilder().Append(isVirtual).Append(" ").Append(pout).Append(" ").Append(id.text)).to_str();
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
public  override  object VisitOverrideVariableStatement( OverrideVariableStatementContext context ){
var r1 = (Result)(Visit(context.id()));
var isMutable = r1.isVirtual;
var isVirtual = " override ";
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
obj+=run(()=>{if ( r2!=null ) {
return (new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).to_str();}
else {
return Terminate+Wrap;}
});
}
return obj;
}
public  override  object VisitOverrideFunctionStatement( OverrideFunctionStatementContext context ){
var id = (Result)(Visit(context.id()));
var isVirtual = " override ";
var obj = "";
obj+=run(()=>{if ( context.n!=null ) {
return "protected ";}
else {
return (new System.Text.StringBuilder().Append(id.permission).Append(" ")).to_str();}
});
var pout = "";
if ( context.parameterClauseOut()!=null ) {
pout = (string)(Visit(context.parameterClauseOut()));
}
if ( context.t.Type==Right_Flow ) {
if ( context.Discard()!=null ) {
pout = "void";
}
else if ( pout!="void" ) {
if ( context.y!=null ) {
pout = (new System.Text.StringBuilder().Append(IEnum).Append("<").Append(pout).Append(">")).to_str();
}
pout = (new System.Text.StringBuilder().Append(Task).Append("<").Append(pout).Append(">")).to_str();
}
else {
pout = Task;
}
obj+=(new System.Text.StringBuilder().Append(isVirtual).Append(" async ").Append(pout).Append(" ").Append(id.text)).to_str();
}
else {
if ( context.y!=null ) {
if ( pout!="void" ) {
pout = (new System.Text.StringBuilder().Append(IEnum).Append("<").Append(pout).Append(">")).to_str();
}
}
obj+=(new System.Text.StringBuilder().Append(isVirtual).Append(" ").Append(pout).Append(" ").Append(id.text)).to_str();
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
}
}
