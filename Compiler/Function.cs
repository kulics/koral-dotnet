using Library;
using static Library.Lib;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using static Compiler.KParser;
using static Compiler.Compiler_static;

namespace Compiler
{
public partial class Parameter{
public string id;
public string type;
public string value;
public string annotation;
public string permission;
}
public partial class KLangVisitor{
public  virtual  string ProcessFunctionSupport( FunctionSupportStatementContext[] items ){
var obj = "";
foreach (var item in items){
obj+=Visit(item);
}
return obj;
}
public  override  object VisitFunctionStatement( FunctionStatementContext context ){
var id = (Result)(Visit(context.id()));
var obj = "";
var pout = "";
if ( context.parameterClauseOut()!=null ) {
pout=(string)(Visit(context.parameterClauseOut()));
}
if ( context.t.Type==Right_Flow ) {
pout=run(()=>{if ( pout!="void" ) {
return (new System.Text.StringBuilder().Append(Task).Append("<").Append(pout).Append(">")).to_str();}
else {
return Task;}
});
obj+=(new System.Text.StringBuilder().Append(pout).Append(" ").Append(id.text)).to_str();
}
else {
if ( context.y!=null ) {
if ( pout!="void" ) {
pout=(new System.Text.StringBuilder().Append(IEnum).Append("<").Append(pout).Append(">")).to_str();
}
}
obj+=(new System.Text.StringBuilder().Append(pout).Append(" ").Append(id.text)).to_str();
}
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = (TemplateItem)(Visit(context.templateDefine()));
obj+=template.Template;
templateContract=template.Contract;
}
this.add_current_set();
this.add_func_stack();
obj+=(new System.Text.StringBuilder().Append(Visit(context.parameterClauseIn())).Append(" ").Append(templateContract).Append(Wrap).Append(BlockLeft).Append(Wrap).Append(" ")).to_str();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
obj+=BlockRight+Wrap;
this.delete_current_set();
if ( get_func_async() ) {
obj=" async "+obj;
}
this.delete_func_stack();
return obj;
}
public  override  object VisitReturnStatement( ReturnStatementContext context ){
if ( context.tupleExpression()!=null ) {
var r = (Result)(Visit(context.tupleExpression()));
return (new System.Text.StringBuilder().Append("return ").Append(r.text).Append(Terminate).Append(Wrap)).to_str();
}
return (new System.Text.StringBuilder().Append("return").Append(Terminate).Append(Wrap)).to_str();
}
public  override  object VisitReturnAsyncStatement( ReturnAsyncStatementContext context ){
if ( context.tupleExpression()!=null ) {
var r = (Result)(Visit(context.tupleExpression()));
return (new System.Text.StringBuilder().Append("return ").Append(Task).Append(".FromResult(").Append(r.text).Append(")").Append(Terminate).Append(Wrap)).to_str();
}
return (new System.Text.StringBuilder().Append("return ").Append(Task).Append(".FromResult(true)").Append(Terminate).Append(Wrap)).to_str();
}
public  override  object VisitYieldReturnStatement( YieldReturnStatementContext context ){
var r = (Result)(Visit(context.tupleExpression()));
return (new System.Text.StringBuilder().Append("yield return ").Append(r.text).Append(Terminate).Append(Wrap)).to_str();
}
public  override  object VisitYieldBreakStatement( YieldBreakStatementContext context ){
return (new System.Text.StringBuilder().Append("yield break").Append(Terminate).Append(Wrap)).to_str();
}
public  override  object VisitTuple( TupleContext context ){
var obj = "(";
foreach (var i in range(0, context.expression().Length-1, 1, true, true)){
var r = (Result)(Visit(context.expression(i)));
obj+=run(()=>{if ( i==0 ) {
return r.text;}
else {
return ", "+r.text;}
});
}
obj+=")";
return (new Result(){data = "var",text = obj});
}
public  override  object VisitTupleExpression( TupleExpressionContext context ){
var obj = "";
foreach (var i in range(0, context.expression().Length-1, 1, true, true)){
var r = (Result)(Visit(context.expression(i)));
obj+=run(()=>{if ( i==0 ) {
return r.text;}
else {
return ", "+r.text;}
});
}
if ( context.expression().Length>1 ) {
obj=(new System.Text.StringBuilder().Append("(").Append(obj).Append(")")).to_str();
}
return (new Result(){data = "var",text = obj});
}
public  override  object VisitParameterClauseIn( ParameterClauseInContext context ){
var obj = "(";
var temp = (new list<string>());
foreach (var i in range(context.parameter().Length-1, 0, 1, false, true)){
var p = (Parameter)(Visit(context.parameter(i)));
temp.add((new System.Text.StringBuilder().Append(p.annotation).Append(" ").Append(p.type).Append(" ").Append(p.id).Append(" ").Append(p.value)).to_str());
this.add_id(p.id);
}
foreach (var i in range(temp.Count-1, 0, 1, false, true)){
obj+=run(()=>{if ( i==temp.Count-1 ) {
return temp[i];}
else {
return ", "+temp[i];}
});
}
obj+=")";
return obj;
}
public  override  object VisitParameterClauseOut( ParameterClauseOutContext context ){
var obj = "";
if ( context.parameter().Length==0 ) {
obj+="void";
}
else if ( context.parameter().Length==1 ) {
var p = (Parameter)(Visit(context.parameter(0)));
obj+=p.type;
}
if ( context.parameter().Length>1 ) {
obj+="( ";
var temp = (new list<string>());
foreach (var i in range(context.parameter().Length-1, 0, 1, false, true)){
var p = (Parameter)(Visit(context.parameter(i)));
temp.add((new System.Text.StringBuilder().Append(p.annotation).Append(" ").Append(p.type).Append(" ").Append(p.id).Append(" ").Append(p.value)).to_str());
}
foreach (var i in range(temp.Count-1, 0, 1, false, true)){
obj+=run(()=>{if ( i==temp.Count-1 ) {
return temp[i];}
else {
return ", "+temp[i];}
});
}
obj+=" )";
}
return obj;
}
public  override  object VisitParameter( ParameterContext context ){
var p = (new Parameter());
var id = (Result)(Visit(context.id()));
p.id=id.text;
p.permission=id.permission;
if ( context.annotationSupport()!=null ) {
p.annotation=(string)(Visit(context.annotationSupport()));
}
if ( context.expression()!=null ) {
p.value=(new System.Text.StringBuilder().Append("= ").Append(((Result)(Visit(context.expression()))).text)).to_str();
}
p.type=(string)(Visit(context.typeType()));
if ( context.Comma_Comma_Comma()!=null ) {
p.type=(new System.Text.StringBuilder().Append("params ").Append(p.type).Append("[]")).to_str();
}
if ( context.Bang()!=null ) {
p.type=(new System.Text.StringBuilder().Append("ref ").Append(p.type)).to_str();
}
return p;
}
}
}
