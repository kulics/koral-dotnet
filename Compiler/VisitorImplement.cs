using Library;
using static Library.Lib;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using static Compiler.LiteParser;
using static Compiler.Compiler_Static;

namespace Compiler
{
public partial class LiteLangVisitor{
public  override  object VisitImplementStatement( ImplementStatementContext context )
{
var Self = ((Parameter)(Visit(context.parameterClauseSelf())));
selfID=Self.id;
var isVirtual = "";
var obj = "";
var extends = "";
if ( context.typeType()!=null ) {
extends+=":"+Visit(context.typeType());
}
obj+=(new System.Text.StringBuilder("").Append(Self.permission).Append(" partial class ").Append(Self.type+extends+BlockLeft+Wrap).Append("")).to_Str();
foreach (var item in context.implementSupportStatement()){
obj+=Visit(item);
}
obj+=BlockRight+Wrap;
selfID="";
return(obj);
}
public  override  object VisitImplementFunctionStatement( ImplementFunctionStatementContext context )
{
var id = ((Result)(Visit(context.id())));
var isVirtual = "";
if ( id.isVirtual ) {
isVirtual=" virtual ";
}
var obj = "";
obj+=(new System.Text.StringBuilder("").Append(id.permission).Append(" ")).to_Str();
if ( context.t.Type==Right_Flow ) {
var pout = ((string)(Visit(context.parameterClauseOut())));
if ( pout!="void" ) {
pout=(new System.Text.StringBuilder("").Append(Task).Append("<").Append(pout).Append(">")).to_Str();
}
else {
pout=Task;
}
obj+=(new System.Text.StringBuilder("").Append(isVirtual).Append(" async ").Append(pout).Append(" ").Append(id.text).Append("")).to_Str();
}
else {
obj+=(new System.Text.StringBuilder("").Append(isVirtual).Append(" ").Append(Visit(context.parameterClauseOut())).Append(" ").Append(id.text).Append("")).to_Str();
}
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = ((TemplateItem)(Visit(context.templateDefine())));
obj+=template.Template;
templateContract=template.Contract;
}
obj+=Visit(context.parameterClauseIn())+templateContract+Wrap+BlockLeft+Wrap;
obj+=ProcessFunctionSupport(context.functionSupportStatement());
obj+=BlockRight+Wrap;
return(obj);
}
public  override  object VisitImplementControlStatement( ImplementControlStatementContext context )
{
var r1 = ((Result)(Visit(context.id())));
var isMutable = true;
var isVirtual = "";
if ( r1.isVirtual ) {
isVirtual=" virtual ";
}
var typ = "";
typ=((string)(Visit(context.typeType())));
var obj = "";
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
obj+=(new System.Text.StringBuilder("").Append(r1.permission).Append(" ").Append(isVirtual).Append(" ").Append(typ).Append(" ").Append(r1.text).Append("").Append(BlockLeft).Append("")).to_Str();
if ( context.expression()!=null ) {
var expr = ((Result)(this.Visit(context.expression())));
obj+=(new System.Text.StringBuilder("get{return ").Append(expr.text).Append("; }set{").Append(expr.text).Append("=value;}")).to_Str();
}
else {
foreach (var item in context.packageControlSubStatement()){
var temp = ((Result)(Visit(item)));
obj+=temp.text;
}
}
obj+=BlockRight+Wrap;
return(obj);
}
public  override  object VisitOverrideStatement( OverrideStatementContext context )
{
var Self = ((Parameter)(Visit(context.parameterClauseSelf())));
selfID=Self.id;
superID=((Result)(Visit(context.id()))).text;
var obj = "";
obj+=(new System.Text.StringBuilder("").Append(Self.permission).Append(" partial class ").Append(Self.type).Append("").Append(BlockLeft+Wrap).Append("")).to_Str();
foreach (var item in context.overrideSupportStatement()){
obj+=Visit(item);
}
obj+=BlockRight+Wrap;
selfID="";
superID="";
return(obj);
}
public  override  object VisitOverrideFunctionStatement( OverrideFunctionStatementContext context )
{
var id = ((Result)(Visit(context.id())));
var isVirtual = " override ";
var obj = "";
if ( context.n!=null ) {
obj+="protected ";
}
else {
obj+=(new System.Text.StringBuilder("").Append(id.permission).Append(" ")).to_Str();
}
if ( context.t.Type==Right_Flow ) {
var pout = ((string)(Visit(context.parameterClauseOut())));
if ( pout!="void" ) {
pout=(new System.Text.StringBuilder("").Append(Task).Append("<").Append(pout).Append(">")).to_Str();
}
else {
pout=Task;
}
obj+=(new System.Text.StringBuilder("").Append(isVirtual).Append(" async ").Append(pout).Append(" ").Append(id.text).Append("")).to_Str();
}
else {
obj+=(new System.Text.StringBuilder("").Append(isVirtual).Append(" ").Append(Visit(context.parameterClauseOut())).Append(" ").Append(id.text).Append("")).to_Str();
}
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = ((TemplateItem)(Visit(context.templateDefine())));
obj+=template.Template;
templateContract=template.Contract;
}
obj+=Visit(context.parameterClauseIn())+templateContract+Wrap+BlockLeft+Wrap;
obj+=ProcessFunctionSupport(context.functionSupportStatement());
obj+=BlockRight+Wrap;
return(obj);
}
public  override  object VisitOverrideControlStatement( OverrideControlStatementContext context )
{
var r1 = ((Result)(Visit(context.id())));
var isMutable = true;
var isVirtual = " override ";
var typ = "";
if ( context.typeType()!=null ) {
typ=((string)(Visit(context.typeType())));
}
var obj = "";
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
obj+=(new System.Text.StringBuilder("").Append(r1.permission).Append(" ").Append(isVirtual).Append(" ").Append(typ).Append(" ").Append(r1.text).Append("").Append(BlockLeft).Append("")).to_Str();
foreach (var item in context.packageControlSubStatement()){
var temp = ((Result)(Visit(item)));
obj+=temp.text;
}
obj+=BlockRight+Wrap;
return(obj);
}
}
}
