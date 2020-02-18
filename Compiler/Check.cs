using Library;
using static Library.Lib;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using static Compiler.KParser;
using static Compiler.Compiler_static;

namespace Compiler
{
public partial class KLangVisitor{
public  override  object VisitCheckStatement( CheckStatementContext context ){
var obj = (new System.Text.StringBuilder().Append("try ").Append(BlockLeft).Append(Wrap)).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
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
this.add_current_set();
var obj = "";
var ID = ((Result)(Visit(context.id()))).text;
this.add_id(ID);
var Type = "Exception";
if ( context.typeType()!=null ) {
Type = (string)(Visit(context.typeType()));
}
obj+=(new System.Text.StringBuilder().Append("catch( ").Append(Type).Append(" ").Append(ID).Append(" )").Append(Wrap).Append(BlockLeft).Append(Wrap)).to_str();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight;
return obj;
}
public  override  object VisitCheckFinallyStatment( CheckFinallyStatmentContext context ){
var obj = (new System.Text.StringBuilder().Append("finally ").Append(Wrap).Append(BlockLeft).Append(Wrap)).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitUsingStatement( UsingStatementContext context ){
var obj = "";
var r1 = (Result)(Visit(context.expression(0)));
var r2 = (Result)(Visit(context.expression(1)));
obj = run(()=>{if ( context.typeType()!=null ) {
var Type = (string)(Visit(context.typeType()));
return (new System.Text.StringBuilder().Append(Type).Append(" ").Append(r1.text).Append(" = ").Append(r2.text)).to_str();}
else {
return (new System.Text.StringBuilder().Append("var ").Append(r1.text).Append(" = ").Append(r2.text)).to_str();}
});
return obj;
}
public  override  object VisitCheckExpression( CheckExpressionContext context ){
var obj = (new System.Text.StringBuilder().Append("run(()=> { ").Append(Wrap).Append("try ").Append(BlockLeft).Append(Wrap)).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
obj+=(new System.Text.StringBuilder().Append("return ").Append(((Result)(Visit(context.tupleExpression()))).text).Append(";")).to_str();
this.delete_current_set();
obj+=BlockRight+Wrap;
foreach (var item in context.checkErrorExpression()){
obj+=Visit(item)+Wrap;
}
if ( context.checkFinallyStatment()!=null ) {
obj+=Visit(context.checkFinallyStatment());
}
obj+="})";
return (new Result(){data = "var",text = obj});
}
public  override  object VisitCheckErrorExpression( CheckErrorExpressionContext context ){
this.add_current_set();
var obj = "";
var ID = ((Result)(Visit(context.id()))).text;
this.add_id(ID);
var Type = "Exception";
if ( context.typeType()!=null ) {
Type = (string)(Visit(context.typeType()));
}
obj+=(new System.Text.StringBuilder().Append("catch( ").Append(Type).Append(" ").Append(ID).Append(" )").Append(Wrap).Append(BlockLeft).Append(Wrap)).to_str();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
obj+=(new System.Text.StringBuilder().Append("return ").Append(((Result)(Visit(context.tupleExpression()))).text).Append(";")).to_str();
this.delete_current_set();
obj+=BlockRight;
return obj;
}
public  override  object VisitCheckReportStatement( CheckReportStatementContext context ){
var obj = (new System.Text.StringBuilder().Append("throw ").Append(((Result)(Visit(context.expression()))).text).Append(Terminate).Append(Wrap)).to_str();
return obj;
}
}
}
