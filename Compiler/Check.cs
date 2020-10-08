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
public  override  object VisitCheckStatement( CheckStatementContext context ){
var obj = (new System.Text.StringBuilder().Append("try ").Append(BlockLeft).Append(Wrap)).To_Str();
this.Add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.Delete_current_set();
obj+=BlockRight;
foreach (var item in context.checkErrorStatement()){
obj+=Visit(item)+Wrap;
}
if ( context.checkFinallyStatment()!=null ) {
obj+=Visit(context.checkFinallyStatment());
}
return obj;
}
public  override  object VisitCheckErrorStatement( CheckErrorStatementContext context ){
this.Add_current_set();
var obj = "";
var ID = ((Result)(Visit(context.id()))).text;
this.Add_ID(ID);
var Type = "Exception";
if ( context.typeType()!=null ) {
Type = (string)(Visit(context.typeType()));
}
obj+=(new System.Text.StringBuilder().Append("catch( ").Append(Type).Append(" ").Append(ID).Append(" )").Append(Wrap).Append(BlockLeft).Append(Wrap)).To_Str();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.Delete_current_set();
obj+=BlockRight;
return obj;
}
public  override  object VisitCheckFinallyStatment( CheckFinallyStatmentContext context ){
var obj = (new System.Text.StringBuilder().Append("finally ").Append(Wrap).Append(BlockLeft).Append(Wrap)).To_Str();
this.Add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.Delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitUsingStatement( UsingStatementContext context ){
var obj = "";
foreach (var (i, v) in Range(context.varId())){
if ( i!=0 ) {
obj+=","+Visit(v);
}
else {
obj+=Visit(v);
}
}
if ( context.varId().Length>1 ) {
obj = "("+obj+")";
}
var r2 = (Result)(Visit(context.tupleExpression()));
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text)).To_Str();
obj = (new System.Text.StringBuilder().Append("using (").Append(obj).Append(") ").Append(BlockLeft).Append(Wrap)).To_Str();
this.Add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.Delete_current_set();
obj+=BlockRight;
return obj;
}
public  override  object VisitCheckReportStatement( CheckReportStatementContext context ){
var obj = (new System.Text.StringBuilder().Append("throw ").Append(((Result)(Visit(context.expression()))).text).Append(Terminate).Append(Wrap)).To_Str();
return obj;
}
}
}
