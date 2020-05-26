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
public  override  object VisitTypeType( TypeTypeContext context ){
var obj = "";
obj=(string)(Visit(context.GetChild(0)));
return obj;
}
public  override  object VisitTypeNullable( TypeNullableContext context ){
var obj = "";
obj=(string)(Visit(context.typeNotNull()));
obj+="?";
return obj;
}
public  override  object VisitTypePackage( TypePackageContext context ){
var obj = "";
obj+=Visit(context.nameSpaceItem());
if ( context.templateCall()!=null ) {
if ( obj=="array" ) {
obj=(new System.Text.StringBuilder().Append(Visit(context.templateCall())).Append("[]")).to_str();
}
else {
obj+="<"+Visit(context.templateCall())+">";
}
}
return obj;
}
public  override  object VisitTypeFunction( TypeFunctionContext context ){
var obj = "";
var @in = (string)(Visit(context.typeFunctionParameterClause(0)));
var @out = (string)(Visit(context.typeFunctionParameterClause(1)));
if ( context.t.Type==Right_Arrow ) {
if ( @out.Length==0 ) {
if ( @in.Length==0 ) {
obj="Action";
}
else {
obj=(new System.Text.StringBuilder().Append("Action<").Append(@in).Append(">")).to_str();
}
}
else {
if ( @out.first_index_of(",")>=0 ) {
@out=(new System.Text.StringBuilder().Append("(").Append(@out).Append(")")).to_str();
}
if ( context.y!=null ) {
@out=(new System.Text.StringBuilder().Append(IEnum).Append("<").Append(@out).Append(">")).to_str();
}
if ( @in.Length==0 ) {
obj=(new System.Text.StringBuilder().Append("Func<").Append(@out).Append(">")).to_str();
}
else {
obj=(new System.Text.StringBuilder().Append("Func<").Append(@in).Append(", ").Append(@out).Append(">")).to_str();
}
}
}
else {
if ( @out.Length==0 ) {
if ( @in.Length==0 ) {
obj=(new System.Text.StringBuilder().Append("Func<").Append(Task).Append(">")).to_str();
}
else {
obj=(new System.Text.StringBuilder().Append("Func<").Append(@in).Append(", ").Append(Task).Append(">")).to_str();
}
}
else {
if ( context.y!=null ) {
@out=(new System.Text.StringBuilder().Append(IEnum).Append("<(").Append(@out).Append(")>")).to_str();
}
if ( @in.Length==0 ) {
obj=(new System.Text.StringBuilder().Append("Func<").Append(Task).Append("<").Append(@out).Append(">>")).to_str();
}
else {
obj=(new System.Text.StringBuilder().Append("Func<").Append(@in).Append(", ").Append(Task).Append("<").Append(@out).Append(">>")).to_str();
}
}
}
return obj;
}
public  override  object VisitTypeAny( TypeAnyContext context ){
return Any;
}
public  override  object VisitTypeFunctionParameterClause( TypeFunctionParameterClauseContext context ){
var obj = "";
foreach (var i in range(0, context.typeType().Length-1, 1, true, true)){
var p = (string)(Visit(context.typeType(i)));
if ( i==0 ) {
obj+=p;
}
else {
obj+=", "+p;
}
}
return obj;
}
public  override  object VisitTypeBasic( TypeBasicContext context ){
return run(()=> { switch (context.t.Type) {
case TypeI8 :
{return I8;}break;
case TypeU8 :
{return U8;}break;
case TypeI16 :
{return I16;}break;
case TypeU16 :
{return U16;}break;
case TypeI32 :
{return I32;}break;
case TypeU32 :
{return U32;}break;
case TypeI64 :
{return I64;}break;
case TypeU64 :
{return U64;}break;
case TypeF32 :
{return F32;}break;
case TypeF64 :
{return F64;}break;
case TypeChr :
{return Chr;}break;
case TypeStr :
{return Str;}break;
case TypeBool :
{return Bool;}break;
case TypeInt :
{return Int;}break;
case TypeNum :
{return Num;}break;
case TypeByte :
{return U8;}break;
default:
{return Any;}break;}
});
}
}
}
