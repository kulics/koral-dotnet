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
public partial class FeelLangVisitorPackage:FeelLangVisitorNameSpace{
public FeelLangVisitorPackage(){}
public  override  object VisitIncludeStatement( IncludeStatementContext context ){
return Visit(context.typeType());
}
public  override  object VisitPackageStatement( PackageStatementContext context ){
var id = ((Result)Visit(context.id()));
var obj = "";
var extend = (new List<string>());
if ( context.packageFieldStatement()!=null ) {
var item = context.packageFieldStatement();
var r = ((Result)Visit(item));
obj+=(new System.Text.StringBuilder().Append("public ").Append(id.text).Append(r.text)).To_Str();
extend.Append_all(((List<string>)r.data));
}
if ( context.packageNewStatement()!=null ) {
var item = context.packageNewStatement();
var r = ((string)Visit(item));
obj+=(new System.Text.StringBuilder().Append("public ").Append(id.text).Append(" ").Append(r)).To_Str();
}
obj+=BlockRight+Wrap;
var header = "";
if ( context.annotationSupport()!=null ) {
header+=Visit(context.annotationSupport());
}
header+=(new System.Text.StringBuilder().Append(id.permission).Append(" partial class ").Append(id.text)).To_Str();
var template = "";
var template_contract = "";
if ( context.templateDefine()!=null ) {
var item = ((TemplateItem)Visit(context.templateDefine()));
template+=item.template;
template_contract=item.contract;
header+=template;
}
if ( extend.Size()>0 ) {
var temp = extend[0];
foreach (var i in 1.Up_until(extend.Size())){
temp+=","+extend[i];
}
header+=":"+temp;
}
header+=template_contract+BlockLeft+Wrap;
obj=header+obj;
return obj;
}
public  override  object VisitPackageFieldStatement( PackageFieldStatementContext context ){
var obj = "";
var extend = (new List<string>());
if ( context.id(0)!=null ) {
var Self = ((Result)Visit(context.id(0)));
self_ID=Self.text;
}
else {
self_ID="this";
}
if ( context.id(1)!=null ) {
var Super = ((Result)Visit(context.id(1)));
super_ID=Super.text;
}
obj+=Visit(context.parameterConstruct());
foreach (var item in context.packageSupportStatement()){
if ( item.GetChild(0).GetType()==typeof(IncludeStatementContext) ) {
var r = ((string)Visit(item));
extend.Append(r);
}
else {
obj+=Visit(item);
}
}
self_ID="";
super_ID="";
return (new Result(extend, obj));
}
public  override  object VisitParameterConstruct( ParameterConstructContext context ){
var obj = "";
var param = "";
var body = "";
foreach (var (i,v) in context.parameter().WithIndex()){
var p = ((Parameter)Visit(context.parameter(i)));
if ( i!=0 ) {
param+=", ";
}
param+=(new System.Text.StringBuilder().Append(p.type).Append(" ").Append(p.id).Append(" ").Append(p.value)).To_Str();
body+=(new System.Text.StringBuilder().Append("this.").Append(p.id).Append(" = ").Append(p.id).Append(Terminate).Append(Wrap)).To_Str();
if ( p.annotation!=null ) {
obj+=p.annotation;
}
if ( self_property_content.Size()>0 ) {
obj+=(new System.Text.StringBuilder().Append(p.permission).Append(" ").Append(p.type).Append(" ").Append(p.id).Append(BlockLeft)).To_Str();
foreach (var content in self_property_content){
obj+=content;
}
obj+=BlockRight+Wrap;
self_property_content.Clear();
}
else {
obj+=(new System.Text.StringBuilder().Append(p.permission).Append(" ").Append(p.type).Append(" ").Append(p.id)).To_Str();
obj+=Terminate+Wrap;
}
}
obj=(new System.Text.StringBuilder().Append("(").Append(param).Append("){").Append(body).Append("}").Append(Wrap)).To_Str()+obj;
return obj;
}
public  override  object VisitPackageFunctionStatement( PackageFunctionStatementContext context ){
var id = ((Result)Visit(context.id()));
var is_virtual = "";
if ( id.is_virtual ) {
is_virtual=" virtual ";
}
var obj = "";
var pout = "";
if ( context.parameterClauseOut()==null ) {
pout="void";
}
else {
pout=((string)Visit(context.parameterClauseOut()));
if ( context.t.Type==Right_Flow ) {
if ( pout!="void" ) {
pout=(new System.Text.StringBuilder().Append(Task).Append("<").Append(pout).Append(">")).To_Str();
}
else {
pout=Task;
}
}
}
obj+=(new System.Text.StringBuilder().Append(is_virtual).Append(" ").Append(pout).Append(" ").Append(id.text)).To_Str();
var template_contract = "";
if ( context.templateDefine()!=null ) {
var template = ((TemplateItem)Visit(context.templateDefine()));
obj+=template.template;
template_contract=template.contract;
}
Add_current_set();
Add_func_stack();
obj+=Visit(context.parameterClauseIn())+template_contract+BlockLeft+Wrap;
obj+=ProcessFunctionSupport(context.functionSupportStatement());
Delete_current_set();
obj+=BlockRight+Wrap;
if ( Get_func_async() ) {
obj=" async "+obj;
}
Delete_func_stack();
obj=(new System.Text.StringBuilder().Append(id.permission).Append(" ")).To_Str()+obj;
return obj;
}
public  override  object VisitPackageNewStatement( PackageNewStatementContext context ){
var text = "";
if ( context.id(0)!=null ) {
var Self = ((Result)Visit(context.id(0)));
self_ID=Self.text;
}
else {
self_ID="this";
}
if ( context.id(1)!=null ) {
var Super = ((Result)Visit(context.id(1)));
super_ID=Super.text;
}
Add_current_set();
text+=((string)Visit(context.parameterClauseIn()));
if ( context.expressionList()!=null ) {
text+=(new System.Text.StringBuilder().Append(":base(").Append(((Result)Visit(context.expressionList())).text).Append(")")).To_Str();
}
text+=BlockLeft+ProcessFunctionSupport(context.functionSupportStatement())+BlockRight+Wrap;
Delete_current_set();
self_ID="";
super_ID="";
return text;
}
public  override  object VisitProtocolStatement( ProtocolStatementContext context ){
var id = ((Result)Visit(context.id()));
var obj = "";
var extend = (new List<string>());
var interfaceProtocol = "";
var ptclName = id.text;
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
if ( context.protocolSubStatement()!=null ) {
var item = context.protocolSubStatement();
var r = ((Result)Visit(item));
interfaceProtocol+=r.text;
extend.Append_all(((List<string>)r.data));
}
obj+=(new System.Text.StringBuilder().Append("public partial interface ").Append(ptclName)).To_Str();
if ( extend.Size()>0 ) {
var temp = extend[0];
foreach (var i in 1.Up_until(extend.Size())){
temp+=","+extend[i];
}
obj+=":"+temp;
}
var template_contract = "";
if ( context.templateDefine()!=null ) {
var template = ((TemplateItem)Visit(context.templateDefine()));
obj+=template.template;
template_contract=template.contract;
}
obj+=template_contract+BlockLeft+Wrap;
obj+=interfaceProtocol;
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitProtocolSubStatement( ProtocolSubStatementContext context ){
var obj = "";
var extend = (new List<string>());
foreach (var item in context.protocolSupportStatement()){
if ( item.GetChild(0).GetType()==typeof(IncludeStatementContext) ) {
var r = ((string)Visit(item));
extend.Append(r);
}
else {
obj+=Visit(item);
}
}
return (new Result(extend, obj));
}
public  override  object VisitProtocolFunctionStatement( ProtocolFunctionStatementContext context ){
var id = ((Result)Visit(context.id()));
var obj = "";
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
var pout = ((string)Visit(context.parameterClauseOut()));
if ( context.t.Type==Right_Flow ) {
if ( pout!="void" ) {
pout=(new System.Text.StringBuilder().Append(Task).Append("<").Append(pout).Append(">")).To_Str();
}
else {
pout=Task;
}
}
obj+=pout+" "+id.text;
var template_contract = "";
if ( context.templateDefine()!=null ) {
var template = ((TemplateItem)Visit(context.templateDefine()));
obj+=template.template;
template_contract=template.contract;
}
obj+=Visit(context.parameterClauseIn())+template_contract+Terminate+Wrap;
return obj;
}
}
}
