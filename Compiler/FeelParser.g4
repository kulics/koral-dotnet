parser grammar FeelParser;

options { tokenVocab=FeelLexer; }

program: statement+;

statement: New_Line* (annotationSupport)?  
exportStatement end? New_Line* (namespaceSupportStatement end|New_Line)* (namespaceSupportStatement end?)? New_Line* ;

// 导出命名空间
exportStatement: Left_Arrow nameSpaceItem;

namespaceSupportStatement:
importStatement |
protocolStatement |
packageStatement |
implementStatement |
namespaceFunctionStatement |
namespaceVariableStatement |
namespaceConstantStatement |
enumStatement |
typeRedefineStatement |
typeTagStatement |
New_Line ;

// 导入命名空间
importStatement: Right_Arrow left_brace ((importSubStatement | typeAliasStatement) end|New_Line)*
((importSubStatement | typeAliasStatement) end?)? right_brace;

importSubStatement: (annotationSupport)? ((Bang? id|Dot) Equal)?
 (nameSpaceItem stringExpr? | nameSpaceItem? stringExpr);

// 类型别名
typeAliasStatement: Bang? id Equal typeType;
// 类型重定义
typeRedefineStatement: Bang? id Equal New_Line* typeType;
// 特殊类型注释
typeTagStatement: Comment_Tag; 

// 枚举
enumStatement: (annotationSupport)? Bang? id Equal New_Line* Coin
 left_brace (enumSupportStatement end|New_Line)* enumSupportStatement end? right_brace;

enumSupportStatement: Or id (Equal (add)? integerExpr)?;
// 命名空间变量
namespaceVariableStatement: (annotationSupport)? Bang id (Equal expression | Colon typeType (Equal expression)?);
// 命名空间常量
namespaceConstantStatement: (annotationSupport)? id (Equal expression | Colon typeType (Equal expression)?);
// 命名空间函数
namespaceFunctionStatement: (annotationSupport)? id templateDefine? Equal
 left_paren parameterClauseIn (t=(Right_Arrow|Right_Flow) New_Line*
parameterClauseOut)? right_paren left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;

// 定义包
packageStatement: (annotationSupport)? id templateDefine? Equal
 (packageFieldStatement|packageStaticStatement|packageNewStatement);

packageStaticStatement: Coin left_brace (packageStaticSupportStatement end|New_Line)* packageStaticSupportStatement end? right_brace left_brace right_brace;
// 包静态语句
packageStaticSupportStatement:
packageStaticFunctionStatement |
packageStaticVariableStatement |
packageStaticConstantStatement ;

// 定义变量
packageStaticVariableStatement: (annotationSupport)? Bang id (Equal expression | Colon typeType (Equal expression)?);
// 定义常量
packageStaticConstantStatement: (annotationSupport)? id (Equal expression | Colon typeType (Equal expression)?);
// 函数
packageStaticFunctionStatement: (annotationSupport)? id templateDefine? Equal
 left_paren parameterClauseIn (t=(Right_Arrow|Right_Flow) New_Line*
parameterClauseOut)? right_paren left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;

packageFieldStatement: Coin (p=Question? id (more id)?)? left_brace (packageSupportStatement end|New_Line)* packageSupportStatement end? right_brace;

// 包支持的语句
packageSupportStatement:
includeStatement |
packageFunctionStatement |
packageVariableStatement |
packageConstantStatement |
packageEventStatement |
overrideFunctionStatement |
overrideVariableStatement |
overrideConstantStatement |
New_Line;

// 包含
includeStatement: typeType;
// 包构造方法
packageNewStatement: (annotationSupport)? left_paren parameterClauseIn Right_Arrow Coin p=Question? (id (more id)?)? right_paren
(left_paren expressionList? right_paren)? left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// 定义变量
packageVariableStatement: (annotationSupport)? Bang id (Equal expression | Colon typeType (Equal expression)?);
// 定义常量
packageConstantStatement: (annotationSupport)? id (Equal expression | Colon typeType (Equal expression)?);
// 函数
packageFunctionStatement: (annotationSupport)? id templateDefine? Equal
 left_paren parameterClauseIn (t=(Right_Arrow|Right_Flow) New_Line*
parameterClauseOut)? right_paren left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// 定义包事件
packageEventStatement: Bang id left_brack Right_Arrow right_brack nameSpaceItem;

// 扩展
implementStatement: id templateDefine? Colon Equal
(packageNewStatement|packageFieldStatement);

// 定义变量
overrideVariableStatement: (annotationSupport)? Dot (n='_')? Bang id (Equal expression | Colon typeType (Equal expression)?);
// 定义常量
overrideConstantStatement: (annotationSupport)? Dot (n='_')? id (Equal expression | Colon typeType (Equal expression)?);
// 函数
overrideFunctionStatement: (annotationSupport)? Dot (n='_')? id templateDefine? Equal
 left_paren parameterClauseIn (t=(Right_Arrow|Right_Flow) New_Line*
parameterClauseOut)? right_paren left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace end;

// 协议
protocolStatement: (annotationSupport)? id templateDefine? Equal protocolSubStatement;

protocolSubStatement: left_brace (protocolSupportStatement end|New_Line)* protocolSupportStatement end? right_brace;
// 协议支持的语句
protocolSupportStatement:
includeStatement |
protocolFunctionStatement |
New_Line ;

// 函数
protocolFunctionStatement: (annotationSupport)? id templateDefine? Colon left_paren parameterClauseIn 
t=(Right_Arrow|Right_Flow) New_Line* parameterClauseOut right_paren;

// 函数
functionStatement: id templateDefine? Equal left_paren parameterClauseIn
 (t=(Right_Arrow|Right_Flow) New_Line* parameterClauseOut)?
  right_paren left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// 返回
returnStatement: Left_Arrow (tupleExpression)?;
// 异步返回
returnAsyncStatement: Left_Flow (tupleExpression)?;
// 生成器
yieldReturnStatement: At Left_Arrow tupleExpression;
yieldBreakStatement: At Left_Arrow;
// 入参
parameterClauseIn: parameter? (more parameter)*;
// 出参
parameterClauseOut: parameter? (more parameter)*;
// 参数结构
parameter: (annotationSupport)? Bang? id Colon (Comma_Comma|Comma_Comma_Comma)? typeType;

// 函数支持的语句
functionSupportStatement:
returnStatement |
returnAsyncStatement |
yieldReturnStatement |
yieldBreakStatement |
judgeEqualStatement |
judgeTypeStatement |
judgeStatement |
loopStatement |
loopCaseStatement |
loopJumpStatement |
loopContinueStatement |
usingStatement |
checkStatement |
checkReportStatement |
functionStatement |
variableDeclaredStatement |
constantDeclaredStatement |
varStatement |
varTypeStatement |
bindStatement |
bindTypeStatement |
assignStatement |
expressionStatement ;

// 条件判断
judgeEqualStatement:
expression Equal_Equal caseEqualStatement+ caseElseStatement?;

judgeTypeStatement:
expression Colon_Colon caseTypeStatement+ caseElseStatement?;

// 判断条件声明
caseElseStatement: New_Line? Or Question left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
caseEqualStatement: New_Line? Or judgeEqualCase (Or New_Line? judgeEqualCase)* Question left_brace
 (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;

judgeEqualCase: expression;

caseTypeStatement: New_Line? Or judgeTypeCase (Or New_Line? judgeTypeCase)* Question left_brace
 (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;

judgeTypeCase: typeType (Double_Arrow id)?;

// 判断
judgeStatement:
judgeIfStatement judgeElseIfStatement* judgeElseStatement?;
// else if 判断
judgeElseIfStatement: New_Line? Or judgeIfStatement;
// else 判断
judgeElseStatement: New_Line? Or Question left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// if 判断
judgeIfStatement: expression Question left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;

// 循环
loopStatement: expression At (left_brack id right_brack)? Bang? id
 left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace ;
// 条件循环
loopCaseStatement: expression At left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace loopElseStatement? ;
// else 判断
loopElseStatement: New_Line? Or At left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// 跳出循环
loopJumpStatement: Tilde At ;
// 跳过当前循环
loopContinueStatement: At ;
// 检查
checkStatement: 
 left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace (checkErrorStatement)* checkFinallyStatment 
| left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace (checkErrorStatement)+ ;
// 定义检查变量
usingStatement: Right_Arrow Bang? constId (more constId)* Equal
tupleExpression left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace ;
// 错误处理
checkErrorStatement: New_Line? And (id | id Colon typeType) Bang left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// 最终执行
checkFinallyStatment: New_Line? And Bang left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// 抛出异常
checkReportStatement: Bang Left_Arrow expression ;

// 声明变量
variableDeclaredStatement: Bang id Colon typeType ;
// 声明常量
constantDeclaredStatement: id Colon typeType ;
// 定义
varStatement: varId (more varId)* Equal tupleExpression ;
varTypeStatement: varIdType (more varIdType)* Equal tupleExpression ;
// 绑定
bindStatement: constId (more constId)* Equal tupleExpression ;
bindTypeStatement: constIdType (more constIdType)* Equal tupleExpression ;
// 复合赋值
assignStatement: tupleExpression assign tupleExpression ;
// 表达式
expressionStatement: expression ;

varId: Bang id | Discard;
varIdType: Bang id Colon typeType | Discard;
constId: id | Discard;
constIdType: id Colon typeType | Discard;

tupleExpression: expression (more expression)* ; // 元组
// 基础表达式
primaryExpression: 
id templateCall |
id |
t=Discard |
left_paren expression right_paren | 
dataStatement;

// 表达式
expression:
linq // 联合查询
| primaryExpression 
| callPkg // 新建包 
| callChannel // 通道访问
| callAsync // 创建异步调用
| list // 列表
| dictionary // 字典
| lambda // lambda表达式
| functionExpression // 函数
| pkgAnonymous // 匿名包
| plusMinus // 正负处理
| bitwiseNotExpression // 位运算取反
| negate // 取反
| expression op=Bang // 取引用
| expression op=Question // 可空判断
| expression orElse // 空值替换
| expression callFunc // 函数调用
| expression callChannel // 调用通道
| expression callElement // 访问元素
| expression callAwait  // 异步等待调用
| expression callExpression // 链式调用
| expression transfer expression // 传递通道值
| expression pow expression // 幂型表达式
| expression mul expression // 积型表达式
| expression add expression // 和型表达式
| expression bitwise expression // 位运算表达式
| expression iterator expression // 迭代器
| expression typeConversion // 类型转换
| expression typeCheck // 类型判断
| expression compare expression // 比较表达式
| expression logic expression // 逻辑表达式
| expression judgeExpression // 判断表达式
; 

callExpression: call New_Line? (id | left_paren id templateCall right_paren | id templateCall) (callFunc|callElement)?;

tuple: left_paren (expression (more expression)* )? right_paren; // 元组

expressionList: expression (more expression)* ; // 表达式列

annotationSupport: annotation New_Line?;

annotation: left_brack annotationList right_brack; // 注解

annotationList: (annotationItem|annotationString) (more annotationItem)*;

annotationItem: (id left_brace id (tuple|lambda)? right_brace | id (tuple|lambda)?);

annotationString: stringExpr|rawStringExpr;

callFunc: tuple; // 函数调用

callAsync: Right_Wave expression; // 异步等待调用

callAwait: Right_Wave tuple; // 异步等待调用

callChannel: Left_Wave expression; // 通道访问

transfer: Left_Wave; // 传递通道值

callElement: left_brack (slice | expression) right_brack; // 元素调用

callPkg: typeNotNull left_brace (pkgAssign|listAssign|dictionaryAssign)? right_brace; // 新建包

orElse: Question_Question expression; // 可空取值

typeConversion: Bang_Bang typeType; // 类型转化

typeCheck: Colon_Colon typeType; // 类型转化

pkgAssign: (pkgAssignElement end)* pkgAssignElement; // 简化赋值

pkgAssignElement: name Equal expression; // 简化赋值元素

listAssign: (expression end)* expression;

dictionaryAssign: (dictionaryElement end)* dictionaryElement;

list: left_brace (expression end)* expression right_brace; // 列表

dictionary: left_brace (dictionaryElement end)* dictionaryElement right_brace; // 字典

dictionaryElement: left_brack expression right_brack Equal expression; // 字典元素

slice: sliceStart | sliceEnd | sliceFull;

sliceFull: expression (Dot_Dot|Dot_Dot_Dot|Dot_Dot_Less|Dot_Dot_Greater) expression; 
sliceStart: expression (Dot_Dot|Dot_Dot_Dot|Dot_Dot_Less|Dot_Dot_Greater);
sliceEnd: (Dot_Dot|Dot_Dot_Dot|Dot_Dot_Less|Dot_Dot_Greater) expression; 

nameSpaceItem: (id call New_Line?)* id;

name: id (call New_Line? id)* ;

templateDefine: templateDefineItem+;

templateDefineItem: Back_Quote (id | left_paren id Colon id right_paren); 

templateCall: (Back_Quote typeType)+;

lambda: left_paren (lambdaIn)? (t=Greater)? right_paren left_brace tupleExpression right_brace
| left_paren (lambdaIn)? (t=Greater)? right_paren left_brace
(functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;

lambdaIn: id (more id)*;

pkgAnonymous: pkgAnonymousAssign; // 匿名包

pkgAnonymousAssign: Coin left_brace (pkgAnonymousAssignElement end)*
 pkgAnonymousAssignElement right_brace left_brace right_brace; // 简化赋值

pkgAnonymousAssignElement: Bang? name Equal expression; // 简化赋值元素

functionExpression: left_paren parameterClauseIn (t=(Right_Arrow|Right_Flow) New_Line*
parameterClauseOut)? right_paren left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;

plusMinus: add expression;

negate: wave expression;

bitwiseNotExpression: bitwiseNot expression;

linq: linqHeadItem Right_Arrow New_Line?  (linqItem)* id New_Line? expression;

linqHeadItem: At Bang? id Equal expression;

linqItem: (linqHeadItem | id (expression)?) Right_Arrow New_Line?;

// 判断表达式
judgeExpression:
judgeIfExpression judgeElseExpression;

// else 判断
judgeElseExpression: New_Line? Or Question left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? tupleExpression right_brace;
// if 判断
judgeIfExpression: Question left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? tupleExpression right_brace;

// 基础数据
dataStatement:
floatExpr | 
integerExpr | 
rawStringExpr | 
stringExpr | 
t=CharLiteral | 
t=TrueLiteral | 
t=FalseLiteral | 
nilExpr | 
t=UndefinedLiteral;

// 字符串表达式
stringExpr: Quote_Open (stringContent | stringTemplate)* Quote_Close;
stringContent: TextLiteral;
stringTemplate: String_Template_Open (expression end)* expression Right_Brace;
// 原始字符串表达式
rawStringExpr: Quote_Quote_Quote_Open (rawStringContent | rawStringTemplate | Raw_Quote)* Quote_Quote_Quote_Close;
rawStringContent: RawTextLiteral;
rawStringTemplate: Raw_String_Template_Open New_Line* (expression end)* expression New_Line* Right_Brace;

floatExpr: FloatLiteral;
integerExpr: DecimalLiteral | BinaryLiteral | OctalLiteral | HexLiteral;

// 类型
typeNotNull:
typeAny | 
typeBasic | 
typePackage | 
typeFunction;

typeType: typeNullable | typeNotNull;

typeNullable: typeNotNull Question;

typePackage: nameSpaceItem | left_paren nameSpaceItem templateCall right_paren | nameSpaceItem templateCall;
typeFunction: left_paren typeFunctionParameterClause t=(Right_Arrow|Right_Flow) New_Line* typeFunctionParameterClause right_paren;
typeAny: TypeAny;

// 函数类型参数
typeFunctionParameterClause: typeType? (more typeType)*;

// 基础类型名
typeBasic:
t=TypeI8 | 
t=TypeU8 | 
t=TypeI16 | 
t=TypeU16 | 
t=TypeI32 | 
t=TypeU32 | 
t=TypeI64 | 
t=TypeU64 | 
t=TypeF32 | 
t=TypeF64 | 
t=TypeChr | 
t=TypeStr | 
t=TypeBool | 
t=TypeInt | 
t=TypeNum | 
t=TypeByte;

// nil值
nilExpr: NilLiteral;
// bool值
boolExpr: t=TrueLiteral|t=FalseLiteral;

// 迭代器
iterator: (Tilde expression)? (Dot_Dot|Dot_Dot_Dot|Dot_Dot_Less|Dot_Dot_Greater);
bitwise: (bitwiseAnd | bitwiseOr | bitwiseXor 
| bitwiseLeftShift | bitwiseRightShift) (New_Line)?;
bitwiseAnd: And_And_And;
bitwiseOr: Or_Or_Or;
bitwiseNot: Tilde_Tilde_Tilde;
bitwiseXor: Caret_Caret_Caret;
bitwiseLeftShift: Less_Less_Less;
bitwiseRightShift: Greater_Greater_Greater;
compare: op=(Equal_Equal | Not_Equal | Less_Equal | Greater_Equal | Less | Greater) (New_Line)?;
logic: op=(And_And | Or_Or) (New_Line)?;
assign: op=(Equal | Add_Equal | Sub_Equal | Mul_Equal | Div_Equal | Mod_Equal | Pow_Equal) (New_Line)?;
add: op=(Add | Sub) (New_Line)?;
mul: op=(Mul | Div | Mod) (New_Line)?;
pow: Caret (New_Line)?;
call: op=Dot (New_Line)?;
wave: op=Tilde_Tilde;

id: (idItem);

idItem: op=(IDPublic|IDPrivate) |
typeBasic |
typeAny ;

end: Semi | New_Line ;
more: Comma  New_Line* ;

left_brace: Left_Brace  New_Line*;
right_brace:  New_Line* Right_Brace;

left_paren: Left_Paren;
right_paren: Right_Paren;

left_brack: Left_Brack  New_Line*;
right_brack:  New_Line* Right_Brack;
