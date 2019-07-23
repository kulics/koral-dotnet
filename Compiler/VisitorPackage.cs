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
public  override  object VisitIncludeStatement( IncludeStatementContext context )
{
return Visit(context.typeType());
}
public  override  object VisitPackageStatement( PackageStatementContext context )
{
var id = ((Result)(Visit(context.id())));
var obj = "";
var extend = "";
foreach (var item in context.packageSupportStatement()){
if ( item.GetChild(0).GetType()==@typeof<IncludeStatementContext>() ) {
if ( extend=="" ) {
extend+=Visit(item);
}
else {
extend+=","+Visit(item);
}
}
else {
obj+=Visit(item);
}
}
obj+=BlockRight+Wrap;
var header = "";
if ( context.annotationSupport()!=null ) {
header+=Visit(context.annotationSupport());
}
header+=(new System.Text.StringBuilder("").Append(id.permission).Append(" partial class ").Append(id.text).Append("")).to_str();
var template = "";
var templateContract = "";
if ( context.templateDefine()!=null ) {
var item = ((TemplateItem)(Visit(context.templateDefine())));
template+=item.Template;
templateContract = item.Contract;
header+=template;
}
if ( extend.Length>0 ) {
header+=":";
if ( extend.Length>0 ) {
header+=extend;
}
}
header+=templateContract+Wrap+BlockLeft+Wrap;
obj = header+obj;
return obj;
}
public  override  object VisitPackageVariableStatement( PackageVariableStatementContext context )
{
var r1 = ((Result)(Visit(context.id())));
var isMutable = r1.isVirtual;
var typ = "";
Result r2 = null;
if ( context.expression()!=null ) {
r2 = ((Result)(Visit(context.expression())));
typ = ((string)(r2.data));
}
if ( context.typeType()!=null ) {
typ = ((string)(Visit(context.typeType())));
}
var obj = "";
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
obj+=(new System.Text.StringBuilder("").Append(r1.permission).Append(" ").Append(typ).Append(" ").Append(r1.text).Append("")).to_str();
if ( r2!=null ) {
obj+=(new System.Text.StringBuilder(" = ").Append(r2.text).Append(" ").Append(Terminate).Append(" ").Append(Wrap).Append("")).to_str();
}
else {
obj+=Terminate+Wrap;
}
return obj;
}
public  override  object VisitPackageControlSubStatement( PackageControlSubStatementContext context )
{
var obj = "";
var id = "";
var typ = "";
this.add_current_set();
(id, typ) = GetControlSub(context.id(0).GetText());
if ( context.id(1)!=null ) {
this.setID=context.id(1).GetText();
}
if ( context.functionSupportStatement().Length>0 ) {
obj+=id+BlockLeft;
foreach (var item in context.functionSupportStatement()){
obj+=Visit(item);
}
obj+=BlockRight+Wrap;
}
else {
obj+=id+Terminate;
}
this.delete_current_set();
this.setID="";
return (new Result(){text = obj,data = typ});
}
public  override  object VisitPackageNewStatement( PackageNewStatementContext context )
{
var text = "";
var Self = ((Parameter)(Visit(context.parameterClauseSelf())));
this.selfID=Self.id;
text+=(new System.Text.StringBuilder("").Append(Self.permission).Append(" partial class ").Append(Self.type).Append("").Append(BlockLeft+Wrap).Append("")).to_str();
text+=(new System.Text.StringBuilder("public ").Append(Self.type).Append(" ")).to_str();
this.add_current_set();
text+=((string)(Visit(context.parameterClauseIn())));
if ( context.expressionList()!=null ) {
text+=(new System.Text.StringBuilder(":base(").Append(((Result)(Visit(context.expressionList()))).text).Append(")")).to_str();
}
text+=BlockLeft+ProcessFunctionSupport(context.functionSupportStatement())+BlockRight+Wrap;
this.delete_current_set();
text+=BlockRight+Wrap;
this.selfID="";
return text;
}
public  override  object VisitPackageEventStatement( PackageEventStatementContext context )
{
var obj = "";
var id = ((Result)(Visit(context.id())));
var nameSpace = Visit(context.nameSpaceItem());
obj+=(new System.Text.StringBuilder("public event ").Append(nameSpace).Append(" ").Append(id.text+Terminate+Wrap).Append("")).to_str();
return obj;
}
public  override  object VisitProtocolStatement( ProtocolStatementContext context )
{
var id = ((Result)(Visit(context.id())));
var obj = "";
var interfaceProtocol = "";
var ptclName = id.text;
if ( context.annotationSupport()!=null ) {
obj+=Visit(context.annotationSupport());
}
foreach (var item in context.protocolSupportStatement()){
var r = ((Result)(Visit(item)));
interfaceProtocol+=r.text;
}
obj+="public partial interface "+ptclName;
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = ((TemplateItem)(Visit(context.templateDefine())));
obj+=template.Template;
templateContract = template.Contract;
}
obj+=templateContract+Wrap+BlockLeft+Wrap;
obj+=interfaceProtocol;
obj+=BlockRight+Wrap;
return obj;
}
public  override  object VisitProtocolControlStatement( ProtocolControlStatementContext context )
{
var id = ((Result)(Visit(context.id())));
var isMutable = id.isVirtual;
var r = (new Result());
if ( context.annotationSupport()!=null ) {
r.text+=Visit(context.annotationSupport());
}
r.permission="public";
var type = ((string)(Visit(context.typeType())));
r.text+=type+" "+id.text;
r.text+=BlockLeft;
if ( context.protocolControlSubStatement().Length>0 ) {
foreach (var item in context.protocolControlSubStatement()){
r.text+=Visit(item);
}
}
else {
r.text+="get;set;";
}
r.text+=BlockRight+Wrap;
return r;
}
public  override  object VisitProtocolControlSubStatement( ProtocolControlSubStatementContext context )
{
var obj = "";
obj = GetControlSub(context.id().GetText()).id+Terminate;
return obj;
}
public  override  object VisitProtocolFunctionStatement( ProtocolFunctionStatementContext context )
{
var id = ((Result)(Visit(context.id())));
var r = (new Result());
if ( context.annotationSupport()!=null ) {
r.text+=Visit(context.annotationSupport());
}
r.permission="public";
if ( context.t.Type==Right_Flow ) {
var pout = ((string)(Visit(context.parameterClauseOut())));
if ( pout!="void" ) {
pout = (new System.Text.StringBuilder("").Append(Task).Append("<").Append(pout).Append(">")).to_str();
}
else {
pout = Task;
}
r.text+=pout+" "+id.text;
}
else {
r.text+=Visit(context.parameterClauseOut())+" "+id.text;
}
var templateContract = "";
if ( context.templateDefine()!=null ) {
var template = ((TemplateItem)(Visit(context.templateDefine())));
r.text+=template.Template;
templateContract = template.Contract;
}
r.text+=Visit(context.parameterClauseIn())+templateContract+Terminate+Wrap;
return r;
}
}
}
