using Library;
using static Library.Lib;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using static Compiler.LiteParser;
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
public partial class LiteLangVisitor{
public  virtual  string ProcessFunctionSupport( FunctionSupportStatementContext[] items ){
var obj = "";
var content = "";
var lazy = (new list<string>());
foreach (var item in items){
content+=run(()=>{if ( item.GetChild(0).@is<UsingStatementContext>() ) {
lazy.add("}");
return (new System.Text.StringBuilder("using (").Append(((string)(Visit(item)))).Append(") ").Append(BlockLeft).Append(" ").Append(Wrap).Append("")).to_str();}
else {
return Visit(item);}
});
}
if ( lazy.Count>0 ) {
foreach (var i in range(lazy.Count-1,0,1,false,true)){
content+=BlockRight;
}
}
obj+=content;
return obj;
}
public  override  object VisitFunctionStatement( FunctionStatementContext context ){
var id = ((Result)(Visit(context.id())));
var obj = "";
var pout = ((string)(Visit(context.parameterClauseOut())));
if ( context.t.Type==Right_Flow ) {
pout = run(()=>{if ( pout!="void" ) {
return (new System.Text.StringBuilder("").Append(Task).Append("<").Append(pout).Append(">")).to_str();}
else {
return Task;}
});
obj+=(new System.Text.StringBuilder(" async ").Append(pout).Append(" ").Append(id.text).Append("")).to_str();
}
else {
if ( context.y!=null ) {
if ( pout!="void" ) {
pout = (new System.Text.StringBuilder("").Append(IEnum).Append("<").Append(pout).Append(">")).to_str();
}
}
obj+=(new System.Text.StringBuilder("").Append(pout).Append(" ").Append(id.text).Append("")).to_str();
}
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = ((TemplateItem)(Visit(context.templateDefine())));
obj+=template.Template;
templateContract = template.Contract;
}
this.add_current_set();
obj+=(new System.Text.StringBuilder("").Append(Visit(context.parameterClauseIn())).Append(" ").Append(templateContract).Append(" ").Append(Wrap).Append(" ").Append(BlockLeft).Append(" ").Append(Wrap).Append("")).to_str();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
obj+=BlockRight+Wrap;
this.delete_current_set();
return obj;
}
public  override  object VisitReturnStatement( ReturnStatementContext context ){
if ( context.tupleExpression()!=null ) {
var r = ((Result)(Visit(context.tupleExpression())));
return "return "+r.text+Terminate+Wrap;
}
return (new System.Text.StringBuilder("return").Append(Terminate).Append("").Append(Wrap).Append("")).to_str();
}
public  override  object VisitYieldReturnStatement( YieldReturnStatementContext context ){
var r = ((Result)(Visit(context.tupleExpression())));
return "yield return "+r.text+Terminate+Wrap;
}
public  override  object VisitYieldBreakStatement( YieldBreakStatementContext context ){
return (new System.Text.StringBuilder("yield break").Append(Terminate).Append("").Append(Wrap).Append("")).to_str();
}
public  override  object VisitTuple( TupleContext context ){
var obj = "(";
foreach (var i in range(0,context.expression().Length-1,1,true,true)){
var r = ((Result)(Visit(context.expression(i))));
obj+=run(()=>{if ( i==0 ) {
return r.text;}
else {
return (new System.Text.StringBuilder(", ").Append(r.text).Append("")).to_str();}
});
}
obj+=")";
return (new Result(){data = "var",text = obj});
}
public  override  object VisitTupleExpression( TupleExpressionContext context ){
var obj = "";
foreach (var i in range(0,context.expression().Length-1,1,true,true)){
var r = ((Result)(Visit(context.expression(i))));
obj+=run(()=>{if ( i==0 ) {
return r.text;}
else {
return (new System.Text.StringBuilder(", ").Append(r.text).Append("")).to_str();}
});
}
if ( context.expression().Length>1 ) {
obj = (new System.Text.StringBuilder("(").Append(obj).Append(")")).to_str();
}
return (new Result(){data = "var",text = obj});
}
public  override  object VisitParameterClauseIn( ParameterClauseInContext context ){
var obj = "(";
var temp = (new list<string>());
foreach (var i in range(context.parameter().Length-1,0,1,false,true)){
var p = ((Parameter)(Visit(context.parameter(i))));
temp.add((new System.Text.StringBuilder("").Append(p.annotation).Append(" ").Append(p.type).Append(" ").Append(p.id).Append(" ").Append(p.value).Append("")).to_str());
this.add_id(p.id);
}
foreach (var i in range(temp.Count-1,0,1,false,true)){
obj+=run(()=>{if ( i==temp.Count-1 ) {
return temp[i];}
else {
return (new System.Text.StringBuilder(", ").Append(temp[i]).Append("")).to_str();}
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
var p = ((Parameter)(Visit(context.parameter(0))));
obj+=p.type;
}
if ( context.parameter().Length>1 ) {
obj+="( ";
var temp = (new list<string>());
foreach (var i in range(context.parameter().Length-1,0,1,false,true)){
var p = ((Parameter)(Visit(context.parameter(i))));
temp.add((new System.Text.StringBuilder("").Append(p.annotation).Append(" ").Append(p.type).Append(" ").Append(p.id).Append(" ").Append(p.value).Append("")).to_str());
}
foreach (var i in range(temp.Count-1,0,1,false,true)){
obj+=run(()=>{if ( i==temp.Count-1 ) {
return temp[i];}
else {
return (new System.Text.StringBuilder(", ").Append(temp[i]).Append("")).to_str();}
});
}
obj+=" )";
}
return obj;
}
public  override  object VisitParameterClauseSelf( ParameterClauseSelfContext context ){
var p = (new Parameter());
var id = ((Result)(Visit(context.id())));
p.id=id.text;
p.permission=id.permission;
p.type=((string)(Visit(context.typeType())));
return p;
}
public  override  object VisitParameter( ParameterContext context ){
var p = (new Parameter());
var id = ((Result)(Visit(context.id())));
p.id=id.text;
p.permission=id.permission;
if ( context.annotationSupport()!=null ) {
p.annotation=((string)(Visit(context.annotationSupport())));
}
if ( context.expression()!=null ) {
p.value=(new System.Text.StringBuilder("=").Append(((Result)(Visit(context.expression()))).text).Append("")).to_str();
}
p.type=((string)(Visit(context.typeType())));
return p;
}
}
}
