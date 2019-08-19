using Library;
using static Library.Lib;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using static Compiler.LiteParser;
using static Compiler.Compiler_static;

namespace Compiler
{
public partial class Iterator
{
public Result begin;
public Result end;
public Result step;
public string order = T ; 
public string attach = F ; 
}
public partial class LiteLangVisitor{
public  override  object VisitIteratorStatement( IteratorStatementContext context )
{
var it = (new Iterator());
if ( context.op.Text==">="||context.op.Text=="<=" ) {
it.attach=T;
}
if ( context.op.Text==">"||context.op.Text==">=" ) {
it.order=F;
}
if ( context.expression().Length==2 ) {
it.begin=((Result)(Visit(context.expression(0))));
it.end=((Result)(Visit(context.expression(1))));
it.step=(new Result(){data = I32,text = "1"});
}
else {
it.begin=((Result)(Visit(context.expression(0))));
it.end=((Result)(Visit(context.expression(1))));
it.step=((Result)(Visit(context.expression(2))));
}
return it;
}
public  override  object VisitLoopStatement( LoopStatementContext context )
{
var obj = "";
var id = ((Result)(Visit(context.id()))).text;
var it = ((Iterator)(Visit(context.iteratorStatement())));
var target = (new System.Text.StringBuilder("range(").Append(it.begin.text).Append(",").Append(it.end.text).Append(",").Append(it.step.text).Append(",").Append(it.order).Append(",").Append(it.attach).Append(")")).to_str();
obj+=(new System.Text.StringBuilder("foreach (var ").Append(id).Append(" in ").Append(target).Append(")")).to_str();
obj+=BlockLeft+Wrap;
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
if ( context.loopElseStatement()!=null ) {
var elseContent = ((string)(Visit(context.loopElseStatement())));
obj = (new System.Text.StringBuilder("if (!can_range(").Append(target).Append(")) ")).to_str()+elseContent+"else"+BlockLeft+Wrap+obj+BlockRight+Wrap;
}
return obj;
}
public  override  object VisitLoopEachStatement( LoopEachStatementContext context )
{
var obj = "";
var arr = ((Result)(Visit(context.expression())));
var target = arr.text;
var id = "ea";
if ( context.id().Length==2 ) {
target = (new System.Text.StringBuilder("range(").Append(target).Append(")")).to_str();
id = (new System.Text.StringBuilder("(").Append(((Result)(Visit(context.id(0)))).text).Append(",").Append(((Result)(Visit(context.id(1)))).text).Append(")")).to_str();
}
else if ( context.id().Length==1 ) {
id = ((Result)(Visit(context.id(0)))).text;
} 
obj+=(new System.Text.StringBuilder("foreach (var ").Append(id).Append(" in ").Append(target).Append(")")).to_str();
obj+=BlockLeft+Wrap;
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
if ( context.loopElseStatement()!=null ) {
var elseContent = ((string)(Visit(context.loopElseStatement())));
obj = (new System.Text.StringBuilder("if (!can_range(").Append(target).Append(")) ")).to_str()+elseContent+"else"+BlockLeft+Wrap+obj+BlockRight+Wrap;
}
return obj;
}
public  override  object VisitLoopCaseStatement( LoopCaseStatementContext context )
{
var obj = "";
var expr = ((Result)(Visit(context.expression())));
obj+=(new System.Text.StringBuilder("for ( ;").Append(expr.text).Append(" ;)")).to_str();
obj+=BlockLeft+Wrap;
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
if ( context.loopElseStatement()!=null ) {
var elseContent = ((string)(Visit(context.loopElseStatement())));
obj = (new System.Text.StringBuilder("if (!(").Append(expr.text).Append(")) ")).to_str()+elseContent+"else"+BlockLeft+Wrap+obj+BlockRight+Wrap;
}
return obj;
}
public  override  object VisitLoopElseStatement( LoopElseStatementContext context )
{
var obj = BlockLeft+Wrap;
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitLoopJumpStatement( LoopJumpStatementContext context )
{
return (new System.Text.StringBuilder("break ").Append(Terminate+Wrap).Append("")).to_str();
}
public  override  object VisitLoopContinueStatement( LoopContinueStatementContext context )
{
return (new System.Text.StringBuilder("continue ").Append(Terminate+Wrap).Append("")).to_str();
}
}
}
