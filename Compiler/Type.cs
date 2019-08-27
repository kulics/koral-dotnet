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
public  override  object @base( TypeTypeContext context ){
var obj = "";
obj = ((string)(Visit(context.GetChild(0))));
return obj;
}
public  override  object @base( TypeReferenceContext context ){
var obj = "ref ";
if ( context.typeNullable()!=null ) {
obj+=Visit(context.typeNullable());
}
else if ( context.typeNotNull()!=null ) {
obj+=Visit(context.typeNotNull());
}
return obj;
}
public  override  object @base( TypeNullableContext context ){
var obj = "";
obj = ((string)(Visit(context.typeNotNull())));
if ( context.typeNotNull().GetChild(0).@is<TypeBasicContext>()&&context.typeNotNull().GetChild(0).GetText()!="any"&&context.typeNotNull().GetChild(0).GetText()!="str" ) {
obj+="?";
}
return obj;
}
public  override  object @base( TypeTupleContext context ){
var obj = "";
obj+="(";
foreach (var (i,v) in range(context.typeType())){
obj+=run(()=>{if ( i==0 ) {
return Visit(v);}
else {
return (new System.Text.StringBuilder(",").Append(Visit(v)).Append("")).to_str();}
});
}
obj+=")";
return obj;
}
public  override  object @base( TypeArrayContext context ){
var obj = "";
obj+=(new System.Text.StringBuilder("").Append(Visit(context.typeType())).Append("[]")).to_str();
return obj;
}
public  override  object @base( TypeListContext context ){
var obj = "";
obj+=(new System.Text.StringBuilder("").Append(Lst).Append("<").Append(Visit(context.typeType())).Append(">")).to_str();
return obj;
}
public  override  object @base( TypeSetContext context ){
var obj = "";
obj+=(new System.Text.StringBuilder("").Append(Set).Append("<").Append(Visit(context.typeType())).Append(">")).to_str();
return obj;
}
public  override  object @base( TypeDictionaryContext context ){
var obj = "";
obj+=(new System.Text.StringBuilder("").Append(Dic).Append("<").Append(Visit(context.typeType(0))).Append(",").Append(Visit(context.typeType(1))).Append(">")).to_str();
return obj;
}
public  override  object @base( TypeStackContext context ){
var obj = "";
obj+=(new System.Text.StringBuilder("").Append(Stk).Append("<").Append(Visit(context.typeType())).Append(">")).to_str();
return obj;
}
public  override  object @base( TypePackageContext context ){
var obj = "";
obj+=Visit(context.nameSpaceItem());
if ( context.templateCall()!=null ) {
obj+=Visit(context.templateCall());
}
return obj;
}
public  override  object @base( TypeFunctionContext context ){
var obj = "";
var @in = ((string)(Visit(context.typeFunctionParameterClause(0))));
var @out = ((string)(Visit(context.typeFunctionParameterClause(1))));
if ( context.t.Type==Right_Arrow ) {
if ( @out.Length==0 ) {
obj = run(()=>{if ( @in.Length==0 ) {
return "Action";}
else {
return (new System.Text.StringBuilder("Action<").Append(@in).Append(">")).to_str();}
});
}
else {
if ( @out.first_index_of(",")>=0 ) {
@out = (new System.Text.StringBuilder("(").Append(@out).Append(")")).to_str();
}
if ( context.y!=null ) {
@out = (new System.Text.StringBuilder("").Append(IEnum).Append("<").Append(@out).Append(">")).to_str();
}
obj = run(()=>{if ( @in.Length==0 ) {
return (new System.Text.StringBuilder("Func<").Append(@out).Append(">")).to_str();}
else {
return (new System.Text.StringBuilder("Func<").Append(@in).Append(", ").Append(@out).Append(">")).to_str();}
});
}
}
else {
if ( @out.Length==0 ) {
obj = run(()=>{if ( @in.Length==0 ) {
return (new System.Text.StringBuilder("Func<").Append(Task).Append(">")).to_str();}
else {
return (new System.Text.StringBuilder("Func<").Append(@in).Append(", ").Append(Task).Append(">")).to_str();}
});
}
else {
if ( context.y!=null ) {
@out = (new System.Text.StringBuilder("").Append(IEnum).Append("<(").Append(@out).Append(")>")).to_str();
}
obj = run(()=>{if ( @in.Length==0 ) {
return (new System.Text.StringBuilder("Func<").Append(Task).Append("<").Append(@out).Append(">>")).to_str();}
else {
return (new System.Text.StringBuilder("Func<").Append(@in).Append(", ").Append(Task).Append("<").Append(@out).Append(">>")).to_str();}
});
}
}
return obj;
}
public  override  object @base( TypeAnyContext context ){
return Any;
}
public  override  object @base( TypeFunctionParameterClauseContext context ){
var obj = "";
foreach (var i in range(0,context.typeType().Length,1,true,false)){
var p = ((string)(Visit(context.typeType(i))));
obj+=run(()=>{if ( i==0 ) {
return p;}
else {
return (new System.Text.StringBuilder(", ").Append(p).Append("")).to_str();}
});
}
return obj;
}
public  override  object @base( TypeBasicContext context ){
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
{return Any;}break;
}
});
}
}
}
