parser grammar FeelParser;

options { tokenVocab=FeelLexer; }

program: statement+;

statement: New_Line* (annotationSupport)?  
exportStatement end? New_Line* (namespaceSupportStatement end|New_Line)* (namespaceSupportStatement end?)? New_Line* ;

// 导出命名空间
exportStatement: Sharp nameSpaceItem;

namespaceSupportStatement:
importStatement |
protocolStatement |
packageStatement |
implementStatement |
namespaceFunctionStatement |
namespaceVariableStatement |
enumStatement |
typeRedefineStatement |
New_Line ;

// 导入命名空间
importStatement: Sharp left_brace ((importSubStatement | typeAliasStatement) end|New_Line)*
((importSubStatement | typeAliasStatement) end?)? right_brace;

importSubStatement: (annotationSupport)? ((id|Dot) Equal)?
 (nameSpaceItem stringExpr? | nameSpaceItem? stringExpr);

// 类型别名
typeAliasStatement: id Equal typeType;
// 类型重定义
typeRedefineStatement: id Equal New_Line* typeType;

// 枚举
enumStatement: (annotationSupport)? id Equal New_Line* Coin
left_brack enumSupportStatement (more enumSupportStatement)+ right_brack New_Line? left_brace right_brace;

enumSupportStatement: id (Equal (add)? integerExpr)?;
// 命名空间变量
namespaceVariableStatement: (annotationSupport)? id (Equal expression | Colon typeType (Equal expression)?);
// 命名空间函数
namespaceFunctionStatement: (annotationSupport)? id Equal (templateDefine New_Line?)?
left_paren parameterClauseIn (t=(Right_Arrow|Right_Flow) New_Line* parameterClauseOut)? right_paren
left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;

// 定义包
packageStatement: (annotationSupport)? id Equal (templateDefine New_Line?)?
 (packageFieldStatement|packageNewStatement);

packageFieldStatement: Coin left_paren New_Line? parameterConstruct New_Line? right_paren 
(Right_Arrow left_paren id (more id)? right_paren)? left_brace ((packageSupportStatement end|New_Line)* packageSupportStatement end?)? right_brace;

// 包支持的语句
packageSupportStatement:
includeStatement |
packageFunctionStatement |
overrideFunctionStatement |
New_Line;

// 包含
includeStatement: typeType;
// 包构造方法
packageNewStatement: (annotationSupport)? left_paren parameterClauseIn Right_Arrow Coin (id (more id)?)? right_paren
(left_paren expressionList? right_paren)? left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// 函数
packageFunctionStatement: (annotationSupport)? id Equal (templateDefine New_Line?)?
left_paren parameterClauseIn (t=(Right_Arrow|Right_Flow) New_Line* parameterClauseOut)? right_paren
left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;

// 扩展
implementStatement: id Colon Equal (templateDefine New_Line?)?
(packageNewStatement|packageFieldStatement);

// 函数
overrideFunctionStatement: (annotationSupport)? Dot (n='_')? id Equal (templateDefine New_Line?)?
left_paren parameterClauseIn (t=(Right_Arrow|Right_Flow) New_Line* parameterClauseOut)? right_paren
left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;

// 协议
protocolStatement: (annotationSupport)? id Equal (templateDefine New_Line?)? protocolSubStatement;

protocolSubStatement: Coin left_brace (protocolSupportStatement end|New_Line)* protocolSupportStatement end? right_brace;
// 协议支持的语句
protocolSupportStatement:
includeStatement |
protocolFunctionStatement |
New_Line ;

// 函数
protocolFunctionStatement: (annotationSupport)? id Colon (templateDefine New_Line?)? left_paren parameterClauseIn 
t=(Right_Arrow|Right_Flow) New_Line* parameterClauseOut right_paren;

// 函数
functionStatement: id Equal (templateDefine New_Line?)? left_paren parameterClauseIn
(t=(Right_Arrow|Right_Flow) New_Line* parameterClauseOut)? right_paren
left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// 返回
returnStatement: Left_Arrow (tupleExpression)?;
// 异步返回
returnAsyncStatement: Left_Flow (tupleExpression)?;
// 入参
parameterClauseIn: parameter? (more parameter)*;
// 出参
parameterClauseOut: parameter? (more parameter)*;
// 构造
parameterConstruct: parameter? (more parameter)*;
// 参数结构
parameter: (annotationSupport)? id Colon Dot_Dot_Dot? Bang? typeType;

// 函数支持的语句
functionSupportStatement:
returnStatement |
returnAsyncStatement |
judgeStatement |
judgeMatchStatement |
loopStatement |
loopCaseStatement |
loopJumpStatement |
loopContinueStatement |
usingStatement |
checkStatement |
checkReportStatement |
functionStatement |
variableDeclaredStatement |
bindStatement |
bindTypeStatement |
assignStatement |
expressionStatement ;

// 条件判断
judgeMatchStatement:
Question expression (caseEqualStatement|caseTypeStatement) 
(Or (caseEqualStatement|caseTypeStatement))* caseElseStatement?;

// 判断条件声明
caseElseStatement: New_Line? Or
 left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
caseEqualStatement: New_Line? Equal_Equal judgeEqualCase (more New_Line? judgeEqualCase)* 
left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;

judgeEqualCase: expression;

caseTypeStatement: New_Line? Colon_Colon judgeTypeCase (more New_Line? judgeTypeCase)* 
left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;

judgeTypeCase: typeType (Equal_Arrow id)?;

// 判断
judgeStatement:
judgeIfStatement judgeElseIfStatement* judgeElseStatement?;
// else if 判断
judgeElseIfStatement: New_Line? Or expression
left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// else 判断
judgeElseStatement: New_Line? Or
left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// if 判断
judgeIfStatement: Question expression
left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;

// 循环
loopStatement: At loopId (more loopId)* Equal expression Dot_Dot_Dot
left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
loopId: id (Colon typeType)?;
// 条件循环
loopCaseStatement: At expression 
left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace loopElseStatement?;
// else 判断
loopElseStatement: New_Line? And
left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// 跳出循环
loopJumpStatement: At Left_Arrow ;
// 跳过当前循环
loopContinueStatement: At ;
// 检查
checkStatement: 
Bang left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace (checkErrorStatement)* checkFinallyStatment 
| Bang left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace (checkErrorStatement)+ ;
// 定义检查变量
usingStatement: Bang id (more id)* Equal tupleExpression And varId (more varId)*
left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// 错误处理
checkErrorStatement: New_Line? And (id | id Colon typeType) left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// 最终执行
checkFinallyStatment: New_Line? And left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;
// 抛出异常
checkReportStatement: Bang Left_Arrow expression ;

// 声明变量
variableDeclaredStatement: id Colon typeType ;
// 绑定
bindStatement: varId (more varId)* Equal tupleExpression ;
bindTypeStatement: varIdType (more varIdType)* Equal tupleExpression ;
// 复合赋值
assignStatement: tupleExpression assign tupleExpression ;
// 表达式
expressionStatement: expression ;

varId: id | Discard;
varIdType: id Colon typeType | Discard;

tupleExpression: expression (more expression)* ; // 元组
// 基础表达式
primaryExpression: 
id left_brack templateCall right_brack |
id |
t=Discard |
left_paren expression right_paren | 
dataStatement;

// 表达式
expression:
primaryExpression 
| callPkg // 新建包 
| callChannel // 通道访问
| callAsync // 创建异步调用
| lambda // lambda表达式
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
| expression typeConversion // 类型转换
| expression typeCheck // 类型判断
| expression compare expression // 比较表达式
| expression logic expression // 逻辑表达式
; 

callExpression: call New_Line? id (left_brack templateCall right_brack)? (callFunc|callElement)?;

tuple: left_paren (expression (more expression)* )? right_paren; // 元组

expressionList: expression (more expression)* ; // 表达式列

annotationSupport: annotation New_Line?;

annotation: Back_Quote annotationList Back_Quote; // 注解

annotationList: (annotationItem|annotationString) (more annotationItem)*;

annotationItem: (id left_brace id (tuple)? right_brace | id (tuple)?);

annotationString: stringExpr|rawStringExpr;

callFunc: tuple; // 函数调用

callAsync: Right_Wave expression; // 异步等待调用

callAwait: Right_Wave tuple; // 异步等待调用

callChannel: Left_Wave expression; // 通道访问

transfer: Left_Wave; // 传递通道值

callElement: Dot left_paren (slice | expression) right_paren; // 元素调用

callPkg: typeNotNull? Coin tuple; // 类型构造

orElse: Question Or expression; // 可空取值

typeConversion: Dot left_brack typeType right_brack; // 类型转化

typeCheck: Colon_Colon typeType; // 类型转化

slice: sliceStart | sliceEnd | sliceFull;

sliceFull: expression Dot_Dot expression; 
sliceStart: expression Dot_Dot;
sliceEnd: Dot_Dot expression; 

nameSpaceItem: (id call New_Line?)* id;

name: id (call New_Line? id)* ;

templateDefine: left_brack templateDefineItem (more templateDefineItem)* right_brack;

templateDefineItem: id (Colon id)?; 

templateCall: typeType (more typeType)*;

lambda: left_paren (lambdaIn)? (t=(Right_Arrow|Right_Flow) parameterClauseOut?)? right_paren 
 left_brace tupleExpression right_brace
| left_paren (lambdaIn)? (t=(Right_Arrow|Right_Flow) parameterClauseOut?)? right_paren
left_brace (functionSupportStatement end|New_Line)* (functionSupportStatement end?)? right_brace;

lambdaIn: id (Colon typeType)? (more id (Colon typeType)?)*;

plusMinus: add expression;

negate: wave expression;

bitwiseNotExpression: bitwiseNot expression;

// 基础数据
dataStatement:
floatExpr | 
integerExpr | 
rawStringExpr | 
stringExpr | 
t=CharLiteral | 
t=TrueLiteral | 
t=FalseLiteral | 
nilExpr;

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
typePackage |
typeFunction;

typeType: typeNullable | typeNotNull;

typeNullable: Question typeNotNull;

typePackage: nameSpaceItem (left_brack templateCall right_brack)?;
typeFunction: left_paren typeFunctionParameterClause t=(Right_Arrow|Right_Flow) New_Line* typeFunctionParameterClause right_paren;
typeAny: TypeAny;

// 函数类型参数
typeFunctionParameterClause: typeType? (more typeType)*;

// nil值
nilExpr: NilLiteral;
// bool值
boolExpr: t=TrueLiteral|t=FalseLiteral;

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

idItem: Identifier |
typeAny ;

end: Semi | New_Line ;
more: Comma  New_Line* ;

left_brace: Left_Brace  New_Line*;
right_brace:  New_Line* Right_Brace;

left_paren: Left_Paren;
right_paren: Right_Paren;

left_brack: Left_Brack  New_Line*;
right_brack:  New_Line* Right_Brack;
