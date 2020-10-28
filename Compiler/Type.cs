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
obj = (string)(Visit(context.GetChild(0)));
return obj;
}
public  override  object VisitTypeNullable( TypeNullableContext context ){
var obj = "";
obj = (string)(Visit(context.typeNotNull()));
obj+="?";
return obj;
}
public  override  object VisitTypePackage( TypePackageContext context ){
var obj = "";
obj+=Visit(context.nameSpaceItem());
switch (obj) {
case "List" :
{ obj = TargetTypeLst;
} break;
case "Dict" :
{ obj = TargetTypeDic;
} break;
}
if ( context.templateCall()!=null ) {
if ( obj=="Array" ) {
obj = (new System.Text.StringBuilder().Append(Visit(context.templateCall())).Append("[]")).To_Str();
}
else {
obj+="<"+Visit(context.templateCall())+">";
}
return obj;
}
switch (obj) {
case "I8" :
{ return TargetTypeI8;
} break;
case "U8" :
{ return TargetTypeU8;
} break;
case "I16" :
{ return TargetTypeI16;
} break;
case "U16" :
{ return TargetTypeU16;
} break;
case "I32" :
{ return TargetTypeI32;
} break;
case "U32" :
{ return TargetTypeU32;
} break;
case "I64" :
{ return TargetTypeI64;
} break;
case "U64" :
{ return TargetTypeU64;
} break;
case "F32" :
{ return TargetTypeF32;
} break;
case "F64" :
{ return TargetTypeF64;
} break;
case "Chr" :
{ return TargetTypeChr;
} break;
case "Str" :
{ return TargetTypeStr;
} break;
case "Bool" :
{ return TargetTypeBool;
} break;
case "Int" :
{ return TargetTypeInt;
} break;
case "Num" :
{ return TargetTypeNum;
} break;
case "Byte" :
{ return TargetTypeU8;
} break;
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
obj = "Action";
}
else {
obj = (new System.Text.StringBuilder().Append("Action<").Append(@in).Append(">")).To_Str();
}
}
else {
if ( @out.First_index_of(",")>=0 ) {
@out = (new System.Text.StringBuilder().Append("(").Append(@out).Append(")")).To_Str();
}
if ( @in.Length==0 ) {
obj = (new System.Text.StringBuilder().Append("Func<").Append(@out).Append(">")).To_Str();
}
else {
obj = (new System.Text.StringBuilder().Append("Func<").Append(@in).Append(", ").Append(@out).Append(">")).To_Str();
}
}
}
else {
if ( @out.Length==0 ) {
if ( @in.Length==0 ) {
obj = (new System.Text.StringBuilder().Append("Func<").Append(Task).Append(">")).To_Str();
}
else {
obj = (new System.Text.StringBuilder().Append("Func<").Append(@in).Append(", ").Append(Task).Append(">")).To_Str();
}
}
else {
if ( @in.Length==0 ) {
obj = (new System.Text.StringBuilder().Append("Func<").Append(Task).Append("<").Append(@out).Append(">>")).To_Str();
}
else {
obj = (new System.Text.StringBuilder().Append("Func<").Append(@in).Append(", ").Append(Task).Append("<").Append(@out).Append(">>")).To_Str();
}
}
}
return obj;
}
public  override  object VisitTypeAny( TypeAnyContext context ){
return TargetTypeAny;
}
public  override  object VisitTypeFunctionParameterClause( TypeFunctionParameterClauseContext context ){
var obj = "";
foreach (var i in Range(0, context.typeType().Length, 1)){
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
}
}
