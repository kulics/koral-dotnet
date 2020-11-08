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
public  override  object VisitCallExpression( CallExpressionContext context ){
var r = (Result)(Visit(context.id()));
r.text="."+r.text;
if ( context.templateCall()!=null ) {
r.text+="<"+((string)(Visit(context.templateCall())))+">";
}
if ( context.callFunc()!=null ) {
var e2 = (Result)(Visit(context.callFunc()));
r.text=r.text+e2.text;
}
else if ( context.callElement()!=null ) {
var e2 = (Result)(Visit(context.callElement()));
r.text=r.text+e2.text;
}
return r;
}
public  override  object VisitCallElement( CallElementContext context ){
if ( context.expression()==null ) {
return ((new Result(){text = (string)(Visit(context.slice()))}));
}
var r = (Result)(Visit(context.expression()));
r.text=(new System.Text.StringBuilder().Append("[").Append(r.text).Append("]")).To_Str();
return r;
}
public  override  object VisitSlice( SliceContext context ){
return (string)(Visit(context.GetChild(0)));
}
public  override  object VisitSliceFull( SliceFullContext context ){
var expr1 = (Result)(Visit(context.expression(0)));
var expr2 = (Result)(Visit(context.expression(1)));
return (new System.Text.StringBuilder().Append(".Slice(").Append(expr1.text).Append(", ").Append(expr2.text).Append(")")).To_Str();
}
public  override  object VisitSliceStart( SliceStartContext context ){
var expr = (Result)(Visit(context.expression()));
return (new System.Text.StringBuilder().Append(".Slice(").Append(expr.text).Append(", null)")).To_Str();
}
public  override  object VisitSliceEnd( SliceEndContext context ){
var expr = (Result)(Visit(context.expression()));
return (new System.Text.StringBuilder().Append(".Slice(null, ").Append(expr.text).Append(")")).To_Str();
}
public  override  object VisitCallFunc( CallFuncContext context ){
var r = (new Result(){data = "var",text = ((Result)(Visit(context.tuple()))).text});
return r;
}
public  override  object VisitCallAsync( CallAsyncContext context ){
var r = (new Result());
var expr = (Result)(Visit(context.expression()));
r.data="var";
r.text="await "+expr.text;
Set_func_async();
return r;
}
public  override  object VisitCallAwait( CallAwaitContext context ){
var r = (new Result(){data = "var",text = ((Result)(Visit(context.tuple()))).text});
return r;
}
public  override  object VisitCallPkg( CallPkgContext context ){
var r = (new Result(){data = Visit(context.typeNotNull())});
r.text=(new System.Text.StringBuilder().Append("(new ").Append(Visit(context.typeNotNull()))).To_Str();
r.text+=((Result)(Visit(context.tuple()))).text;
r.text+=")";
return r;
}
public  override  object VisitFunctionExpression( FunctionExpressionContext context ){
var r = (new Result());
r.text+=Visit(context.parameterClauseIn())+" => "+BlockLeft+Wrap;
this.Add_current_set();
this.Add_func_stack();
r.text+=ProcessFunctionSupport(context.functionSupportStatement());
this.Delete_current_set();
r.text+=BlockRight+Wrap;
if ( Get_func_async() ) {
r.text=" async "+r.text;
}
this.Delete_func_stack();
r.data="var";
return r;
}
public  override  object VisitLambda( LambdaContext context ){
this.Add_current_set();
this.Add_func_stack();
var r = (new Result(){data = "var"});
r.text+="(";
if ( context.lambdaIn()!=null ) {
r.text+=Visit(context.lambdaIn());
}
r.text+=")";
r.text+="=>";
if ( context.tupleExpression()!=null ) {
r.text+=((Result)(Visit(context.tupleExpression()))).text;
}
else {
r.text+=(new System.Text.StringBuilder().Append("{").Append(ProcessFunctionSupport(context.functionSupportStatement())).Append("}")).To_Str();
}
this.Delete_current_set();
if ( Get_func_async()||context.t.Type==Right_Flow ) {
r.text=" async "+r.text;
}
this.Delete_func_stack();
return r;
}
public  override  object VisitLambdaIn( LambdaInContext context ){
var obj = "";
foreach (var i in Range(0, context.id().Length, 1)){
var r = (Result)(Visit(context.id(i)));
if ( i==0 ) {
obj+=r.text;
}
else {
obj+=", "+r.text;
}
this.Add_ID(r.text);
}
return obj;
}
}
}
