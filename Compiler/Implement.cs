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
public  override  object VisitImplementStatement( ImplementStatementContext context ){
var Self = ((Parameter)(Visit(context.parameterClauseSelf())));
this.selfID=Self.id;
var isVirtual = "";
var obj = "";
var extends = "";
if ( context.typeType()!=null ) {
extends+=":"+Visit(context.typeType());
}
obj+=(new System.Text.StringBuilder("").Append(Self.permission).Append(" partial class ").Append(Self.type+extends+BlockLeft+Wrap).Append("")).to_str();
foreach (var item in context.implementSupportStatement()){
obj+=Visit(item);
}
obj+=BlockRight+Wrap;
this.selfID="";
return obj;
}
public  override  object VisitImplementVariableStatement( ImplementVariableStatementContext context ){
var r1 = ((Result)(Visit(context.id())));
var isMutable = r1.isVirtual;
var isVirtual = "";
if ( r1.isVirtual ) {
isVirtual = " virtual ";
}
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
this.selfPropertyID=r1.text;
obj+=Visit(context.annotationSupport());
}
if ( this.selfPropertyContent.len>0 ) {
var pri = "";
if ( this.selfPropertyVariable ) {
pri = (new System.Text.StringBuilder("private ").Append(typ).Append(" _").Append(r1.text).Append("")).to_str();
if ( r2!=null ) {
pri+=" = "+r2.text;
}
pri+=Terminate+Wrap;
}
obj = pri+obj;
obj+=(new System.Text.StringBuilder("").Append(r1.permission).Append(" ").Append(isVirtual).Append(" ").Append(typ).Append(" ").Append(r1.text).Append("").Append(BlockLeft).Append("")).to_str();
foreach (var v in this.selfPropertyContent){
obj+=v;
}
obj+=BlockRight+Wrap;
this.selfPropertyContent.clear();
this.selfPropertyID="";
this.selfPropertyVariable=false;
}
else {
obj+=(new System.Text.StringBuilder("").Append(r1.permission).Append(" ").Append(typ).Append(" ").Append(r1.text).Append("")).to_str();
obj+=run(()=>{if ( r2!=null ) {
return (new System.Text.StringBuilder(" = ").Append(r2.text).Append(" ").Append(Terminate).Append(" ").Append(Wrap).Append("")).to_str();}
else {
return Terminate+Wrap;}
});
}
return obj;
}
public  override  object VisitImplementFunctionStatement( ImplementFunctionStatementContext context ){
var id = ((Result)(Visit(context.id())));
var isVirtual = "";
if ( id.isVirtual ) {
isVirtual = " virtual ";
}
var obj = "";
obj+=(new System.Text.StringBuilder("").Append(id.permission).Append(" ")).to_str();
var pout = ((string)(Visit(context.parameterClauseOut())));
if ( context.t.Type==Right_Flow ) {
pout = run(()=>{if ( pout!="void" ) {
return (new System.Text.StringBuilder("").Append(Task).Append("<").Append(pout).Append(">")).to_str();}
else {
return Task;}
});
obj+=(new System.Text.StringBuilder("").Append(isVirtual).Append(" async ").Append(pout).Append(" ").Append(id.text).Append("")).to_str();
}
else {
if ( context.y!=null ) {
if ( pout!="void" ) {
pout = (new System.Text.StringBuilder("").Append(IEnum).Append("<").Append(pout).Append(">")).to_str();
}
}
obj+=(new System.Text.StringBuilder("").Append(isVirtual).Append(" ").Append(pout).Append(" ").Append(id.text).Append("")).to_str();
}
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = ((TemplateItem)(Visit(context.templateDefine())));
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
public  override  object VisitOverrideFunctionStatement( OverrideFunctionStatementContext context ){
var id = ((Result)(Visit(context.id(1))));
this.superID=((Result)(Visit(context.id(0)))).text;
var isVirtual = " override ";
var obj = "";
obj+=run(()=>{if ( context.n!=null ) {
return "protected ";}
else {
return (new System.Text.StringBuilder("").Append(id.permission).Append(" ")).to_str();}
});
var pout = ((string)(Visit(context.parameterClauseOut())));
if ( context.t.Type==Right_Flow ) {
pout = run(()=>{if ( pout!="void" ) {
return (new System.Text.StringBuilder("").Append(Task).Append("<").Append(pout).Append(">")).to_str();}
else {
return Task;}
});
obj+=(new System.Text.StringBuilder("").Append(isVirtual).Append(" async ").Append(pout).Append(" ").Append(id.text).Append("")).to_str();
}
else {
if ( context.y!=null ) {
if ( pout!="void" ) {
pout = (new System.Text.StringBuilder("").Append(IEnum).Append("<").Append(pout).Append(">")).to_str();
}
}
obj+=(new System.Text.StringBuilder("").Append(isVirtual).Append(" ").Append(pout).Append(" ").Append(id.text).Append("")).to_str();
}
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = ((TemplateItem)(Visit(context.templateDefine())));
obj+=template.Template;
templateContract = template.Contract;
}
this.add_current_set();
obj+=Visit(context.parameterClauseIn())+templateContract+BlockLeft+Wrap;
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
this.superID="";
return obj;
}
public  override  object VisitOverrideControlStatement( OverrideControlStatementContext context ){
var r1 = ((Result)(Visit(context.id(1))));
this.superID=((Result)(Visit(context.id(0)))).text;
var isMutable = true;
var isVirtual = " override ";
var typ = "";
if ( context.typeType()!=null ) {
typ = ((string)(Visit(context.typeType())));
}
var obj = "";
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
obj+=(new System.Text.StringBuilder("").Append(r1.permission).Append(" ").Append(isVirtual).Append(" ").Append(typ).Append(" ").Append(r1.text).Append("").Append(BlockLeft).Append("")).to_str();
foreach (var item in context.packageControlSubStatement()){
var temp = ((Result)(Visit(item)));
obj+=temp.text;
}
obj+=BlockRight+Wrap;
this.superID="";
return obj;
}
public  override  object VisitImplementNewStatement( ImplementNewStatementContext context ){
var text = "";
var Self = ((Parameter)(Visit(context.parameterClauseSelf())));
this.selfID=Self.id;
text+=(new System.Text.StringBuilder("").Append(Self.permission).Append(" partial class ").Append(Self.type).Append("").Append(BlockLeft+Wrap).Append("")).to_str();
text+=(new System.Text.StringBuilder("public ").Append(Self.type).Append(" ")).to_str();
this.add_current_set();
text+=((string)(Visit(context.parameterClauseIn())));
if ( context.expressionList()!=null ) {
text+=(new System.Text.StringBuilder(":base(").Append(((Result)(Visit(context.expressionList()))).text).Append(")")).to_str();
}
text+=BlockLeft+ProcessFunctionSupport(context.functionSupportStatement())+BlockRight+Wrap;
this.delete_current_set();
text+=BlockRight+Wrap;
this.selfID="";
return text;
}
}
}
