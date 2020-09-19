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
public  override  object VisitJudgeEqualStatement( JudgeEqualStatementContext context ){
var obj = "";
var expr = (Result)(Visit(context.expression()));
obj+=(new System.Text.StringBuilder().Append("switch (").Append(expr.text).Append(") ").Append(BlockLeft).Append(Wrap)).to_str();
if ( context.caseEqualStatement()!=null ) {
obj+=(string)(Visit(context.caseEqualStatement()))+Wrap;
if ( context.caseElseStatement()!=null ) {
obj+=(string)(Visit(context.caseElseStatement()))+Wrap;
}
}
else {
foreach (var (i, v) in range(context.caseMultiwayEqualStatement())){
obj+=(string)(Visit(v))+Wrap;
}
if ( context.caseMultiwayElseStatement()!=null ) {
obj+=(string)(Visit(context.caseMultiwayElseStatement()))+Wrap;
}
}
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitJudgeTypeStatement( JudgeTypeStatementContext context ){
var obj = "";
var expr = (Result)(Visit(context.expression()));
obj+=(new System.Text.StringBuilder().Append("switch (").Append(expr.text).Append(") ").Append(BlockLeft).Append(Wrap)).to_str();
if ( context.caseTypeStatement()!=null ) {
obj+=(string)(Visit(context.caseTypeStatement()))+Wrap;
if ( context.caseElseStatement()!=null ) {
obj+=(string)(Visit(context.caseElseStatement()))+Wrap;
}
}
else {
foreach (var (i, v) in range(context.caseMultiwayTypeStatement())){
obj+=(string)(Visit(v))+Wrap;
}
if ( context.caseMultiwayElseStatement()!=null ) {
obj+=(string)(Visit(context.caseMultiwayElseStatement()))+Wrap;
}
}
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitJudgeEqualCase( JudgeEqualCaseContext context ){
var obj = "";
var expr = (Result)(Visit(context.expression()));
obj = (new System.Text.StringBuilder().Append("case ").Append(expr.text).Append(" :").Append(Wrap)).to_str();
return obj;
}
public  override  object VisitJudgeTypeCase( JudgeTypeCaseContext context ){
var obj = "";
var id = "it";
if ( context.id()!=null ) {
id = ((Result)(Visit(context.id()))).text;
}
this.add_id(id);
var type = (string)(Visit(context.typeType()));
obj = (new System.Text.StringBuilder().Append("case ").Append(type).Append(" ").Append(id).Append(" :").Append(Wrap)).to_str();
return obj;
}
public  override  object VisitCaseEqualStatement( CaseEqualStatementContext context ){
var obj = "";
this.add_current_set();
var rList = (new list<string>());
foreach (var item in context.judgeEqualCase()){
var r = (string)(Visit(item));
rList.add(r);
}
var process = (new System.Text.StringBuilder().Append(BlockLeft).Append(" ").Append(ProcessFunctionSupport(context.functionSupportStatement())).Append(BlockRight).Append(" break;")).to_str();
foreach (var r in rList){
obj+=r+process;
}
this.delete_current_set();
return obj;
}
public  override  object VisitCaseMultiwayEqualStatement( CaseMultiwayEqualStatementContext context ){
var obj = "";
this.add_current_set();
var rList = (new list<string>());
foreach (var item in context.judgeEqualCase()){
var r = (string)(Visit(item));
rList.add(r);
}
var process = (new System.Text.StringBuilder().Append(BlockLeft).Append(" ").Append(ProcessFunctionSupport(context.functionSupportStatement())).Append(BlockRight).Append(" break;")).to_str();
foreach (var r in rList){
obj+=r+process;
}
this.delete_current_set();
return obj;
}
public  override  object VisitCaseTypeStatement( CaseTypeStatementContext context ){
var obj = "";
this.add_current_set();
var rList = (new list<string>());
foreach (var item in context.judgeTypeCase()){
var r = (string)(Visit(item));
rList.add(r);
}
var process = (new System.Text.StringBuilder().Append(BlockLeft).Append(" ").Append(ProcessFunctionSupport(context.functionSupportStatement())).Append(BlockRight).Append(" break;")).to_str();
foreach (var r in rList){
obj+=r+process;
}
this.delete_current_set();
return obj;
}
public  override  object VisitCaseMultiwayTypeStatement( CaseMultiwayTypeStatementContext context ){
var obj = "";
this.add_current_set();
var rList = (new list<string>());
foreach (var item in context.judgeTypeCase()){
var r = (string)(Visit(item));
rList.add(r);
}
var process = (new System.Text.StringBuilder().Append(BlockLeft).Append(" ").Append(ProcessFunctionSupport(context.functionSupportStatement())).Append(BlockRight).Append(" break;")).to_str();
foreach (var r in rList){
obj+=r+process;
}
this.delete_current_set();
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
public  override  object VisitCaseMultiwayElseStatement( CaseMultiwayElseStatementContext context ){
var obj = "";
this.add_current_set();
var process = (new System.Text.StringBuilder().Append(BlockLeft).Append(" ").Append(ProcessFunctionSupport(context.functionSupportStatement())).Append(BlockRight).Append(" break;")).to_str();
this.delete_current_set();
obj+="default:"+Wrap+process;
return obj;
}
public  override  object VisitJudgeStatement( JudgeStatementContext context ){
var obj = "";
if ( context.judgeIfStatement()!=null ) {
obj+=Visit(context.judgeIfStatement());
if ( context.judgeElseStatement()!=null ) {
obj+=Visit(context.judgeElseStatement());
}
}
else {
foreach (var (i, v) in range(context.judgeMultiwayIfStatement())){
if ( i!=0 ) {
obj+=" else ";
}
obj+=Visit(v);
}
if ( context.judgeMultiwayElseStatement()!=null ) {
obj+=Visit(context.judgeMultiwayElseStatement());
}
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
public  override  object VisitJudgeElseStatement( JudgeElseStatementContext context ){
var obj = (new System.Text.StringBuilder().Append("else ").Append(BlockLeft).Append(Wrap)).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitJudgeMultiwayIfStatement( JudgeMultiwayIfStatementContext context ){
var b = (Result)(Visit(context.expression()));
var obj = (new System.Text.StringBuilder().Append("if ( ").Append(b.text).Append(" ) ").Append(BlockLeft).Append(Wrap)).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitJudgeMultiwayElseStatement( JudgeMultiwayElseStatementContext context ){
var obj = (new System.Text.StringBuilder().Append("else ").Append(BlockLeft).Append(Wrap)).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitJudgeExpression( JudgeExpressionContext context ){
Func<string, Result> fn = (expr)=>{var r = (new Result());
r.data="var";
r.text="run(()=> "+BlockLeft+" if (";
r.text+=expr;
r.text+=Visit(context.judgeIfExpression());
r.text+=Visit(context.judgeElseExpression());
r.text+=BlockRight+")";
return r;
};
return fn;
}
public  override  object VisitJudgeIfExpression( JudgeIfExpressionContext context ){
var obj = (new System.Text.StringBuilder().Append(" ) ").Append(BlockLeft).Append(Wrap)).to_str();
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
}
}
