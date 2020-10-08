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
public string template;
public string contract;
}
public partial class DicEle{
public string key;
public string value;
public string text;
}
public partial class FeelLangVisitor{
public  override  object VisitBindStatement( BindStatementContext context ){
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
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).To_Str();
return obj;
}
public  override  object VisitBindTypeStatement( BindTypeStatementContext context ){
var obj = "";
foreach (var (i, v) in Range(context.varIdType())){
if ( i!=0 ) {
obj+=","+Visit(v);
}
else {
obj+=Visit(v);
}
}
if ( context.varIdType().Length>1 ) {
obj = "("+obj+")";
}
var r2 = (Result)(Visit(context.tupleExpression()));
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).To_Str();
return obj;
}
public  override  object VisitVariableDeclaredStatement( VariableDeclaredStatementContext context ){
var obj = "";
var Type = (string)(Visit(context.typeType()));
var r = (Result)(Visit(context.id()));
if ( !this.Has_ID(r.text) ) {
this.Add_ID(r.text);
}
obj = (new System.Text.StringBuilder().Append(Type).Append(" ").Append(r.text).Append(Terminate).Append(Wrap)).To_Str();
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
case CompareContext it :
{ r.data=TargetTypeBool;
} break;
case LogicContext it :
{ r.data=TargetTypeBool;
} break;
case AddContext it :
{ if ( (string)(e1.data)==TargetTypeStr||(string)(((Result)(e2)).data)==TargetTypeStr ) {
r.data=TargetTypeStr;
}
else if ( (string)(e1.data)==TargetTypeI32&&(string)(((Result)(e2)).data)==TargetTypeI32 ) {
r.data=TargetTypeI32;
}
else {
r.data=TargetTypeF64;
}
} break;
case MulContext it :
{ if ( (string)(e1.data)==TargetTypeI32&&(string)(((Result)(e2)).data)==TargetTypeI32 ) {
r.data=TargetTypeI32;
}
else {
r.data=TargetTypeF64;
}
} break;
case PowContext it :
{ r.data=TargetTypeF64;
r.text=(new System.Text.StringBuilder().Append(op).Append("(").Append(e1.text).Append(", ").Append(((Result)(e2)).text).Append(")")).To_Str();
return r;
} break;
}
r.text=e1.text+op+((Result)(e2)).text;
} break;
case 2 :
{ r = (Result)(Visit(context.GetChild(0)));
switch (context.GetChild(1)) {
case IteratorContext it :
{ var fn = (Func<Result, Result>)(Visit(it));
return fn(r);
} break;
case TypeConversionContext it :
{ var e2 = (string)(Visit(it));
r.data=e2;
r.text=(new System.Text.StringBuilder().Append("(").Append(e2).Append(")(").Append(r.text).Append(")")).To_Str();
} break;
case TypeCheckContext it :
{ var e2 = (string)(Visit(it));
r.data=e2;
r.text=(new System.Text.StringBuilder().Append("(").Append(r.text).Append(" is ").Append(e2).Append(")")).To_Str();
} break;
case OrElseContext it :
{ var e2 = (Result)(Visit(it));
r.text=(new System.Text.StringBuilder().Append("(").Append(r.text).Append("??").Append(e2.text).Append(")")).To_Str();
} break;
case CallExpressionContext it :
{ var e2 = (Result)(Visit(it));
r.text=r.text+e2.text;
} break;
case CallFuncContext it :
{ var e2 = (Result)(Visit(it));
if ( this.Is_type(r.rootID) ) {
r.text=(new System.Text.StringBuilder().Append("(new ").Append(r.text).Append(e2.text).Append(")")).To_Str();
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
Set_func_async();
} break;
case CallElementContext it :
{ var e2 = (Result)(Visit(it));
r.text=r.text+e2.text;
} break;
case JudgeExpressionContext it :
{ var e2 = (Func<string, Result>)(Visit(it));
r.text=e2(r.text).text;
} break;
default:
{ if ( context.op.Type==FeelParser.Bang ) {
r.text=(new System.Text.StringBuilder().Append("ref ").Append(r.text)).To_Str();
}
else if ( context.op.Type==FeelParser.Question ) {
r.text+="?";
}
} break;
}
} break;
case 1 :
{ r = (Result)(Visit(context.GetChild(0)));
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
return context.op.Text;
}
public  override  object VisitAdd( AddContext context ){
return context.op.Text;
}
public  override  object VisitMul( MulContext context ){
return context.op.Text;
}
public  override  object VisitPow( PowContext context ){
return "Pow";
}
public  override  object VisitPrimaryExpression( PrimaryExpressionContext context ){
switch (context.ChildCount) {
case 1 :
{ var c = context.GetChild(0);
switch (c) {
case DataStatementContext it :
{ return Visit(context.dataStatement());
} break;
case IdContext it :
{ return Visit(context.id());
} break;
}
if ( context.t.Type==Discard ) {
return (new Result(){text = "_",data = "var"});
}
} break;
case 4 :
{ var id = (Result)(Visit(context.id()));
var template = "<"+((string)(Visit(context.templateCall())))+">";
return (new Result(){text = id.text+template,data = id.text+template,rootID = id.text});
} break;
}
var r = (Result)(Visit(context.expression()));
return (new Result(){text = (new System.Text.StringBuilder().Append("(").Append(r.text).Append(")")).To_Str(),data = r.data});
}
public  override  object VisitExpressionList( ExpressionListContext context ){
var r = (new Result());
var obj = "";
foreach (var i in Range(0, context.expression().Length, 1)){
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
item.template+="<";
foreach (var i in Range(0, context.templateDefineItem().Length, 1)){
if ( i>0 ) {
item.template+=",";
if ( item.contract.Size()>0 ) {
item.contract+=",";
}
}
var r = (TemplateItem)(Visit(context.templateDefineItem(i)));
item.template+=r.template;
item.contract+=r.contract;
}
item.template+=">";
return item;
}
public  override  object VisitTemplateDefineItem( TemplateDefineItemContext context ){
var item = (new TemplateItem());
if ( context.id().Size()==1 ) {
var id1 = context.id(0).GetText();
item.template=id1;
}
else {
var id1 = context.id(0).GetText();
item.template=id1;
var id2 = context.id(1).GetText();
item.contract=(new System.Text.StringBuilder().Append(" where ").Append(id1).Append(":").Append(id2)).To_Str();
}
return item;
}
public  override  object VisitTemplateCall( TemplateCallContext context ){
var obj = "";
foreach (var i in Range(0, context.typeType().Length, 1)){
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
r.data=TargetTypeAny;
r.text="null";
}
else if ( context.floatExpr()!=null ) {
r.data=TargetTypeF64;
r.text=(string)(Visit(context.floatExpr()));
}
else if ( context.integerExpr()!=null ) {
r.data=TargetTypeI32;
r.text=(string)(Visit(context.integerExpr()));
}
else if ( context.rawStringExpr()!=null ) {
r.data=TargetTypeStr;
r.text=(string)(Visit(context.rawStringExpr()));
}
else if ( context.stringExpr()!=null ) {
r.data=TargetTypeStr;
r.text=(string)(Visit(context.stringExpr()));
}
else if ( context.t.Type==FeelParser.CharLiteral ) {
r.data=TargetTypeChr;
r.text=context.CharLiteral().GetText();
}
else if ( context.t.Type==FeelParser.TrueLiteral ) {
r.data=TargetTypeBool;
r.text=T;
}
else if ( context.t.Type==FeelParser.FalseLiteral ) {
r.data=TargetTypeBool;
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
return (new System.Text.StringBuilder().Append("\"").Append(text).Append("\"")).To_Str();
}
else {
text = "(new System.Text.StringBuilder()";
foreach (var i in Range(1, context.ChildCount-1, 1)){
var v = context.GetChild(i);
var r = (string)(Visit(context.GetChild(i)));
switch (v) {
case StringContentContext it :
{ text+=(new System.Text.StringBuilder().Append(".Append(").Append("\"").Append(r).Append("\"").Append(")")).To_Str();
} break;
default:
{ text+=r;
} break;
}
}
text+=").To_Str()";
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
text+=(new System.Text.StringBuilder().Append(".Append(").Append(r.text).Append(")")).To_Str();
}
return text;
}
public  override  object VisitRawStringExpr( RawStringExprContext context ){
var text = "";
if ( context.rawStringTemplate().Length==0 ) {
foreach (var i in Range(1, context.ChildCount-1, 1)){
var v = context.GetChild(i);
var r = (string)(Visit(context.GetChild(i)));
switch (v) {
case RawStringContentContext it :
{ text+=r;
} break;
default:
{ text+="\"\"";
} break;
}
}
return (new System.Text.StringBuilder().Append("@").Append("\"").Append(text).Append("\"")).To_Str();
}
else {
text = "(new System.Text.StringBuilder()";
foreach (var i in Range(1, context.ChildCount-1, 1)){
var v = context.GetChild(i);
var r = (string)(Visit(context.GetChild(i)));
switch (v) {
case RawStringContentContext it :
{ text+=(new System.Text.StringBuilder().Append(".Append(@").Append("\"").Append(r).Append("\"").Append(")")).To_Str();
} break;
case RawStringTemplateContext it :
{ text+=r;
} break;
default:
{ text+=".Append('\"')";
} break;
}
}
text+=").To_Str()";
return text;
}
}
public  override  object VisitRawStringContent( RawStringContentContext context ){
if ( context.RawTextLiteral().GetText()=="\\\\" ) {
return "\\";
}
return context.RawTextLiteral().GetText();
}
public  override  object VisitRawStringTemplate( RawStringTemplateContext context ){
var text = "";
foreach (var v in context.expression()){
var r = (Result)(Visit(v));
text+=(new System.Text.StringBuilder().Append(".Append(").Append(r.text).Append(")")).To_Str();
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
r.text+=(new System.Text.StringBuilder().Append(Visit(item)).Append(" ")).To_Str();
}
r.text+=(new System.Text.StringBuilder().Append(((Result)(Visit(context.id()))).text).Append(" ").Append(((Result)(Visit(context.expression()))).text)).To_Str();
return r;
}
public  override  object VisitLinqItem( LinqItemContext context ){
if ( context.linqHeadItem()!=null ) {
return (string)(Visit(context.linqHeadItem()));
}
var obj = ((Result)(Visit(context.id()))).text;
if ( context.expression()!=null ) {
obj+=(new System.Text.StringBuilder().Append(" ").Append(((Result)(Visit(context.expression()))).text)).To_Str();
}
return obj;
}
public  override  object VisitLinqHeadItem( LinqHeadItemContext context ){
var obj = "";
var id = (Result)(Visit(context.id()));
obj+=(new System.Text.StringBuilder().Append("from ").Append(id.text).Append(" in ").Append(((Result)(Visit(context.expression()))).text).Append(" ")).To_Str();
return obj;
}
}
public partial class Compiler_static {
public static List<string> keywords = (new List<string>(){ "abstract","as","base","bool","break","byte","case","catch","char","checked","class","const","continue","decimal","default","delegate","do","double","enum","event","explicit","extern","false","finally","fixed","float","for","foreach","goto","implicit","in","int","interface","internal","is","lock","long","namespace","new","null","object","operator","out","override","params","private","protected","public","readonly","ref","return","sbyte","sealed","short","sizeof","stackalloc","static","string","struct","switch","this","throw","true","try","uint","ulong","unchecked","unsafe","ushort","using","virtual","void","volatile","while" });
}
}
