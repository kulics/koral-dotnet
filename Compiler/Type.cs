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
if ( context.templateCall()!=null ) {
if ( obj=="Array" ) {
obj = (new System.Text.StringBuilder().Append(Visit(context.templateCall())).Append("[]")).to_str();
}
else {
obj+="<"+Visit(context.templateCall())+">";
}
return obj;
}
switch (obj) {
case "I8" :
{ return I8;
} break;
case "U8" :
{ return U8;
} break;
case "I16" :
{ return I16;
} break;
case "U16" :
{ return U16;
} break;
case "I32" :
{ return I32;
} break;
case "U32" :
{ return U32;
} break;
case "I64" :
{ return I64;
} break;
case "U64" :
{ return U64;
} break;
case "F32" :
{ return F32;
} break;
case "F64" :
{ return F64;
} break;
case "Chr" :
{ return Chr;
} break;
case "Str" :
{ return Str;
} break;
case "Bool" :
{ return Bool;
} break;
case "Int" :
{ return Int;
} break;
case "Num" :
{ return Num;
} break;
case "Byte" :
{ return U8;
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
obj = (new System.Text.StringBuilder().Append("Action<").Append(@in).Append(">")).to_str();
}
}
else {
if ( @out.first_index_of(",")>=0 ) {
@out = (new System.Text.StringBuilder().Append("(").Append(@out).Append(")")).to_str();
}
if ( @in.Length==0 ) {
obj = (new System.Text.StringBuilder().Append("Func<").Append(@out).Append(">")).to_str();
}
else {
obj = (new System.Text.StringBuilder().Append("Func<").Append(@in).Append(", ").Append(@out).Append(">")).to_str();
}
}
}
else {
if ( @out.Length==0 ) {
if ( @in.Length==0 ) {
obj = (new System.Text.StringBuilder().Append("Func<").Append(Task).Append(">")).to_str();
}
else {
obj = (new System.Text.StringBuilder().Append("Func<").Append(@in).Append(", ").Append(Task).Append(">")).to_str();
}
}
else {
if ( @in.Length==0 ) {
obj = (new System.Text.StringBuilder().Append("Func<").Append(Task).Append("<").Append(@out).Append(">>")).to_str();
}
else {
obj = (new System.Text.StringBuilder().Append("Func<").Append(@in).Append(", ").Append(Task).Append("<").Append(@out).Append(">>")).to_str();
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
foreach (var i in range(0, context.typeType().Length, 1, true, false)){
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
