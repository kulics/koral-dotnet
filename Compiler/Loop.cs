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
}
public partial class FeelLangVisitor{
public  override  object VisitIterator( IteratorContext context ){
Func<Result, Result> fn = (e1)=>{var it = (new Iterator());
var e2 = (Result)(Visit(context.expression(0)));
var step = context.expression(1);
if ( step==null ) {
it.begin=e1;
it.end=e2;
it.step=(new Result(){data = TargetTypeI32,text = "1"});
}
else {
it.begin=e1;
it.end=e2;
it.step=(Result)(Visit(step));
}
var r = (new Result());
r.data="IEnumerable<int>";
if ( context.Dot_Dot_Dot()!=null ) {
r.text=(new System.Text.StringBuilder().Append("Range_close(").Append(it.begin.text).Append(", ").Append(it.end.text).Append(", ").Append(it.step.text).Append(")")).To_Str();
return r;
}
r.text=(new System.Text.StringBuilder().Append("Range(").Append(it.begin.text).Append(", ").Append(it.end.text).Append(", ").Append(it.step.text).Append(")")).To_Str();
return r;
};
return fn;
}
public  override  object VisitLoopStatement( LoopStatementContext context ){
var obj = "";
var arr = (Result)(Visit(context.expression()));
var target = arr.text;
var ids = "";
foreach (var (i, v) in Range(context.loopId())){
if ( i!=0 ) {
ids+=","+Visit(v);
}
else {
ids+=Visit(v);
}
}
if ( context.loopId().Length>1 ) {
ids = "("+ids+")";
}
obj+=(new System.Text.StringBuilder().Append("foreach (var ").Append(ids).Append(" in ").Append(target).Append(")")).To_Str();
obj+=BlockLeft+Wrap;
this.Add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.Delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitLoopId( LoopIdContext context ){
var id = ((Result)(Visit(context.id()))).text;
if ( this.Has_ID(id) ) {
return id;
}
else {
this.Add_ID(id);
return id;
}
}
public  override  object VisitLoopCaseStatement( LoopCaseStatementContext context ){
var obj = "";
var expr = (Result)(Visit(context.expression()));
obj+=(new System.Text.StringBuilder().Append("while (true) { ").Append(Wrap).Append(" if (").Append(expr.text).Append(") ")).To_Str();
obj+=BlockLeft+Wrap;
this.Add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.Delete_current_set();
obj+=BlockRight+Wrap;
obj+=(new System.Text.StringBuilder().Append(" else { ").Append(Wrap)).To_Str();
if ( context.loopElseStatement()!=null ) {
obj+=Visit(context.loopElseStatement());
}
obj+=(new System.Text.StringBuilder().Append(" break; ").Append(Wrap).Append(" } }")).To_Str();
return obj;
}
public  override  object VisitLoopElseStatement( LoopElseStatementContext context ){
var obj = "";
this.Add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.Delete_current_set();
return obj;
}
public  override  object VisitLoopJumpStatement( LoopJumpStatementContext context ){
return (new System.Text.StringBuilder().Append("break").Append(Terminate).Append(Wrap)).To_Str();
}
public  override  object VisitLoopContinueStatement( LoopContinueStatementContext context ){
return (new System.Text.StringBuilder().Append("continue").Append(Terminate).Append(Wrap)).To_Str();
}
}
}
