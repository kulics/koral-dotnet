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
public  override  object VisitJudgeCaseStatement( JudgeCaseStatementContext context )
{
var obj = "";
var expr = ((Result)(Visit(context.expression())));
obj+=(new System.Text.StringBuilder("switch (").Append(expr.text).Append(") ").Append(BlockLeft+Wrap).Append("")).to_str();
foreach (var item in context.caseStatement()){
var r = ((string)(Visit(item)));
obj+=r+Wrap;
}
obj+=(new System.Text.StringBuilder("").Append(BlockRight).Append(" ").Append(Wrap).Append("")).to_str();
return obj;
}
public  override  object VisitCaseExprStatement( CaseExprStatementContext context )
{
var obj = "";
if ( context.expression()!=null ) {
var expr = ((Result)(Visit(context.expression())));
obj+=(new System.Text.StringBuilder("case ").Append(expr.text).Append(" :").Append(Wrap).Append("")).to_str();
}
else if ( context.typeType()!=null ) {
var id = "it";
if ( context.id()!=null ) {
id = ((Result)(Visit(context.id()))).text;
}
this.add_id(id);
var type = ((string)(Visit(context.typeType())));
obj+=(new System.Text.StringBuilder("case ").Append(type).Append(" ").Append(id).Append(" :").Append(Wrap).Append("")).to_str();
} 
else {
obj+=(new System.Text.StringBuilder("default:").Append(Wrap).Append("")).to_str();
}
return obj;
}
public  override  object VisitCaseStatement( CaseStatementContext context )
{
var obj = "";
foreach (var item in context.caseExprStatement()){
var r = ((string)(Visit(item)));
this.add_current_set();
var process = (new System.Text.StringBuilder("").Append(BlockLeft).Append(" ").Append(ProcessFunctionSupport(context.functionSupportStatement())).Append("").Append(BlockRight).Append("break;")).to_str();
this.delete_current_set();
obj+=r+process;
}
return obj;
}
public  override  object VisitJudgeStatement( JudgeStatementContext context )
{
var obj = "";
obj+=Visit(context.judgeIfStatement());
foreach (var it in context.judgeElseIfStatement()){
obj+=Visit(it);
}
if ( context.judgeElseStatement()!=null ) {
obj+=Visit(context.judgeElseStatement());
}
return obj;
}
public  override  object VisitJudgeIfStatement( JudgeIfStatementContext context )
{
var b = ((Result)(Visit(context.expression())));
var obj = (new System.Text.StringBuilder("if ( ").Append(b.text).Append(" ) ").Append(BlockLeft+Wrap).Append("")).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=(new System.Text.StringBuilder("").Append(BlockRight).Append("").Append(Wrap).Append("")).to_str();
return obj;
}
public  override  object VisitJudgeElseIfStatement( JudgeElseIfStatementContext context )
{
var b = ((Result)(Visit(context.expression())));
var obj = (new System.Text.StringBuilder("else if ( ").Append(b.text).Append(" ) ").Append(BlockLeft+Wrap).Append("")).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=(new System.Text.StringBuilder("").Append(BlockRight).Append(" ").Append(Wrap).Append("")).to_str();
return obj;
}
public  override  object VisitJudgeElseStatement( JudgeElseStatementContext context )
{
var obj = (new System.Text.StringBuilder("else ").Append(BlockLeft+Wrap).Append("")).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=(new System.Text.StringBuilder("").Append(BlockRight).Append("").Append(Wrap).Append("")).to_str();
return obj;
}
}
}
