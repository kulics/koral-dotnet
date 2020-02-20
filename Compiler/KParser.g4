parser grammar KParser;

options { tokenVocab=KLexer; }

program: statement+;

statement: (New_Line)* (annotationSupport)?  
exportStatement (New_Line)* namespaceSupportStatement*;

// 导出命名空间
exportStatement: nameSpaceItem call left_brace (importStatement|typeAliasStatement|New_Line)* right_brace end;

// 导入命名空间
importStatement: (annotationSupport)? (((id|Discard) Colon) | (Coin (id|Discard) Equal))? nameSpaceItem stringExpr? end;

namespaceSupportStatement:
namespaceFunctionStatement |
namespaceVariableStatement |
namespaceConstantStatement |
packageStatement |
protocolStatement |
implementStatement |
enumStatement |
typeRedefineStatement |
typeTagStatement |
New_Line ;

// 类型别名
typeAliasStatement: ((id Colon)|(Coin id Equal)) typeType end;
// 类型重定义
typeRedefineStatement: ((id Colon)|(Coin id Equal)) New_Line* typeType end;
// 特殊类型注释
typeTagStatement: Comment_Tag; 

// 枚举
enumStatement: (annotationSupport)? ((id Colon)|(Coin id Equal)) New_Line* Dot_Dot
 left_brace enumSupportStatement* right_brace end;

enumSupportStatement: id (Colon (add)? integerExpr)? end;
// 命名空间变量
namespaceVariableStatement: (annotationSupport)? Coin id (Equal expression| typeType (Equal expression)?) end;
// 命名空间常量
namespaceConstantStatement: (annotationSupport)? id (typeType)? Colon expression end;
// 命名空间函数
namespaceFunctionStatement: (annotationSupport)? (id| left_brack id templateDefine right_brack) Colon
 left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line*
(parameterClauseOut|Discard) right_paren left_brace (functionSupportStatement)* right_brace end;

// 定义包
packageStatement: (annotationSupport)? (((id | left_brack id templateDefine right_brack) Colon) | (Coin (id | left_brack id templateDefine right_brack) Equal))
 (packageNewStatement|packageFieldStatement|packageImplementStatement|includeStatement|packageStaticStatement)
 (And (packageNewStatement|packageFieldStatement|packageImplementStatement|includeStatement|packageStaticStatement))* end;

packageStaticStatement: left_brace (packageStaticSupportStatement)* right_brace;
// 包静态语句
packageStaticSupportStatement:
packageStaticFunctionStatement |
packageStaticVariableStatement;

// 定义变量
packageStaticVariableStatement: (annotationSupport)? Coin id (Equal expression | typeType (Equal expression)?) end;
// 函数
packageStaticFunctionStatement: (annotationSupport)? (id | left_brack id templateDefine right_brack) Colon
 left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line*
(parameterClauseOut|Discard) right_paren left_brace (functionSupportStatement)* right_brace end;

packageFieldStatement: Coin (left_paren p=Question? id (more id)? right_paren)? left_brace (packageSupportStatement)* right_brace;

// 包支持的语句
packageSupportStatement:
packageFunctionStatement |
packageVariableStatement |
packageEventStatement |
overrideFunctionStatement |
overrideVariableStatement |
New_Line;

// 包含
includeStatement: typeType;
// 包构造方法
packageNewStatement: (annotationSupport)? left_paren parameterClauseIn Right_Arrow Coin p=Question? (id (more id)?)? right_paren
(left_paren expressionList? right_paren)? left_brace (functionSupportStatement)* right_brace;
// 定义变量
packageVariableStatement: (annotationSupport)? Coin id (Equal expression| typeType (Equal expression)?) end;
// 函数
packageFunctionStatement: (annotationSupport)? (id | left_brack id templateDefine right_brack) Colon
 left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line*
(parameterClauseOut|Discard) right_paren left_brace (functionSupportStatement)* right_brace end;
// 定义子方法
packageControlSubStatement: id (left_paren id right_paren)? left_brace (functionSupportStatement)+ right_brace end;
// 定义包事件
packageEventStatement: Coin id left_brack Right_Arrow right_brack nameSpaceItem end;
// 包实现接口
packageImplementStatement: Coin typeType (left_paren p=Question? id (more id)? right_paren)? left_brace (implementSupportStatement)* right_brace;

// 扩展
implementStatement: (id| left_brack id templateDefine right_brack) Add_Equal 
(packageNewStatement|packageFieldStatement|packageImplementStatement|includeStatement)
(And (packageNewStatement|packageFieldStatement|packageImplementStatement|includeStatement))* end;

// 实现支持的语句
implementSupportStatement: 
implementFunctionStatement |
implementVariableStatement |
overrideFunctionStatement |
overrideVariableStatement |
New_Line;

// 定义变量
implementVariableStatement: (annotationSupport)? Coin id (Equal expression | typeType (Equal expression)?) end;
// 函数
implementFunctionStatement: (annotationSupport)? (id | left_brack id templateDefine right_brack) Colon
left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line*
(parameterClauseOut|Discard) right_paren left_brace (functionSupportStatement)* right_brace end;

// 定义变量
overrideVariableStatement: (annotationSupport)? Dot (n='_')? Coin id (Equal expression | typeType (Equal expression)?) end;
// 函数
overrideFunctionStatement: (annotationSupport)? Dot (n='_')? (id | left_brack id templateDefine right_brack) Colon
 left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line*
(parameterClauseOut|Discard) right_paren left_brace (functionSupportStatement)* right_brace end;

// 协议
protocolStatement: (annotationSupport)? (((id | left_brack id templateDefine right_brack) Colon) | (Coin (id | left_brack id templateDefine right_brack) Equal))
(protocolSubStatement (And (protocolSubStatement|includeStatement))*|
includeStatement (And (protocolSubStatement|includeStatement))* And protocolSubStatement
 (And (protocolSubStatement|includeStatement))* ) end;

protocolSubStatement: Coin Coin (left_paren p=Question? id (more id)? right_paren)? left_brace (protocolSupportStatement)* right_brace;
// 协议支持的语句
protocolSupportStatement:
protocolFunctionStatement |
protocolVariableStatement |
New_Line ;
// 定义控制
protocolVariableStatement: (annotationSupport)? Coin? id (Equal expression | typeType (Equal expression)?) end;
// 函数
protocolFunctionStatement: (annotationSupport)? (id | left_brack id templateDefine right_brack) left_paren parameterClauseIn 
t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line* parameterClauseOut right_paren end;

// 函数
functionStatement: (id | left_brack id templateDefine right_brack) Colon left_paren parameterClauseIn
 t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line*
 (parameterClauseOut|Discard) right_paren left_brace (functionSupportStatement)* right_brace end;
// 返回
returnStatement: Left_Arrow (tupleExpression)? end;
// 异步等待返回
returnAwaitStatement: Left_Flow (tupleExpression)? end;
// 生成器
yieldReturnStatement: At Left_Arrow tupleExpression end;
yieldBreakStatement: At Left_Arrow end;
// 入参
parameterClauseIn: parameter? (more parameter)*;
// 出参
parameterClauseOut: parameter? (more parameter)*;
// 参数结构
parameter: (annotationSupport)? Coin? id (Dot_Dot|Dot_Dot_Dot)? typeType (Equal expression)?;

// 函数支持的语句
functionSupportStatement:
returnStatement |
returnAwaitStatement |
yieldReturnStatement |
yieldBreakStatement |
judgeCaseStatement |
judgeStatement |
loopStatement |
loopEachStatement |
loopCaseStatement |
loopJumpStatement |
loopContinueStatement |
usingStatement |
checkStatement |
checkReportStatement |
functionStatement |
variableStatement |
variableDeclaredStatement |
channelAssignStatement |
assignStatement |
expressionStatement |
New_Line;

// 条件判断
judgeCaseStatement: Question expression Dot_Dot (caseStatement)+ end;
// 判断条件声明
caseStatement: caseExprStatement (more caseExprStatement)* left_brace (functionSupportStatement)* right_brace;
caseExprStatement: Discard | expression | (id|Discard) typeType;
// 判断
judgeStatement:
judgeIfStatement (judgeElseIfStatement)* judgeElseStatement end
| judgeIfStatement (judgeElseIfStatement)* end;
// else 判断
judgeElseStatement: Discard left_brace (functionSupportStatement)* right_brace;
// if 判断
judgeIfStatement: Question expression left_brace (functionSupportStatement)* right_brace;
// else if 判断
judgeElseIfStatement: expression left_brace (functionSupportStatement)* right_brace;
// 循环
loopStatement: At ((id Colon)|(Coin id Equal)) iteratorStatement Dot_Dot left_brace (functionSupportStatement)* right_brace loopElseStatement? end;
// 集合循环
loopEachStatement: At (((left_brack id right_brack)? id Colon)|(Coin (left_brack id right_brack)? id Equal)) expression Dot_Dot
 left_brace (functionSupportStatement)* right_brace loopElseStatement? end;
// 条件循环
loopCaseStatement: At expression left_brace (functionSupportStatement)* right_brace loopElseStatement? end;
// else 判断
loopElseStatement: Discard left_brace (functionSupportStatement)* right_brace;
// 跳出循环
loopJumpStatement: Tilde At end;
// 跳过当前循环
loopContinueStatement: Dot_Dot At end;
// 检查
checkStatement: 
Bang left_brace (functionSupportStatement)* right_brace (checkErrorStatement)* checkFinallyStatment end
|Bang left_brace (functionSupportStatement)* right_brace (checkErrorStatement)+ end;
// 定义检查变量
usingStatement: Bang ((expression (typeType)? Colon) | (Coin expression (typeType)? Equal)) expression end;
// 错误处理
checkErrorStatement: (id|id typeType) left_brace (functionSupportStatement)* right_brace;
// 最终执行
checkFinallyStatment: Discard left_brace (functionSupportStatement)* right_brace;
// 抛出异常
checkReportStatement: Bang Left_Arrow expression end;

// 迭代器
iteratorStatement: expression (Tilde|Tilde_Tilde) (left_paren expression right_paren)? expression;

// 定义变量
variableStatement: (Coin idExpression typeType? Equal expression | idExpression typeType? Colon expression) end;
// 声明变量
variableDeclaredStatement: Coin? idExpression typeType end;
// 通道赋值
channelAssignStatement: expression left_brack Dot right_brack assign expression end;
// 赋值
assignStatement: tupleExpression assign tupleExpression end;

expressionStatement: expression end;

idExpression: idExprItem (more idExprItem)*;
idExprItem: id | Discard;

tupleExpression: expression (more expression)* ; // 元组
// 基础表达式
primaryExpression: 
left_brack id templateCall right_brack |
id |
t=Discard |
left_paren expression right_paren | 
dataStatement;

// 表达式
expression:
linq | // 联合查询
primaryExpression | 
callPkg | // 新建包 
callAwait | // 异步等待调用
callAsync | // 创建异步调用
list | // 列表
set | // 集合
dictionary | // 字典
lambda | // lambda表达式
functionExpression | // 函数
pkgAnonymous | // 匿名包
plusMinus | // 正负处理
bitwiseNotExpression | // 位运算取反
negate | // 取反
judgeExpression | // 判断表达式
judgeCaseExpression | // 条件判断表达式
loopExpression | // 循环表达式
loopEachExpression | // 集合循环表达式
checkExpression | // 检查表达式
expression op=Coin | // 取引用
expression op=Question | // 可空判断
expression orElse | // 空值替换
expression callFunc | // 函数调用
callNew | // 构造类对象
expression callChannel | // 调用通道
expression callElement | // 访问元素
expression callExpression | // 链式调用
expression pow expression | // 幂型表达式
expression mul expression | // 积型表达式
expression add expression | // 和型表达式
expression bitwise expression | // 位运算表达式
expression typeConversion | // 类型转换
expression typeCheck | // 类型判断
expression compareCombine expression | // 组合比较表达式
expression compare expression | // 比较表达式
expression logic expression; // 逻辑表达式

callExpression: call New_Line? (id | left_brack id templateCall right_brack) (callFunc|callChannel|callElement)?;

tuple: left_paren (expression (more expression)* )? right_paren; // 元组

expressionList: expression (more expression)* ; // 表达式列

annotationSupport: annotation (New_Line)?;

annotation: Less (id Right_Arrow)? annotationList Greater; // 注解

annotationList: annotationItem (more annotationItem)*;

annotationItem: id (tuple|lambda)?;

callFunc: (tuple|lambda); // 函数调用

callChannel: left_brack Dot right_brack; // 通道调用

callElement: left_brack (slice | expression) right_brack; // 元素调用

callPkg: typeType left_brace (pkgAssign|listAssign|setAssign|dictionaryAssign)? right_brace; // 新建包

callNew: typeType left_paren New_Line? expressionList? New_Line? right_paren; // 构造类对象

orElse: Question Question expression; // 类型转化

typeConversion: typeType Bang; // 类型转化

typeCheck: typeType Question; // 类型转化

pkgAssign: (pkgAssignElement end)* pkgAssignElement; // 简化赋值

pkgAssignElement: name Equal expression; // 简化赋值元素

listAssign: (expression end)* expression;

setAssign: (setElement end)* setElement;

dictionaryAssign: (dictionaryElement end)* dictionaryElement;

callAwait: Less_Less expression; // 异步等待调用
callAsync: Greater_Greater expression; // 创建异步调用

list: left_brace (expression end)* expression right_brace; // 列表

set: left_brace (setElement end)* setElement right_brace; // 无序集合

setElement: left_brack expression right_brack Discard; // 无序集合元素

dictionary: left_brace (dictionaryElement end)* dictionaryElement right_brace; // 字典

dictionaryElement: left_brack expression right_brack expression; // 字典元素

slice: sliceStart | sliceEnd | sliceFull;

sliceFull: expression (Tilde|Tilde_Tilde) expression; 
sliceStart: expression (Tilde|Tilde_Tilde) Discard;
sliceEnd: Discard (Tilde|Tilde_Tilde) expression; 

nameSpaceItem: (id call New_Line?)* id;

name: id (call New_Line? id)* ;

templateDefine: templateDefineItem (more templateDefineItem)*;

templateDefineItem: id | left_paren id id right_paren; 

templateCall: typeType (more typeType)*;

lambda: left_brace (lambdaIn)? t=(Right_Arrow|Right_Flow) New_Line* tupleExpression right_brace
| left_brace (lambdaIn)? t=(Right_Arrow|Right_Flow) New_Line* 
(functionSupportStatement)* right_brace;

lambdaIn: id (more id)*;

pkgAnonymous: pkgAnonymousAssign; // 匿名包

pkgAnonymousAssign: left_brace (pkgAnonymousAssignElement end)* pkgAnonymousAssignElement right_brace; // 简化赋值

pkgAnonymousAssignElement: ((Coin name t=Equal)|(name t=Colon)) expression; // 简化赋值元素

functionExpression: left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line*
parameterClauseOut right_paren left_brace (functionSupportStatement)* right_brace;

plusMinus: add expression;

negate: wave expression;

bitwiseNotExpression: bitwiseNot expression;

linq: linqHeadItem Right_Arrow New_Line?  (linqItem)* id New_Line? expression;

linqHeadItem: At ((id Colon)|(Coin id Equal)) expression Dot_Dot;

linqItem: (linqHeadItem | id (expression)?) Right_Arrow New_Line?;

// 判断表达式
judgeExpression: judgeExpressionIfStatement (judgeExpressionElseIfStatement)* judgeExpressionElseStatement;

// else 判断
judgeExpressionElseStatement: Discard left_brace (functionSupportStatement)* tupleExpression right_brace;
// if 判断
judgeExpressionIfStatement: Question Right_Arrow expression left_brace (functionSupportStatement)* tupleExpression right_brace;
// else if 判断
judgeExpressionElseIfStatement: expression left_brace (functionSupportStatement)* tupleExpression right_brace;

// 条件判断表达式
judgeCaseExpression: Question expression Dot_Dot Right_Arrow (caseExpressionStatement)+;
// 判断条件声明
caseExpressionStatement: caseExprStatement (more caseExprStatement)* 
left_brace (functionSupportStatement)* tupleExpression right_brace;
// 循环
loopExpression: At ((id Colon)|(Coin id Equal)) iteratorStatement Dot_Dot Right_Arrow 
left_brace (functionSupportStatement)* tupleExpression right_brace loopElseExpression?;
// 集合循环表达式
loopEachExpression: At (id Colon)? ((id Colon)|(Coin id Equal)) expression Dot_Dot Right_Arrow 
left_brace (functionSupportStatement)* tupleExpression right_brace loopElseExpression?;
// else 判断
loopElseExpression: Discard left_brace (functionSupportStatement)* tupleExpression right_brace;
// 检查
checkExpression: 
Bang Right_Arrow left_brace (functionSupportStatement)* tupleExpression right_brace (checkErrorExpression)* checkFinallyStatment |
Bang Right_Arrow left_brace (functionSupportStatement)* tupleExpression right_brace (checkErrorExpression)+ ;
// 错误处理
checkErrorExpression: (id|id typeType) left_brace (functionSupportStatement)* tupleExpression right_brace;
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
typeArray | 
typeList | 
typeSet | 
typeDictionary | 
typeStack | 
typeQueue | 
typeChannel | 
typeBasic | 
typePackage | 
typeFunction;

typeType: typeNotNull | typeNullable;

typeNullable: Question typeNotNull;

typeArray: left_brack Dot right_brack typeType;
typeList: left_brack right_brack typeType;
typeSet: left_brack typeType right_brack Discard;
typeDictionary: left_brack typeType right_brack typeType;
typeStack: left_brack Greater right_brack typeType;
typeQueue: left_brack Less right_brack typeType;
typeChannel: left_brack Less_Less right_brack typeType;
typePackage: nameSpaceItem | left_brack nameSpaceItem templateCall right_brack;
typeFunction: left_paren typeFunctionParameterClause t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line* typeFunctionParameterClause right_paren;
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

bitwise: (bitwiseAnd | bitwiseOr | bitwiseXor 
| bitwiseLeftShift | bitwiseRightShift) (New_Line)?;
bitwiseAnd: And_And;
bitwiseOr: Or_Or;
bitwiseNot: Tilde_Tilde;
bitwiseXor: Caret_Caret;
bitwiseLeftShift: Less_Less;
bitwiseRightShift: Greater_Greater;
compareCombine: Combine_Equal;
compare: op=(Equal_Equal | Not_Equal | Less_Equal | Greater_Equal | Less | Greater) (New_Line)?;
logic: op=(And | Or) (New_Line)?;
assign: op=(Equal | Add_Equal | Sub_Equal | Mul_Equal | Div_Equal | Mod_Equal | Pow_Equal) (New_Line)?;
add: op=(Add | Sub) (New_Line)?;
mul: op=(Mul | Div | Mod) (New_Line)?;
pow: Caret (New_Line)?;
call: op=Dot (New_Line)?;
wave: op=Tilde;

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
