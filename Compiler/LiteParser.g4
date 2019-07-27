parser grammar LiteParser;

options { tokenVocab=LiteLexer; }

program: statement+;

statement: (New_Line)* (annotationSupport)?  
exportStatement (New_Line)* namespaceSupportStatement*;

// 导出命名空间
exportStatement: TextLiteral left_brace (importStatement|New_Line)* right_brace end;

// 导入命名空间
importStatement: (annotationSupport)? (id call?)? TextLiteral end;

namespaceSupportStatement:
namespaceVariableStatement
|namespaceControlStatement
|namespaceFunctionStatement
|namespaceConstantStatement
|packageStatement
|protocolStatement
|implementStatement
|overrideStatement
|packageNewStatement
|enumStatement
|typeAliasStatement
|typeRedefineStatement
|New_Line
;

// 类型别名
typeAliasStatement: id Equal_Arrow typeType end;
// 类型重定义
typeRedefineStatement: id Right_Arrow typeType end;

// 枚举
enumStatement: (annotationSupport)? id Right_Arrow New_Line* typeType left_brack enumSupportStatement* right_brack end;

enumSupportStatement: id (Equal (add)? integerExpr)? end;
// 命名空间变量
namespaceVariableStatement: (annotationSupport)? id (Equal expression| typeType (Equal expression)?) end;
// 命名空间控制
namespaceControlStatement: (annotationSupport)? id left_paren expression? right_paren typeType
(left_brace (packageControlSubStatement)+ right_brace)? end;
// 命名空间常量
namespaceConstantStatement: (annotationSupport)? id (typeType)? Colon expression end;
// 命名空间函数
namespaceFunctionStatement: (annotationSupport)? id (templateDefine)? left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) New_Line*
parameterClauseOut right_paren left_brace (functionSupportStatement)* right_brace end;

// 定义包
packageStatement: (annotationSupport)? id (templateDefine)? Right_Arrow left_brace (packageSupportStatement)* right_brace end;

// 包支持的语句
packageSupportStatement:
includeStatement
|packageVariableStatement
|packageEventStatement
|New_Line
;

// 包含
includeStatement: Discard typeType end;
// 包构造方法
packageNewStatement: (annotationSupport)? parameterClauseSelf left_paren parameterClauseIn right_paren
(left_paren expressionList? right_paren)? left_brace (functionSupportStatement)* right_brace;
// 定义变量
packageVariableStatement: (annotationSupport)? id (Equal expression| typeType (Equal expression)?) end;

// 定义子方法
packageControlSubStatement: id (left_paren id right_paren)? left_brace (functionSupportStatement)+ right_brace end;
// 定义包事件
packageEventStatement: id left_brack Question right_brack nameSpaceItem end;

// 实现
implementStatement: parameterClauseSelf Right_Arrow (typeType)? New_Line* left_brace (implementSupportStatement)* right_brace end;

// 实现支持的语句
implementSupportStatement: implementFunctionStatement | implementControlStatement | New_Line;

// 函数
implementFunctionStatement: (annotationSupport)? id (templateDefine)? left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) New_Line*
parameterClauseOut right_paren left_brace (functionSupportStatement)* right_brace end;
// 定义控制
implementControlStatement: (annotationSupport)? id left_paren expression? right_paren 
 typeType (left_brace (packageControlSubStatement)+ right_brace)? end;

// 重载
overrideStatement: left_paren id right_paren parameterClauseSelf 
 Right_Arrow New_Line* left_brace (overrideSupportStatement)* right_brace end;

// 实现支持的语句
overrideSupportStatement: overrideFunctionStatement | overrideControlStatement | New_Line;

// 函数
overrideFunctionStatement: (annotationSupport)? (n='_')? id (templateDefine)? left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) New_Line*
parameterClauseOut right_paren left_brace (functionSupportStatement)* right_brace end;
// 定义控制
overrideControlStatement: (annotationSupport)? (n='_')? id left_paren expression? right_paren
 typeType (left_brace (packageControlSubStatement)+ right_brace)? end;

// 协议
protocolStatement: (annotationSupport)? id (templateDefine)? Left_Arrow left_brace (protocolSupportStatement)* right_brace end;
// 协议支持的语句
protocolSupportStatement:
includeStatement
|protocolFunctionStatement
|protocolControlStatement
|New_Line
;
// 定义控制
protocolControlStatement: (annotationSupport)? id left_paren right_paren typeType
 (left_brace (protocolControlSubStatement)* right_brace)? end;
// 定义子方法
protocolControlSubStatement: id;
// 函数
protocolFunctionStatement: (annotationSupport)? id (templateDefine)? left_paren parameterClauseIn 
t=(Right_Arrow|Right_Flow) New_Line* parameterClauseOut right_paren end;

// 函数
functionStatement: id (templateDefine)? left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) New_Line*
 parameterClauseOut right_paren left_brace (functionSupportStatement)* right_brace end;
// 返回
returnStatement: Left_Arrow (tupleExpression)? end;
// 生成器
yieldReturnStatement: At Left_Arrow tupleExpression end;
yieldBreakStatement: At Left_Arrow end;
// 入参
parameterClauseIn: parameter? (more parameter)*;
// 出参
parameterClauseOut: parameter? (more parameter)*;
// 接收器
parameterClauseSelf: id typeType;
// 参数结构
parameter: (annotationSupport)? id typeType (Equal expression)?;

// 函数支持的语句
functionSupportStatement:
 returnStatement
| yieldReturnStatement
| yieldBreakStatement
| judgeCaseStatement
| judgeStatement
| loopStatement
| loopEachStatement
| loopCaseStatement
| loopInfiniteStatement
| loopJumpStatement
| loopContinueStatement
| usingStatement
| checkStatement
| functionStatement
| variableStatement
| variableDeclaredStatement
| channelAssignStatement
| assignStatement
| expressionStatement
| New_Line
;

// 条件判断
judgeCaseStatement: expression Question (caseStatement)+ end;
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
loopStatement: id At iteratorStatement left_brace (functionSupportStatement)* right_brace end;
// 集合循环
loopEachStatement: (Left_Brack id Right_Brack)? id At expression left_brace (functionSupportStatement)* right_brace end;
// 条件循环
loopCaseStatement: At expression left_brace (functionSupportStatement)* right_brace end;
// 无限循环
loopInfiniteStatement: At left_brace (functionSupportStatement)* right_brace end;
// 跳出循环
loopJumpStatement: At Dot_Dot end;
// 跳出当前循环
loopContinueStatement: Dot_Dot At end;
// 检查
checkStatement: 
Bang left_brace (functionSupportStatement)* right_brace (checkErrorStatement)* checkFinallyStatment end
|Bang left_brace (functionSupportStatement)* right_brace (checkErrorStatement)+ end;
// 定义检查变量
usingStatement: Bang expression (typeType)? Equal expression end;
// 错误处理
checkErrorStatement: (id|id typeType) left_brace (functionSupportStatement)* right_brace;
// 最终执行
checkFinallyStatment: Discard left_brace (functionSupportStatement)* right_brace;

// 迭代器
iteratorStatement: expression Dot_Dot op=(Less|Less_Equal|Greater|Greater_Equal) expression
 Colon expression | expression Dot_Dot op=(Less|Less_Equal|Greater|Greater_Equal) expression;

// 定义变量
variableStatement: idExpression typeType? Equal expression end;
// 声明变量
variableDeclaredStatement: idExpression typeType end;
// 通道赋值
channelAssignStatement: expression Left_Brack Left_Arrow Right_Brack assign expression end;
// 赋值
assignStatement: tupleExpression assign tupleExpression end;

expressionStatement: expression end;

idExpression: idExprItem (more idExprItem)*;
idExprItem: id | Discard;

tupleExpression: expression (more expression)* ; // 元组
// 基础表达式
primaryExpression: 
id (templateCall)?
| t=Discard
| left_paren expression right_paren
| dataStatement
;

// 表达式
expression:
linq // 联合查询
| primaryExpression
| callNew // 构造类对象
| callPkg // 新建包
| callAwait // 异步等待调用
| list // 列表
| set // 集合
| dictionary // 字典
| lambda // lambda表达式
| functionExpression // 函数
| pkgAnonymous // 匿名包
| plusMinus // 正负处理
| bitwiseNotExpression // 位运算取反
| negate // 取反
| expression op=Bang // 引用判断
| expression op=Question // 可空判断
| expression op=Left_Flow // 异步执行
| expression typeConversion // 类型转换
| expression callFunc // 函数调用
| expression callChannel // 调用通道
| expression callElement // 访问元素
| expression callExpression // 链式调用
| expression bitwise expression // 位运算表达式
| expression judge expression // 判断型表达式
| expression add expression // 和型表达式
| expression mul expression // 积型表达式
| expression pow expression // 幂型表达式
| stringExpression // 字符串插值
;

callExpression: call New_Line? id (templateCall)? (callFunc|callChannel|callElement)?;

tuple: left_paren (expression (more expression)* )? right_paren; // 元组

expressionList: expression (more expression)* ; // 表达式列

annotationSupport: annotation (New_Line)?;

annotation: Left_Brack (id Right_Arrow)? annotationList Right_Brack; // 注解

annotationList: annotationItem (more annotationItem)*;

annotationItem: id ( left_paren annotationAssign (more annotationAssign)* right_paren)? ;

annotationAssign: (id Equal)? expression ;

callFunc: (tuple|lambda); // 函数调用

callChannel: Left_Brack Left_Arrow Right_Brack; // 通道调用

callElement: Left_Brack (slice | expression) Right_Brack; // 元素调用

callPkg: typeType left_brace (pkgAssign|listAssign|setAssign|dictionaryAssign)? right_brace; // 新建包

callNew: Less typeType Greater left_paren New_Line? expressionList? New_Line? right_paren; // 构造类对象

typeConversion: Dot left_paren typeType right_paren; // 类型转化

pkgAssign: pkgAssignElement (more pkgAssignElement)* ; // 简化赋值

pkgAssignElement: name Equal expression; // 简化赋值元素

listAssign: expression (more expression)* ;

setAssign: Left_Brack expression Right_Brack (more Left_Brack expression Right_Brack)* ;

dictionaryAssign: dictionaryElement (more dictionaryElement)* ;

callAwait: Left_Flow expression; // 异步调用

list: left_brace expression (more expression)* right_brace; // 列表

set: left_brace Left_Brack expression Right_Brack (more Left_Brack expression Right_Brack)* right_brace; // 无序集合

dictionary:  left_brace dictionaryElement (more dictionaryElement)* right_brace; // 字典

dictionaryElement: Left_Brack expression Right_Brack expression; // 字典元素

slice: sliceFull | sliceStart | sliceEnd;

sliceFull: expression Dot_Dot op=(Less|Less_Equal|Greater|Greater_Equal) expression; 
sliceStart: expression Dot_Dot op=(Less|Less_Equal|Greater|Greater_Equal);
sliceEnd: Dot_Dot op=(Less|Less_Equal|Greater|Greater_Equal) expression; 

nameSpaceItem: (id call New_Line?)* id;

name: id (call New_Line? id)* ;

templateDefine: Less templateDefineItem (more templateDefineItem)* Greater;

templateDefineItem: id (id)?; 

templateCall: Less typeType (more typeType)* Greater;

lambda: left_brace (lambdaIn)? t=(Right_Arrow|Right_Flow) New_Line* tupleExpression right_brace
| left_brace (lambdaIn)? t=(Right_Arrow|Right_Flow) New_Line* 
(functionSupportStatement)* right_brace;

lambdaIn: id (more id)*;

pkgAnonymous: pkgAnonymousAssign; // 匿名包

pkgAnonymousAssign: left_brace pkgAnonymousAssignElement (more pkgAnonymousAssignElement)* right_brace; // 简化赋值

pkgAnonymousAssignElement: name Equal expression; // 简化赋值元素

functionExpression: left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) New_Line*
parameterClauseOut right_paren left_brace (functionSupportStatement)* right_brace;

plusMinus: add expression;

negate: wave expression;

bitwiseNotExpression: bitwiseNot expression;

linq: linqHeadKeyword New_Line? expression Right_Arrow New_Line?  (linqItem)+ k=(LinqSelect|LinqBy) New_Line? expression;

linqItem: linqKeyword (expression)? Right_Arrow New_Line?;

linqKeyword: linqHeadKeyword | linqBodyKeyword ;
linqHeadKeyword: k=LinqFrom;
linqBodyKeyword: k=(LinqSelect|LinqBy|LinqWhere|LinqGroup|LinqInto|LinqOrderby|LinqJoin|LinqLet|LinqIn|LinqOn|LinqEquals|LinqAscending|LinqDescending);

stringExpression: TextLiteral (stringExpressionElement)+;

stringExpressionElement: expression TextLiteral;

// 基础数据
dataStatement:
floatExpr
| integerExpr
| t=TextLiteral
| t=CharLiteral
| t=TrueLiteral
| t=FalseLiteral
| nilExpr
| t=UndefinedLiteral
;

floatExpr: integerExpr call integerExpr;
integerExpr: NumberLiteral;

// 类型
typeNotNull:
typeAny
| typeTuple
| typeArray
| typeList
| typeSet
| typeDictionary
| typeChannel
| typeStack
| typeBasic
| typePackage
| typeFunction
;

typeType: typeNotNull | typeNullable | typeReference;

typeReference: Bang (typeNotNull | typeNullable);
typeNullable: Question typeNotNull;

typeTuple: Less typeType (more typeType)+ Greater;
typeArray: Left_Brack Colon Right_Brack typeType;
typeList: Left_Brack Right_Brack typeType;
typeSet: Left_Brack typeType Right_Brack;
typeDictionary: Left_Brack typeType Right_Brack typeType;
typeChannel: Left_Brack Right_Arrow Right_Brack typeType;
typeStack: Left_Brack Xor Right_Brack typeType;
typePackage: nameSpaceItem (templateCall)? ;
typeFunction: left_paren typeFunctionParameterClause t=(Right_Arrow|Right_Flow) New_Line* typeFunctionParameterClause right_paren;
typeAny: TypeAny;

// 函数类型参数
typeFunctionParameterClause: typeType? (more typeType)*;

// 基础类型名
typeBasic:
t=TypeI8
| t=TypeU8
| t=TypeI16
| t=TypeU16
| t=TypeI32
| t=TypeU32
| t=TypeI64
| t=TypeU64
| t=TypeF32
| t=TypeF64
| t=TypeChr
| t=TypeStr
| t=TypeBool
| t=TypeInt
| t=TypeNum
| t=TypeByte
;
// nil值
nilExpr: NilLiteral;
// bool值
boolExpr: t=TrueLiteral|t=FalseLiteral;

judgeType: op=(Equal_Equal|Not_Equal);
bitwise: (bitwiseAnd | bitwiseOr | bitwiseXor 
| bitwiseLeftShift | bitwiseRightShift) (New_Line)?;
bitwiseAnd: And And;
bitwiseOr: Or Or;
bitwiseNot: Wave Wave;
bitwiseXor: Xor Xor;
bitwiseLeftShift: Less Less;
bitwiseRightShift: Greater Greater;
judge: op=(Or | And | Equal_Equal | Not_Equal | Less_Equal | Greater_Equal | Less | Greater) (New_Line)?;
assign: op=(Equal | Add_Equal | Sub_Equal | Mul_Equal | Div_Equal | Mod_Equal) (New_Line)?;
add: op=(Add | Sub) (New_Line)?;
mul: op=(Mul | Div | Mod) (New_Line)?;
pow: op=(Pow | Root | Log) (New_Line)?;
call: op=Dot (New_Line)?;
wave: op=Wave;

id: (idItem);

idItem: op=(IDPublic|IDPrivate)
|typeBasic
|typeAny
|linqKeyword
;

end: Semi | New_Line ;
more: Comma  New_Line* ;

left_brace: Left_Brace  New_Line*;
right_brace:  New_Line* Right_Brace;

left_paren: Left_Paren;
right_paren: Right_Paren;

left_brack: Left_Brack  New_Line*;
right_brack:  New_Line* Right_Brack;
