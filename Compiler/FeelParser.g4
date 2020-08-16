parser grammar FeelParser;

options { tokenVocab=FeelLexer; }

program: statement+;

statement: (New_Line)* (annotationSupport)?  
exportStatement (New_Line)* namespaceSupportStatement*;

// 导出命名空间
exportStatement: Left_Arrow nameSpaceItem end;

namespaceSupportStatement:
importStatement |
packageStatement |
protocolStatement |
implementStatement |
namespaceFunctionStatement |
namespaceVariableStatement |
namespaceConstantStatement |
enumStatement |
typeRedefineStatement |
typeTagStatement |
New_Line ;

// 导入命名空间
importStatement: Right_Arrow left_brace (importSubStatement | typeAliasStatement | New_Line)* right_brace end;

importSubStatement: (annotationSupport)? ((Bang? id|Discard) Colon_Equal)?
 (nameSpaceItem stringExpr? | nameSpaceItem? stringExpr) end;

// 类型别名
typeAliasStatement: Bang? id Colon_Equal typeType end;
// 类型重定义
typeRedefineStatement: Bang? id Colon_Equal New_Line* typeType end;
// 特殊类型注释
typeTagStatement: Comment_Tag; 

// 枚举
enumStatement: (annotationSupport)? Bang? id Colon_Equal New_Line* left_brack Question right_brack
 left_brace enumSupportStatement* right_brace end;

enumSupportStatement: id (Equal (add)? integerExpr)? end;
// 命名空间变量
namespaceVariableStatement: (annotationSupport)? Bang id (Colon_Equal expression | Colon typeType (Equal expression)?) end;
// 命名空间常量
namespaceConstantStatement: (annotationSupport)? id (Colon_Equal expression | Colon typeType (Equal expression)?) end;
// 命名空间函数
namespaceFunctionStatement: (annotationSupport)? id Colon_Equal (left_paren templateDefine right_paren | templateDefine)?
 left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line*
parameterClauseOut right_paren left_brace (functionSupportStatement)* right_brace end;

// 定义包
packageStatement: (annotationSupport)? id Colon_Equal (left_paren templateDefine right_paren | templateDefine)?
 (packageFieldStatement|packageStaticStatement|packageNewStatement) end;

packageStaticStatement: left_brace (packageStaticSupportStatement)* right_brace;
// 包静态语句
packageStaticSupportStatement:
packageStaticFunctionStatement |
packageStaticVariableStatement |
packageStaticConstantStatement |
New_Line;

// 定义变量
packageStaticVariableStatement: (annotationSupport)? Bang id (Colon_Equal expression | Colon typeType (Equal expression)?) end;
// 定义常量
packageStaticConstantStatement: (annotationSupport)? id (Colon_Equal expression | Colon typeType (Equal expression)?) end;
// 函数
packageStaticFunctionStatement: (annotationSupport)? id Colon_Equal (left_paren templateDefine right_paren | templateDefine)?
 left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line*
parameterClauseOut right_paren left_brace (functionSupportStatement)* right_brace end;

packageFieldStatement: Coin (p=Question? id (more id)?)? left_brace (packageSupportStatement)* right_brace;

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
includeStatement: typeType end;
// 包构造方法
packageNewStatement: (annotationSupport)? left_paren parameterClauseIn Right_Arrow Coin p=Question? (id (more id)?)? right_paren
(left_paren expressionList? right_paren)? left_brace (functionSupportStatement)* right_brace;
// 定义变量
packageVariableStatement: (annotationSupport)? Bang id (Colon_Equal expression | Colon typeType (Equal expression)?) end;
// 定义常量
packageConstantStatement: (annotationSupport)? id (Colon_Equal expression | Colon typeType (Equal expression)?) end;
// 函数
packageFunctionStatement: (annotationSupport)? id Colon_Equal (left_paren templateDefine right_paren | templateDefine)?
 left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line*
parameterClauseOut right_paren left_brace (functionSupportStatement)* right_brace end;
// 定义子方法
packageControlSubStatement: id (left_paren id right_paren)? left_brace (functionSupportStatement)+ right_brace end;
// 定义包事件
packageEventStatement: Bang id left_brack Right_Arrow right_brack nameSpaceItem end;

// 扩展
implementStatement: id Colon Colon_Equal  (left_paren templateDefine right_paren | templateDefine)?
(packageNewStatement|packageFieldStatement) end;

// 定义变量
overrideVariableStatement: (annotationSupport)? Dot (n='_')? Bang id (Colon_Equal expression | Colon typeType (Equal expression)?) end;
// 定义常量
overrideConstantStatement: (annotationSupport)? Dot (n='_')? id (Colon_Equal expression | Colon typeType (Equal expression)?) end;
// 函数
overrideFunctionStatement: (annotationSupport)? Dot (n='_')? id Colon_Equal (left_paren templateDefine right_paren | templateDefine)?
 left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line*
parameterClauseOut right_paren left_brace (functionSupportStatement)* right_brace end;

// 协议
protocolStatement: (annotationSupport)? id Colon_Equal (left_paren templateDefine right_paren | templateDefine)? protocolSubStatement end;

protocolSubStatement: Left_Brack Coin Right_Brack (p=Question? id (more id)?)? left_brace (protocolSupportStatement)* right_brace;
// 协议支持的语句
protocolSupportStatement:
includeStatement |
protocolFunctionStatement |
protocolVariableStatement |
New_Line ;
// 定义控制
protocolVariableStatement: (annotationSupport)? Bang? id (Colon_Equal expression | Colon typeType (Equal expression)?) end;
// 函数
protocolFunctionStatement: (annotationSupport)? id Colon (left_paren templateDefine right_paren | templateDefine)? left_paren parameterClauseIn 
t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line* parameterClauseOut right_paren end;

// 函数
functionStatement: id Colon_Equal (left_paren templateDefine right_paren | templateDefine)? left_paren parameterClauseIn
 t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line*
 parameterClauseOut right_paren left_brace (functionSupportStatement)* right_brace end;
// 返回
returnStatement: Left_Arrow (tupleExpression)? end;
// 异步返回
returnAsyncStatement: Left_Flow (tupleExpression)? end;
// 生成器
yieldReturnStatement: At Left_Arrow tupleExpression end;
yieldBreakStatement: At Left_Arrow end;
// 入参
parameterClauseIn: parameter? (more parameter)*;
// 出参
parameterClauseOut: parameter? (more parameter)*;
// 参数结构
parameter: (annotationSupport)? Bang? id Colon (Comma_Comma|Comma_Comma_Comma)? typeType (Equal expression)?;

// 函数支持的语句
functionSupportStatement:
returnStatement |
returnAsyncStatement |
yieldReturnStatement |
yieldBreakStatement |
judgeCaseStatement |
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
expressionStatement |
annotationStatement |
New_Line;

// 条件判断
judgeCaseStatement: Left_Brack expression Right_Brack (caseStatement)+ caseElseStatement end |
Left_Brack expression Right_Brack (caseStatement)+ end;
// 判断条件声明
caseElseStatement: Discard left_brace (functionSupportStatement)* right_brace;
caseStatement: judgeCase (more judgeCase)* Question left_brace (functionSupportStatement)* right_brace;
judgeCase: expression | (id | Discard) Colon typeType;

// 判断
judgeStatement:
judgeIfStatement (judgeElseIfStatement)* judgeElseStatement end
| judgeIfStatement (judgeElseIfStatement)* end;
// else 判断
judgeElseStatement: Discard left_brace (functionSupportStatement)* right_brace;
// if 判断
judgeIfStatement: expression Question left_brace (functionSupportStatement)* right_brace;
// else if 判断
judgeElseIfStatement: expression Question left_brace (functionSupportStatement)* right_brace;
// 循环
loopStatement: expression At (left_brack id right_brack)? Bang? id
 left_brace (functionSupportStatement)* right_brace loopElseStatement? end;
// 条件循环
loopCaseStatement: expression At left_brace (functionSupportStatement)* right_brace loopElseStatement? end;
// else 判断
loopElseStatement: Discard left_brace (functionSupportStatement)* right_brace;
// 跳出循环
loopJumpStatement: Tilde At end;
// 跳过当前循环
loopContinueStatement: At end;
// 检查
checkStatement: 
Bang left_brace (functionSupportStatement)* right_brace (checkErrorStatement)* checkFinallyStatment end
|Bang left_brace (functionSupportStatement)* right_brace (checkErrorStatement)+ end;
// 定义检查变量
usingStatement: Right_Arrow Bang? constId (more constId)* Colon_Equal
tupleExpression left_brace (functionSupportStatement)* right_brace end;
// 错误处理
checkErrorStatement: (id | id Colon typeType) left_brace (functionSupportStatement)* right_brace;
// 最终执行
checkFinallyStatment: Discard left_brace (functionSupportStatement)* right_brace;
// 抛出异常
checkReportStatement: Bang Left_Arrow expression end;

// 声明变量
variableDeclaredStatement: Bang id Colon typeType end;
// 声明常量
constantDeclaredStatement: id Colon typeType end;
// 定义
varStatement: varId (more varId)* Colon_Equal tupleExpression end;
varTypeStatement: varIdType (more varIdType)* Equal tupleExpression end;
// 绑定
bindStatement: constId (more constId)* Colon_Equal tupleExpression end;
bindTypeStatement: constIdType (more constIdType)* Equal tupleExpression end;
// 复合赋值
assignStatement: tupleExpression assign tupleExpression end;
// 表达式
expressionStatement: expression end;

annotationStatement: annotationString end;

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
| judgeCaseExpression // 条件判断表达式
| checkExpression // 检查表达式
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
| expression compareCombine expression // 组合比较表达式
| expression compare expression // 比较表达式
| expression logic expression // 逻辑表达式
| expression judgeExpression // 判断表达式
| expression loopExpression // 集合循环表达式
; 

callExpression: call New_Line? (id | left_paren id templateCall right_paren | id templateCall) (callFunc|callElement)?;

tuple: left_paren (expression (more expression)* )? right_paren; // 元组

expressionList: expression (more expression)* ; // 表达式列

annotationSupport: annotation;

annotation: annotationList; // 注解

annotationList: ((annotationItem|annotationString) New_Line?)+;

annotationItem: Sharp (id left_brace id (tuple|lambda)? right_brace | id (tuple|lambda)?);

annotationString: Sharp (stringExpr|rawStringExpr);

callFunc: (tuple|lambda); // 函数调用

callAsync: Right_Wave expression; // 异步等待调用

callAwait: Right_Wave (tuple|lambda); // 异步等待调用

callChannel: Left_Wave expression; // 通道访问

transfer: Left_Wave; // 传递通道值

callElement: left_brack (slice | expression) right_brack; // 元素调用

callPkg: typeNotNull left_brace (pkgAssign|listAssign|dictionaryAssign)? right_brace; // 新建包

orElse: Question Question expression; // 类型转化

typeConversion: Bang Colon typeType; // 类型转化

typeCheck: Question Colon typeType; // 类型转化

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

templateDefineItem: id | id Colon id; 

templateCall: typeType+;

lambda: left_brace (lambdaIn)? t=(Right_Arrow|Right_Flow) New_Line* tupleExpression right_brace
| left_brace (lambdaIn)? t=(Right_Arrow|Right_Flow) New_Line* 
(functionSupportStatement)* right_brace;

lambdaIn: id (more id)*;

pkgAnonymous: pkgAnonymousAssign; // 匿名包

pkgAnonymousAssign: left_brace (pkgAnonymousAssignElement end)* pkgAnonymousAssignElement right_brace; // 简化赋值

pkgAnonymousAssignElement: Bang? name t=Colon_Equal expression; // 简化赋值元素

functionExpression: left_paren parameterClauseIn t=(Right_Arrow|Right_Flow) b=Bang? y=At? New_Line*
parameterClauseOut right_paren left_brace (functionSupportStatement)* right_brace;

plusMinus: add expression;

negate: wave expression;

bitwiseNotExpression: bitwiseNot expression;

linq: linqHeadItem Right_Arrow New_Line?  (linqItem)* id New_Line? expression;

linqHeadItem: At Bang? id Colon_Equal expression;

linqItem: (linqHeadItem | id (expression)?) Right_Arrow New_Line?;

// 判断表达式
judgeExpression: judgeIfExpression (judgeElseIfExpression)* judgeElseExpression;

// else 判断
judgeElseExpression: Discard left_brace (functionSupportStatement)* tupleExpression right_brace;
// if 判断
judgeIfExpression: Question left_brace (functionSupportStatement)* tupleExpression right_brace;
// else if 判断
judgeElseIfExpression: expression Question left_brace (functionSupportStatement)* tupleExpression right_brace;

// 条件判断表达式
judgeCaseExpression: Left_Brack expression Right_Brack (caseExpression)* caseElseExpression;
// 判断条件声明
caseExpression: judgeCase (more judgeCase)* Question left_brace (functionSupportStatement)* tupleExpression right_brace;

caseElseExpression: Discard left_brace (functionSupportStatement)* tupleExpression right_brace;

judgeCaseElseExpression: Discard left_brace (functionSupportStatement)* tupleExpression right_brace;

// 循环表达式
loopExpression: At (left_brack id right_brack)? Bang? id 
left_brace (functionSupportStatement)* tupleExpression right_brace loopElseExpression;
// else 判断
loopElseExpression: Discard left_brace (functionSupportStatement)* tupleExpression right_brace;
// 检查
checkExpression: 
Bang left_brace (functionSupportStatement)* tupleExpression right_brace (checkErrorExpression)+ checkFinallyStatment? ;
// 错误处理
checkErrorExpression: (id | id Colon typeType) left_brace (functionSupportStatement)* tupleExpression right_brace;
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

// 迭代器
iterator: (Dot_Dot|Dot_Dot_Dot|Dot_Dot_Less|Dot_Dot_Greater) (left_paren expression right_paren)?;
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
