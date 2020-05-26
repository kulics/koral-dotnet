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
public  override  object VisitIteratorStatement( IteratorStatementContext context ){
var it = (new Iterator());
if ( context.Dot_Dot_Dot()!=null||context.Dot_Dot_Greater()!=null ) {
it.order=F;
}
if ( context.Dot_Dot_Less()!=null||context.Dot_Dot_Greater()!=null ) {
it.close=F;
}
if ( context.expression().Length==2 ) {
it.begin=(Result)(Visit(context.expression(0)));
it.end=(Result)(Visit(context.expression(1)));
it.step=(new Result(){data = I32,text = "1"});
}
else {
it.begin=(Result)(Visit(context.expression(0)));
it.end=(Result)(Visit(context.expression(2)));
it.step=(Result)(Visit(context.expression(1)));
}
return it;
}
public  override  object VisitLoopStatement( LoopStatementContext context ){
var obj = "";
var id = ((Result)(Visit(context.id()))).text;
var it = (Iterator)(Visit(context.iteratorStatement()));
var target = (new System.Text.StringBuilder().Append("range(").Append(it.begin.text).Append(", ").Append(it.end.text).Append(", ").Append(it.step.text).Append(", ").Append(it.order).Append(", ").Append(it.close).Append(")")).to_str();
obj+=(new System.Text.StringBuilder().Append("foreach (var ").Append(id).Append(" in ").Append(target).Append(")")).to_str();
obj+=BlockLeft+Wrap;
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
if ( context.loopElseStatement()!=null ) {
var elseContent = (string)(Visit(context.loopElseStatement()));
obj=(new System.Text.StringBuilder().Append("if (!can_range(").Append(target).Append(")) ").Append(elseContent).Append(" else ").Append(BlockLeft).Append(Wrap).Append(obj).Append(BlockRight).Append(Wrap)).to_str();
}
return obj;
}
public  override  object VisitLoopEachStatement( LoopEachStatementContext context ){
var obj = "";
var arr = (Result)(Visit(context.expression()));
var target = arr.text;
var id = "ea";
if ( context.id().Length==2 ) {
target=(new System.Text.StringBuilder().Append("range(").Append(target).Append(")")).to_str();
id=(new System.Text.StringBuilder().Append("(").Append(((Result)(Visit(context.id(0)))).text).Append(", ").Append(((Result)(Visit(context.id(1)))).text).Append(")")).to_str();
}
else if ( context.id().Length==1 ) {
id=((Result)(Visit(context.id(0)))).text;
}
obj+=(new System.Text.StringBuilder().Append("foreach (var ").Append(id).Append(" in ").Append(target).Append(")")).to_str();
obj+=BlockLeft+Wrap;
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
if ( context.loopElseStatement()!=null ) {
var elseContent = (string)(Visit(context.loopElseStatement()));
obj=(new System.Text.StringBuilder().Append("if (!can_range(").Append(target).Append(")) ").Append(elseContent).Append(" else ").Append(BlockLeft).Append(Wrap).Append(obj).Append(BlockRight).Append(Wrap)).to_str();
}
return obj;
}
public  override  object VisitLoopCaseStatement( LoopCaseStatementContext context ){
var obj = "";
var expr = (Result)(Visit(context.expression()));
obj+=(new System.Text.StringBuilder().Append("for ( ;").Append(expr.text).Append(" ;)")).to_str();
obj+=BlockLeft+Wrap;
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
if ( context.loopElseStatement()!=null ) {
var elseContent = (string)(Visit(context.loopElseStatement()));
obj=(new System.Text.StringBuilder().Append("if (!(").Append(expr.text).Append(")) ").Append(elseContent).Append(" else ").Append(BlockLeft).Append(Wrap).Append(obj).Append(BlockRight).Append(Wrap)).to_str();
}
return obj;
}
public  override  object VisitLoopElseStatement( LoopElseStatementContext context ){
var obj = BlockLeft+Wrap;
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitLoopJumpStatement( LoopJumpStatementContext context ){
return (new System.Text.StringBuilder().Append("break").Append(Terminate).Append(Wrap)).to_str();
}
public  override  object VisitLoopContinueStatement( LoopContinueStatementContext context ){
return (new System.Text.StringBuilder().Append("continue").Append(Terminate).Append(Wrap)).to_str();
}
public  override  object VisitLoopExpression( LoopExpressionContext context ){
var obj = "";
var id = ((Result)(Visit(context.id()))).text;
var it = (Iterator)(Visit(context.iteratorStatement()));
var target = (new System.Text.StringBuilder().Append("range(").Append(it.begin.text).Append(", ").Append(it.end.text).Append(", ").Append(it.step.text).Append(", ").Append(it.order).Append(")")).to_str();
obj+=(new System.Text.StringBuilder().Append("runloop(").Append(target).Append(", (").Append(id).Append(")=>")).to_str();
obj+=BlockLeft+Wrap;
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
obj+=(new System.Text.StringBuilder().Append("return ").Append(((Result)(Visit(context.tupleExpression()))).text).Append(";")).to_str();
this.delete_current_set();
obj+=BlockRight;
var elseContent = (string)(Visit(context.loopElseExpression()));
obj+=(new System.Text.StringBuilder().Append(", ()=> ").Append(elseContent).Append(")")).to_str();
return (new Result(){data = "var",text = obj});
}
public  override  object VisitLoopEachExpression( LoopEachExpressionContext context ){
var obj = "";
var arr = (Result)(Visit(context.expression()));
var target = arr.text;
var id = "ea";
if ( context.id().Length==2 ) {
target=(new System.Text.StringBuilder().Append("range(").Append(target).Append(")")).to_str();
id=(new System.Text.StringBuilder().Append("(").Append(((Result)(Visit(context.id(0)))).text).Append(", ").Append(((Result)(Visit(context.id(1)))).text).Append(")")).to_str();
}
else if ( context.id().Length==1 ) {
id=((Result)(Visit(context.id(0)))).text;
}
obj+=(new System.Text.StringBuilder().Append("runloop(").Append(target).Append(", (").Append(id).Append(")=>")).to_str();
obj+=BlockLeft+Wrap;
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
obj+=(new System.Text.StringBuilder().Append("return ").Append(((Result)(Visit(context.tupleExpression()))).text).Append(";")).to_str();
this.delete_current_set();
obj+=BlockRight;
var elseContent = (string)(Visit(context.loopElseExpression()));
obj+=(new System.Text.StringBuilder().Append(", ()=> ").Append(elseContent).Append(")")).to_str();
return (new Result(){data = "var",text = obj});
}
public  override  object VisitLoopElseExpression( LoopElseExpressionContext context ){
var obj = BlockLeft+Wrap;
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
obj+=(new System.Text.StringBuilder().Append("return ").Append(((Result)(Visit(context.tupleExpression()))).text).Append(";")).to_str();
this.delete_current_set();
obj+=BlockRight;
return obj;
}
}
}
