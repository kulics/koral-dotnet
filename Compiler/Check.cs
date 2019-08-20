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
public  override  object VisitCheckStatement( CheckStatementContext context ){
var obj = (new System.Text.StringBuilder("try ").Append(BlockLeft+Wrap).Append("")).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=(new System.Text.StringBuilder("").Append(BlockRight+Wrap).Append("")).to_str();
foreach (var item in context.checkErrorStatement()){
obj+=(new System.Text.StringBuilder("").Append(Visit(item)).Append("").Append(Wrap).Append("")).to_str();
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
Type = ((string)(Visit(context.typeType())));
}
obj+=(new System.Text.StringBuilder("catch( ").Append(Type).Append(" ").Append(ID).Append(" )")).to_str()+Wrap+BlockLeft+Wrap;
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight;
return obj;
}
public  override  object VisitCheckFinallyStatment( CheckFinallyStatmentContext context ){
var obj = (new System.Text.StringBuilder("finally ").Append(Wrap+BlockLeft+Wrap).Append("")).to_str();
this.add_current_set();
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=(new System.Text.StringBuilder("").Append(BlockRight).Append("").Append(Wrap).Append("")).to_str();
return obj;
}
public  override  object VisitUsingStatement( UsingStatementContext context ){
var obj = "";
var r1 = ((Result)(Visit(context.expression(0))));
var r2 = ((Result)(Visit(context.expression(1))));
if ( context.typeType()!=null ) {
var Type = ((string)(Visit(context.typeType())));
obj = (new System.Text.StringBuilder("").Append(Type).Append(" ").Append(r1.text).Append(" = ").Append(r2.text).Append("")).to_str();
}
else {
obj = (new System.Text.StringBuilder("var ").Append(r1.text).Append(" = ").Append(r2.text).Append("")).to_str();
}
return obj;
}
}
}
