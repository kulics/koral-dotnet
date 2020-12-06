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
public partial class Parameter{
public Parameter (string id  = "", string type  = "", string value  = "", string annotation  = "", string permission  = ""){ this.id = id ; 
this.type = type ; 
this.value = value ; 
this.annotation = annotation ; 
this.permission = permission ; 
 }
public string id ;
public string type ;
public string value ;
public string annotation ;
public string permission ;
}
public partial class FeelLangVisitorFunction:FeelLangVisitorExpression{
public FeelLangVisitorFunction (){  }
public  override  object VisitFunctionStatement( FunctionStatementContext context ){
var id = ((Result)Visit(context.id()));
var obj = "";
var pout = "";
if ( context.parameterClauseOut()==null ) {
pout = "void";
}
else {
pout = ((string)Visit(context.parameterClauseOut()));
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
var template = ((TemplateItem)Visit(context.templateDefine()));
obj+=template.template;
template_contract = template.contract;
}
Add_current_set();
Add_func_stack();
obj+=(new System.Text.StringBuilder().Append(Visit(context.parameterClauseIn())).Append(" ").Append(template_contract).Append(Wrap).Append(BlockLeft).Append(Wrap).Append(" ")).To_Str();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
obj+=BlockRight+Wrap;
Delete_current_set();
if ( Get_func_async() ) {
obj = " async "+obj;
}
Delete_func_stack();
return obj;
}
public  override  object VisitReturnStatement( ReturnStatementContext context ){
if ( context.tupleExpression()!=null ) {
var r = ((Result)Visit(context.tupleExpression()));
return (new System.Text.StringBuilder().Append("return ").Append(r.text).Append(Terminate).Append(Wrap)).To_Str();
}
return (new System.Text.StringBuilder().Append("return").Append(Terminate).Append(Wrap)).To_Str();
}
public  override  object VisitReturnAsyncStatement( ReturnAsyncStatementContext context ){
if ( context.tupleExpression()!=null ) {
var r = ((Result)Visit(context.tupleExpression()));
return (new System.Text.StringBuilder().Append("return ").Append(Task).Append(".FromResult(").Append(r.text).Append(")").Append(Terminate).Append(Wrap)).To_Str();
}
return (new System.Text.StringBuilder().Append("return ").Append(Task).Append(".FromResult(true)").Append(Terminate).Append(Wrap)).To_Str();
}
public  override  object VisitTuple( TupleContext context ){
var obj = "(";
foreach (var (i,v) in context.expression().WithIndex()){
var r = ((Result)Visit(v));
if ( i==0 ) {
obj+=r.text;
}
else {
obj+=", "+r.text;
}
}
obj+=")";
return (new Result("var", obj));
}
public  override  object VisitTupleExpression( TupleExpressionContext context ){
var obj = "";
foreach (var (i,v) in context.expression().WithIndex()){
var r = ((Result)Visit(v));
if ( i==0 ) {
obj+=r.text;
}
else {
obj+=", "+r.text;
}
}
if ( context.expression().Length>1 ) {
obj = (new System.Text.StringBuilder().Append("(").Append(obj).Append(")")).To_Str();
}
return (new Result("var", obj));
}
public  override  object VisitParameterClauseIn( ParameterClauseInContext context ){
var obj = "(";
foreach (var (i,v) in context.parameter().WithIndex()){
var p = ((Parameter)Visit(v));
var param = (new System.Text.StringBuilder().Append(p.annotation).Append(" ").Append(p.type).Append(" ").Append(p.id).Append(" ").Append(p.value)).To_Str();
if ( i==0 ) {
obj+=param;
}
else {
obj+=", "+param;
}
Add_ID(p.id);
}
obj+=")";
return obj;
}
public  override  object VisitParameterClauseOut( ParameterClauseOutContext context ){
var obj = "";
switch (context.parameter().Length) {
case 0 :
{ obj+="void";
} break;
case 1 :
{ var p = ((Parameter)Visit(context.parameter(0)));
obj+=p.type;
} break;
}
if ( context.parameter().Length>1 ) {
obj+="(";
foreach (var (i,v) in context.parameter().WithIndex()){
var p = ((Parameter)Visit(v));
var param = (new System.Text.StringBuilder().Append(p.annotation).Append(" ").Append(p.type).Append(" ").Append(p.id).Append(" ").Append(p.value)).To_Str();
if ( i==0 ) {
obj+=param;
}
else {
obj+=", "+param;
}
}
obj+=")";
}
return obj;
}
public  override  object VisitParameter( ParameterContext context ){
var p = (new Parameter());
var id = ((Result)Visit(context.id()));
p.id=id.text;
p.permission=id.permission;
if ( context.annotationSupport()!=null ) {
p.annotation=((string)Visit(context.annotationSupport()));
}
p.type=((string)Visit(context.typeType()));
if ( context.Dot_Dot_Dot()!=null ) {
p.type=(new System.Text.StringBuilder().Append("params ").Append(p.type).Append("[]")).To_Str();
}
if ( context.Bang()!=null ) {
p.type=(new System.Text.StringBuilder().Append("ref ").Append(p.type)).To_Str();
}
if ( context.expression()!=null ) {
p.value=" = "+(((Result)Visit(context.expression()))).text;
}
return p;
}
}
}
