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
public partial class Iterator{
public Result begin;
public Result end;
public Result step;
public string order = T;
public string close = T;
}
public partial class FeelLangVisitor{
public  override  object VisitIterator( IteratorContext context ){
Func<Result, Result> fn = (e1)=>{var it = (new Iterator());
if ( context.Dot_Dot_Dot()!=null||context.Dot_Dot_Greater()!=null ) {
it.order=F;
}
if ( context.Dot_Dot_Less()!=null||context.Dot_Dot_Greater()!=null ) {
it.close=F;
}
var e2 = (Result)(Visit(context.expression(0)));
var step = context.expression(1);
if ( step==null ) {
it.begin=e1;
it.end=e2;
it.step=(new Result(){data = I32,text = "1"});
}
else {
it.begin=e1;
it.end=e2;
it.step=(Result)(Visit(step));
}
var r = (new Result());
r.data="IEnumerable<int>";
r.text=(new System.Text.StringBuilder().Append("range(").Append(it.begin.text).Append(", ").Append(it.end.text).Append(", ").Append(it.step.text).Append(", ").Append(it.order).Append(", ").Append(it.close).Append(")")).to_str();
return r;
};
return fn;
}
public  override  object VisitLoopStatement( LoopStatementContext context ){
var obj = "";
var arr = (Result)(Visit(context.expression()));
var target = arr.text;
var id = "ea";
if ( context.id().Length==2 ) {
target = (new System.Text.StringBuilder().Append("range(").Append(target).Append(")")).to_str();
id = (new System.Text.StringBuilder().Append("(").Append(((Result)(Visit(context.id(0)))).text).Append(", ").Append(((Result)(Visit(context.id(1)))).text).Append(")")).to_str();
}
else if ( context.id().Length==1 ) {
id = ((Result)(Visit(context.id(0)))).text;
}
obj+=(new System.Text.StringBuilder().Append("foreach (var ").Append(id).Append(" in ").Append(target).Append(")")).to_str();
obj+=BlockLeft+Wrap;
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitLoopCaseStatement( LoopCaseStatementContext context ){
var obj = "";
var expr = (Result)(Visit(context.expression()));
obj+=(new System.Text.StringBuilder().Append("while (true) { ").Append(Wrap).Append(" if (").Append(expr.text).Append(") ")).to_str();
obj+=BlockLeft+Wrap;
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
obj+=(new System.Text.StringBuilder().Append(" else { ").Append(Wrap)).to_str();
if ( context.loopElseStatement()!=null ) {
obj+=Visit(context.loopElseStatement());
}
obj+=(new System.Text.StringBuilder().Append(" break; ").Append(Wrap).Append(" } }")).to_str();
return obj;
}
public  override  object VisitLoopElseStatement( LoopElseStatementContext context ){
var obj = "";
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
return obj;
}
public  override  object VisitLoopJumpStatement( LoopJumpStatementContext context ){
return (new System.Text.StringBuilder().Append("break").Append(Terminate).Append(Wrap)).to_str();
}
public  override  object VisitLoopContinueStatement( LoopContinueStatementContext context ){
return (new System.Text.StringBuilder().Append("continue").Append(Terminate).Append(Wrap)).to_str();
}
}
}
