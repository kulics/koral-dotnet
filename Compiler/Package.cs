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
public  override  object VisitIncludeStatement( IncludeStatementContext context ){
return Visit(context.typeType());
}
public  override  object VisitPackageStatement( PackageStatementContext context ){
var id = (Result)(Visit(context.id()));
var obj = "";
var extend = (new list<string>());
if ( context.packageStaticStatement()!=null ) {
var item = context.packageStaticStatement();
var r = (Result)(Visit(item));
obj+=r.text;
}
if ( context.packageFieldStatement()!=null ) {
var item = context.packageFieldStatement();
var r = (Result)(Visit(item));
obj+=r.text;
extend+=(list<string>)(r.data);
}
if ( context.packageNewStatement()!=null ) {
var item = context.packageNewStatement();
var r = (string)(Visit(item));
obj+=(new System.Text.StringBuilder().Append("public ").Append(id.text).Append(" ").Append(r)).to_str();
}
obj+=BlockRight+Wrap;
var header = "";
if ( context.annotationSupport()!=null ) {
header+=Visit(context.annotationSupport());
}
header+=(new System.Text.StringBuilder().Append(id.permission).Append(" partial class ").Append(id.text)).to_str();
var template = "";
var templateContract = "";
if ( context.templateDefine()!=null ) {
var item = (TemplateItem)(Visit(context.templateDefine()));
template+=item.Template;
templateContract = item.Contract;
header+=template;
}
if ( extend.length>0 ) {
var temp = extend[0];
foreach (var i in range(1, extend.length-1, 1, true, true)){
temp+=","+extend[i];
}
header+=":"+temp;
}
header+=templateContract+BlockLeft+Wrap;
obj = header+obj;
return obj;
}
public  override  object VisitPackageStaticStatement( PackageStaticStatementContext context ){
var obj = "";
foreach (var item in context.packageStaticSupportStatement()){
obj+=Visit(item);
}
return (new Result(){text = obj});
}
public  override  object VisitPackageStaticVariableStatement( PackageStaticVariableStatementContext context ){
var r1 = (Result)(Visit(context.id()));
var typ = "";
Result r2 = null;
if ( context.expression()!=null ) {
r2 = (Result)(Visit(context.expression()));
typ = (string)(r2.data);
}
if ( context.typeType()!=null ) {
typ = (string)(Visit(context.typeType()));
}
var obj = "";
if ( context.annotationSupport()!=null ) {
this.selfPropertyID=r1.text;
obj+=Visit(context.annotationSupport());
}
if ( this.selfPropertyContent.len>0 ) {
var pri = "";
if ( this.selfPropertyVariable ) {
pri = (new System.Text.StringBuilder().Append("private static ").Append(typ).Append(" _").Append(r1.text)).to_str();
if ( r2!=null ) {
pri+=" = "+r2.text;
}
pri+=Terminate+Wrap;
}
obj = pri+obj;
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" static ").Append(typ).Append(" ").Append(r1.text).Append(BlockLeft)).to_str();
foreach (var v in this.selfPropertyContent){
obj+=v;
}
obj+=BlockRight+Wrap;
this.selfPropertyContent.clear();
this.selfPropertyID="";
this.selfPropertyVariable=false;
}
else {
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" static ").Append(typ).Append(" ").Append(r1.text)).to_str();
if ( r2!=null ) {
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).to_str();
}
else {
obj+=Terminate+Wrap;
}
}
return obj;
}
public  override  object VisitPackageStaticConstantStatement( PackageStaticConstantStatementContext context ){
var r1 = (Result)(Visit(context.id()));
var typ = "";
Result? r2 = null;
if ( context.expression()!=null ) {
r2 = (Result)(Visit(context.expression()));
typ = (string)(r2.data);
}
if ( context.typeType()!=null ) {
typ = (string)(Visit(context.typeType()));
}
var obj = "";
if ( context.annotationSupport()!=null ) {
this.selfPropertyID=r1.text;
obj+=Visit(context.annotationSupport());
}
if ( this.selfPropertyContent.len>0 ) {
var pri = "";
if ( this.selfPropertyVariable ) {
pri = (new System.Text.StringBuilder().Append("private static ").Append(typ).Append(" _").Append(r1.text)).to_str();
if ( r2!=null ) {
pri+=" = "+r2.text;
}
pri+=Terminate+Wrap;
}
obj = pri+obj;
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" static ").Append(typ).Append(" ").Append(r1.text).Append(BlockLeft)).to_str();
foreach (var v in this.selfPropertyContent){
obj+=v;
}
obj+=BlockRight+Wrap;
this.selfPropertyContent.clear();
this.selfPropertyID="";
this.selfPropertyVariable=false;
}
else {
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" static ").Append(typ).Append(" ").Append(r1.text)).to_str();
if ( r2!=null ) {
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).to_str();
}
else {
obj+=Terminate+Wrap;
}
}
return obj;
}
public  override  object VisitPackageStaticFunctionStatement( PackageStaticFunctionStatementContext context ){
var id = (Result)(Visit(context.id()));
var obj = "";
var pout = "";
if ( context.parameterClauseOut()!=null ) {
pout = (string)(Visit(context.parameterClauseOut()));
}
if ( context.t.Type==Right_Flow ) {
if ( pout!="void" ) {
pout = (new System.Text.StringBuilder().Append(Task).Append("<").Append(pout).Append(">")).to_str();
}
else {
pout = Task;
}
}
obj+=(new System.Text.StringBuilder().Append(pout).Append(" ").Append(id.text)).to_str();
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = (TemplateItem)(Visit(context.templateDefine()));
obj+=template.Template;
templateContract = template.Contract;
}
this.add_current_set();
this.add_func_stack();
obj+=Visit(context.parameterClauseIn())+templateContract+BlockLeft+Wrap;
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
if ( get_func_async() ) {
obj = " async "+obj;
}
this.delete_func_stack();
obj = (new System.Text.StringBuilder().Append(id.permission).Append(" static ")).to_str()+obj;
return obj;
}
public  override  object VisitPackageFieldStatement( PackageFieldStatementContext context ){
var obj = "";
var extend = (new list<string>());
if ( context.id(0)!=null ) {
var Self = (Result)(Visit(context.id(0)));
this.selfID=Self.text;
}
if ( context.id(1)!=null ) {
var Super = (Result)(Visit(context.id(1)));
this.superID=Super.text;
}
foreach (var item in context.packageSupportStatement()){
if ( item.GetChild(0).GetType()==typeof(IncludeStatementContext) ) {
var r = (string)(Visit(item));
extend+=r;
}
else {
obj+=Visit(item);
}
}
this.selfID="";
this.superID="";
return (new Result(){text = obj,data = extend});
}
public  override  object VisitPackageVariableStatement( PackageVariableStatementContext context ){
var r1 = (Result)(Visit(context.id()));
var isMutable = r1.isVirtual;
var isVirtual = "";
if ( r1.isVirtual ) {
isVirtual = " virtual ";
}
var typ = "";
Result? r2 = null;
if ( context.expression()!=null ) {
r2 = (Result)(Visit(context.expression()));
typ = (string)(r2.data);
}
if ( context.typeType()!=null ) {
typ = (string)(Visit(context.typeType()));
}
var obj = "";
if ( context.annotationSupport()!=null ) {
this.selfPropertyID=r1.text;
obj+=Visit(context.annotationSupport());
}
if ( this.selfPropertyContent.len>0 ) {
var pri = "";
if ( this.selfPropertyVariable ) {
pri = (new System.Text.StringBuilder().Append("private ").Append(typ).Append(" _").Append(r1.text)).to_str();
if ( r2!=null ) {
pri+=" = "+r2.text;
}
pri+=Terminate+Wrap;
}
obj = pri+obj;
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" ").Append(isVirtual).Append(" ").Append(typ).Append(" ").Append(r1.text).Append(BlockLeft)).to_str();
foreach (var v in this.selfPropertyContent){
obj+=v;
}
obj+=BlockRight+Wrap;
this.selfPropertyContent.clear();
this.selfPropertyID="";
this.selfPropertyVariable=false;
}
else {
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" ").Append(typ).Append(" ").Append(r1.text)).to_str();
if ( r2!=null ) {
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).to_str();
}
else {
obj+=Terminate+Wrap;
}
}
return obj;
}
public  override  object VisitPackageConstantStatement( PackageConstantStatementContext context ){
var r1 = (Result)(Visit(context.id()));
var isMutable = r1.isVirtual;
var isVirtual = "";
if ( r1.isVirtual ) {
isVirtual = " virtual ";
}
var typ = "";
Result? r2 = null;
if ( context.expression()!=null ) {
r2 = (Result)(Visit(context.expression()));
typ = (string)(r2.data);
}
if ( context.typeType()!=null ) {
typ = (string)(Visit(context.typeType()));
}
var obj = "";
if ( context.annotationSupport()!=null ) {
this.selfPropertyID=r1.text;
obj+=Visit(context.annotationSupport());
}
if ( this.selfPropertyContent.len>0 ) {
var pri = "";
if ( this.selfPropertyVariable ) {
pri = (new System.Text.StringBuilder().Append("private ").Append(typ).Append(" _").Append(r1.text)).to_str();
if ( r2!=null ) {
pri+=" = "+r2.text;
}
pri+=Terminate+Wrap;
}
obj = pri+obj;
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" ").Append(isVirtual).Append(" ").Append(typ).Append(" ").Append(r1.text).Append(BlockLeft)).to_str();
foreach (var v in this.selfPropertyContent){
obj+=v;
}
obj+=BlockRight+Wrap;
this.selfPropertyContent.clear();
this.selfPropertyID="";
this.selfPropertyVariable=false;
}
else {
obj+=(new System.Text.StringBuilder().Append(r1.permission).Append(" ").Append(typ).Append(" ").Append(r1.text)).to_str();
if ( r2!=null ) {
obj+=(new System.Text.StringBuilder().Append(" = ").Append(r2.text).Append(Terminate).Append(Wrap)).to_str();
}
else {
obj+=Terminate+Wrap;
}
}
return obj;
}
public  override  object VisitPackageFunctionStatement( PackageFunctionStatementContext context ){
var id = (Result)(Visit(context.id()));
var isVirtual = "";
if ( id.isVirtual ) {
isVirtual = " virtual ";
}
var obj = "";
var pout = "";
if ( context.parameterClauseOut()!=null ) {
pout = (string)(Visit(context.parameterClauseOut()));
}
if ( context.t.Type==Right_Flow ) {
if ( pout!="void" ) {
pout = (new System.Text.StringBuilder().Append(Task).Append("<").Append(pout).Append(">")).to_str();
}
else {
pout = Task;
}
}
obj+=(new System.Text.StringBuilder().Append(isVirtual).Append(" ").Append(pout).Append(" ").Append(id.text)).to_str();
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = (TemplateItem)(Visit(context.templateDefine()));
obj+=template.Template;
templateContract = template.Contract;
}
this.add_current_set();
this.add_func_stack();
obj+=Visit(context.parameterClauseIn())+templateContract+BlockLeft+Wrap;
obj+=ProcessFunctionSupport(context.functionSupportStatement());
this.delete_current_set();
obj+=BlockRight+Wrap;
if ( get_func_async() ) {
obj = " async "+obj;
}
this.delete_func_stack();
obj = (new System.Text.StringBuilder().Append(id.permission).Append(" ")).to_str()+obj;
return obj;
}
public  override  object VisitPackageNewStatement( PackageNewStatementContext context ){
var text = "";
if ( context.id(0)!=null ) {
var Self = (Result)(Visit(context.id(0)));
this.selfID=Self.text;
}
if ( context.id(1)!=null ) {
var Super = (Result)(Visit(context.id(1)));
this.superID=Super.text;
}
this.add_current_set();
text+=(string)(Visit(context.parameterClauseIn()));
if ( context.expressionList()!=null ) {
text+=(new System.Text.StringBuilder().Append(":base(").Append(((Result)(Visit(context.expressionList()))).text).Append(")")).to_str();
}
text+=BlockLeft+ProcessFunctionSupport(context.functionSupportStatement())+BlockRight+Wrap;
this.delete_current_set();
this.selfID="";
this.superID="";
return text;
}
public  override  object VisitPackageEventStatement( PackageEventStatementContext context ){
var obj = "";
var id = (Result)(Visit(context.id()));
var nameSpace = Visit(context.nameSpaceItem());
obj+=(new System.Text.StringBuilder().Append("public event ").Append(nameSpace).Append(" ").Append(id.text).Append(Terminate).Append(Wrap)).to_str();
return obj;
}
public  override  object VisitProtocolStatement( ProtocolStatementContext context ){
var id = (Result)(Visit(context.id()));
var obj = "";
var extend = (new list<string>());
var interfaceProtocol = "";
var ptclName = id.text;
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
if ( context.protocolSubStatement()!=null ) {
var item = context.protocolSubStatement();
var r = (Result)(Visit(item));
interfaceProtocol+=r.text;
extend+=(list<string>)(r.data);
}
obj+=(new System.Text.StringBuilder().Append("public partial interface ").Append(ptclName)).to_str();
if ( extend.length>0 ) {
var temp = extend[0];
foreach (var i in range(1, extend.length-1, 1, true, true)){
temp+=","+extend[i];
}
obj+=":"+temp;
}
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = (TemplateItem)(Visit(context.templateDefine()));
obj+=template.Template;
templateContract = template.Contract;
}
obj+=templateContract+BlockLeft+Wrap;
obj+=interfaceProtocol;
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitProtocolSubStatement( ProtocolSubStatementContext context ){
var obj = "";
var extend = (new list<string>());
foreach (var item in context.protocolSupportStatement()){
if ( item.GetChild(0).GetType()==typeof(IncludeStatementContext) ) {
var r = (string)(Visit(item));
extend+=r;
}
else {
obj+=Visit(item);
}
}
return (new Result(){text = obj,data = extend});
}
public  override  object VisitProtocolFunctionStatement( ProtocolFunctionStatementContext context ){
var id = (Result)(Visit(context.id()));
var obj = "";
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
var pout = (string)(Visit(context.parameterClauseOut()));
if ( context.t.Type==Right_Flow ) {
if ( pout!="void" ) {
pout = (new System.Text.StringBuilder().Append(Task).Append("<").Append(pout).Append(">")).to_str();
}
else {
pout = Task;
}
}
obj+=pout+" "+id.text;
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = (TemplateItem)(Visit(context.templateDefine()));
obj+=template.Template;
templateContract = template.Contract;
}
obj+=Visit(context.parameterClauseIn())+templateContract+Terminate+Wrap;
return obj;
}
}
}
