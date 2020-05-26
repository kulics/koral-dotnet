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
public  override  object VisitJudgeCaseStatement( JudgeCaseStatementContext context ){
var obj = "";
var expr = (Result)(Visit(context.expression()));
obj+=(new System.Text.StringBuilder().Append("switch (").Append(expr.text).Append(") ").Append(BlockLeft).Append(Wrap)).to_str();
foreach (var item in context.caseStatement()){
var r = (string)(Visit(item));
obj+=r+Wrap;
}
if ( context.caseElseStatement()!=null ) {
obj+=(string)(Visit(context.caseElseStatement()))+Wrap;
}
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitJudgeCase( JudgeCaseContext context ){
var obj = "";
if ( context.expression()!=null ) {
var expr = (Result)(Visit(context.expression()));
obj=(new System.Text.StringBuilder().Append("case ").Append(expr.text).Append(" :").Append(Wrap)).to_str();
}
else if ( context.typeType()!=null ) {
var id = "it";
if ( context.id()!=null ) {
id=((Result)(Visit(context.id()))).text;
}
this.add_id(id);
var type = (string)(Visit(context.typeType()));
obj=(new System.Text.StringBuilder().Append("case ").Append(type).Append(" ").Append(id).Append(" :").Append(Wrap)).to_str();
}
else {
obj=(new System.Text.StringBuilder().Append("default:").Append(Wrap)).to_str();
}
return obj;
}
public  override  object VisitCaseStatement( CaseStatementContext context ){
var obj = "";
this.add_current_set();
var process = (new System.Text.StringBuilder().Append(BlockLeft).Append(" ").Append(ProcessFunctionSupport(context.functionSupportStatement())).Append(BlockRight).Append(" break;")).to_str();
this.delete_current_set();
foreach (var item in context.judgeCase()){
var r = (string)(Visit(item));
obj+=r+process;
}
return obj;
}
public  override  object VisitCaseElseStatement( CaseElseStatementContext context ){
var obj = "";
this.add_current_set();
var process = (new System.Text.StringBuilder().Append(BlockLeft).Append(" ").Append(ProcessFunctionSupport(context.functionSupportStatement())).Append(BlockRight).Append(" break;")).to_str();
this.delete_current_set();
obj+="default:"+Wrap+process;
return obj;
}
public  override  object VisitJudgeStatement( JudgeStatementContext context ){
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
public  override  object VisitJudgeIfStatement( JudgeIfStatementContext context ){
var b = (Result)(Visit(context.expression()));
var obj = (new System.Text.StringBuilder().Append("if ( ").Append(b.text).Append(" ) ").Append(BlockLeft).Append(Wrap)).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitJudgeElseIfStatement( JudgeElseIfStatementContext context ){
var b = (Result)(Visit(context.expression()));
var obj = (new System.Text.StringBuilder().Append("else if ( ").Append(b.text).Append(" ) ").Append(BlockLeft).Append(Wrap)).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitJudgeElseStatement( JudgeElseStatementContext context ){
var obj = (new System.Text.StringBuilder().Append("else ").Append(BlockLeft).Append(Wrap)).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitJudgeExpression( JudgeExpressionContext context ){
var r = (new Result());
r.data="var";
r.text="run(()=>"+BlockLeft;
r.text+=Visit(context.judgeIfExpression());
foreach (var it in context.judgeElseIfExpression()){
r.text+=Visit(it);
}
r.text+=Visit(context.judgeElseExpression());
r.text+=BlockRight+")";
return r;
}
public  override  object VisitJudgeIfExpression( JudgeIfExpressionContext context ){
var b = (Result)(Visit(context.expression()));
var obj = (new System.Text.StringBuilder().Append("if ( ").Append(b.text).Append(" ) ").Append(BlockLeft).Append(Wrap)).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
obj+=(new System.Text.StringBuilder().Append("return ").Append(((Result)(Visit(context.tupleExpression()))).text).Append(";")).to_str();
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitJudgeElseIfExpression( JudgeElseIfExpressionContext context ){
var b = (Result)(Visit(context.expression()));
var obj = (new System.Text.StringBuilder().Append("else if ( ").Append(b.text).Append(" ) ").Append(BlockLeft).Append(Wrap)).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
obj+=(new System.Text.StringBuilder().Append("return ").Append(((Result)(Visit(context.tupleExpression()))).text).Append(";")).to_str();
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitJudgeElseExpression( JudgeElseExpressionContext context ){
var obj = (new System.Text.StringBuilder().Append("else ").Append(BlockLeft).Append(Wrap)).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
obj+=(new System.Text.StringBuilder().Append("return ").Append(((Result)(Visit(context.tupleExpression()))).text).Append(";")).to_str();
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitJudgeCaseExpression( JudgeCaseExpressionContext context ){
var r = (new Result());
var expr = (Result)(Visit(context.expression()));
r.text=(new System.Text.StringBuilder().Append("run(()=> { switch (").Append(expr.text).Append(") ")).to_str();
r.text+=BlockLeft+Wrap;
foreach (var item in context.caseExpression()){
var temp = (string)(Visit(item));
r.text+=temp+Wrap;
}
r.text+=(string)(Visit(context.caseElseExpression()));
r.text+=BlockRight+Wrap+"})";
return r;
}
public  override  object VisitCaseExpression( CaseExpressionContext context ){
var obj = "";
foreach (var item in context.judgeCase()){
var r = (string)(Visit(item));
this.add_current_set();
var process = BlockLeft+ProcessFunctionSupport(context.functionSupportStatement());
process+=(new System.Text.StringBuilder().Append("return ").Append(((Result)(Visit(context.tupleExpression()))).text).Append(";")).to_str();
process+=(new System.Text.StringBuilder().Append(BlockRight).Append("break;")).to_str();
this.delete_current_set();
obj+=r+process;
}
return obj;
}
public  override  object VisitCaseElseExpression( CaseElseExpressionContext context ){
var obj = "";
this.add_current_set();
var process = BlockLeft+ProcessFunctionSupport(context.functionSupportStatement());
process+=(new System.Text.StringBuilder().Append("return ").Append(((Result)(Visit(context.tupleExpression()))).text).Append(";")).to_str();
process+=(new System.Text.StringBuilder().Append(BlockRight).Append("break;")).to_str();
this.delete_current_set();
obj+="default:"+Wrap+process;
return obj;
}
}
}
