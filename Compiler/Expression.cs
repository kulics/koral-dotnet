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
public partial class TemplateItem{
public string Template;
public string Contract;
}
public partial class DicEle{
public string key;
public string value;
public string text;
}
public partial class FeelLangVisitor{
public  override  object VisitVarStatement( VarStatementContext context ){
var obj = "";
foreach (var (i, v) in range(context.varId())){
if ( i!=0 ) {
obj+=","+Visit(v);
}
else {
obj+=Visit(v);
}
}
if ( context.varId().Length>1 ) {
obj="("+obj+")";
}
var r2 = (Result)(Visit(context.tupleExpression()));
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).to_str();
return obj;
}
public  override  object VisitVarTypeStatement( VarTypeStatementContext context ){
var obj = "";
foreach (var (i, v) in range(context.varIdType())){
if ( i!=0 ) {
obj+=","+Visit(v);
}
else {
obj+=Visit(v);
}
}
if ( context.varIdType().Length>1 ) {
obj="("+obj+")";
}
var r2 = (Result)(Visit(context.tupleExpression()));
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).to_str();
return obj;
}
public  override  object VisitBindStatement( BindStatementContext context ){
var obj = "";
foreach (var (i, v) in range(context.constId())){
if ( i!=0 ) {
obj+=","+Visit(v);
}
else {
obj+=Visit(v);
}
}
if ( context.constId().Length>1 ) {
obj="("+obj+")";
}
var r2 = (Result)(Visit(context.tupleExpression()));
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).to_str();
return obj;
}
public  override  object VisitBindTypeStatement( BindTypeStatementContext context ){
var obj = "";
foreach (var (i, v) in range(context.constIdType())){
if ( i!=0 ) {
obj+=","+Visit(v);
}
else {
obj+=Visit(v);
}
}
if ( context.constIdType().Length>1 ) {
obj="("+obj+")";
}
var r2 = (Result)(Visit(context.tupleExpression()));
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).to_str();
return obj;
}
public  override  object VisitVariableDeclaredStatement( VariableDeclaredStatementContext context ){
var obj = "";
var Type = (string)(Visit(context.typeType()));
var r = (Result)(Visit(context.id()));
obj=(new System.Text.StringBuilder().Append(Type).Append(" ").Append(r.text).Append(Terminate).Append(Wrap)).to_str();
return obj;
}
public  override  object VisitConstantDeclaredStatement( ConstantDeclaredStatementContext context ){
var obj = "";
var Type = (string)(Visit(context.typeType()));
var r = (Result)(Visit(context.id()));
obj=(new System.Text.StringBuilder().Append(Type).Append(" ").Append(r.text).Append(Terminate).Append(Wrap)).to_str();
return obj;
}
public  override  object VisitAssignStatement( AssignStatementContext context ){
var r1 = (Result)(Visit(context.tupleExpression(0)));
var r2 = (Result)(Visit(context.tupleExpression(1)));
var obj = r1.text+Visit(context.assign())+r2.text+Terminate+Wrap;
return obj;
}
public  override  object VisitAssign( AssignContext context ){
return context.op.Text;
}
public  override  object VisitExpressionStatement( ExpressionStatementContext context ){
var r = (Result)(Visit(context.expression()));
return r.text+Terminate+Wrap;
}
public  override  object VisitExpression( ExpressionContext context ){
var count = context.ChildCount;
var r = (new Result());
switch (count) {
case 3 :
{ var e1 = (Result)(Visit(context.GetChild(0)));
var e2 = Visit(context.GetChild(2));
var op = Visit(context.GetChild(1));
switch (context.GetChild(1)) {
case CompareCombineContext it :
{ r.data=I32;
var s1 = e1.text;
var s2 = ((Result)(e2)).text;
r.text=(new System.Text.StringBuilder().Append(s1).Append(" > ").Append(s2).Append(" ? 1 : ( ").Append(s1).Append("==").Append(s2).Append(" ? 0 : -1 )")).to_str();
return r;
} break;
case CompareContext it :
{ r.data=Bool;
} break;
case LogicContext it :
{ r.data=Bool;
} break;
case AddContext it :
{ if ( (string)(e1.data)==Str||(string)(((Result)(e2)).data)==Str ) {
r.data=Str;
}
else if ( (string)(e1.data)==I32&&(string)(((Result)(e2)).data)==I32 ) {
r.data=I32;
}
else {
r.data=F64;
}
} break;
case MulContext it :
{ if ( (string)(e1.data)==I32&&(string)(((Result)(e2)).data)==I32 ) {
r.data=I32;
}
else {
r.data=F64;
}
} break;
case PowContext it :
{ r.data=F64;
r.text=(new System.Text.StringBuilder().Append(op).Append("(").Append(e1.text).Append(", ").Append(((Result)(e2)).text).Append(")")).to_str();
return r;
} break;
}
r.text=e1.text+op+((Result)(e2)).text;
} break;
case 2 :
{ r=(Result)(Visit(context.GetChild(0)));
switch (context.GetChild(1)) {
case TypeConversionContext it :
{ var e2 = (string)(Visit(it));
r.data=e2;
r.text=(new System.Text.StringBuilder().Append("(").Append(e2).Append(")(").Append(r.text).Append(")")).to_str();
} break;
case TypeCheckContext it :
{ var e2 = (string)(Visit(it));
r.data=e2;
r.text=(new System.Text.StringBuilder().Append(r.text).Append(".@is<").Append(e2).Append(">()")).to_str();
} break;
case OrElseContext it :
{ var e2 = (Result)(Visit(it));
r.text=(new System.Text.StringBuilder().Append("(").Append(r.text).Append("??").Append(e2.text).Append(")")).to_str();
} break;
case CallExpressionContext it :
{ var e2 = (Result)(Visit(it));
r.text=r.text+e2.text;
} break;
case CallFuncContext it :
{ var e2 = (Result)(Visit(it));
if ( this.is_type(r.rootID) ) {
r.text=(new System.Text.StringBuilder().Append("(new ").Append(r.text).Append(e2.text).Append(")")).to_str();
r.data=r.rootID;
}
else {
r.text=r.text+e2.text;
}
} break;
case CallAsyncContext it :
{ var e2 = (Result)(Visit(it));
r.text=r.text+e2.text;
} break;
case CallAwaitContext it :
{ var e2 = (Result)(Visit(it));
r.text="await "+r.text+e2.text;
set_func_async();
} break;
case CallElementContext it :
{ var e2 = (Result)(Visit(it));
r.text=r.text+e2.text;
} break;
default:
{ if ( context.op.Type==FeelParser.Bang ) {
r.text=(new System.Text.StringBuilder().Append("ref ").Append(r.text)).to_str();
}
else if ( context.op.Type==FeelParser.Question ) {
r.text+="?";
}
} break;
}
} break;
case 1 :
{ r=(Result)(Visit(context.GetChild(0)));
} break;
}
return r;
}
public  override  object VisitOrElse( OrElseContext context ){
return (Result)(Visit(context.expression()));
}
public  override  object VisitTypeConversion( TypeConversionContext context ){
return (string)(Visit(context.typeType()));
}
public  override  object VisitTypeCheck( TypeCheckContext context ){
return (string)(Visit(context.typeType()));
}
public  override  object VisitCall( CallContext context ){
return context.op.Text;
}
public  override  object VisitWave( WaveContext context ){
return context.op.Text;
}
public  override  object VisitBitwise( BitwiseContext context ){
return (string)(this.Visit(context.GetChild(0)));
}
public  override  object VisitBitwiseAnd( BitwiseAndContext context ){
return "&";
}
public  override  object VisitBitwiseOr( BitwiseOrContext context ){
return "|";
}
public  override  object VisitBitwiseXor( BitwiseXorContext context ){
return "^";
}
public  override  object VisitBitwiseLeftShift( BitwiseLeftShiftContext context ){
return "<<";
}
public  override  object VisitBitwiseRightShift( BitwiseRightShiftContext context ){
return ">>";
}
public  override  object VisitCompare( CompareContext context ){
if ( context.op.Type==Not_Equal ) {
return "!=";
}
return context.op.Text;
}
public  override  object VisitLogic( LogicContext context ){
if ( context.op.Type==And ) {
return "&&";
}
else if ( context.op.Type==Or ) {
return "||";
}
return context.op.Text;
}
public  override  object VisitAdd( AddContext context ){
return context.op.Text;
}
public  override  object VisitMul( MulContext context ){
return context.op.Text;
}
public  override  object VisitPow( PowContext context ){
return "pow";
}
public  override  object VisitPrimaryExpression( PrimaryExpressionContext context ){
if ( context.ChildCount==1 ) {
var c = context.GetChild(0);
if ( c.@is<DataStatementContext>() ) {
return Visit(context.dataStatement());
}
else if ( c.@is<IdContext>() ) {
return Visit(context.id());
}
else if ( context.t.Type==Discard ) {
return (new Result(){text = "_",data = "var"});
}
}
else if ( context.ChildCount==4 ) {
var id = (Result)(Visit(context.id()));
var template = (string)("<"+Visit(context.templateCall()))+">";
return (new Result(){text = id.text+template,data = id.text+template,rootID = id.text});
}
var r = (Result)(Visit(context.expression()));
return (new Result(){text = (new System.Text.StringBuilder().Append("(").Append(r.text).Append(")")).to_str(),data = r.data});
}
public  override  object VisitExpressionList( ExpressionListContext context ){
var r = (new Result());
var obj = "";
foreach (var i in range(0, context.expression().Length-1, 1, true, true)){
var temp = (Result)(Visit(context.expression(i)));
if ( i==0 ) {
obj+=temp.text;
}
else {
obj+=", "+temp.text;
}
}
r.text=obj;
r.data="var";
return r;
}
public  override  object VisitTemplateDefine( TemplateDefineContext context ){
var item = (new TemplateItem());
item.Template+="<";
foreach (var i in range(0, context.templateDefineItem().Length-1, 1, true, true)){
if ( i>0 ) {
item.Template+=",";
if ( item.Contract.len()>0 ) {
item.Contract+=",";
}
}
var r = (TemplateItem)(Visit(context.templateDefineItem(i)));
item.Template+=r.Template;
item.Contract+=r.Contract;
}
item.Template+=">";
return item;
}
public  override  object VisitTemplateDefineItem( TemplateDefineItemContext context ){
var item = (new TemplateItem());
if ( context.id().len()==1 ) {
var id1 = context.id(0).GetText();
item.Template=id1;
}
else {
var id1 = context.id(0).GetText();
item.Template=id1;
var id2 = context.id(1).GetText();
item.Contract=(new System.Text.StringBuilder().Append(" where ").Append(id1).Append(":").Append(id2)).to_str();
}
return item;
}
public  override  object VisitTemplateCall( TemplateCallContext context ){
var obj = "";
foreach (var i in range(0, context.typeType().Length-1, 1, true, true)){
if ( i>0 ) {
obj+=",";
}
var r = Visit(context.typeType(i));
obj+=r;
}
return obj;
}
public  override  object VisitDataStatement( DataStatementContext context ){
var r = (new Result());
if ( context.nilExpr()!=null ) {
r.data=Any;
r.text="null";
}
else if ( context.floatExpr()!=null ) {
r.data=F64;
r.text=(string)(Visit(context.floatExpr()));
}
else if ( context.integerExpr()!=null ) {
r.data=I32;
r.text=(string)(Visit(context.integerExpr()));
}
else if ( context.rawStringExpr()!=null ) {
r.data=Str;
r.text=(string)(Visit(context.rawStringExpr()));
}
else if ( context.stringExpr()!=null ) {
r.data=Str;
r.text=(string)(Visit(context.stringExpr()));
}
else if ( context.t.Type==FeelParser.CharLiteral ) {
r.data=Chr;
r.text=context.CharLiteral().GetText();
}
else if ( context.t.Type==FeelParser.TrueLiteral ) {
r.data=Bool;
r.text=T;
}
else if ( context.t.Type==FeelParser.FalseLiteral ) {
r.data=Bool;
r.text=F;
}
return r;
}
public  override  object VisitStringExpr( StringExprContext context ){
var text = "";
if ( context.stringTemplate().Length==0 ) {
foreach (var v in context.stringContent()){
text+=Visit(v);
}
return (new System.Text.StringBuilder().Append("\"").Append(text).Append("\"")).to_str();
}
else {
text="(new System.Text.StringBuilder()";
foreach (var i in range(1, context.ChildCount-2, 1, true, true)){
var v = context.GetChild(i);
var r = (string)(Visit(context.GetChild(i)));
if ( v.@is<StringContentContext>() ) {
text+=(new System.Text.StringBuilder().Append(".Append(").Append("\"").Append(r).Append("\"").Append(")")).to_str();
}
else {
text+=r;
}
}
text+=").to_str()";
return text;
}
}
public  override  object VisitStringContent( StringContentContext context ){
if ( context.TextLiteral().GetText()=="\\$" ) {
return "$";
}
return context.TextLiteral().GetText();
}
public  override  object VisitStringTemplate( StringTemplateContext context ){
var text = "";
foreach (var v in context.expression()){
var r = (Result)(Visit(v));
text+=(new System.Text.StringBuilder().Append(".Append(").Append(r.text).Append(")")).to_str();
}
return text;
}
public  override  object VisitRawStringExpr( RawStringExprContext context ){
var text = "";
if ( context.rawStringTemplate().Length==0 ) {
foreach (var i in range(1, context.ChildCount-2, 1, true, true)){
var v = context.GetChild(i);
var r = (string)(Visit(context.GetChild(i)));
if ( v.@is<RawStringContentContext>() ) {
text+=r;
}
else {
text+="\"\"";
}
}
return (new System.Text.StringBuilder().Append("@").Append("\"").Append(text).Append("\"")).to_str();
}
else {
text="(new System.Text.StringBuilder()";
foreach (var i in range(1, context.ChildCount-2, 1, true, true)){
var v = context.GetChild(i);
var r = (string)(Visit(context.GetChild(i)));
if ( v.@is<RawStringContentContext>() ) {
text+=(new System.Text.StringBuilder().Append(".Append(@").Append("\"").Append(r).Append("\"").Append(")")).to_str();
}
else if ( v.@is<RawStringTemplateContext>() ) {
text+=r;
}
else {
text+=".Append('\"')";
}
}
text+=").to_str()";
return text;
}
}
public  override  object VisitRawStringContent( RawStringContentContext context ){
if ( context.RawTextLiteral().GetText()=="\\$" ) {
return "$";
}
return context.RawTextLiteral().GetText();
}
public  override  object VisitRawStringTemplate( RawStringTemplateContext context ){
var text = "";
foreach (var v in context.expression()){
var r = (Result)(Visit(v));
text+=(new System.Text.StringBuilder().Append(".Append(").Append(r.text).Append(")")).to_str();
}
return text;
}
public  override  object VisitFloatExpr( FloatExprContext context ){
var number = context.FloatLiteral().GetText();
return number;
}
public  override  object VisitIntegerExpr( IntegerExprContext context ){
var number = context.GetChild(0).GetText();
return number;
}
public  override  object VisitPlusMinus( PlusMinusContext context ){
var r = (new Result());
var expr = (Result)(Visit(context.expression()));
var op = Visit(context.add());
r.data=expr.data;
r.text=op+expr.text;
return r;
}
public  override  object VisitNegate( NegateContext context ){
var r = (new Result());
var expr = (Result)(Visit(context.expression()));
r.data=expr.data;
r.text="!"+expr.text;
return r;
}
public  override  object VisitBitwiseNotExpression( BitwiseNotExpressionContext context ){
var r = (new Result());
var expr = (Result)(Visit(context.expression()));
r.data=expr.data;
r.text="~"+expr.text;
return r;
}
public  override  object VisitLinq( LinqContext context ){
var r = (new Result(){data = "var"});
r.text+=(string)(Visit(context.linqHeadItem()));
foreach (var item in context.linqItem()){
r.text+=(new System.Text.StringBuilder().Append(Visit(item)).Append(" ")).to_str();
}
r.text+=(new System.Text.StringBuilder().Append(((Result)(Visit(context.id()))).text).Append(" ").Append(((Result)(Visit(context.expression()))).text)).to_str();
return r;
}
public  override  object VisitLinqItem( LinqItemContext context ){
if ( context.linqHeadItem()!=null ) {
return (string)(Visit(context.linqHeadItem()));
}
var obj = ((Result)(Visit(context.id()))).text;
if ( context.expression()!=null ) {
obj+=(new System.Text.StringBuilder().Append(" ").Append(((Result)(Visit(context.expression()))).text)).to_str();
}
return obj;
}
public  override  object VisitLinqHeadItem( LinqHeadItemContext context ){
var obj = "";
var id = (Result)(Visit(context.id()));
obj+=(new System.Text.StringBuilder().Append("from ").Append(id.text).Append(" in ").Append(((Result)(Visit(context.expression()))).text).Append(" ")).to_str();
return obj;
}
}
public partial class Compiler_static {
public static list<string> keywords = (new list<string>(){ "abstract","as","base","bool","break","byte","case","catch","char","checked","class","const","continue","decimal","default","delegate","do","double","enum","event","explicit","extern","false","finally","fixed","float","for","foreach","goto","implicit","in","int","interface","internal","is","lock","long","namespace","new","null","object","operator","out","override","params","private","protected","public","readonly","ref","return","sbyte","sealed","short","sizeof","stackalloc","static","string","struct","switch","this","throw","true","try","uint","ulong","unchecked","unsafe","ushort","using","virtual","void","volatile","while" });
}
}
