parser grammar KoralParser;

options { tokenVocab=KoralLexer; }

program: moduleDeclaration NewLine* (NewLine* globalDeclaration NewLine*)* NewLine* EOF;

moduleDeclaration: Module variableIdentifier SemiColon;

globalDeclaration
    : globalVariableDeclaration
    | globalFunctionDeclaration
    | globalRecordDeclaration
    | globalSumTypeDeclaration
    | globalInterfaceDeclaration
    | globalExtensionDeclaration
    ;

globalVariableDeclaration
    : Let Mut? variableIdentifier (type)? Equal NewLine* expressionWithTerminator;
globalFunctionDeclaration
    : Let typeParameterList? variableIdentifier parameterList (type)? Equal NewLine* expressionWithTerminator;
globalRecordDeclaration: Type typeParameterList? typeIdentifier fieldList (Colon type)? (SemiColon | With methodList);
globalInterfaceDeclaration: Type typeParameterList? typeIdentifier (SemiColon | Equal NewLine* virtualMethodList);
globalExtensionDeclaration: Given typeParameterList? typeIdentifier (Colon type)? With methodList SemiColon?;
globalSumTypeDeclaration
    : Type typeParameterList? typeIdentifier Equal NewLine*
     LeftBrace NewLine* recordConstructor (Comma NewLine* recordConstructor)* NewLine* RightBrace SemiColon?;

recordConstructor: typeIdentifier fieldList;

typeParameterList: LeftBrack NewLine* typeParameter (Comma NewLine* typeParameter)* NewLine* RightBrack;

typeParameter: typeIdentifier type;

parameterList: LeftParen NewLine* (parameter (Comma NewLine* parameter)*)? NewLine* RightParen;

parameter: variableIdentifier type;

fieldList: LeftParen NewLine* (field (Comma NewLine* field)*)? NewLine* RightParen;

field: Mut? variableIdentifier type;

methodList: LeftBrace NewLine* (NewLine* method NewLine*)* NewLine* RightBrace;

method: variableIdentifier parameterList (type)? Equal NewLine* expressionWithTerminator;

virtualMethodList: LeftBrace NewLine* (NewLine* virtualMethod NewLine*)* NewLine* RightBrace;

virtualMethod: variableIdentifier parameterList type (Equal NewLine* expression)? SemiColon;

statement
    : variableDeclaration
    | functionDeclaration
    | expressionStatement
    ;

expressionStatement
    : expression SemiColon
    | expressionWithBlock NewLine
    ;

expressionWithTerminator: expressionWithBlock NewLine | expression SemiColon;

expression
    : primaryExpression
    | expressionWithBlock
    | ifDoExpression
    | ifThenElseExpression
    | whileDoExpression
    | assignmentExpression
    | expression memberAccessCallSuffix
    | expression callSuffix
    | expression memberAccess
    | expression multiplicativeOperator expression
    | expression additiveOperator expression
    | expression compareOperator expression
    | expression logicAndOperator expression
    | expression logicOrOperator expression
    | lambdaExpression
    ;

expressionWithBlock
    : blockExpression
    | ifDoExpressionWithBlock
    | ifThenElseExpressionWithBlock
    | whileDoExpressionWithBlock
    | assignmentExpressionWithBlock
    ;

variableDeclaration
    : Let Mut? variableIdentifier (type)? Equal NewLine* expressionWithTerminator;
functionDeclaration
    : Let variableIdentifier parameterList (type)? Equal NewLine* expressionWithTerminator;

memberAccessCallSuffix
    : NewLine? Dot (LeftParen NewLine* type (Comma NewLine* type)* NewLine* RightParen)?
     variableIdentifier LeftParen NewLine* (expression (Comma NewLine* expression)*)? NewLine* RightParen;

callSuffix
    : LeftParen NewLine* (expression (Comma NewLine* expression)*)? NewLine* RightParen
    ;

memberAccess: NewLine? Dot variableIdentifier;

assignmentExpression: variableIdentifier Equal NewLine* expression;

assignmentExpressionWithBlock: variableIdentifier Equal NewLine* expressionWithBlock;

lambdaExpression: parameterList (type)? Arrow NewLine* expression;

ifDoExpression
    : If NewLine* condition Then NewLine* expression
    ;

ifDoExpressionWithBlock
    : If NewLine* condition Then NewLine* expressionWithBlock
    ;

ifThenElseExpression
    : If NewLine* condition Then NewLine* expression Else NewLine* expression
    ;

ifThenElseExpressionWithBlock
    : If NewLine* condition Then NewLine* expression Else NewLine* expressionWithBlock
    ;

whileDoExpression
    : While NewLine* condition Then NewLine* expression
    ;

whileDoExpressionWithBlock
    : While NewLine* condition Then NewLine* expressionWithBlock
    ;

condition
    : expression (Is NewLine* pattern NewLine*)?
    | condition And condition
    | condition Or condition
    | LeftParen NewLine* condition NewLine* RightParen
    ;

blockExpression
    : LeftBrace NewLine* (NewLine* statement)* NewLine* expression NewLine* RightBrace
    | LeftBrace NewLine* (NewLine* statement)* NewLine* RightBrace
    ;

pattern
    : typePattern
    | literalPattern
    | identifierPattern
    | deconstructPattern
    | wildcardPattern
    ;

typePattern
    : identifierPattern type
    | wildcardPattern type
    | type deconstructPattern
    ;

deconstructPattern
    : LeftParen NewLine* (pattern (Comma NewLine* pattern)*)? NewLine* RightParen
    ;

identifierPattern
    : variableIdentifier
    ;

wildcardPattern
    : Discard
    ;

literalPattern
    : literalExpression
    ;

primaryExpression
    : literalExpression
    | constructExpression
    | functionCallExpression
    | variableIdentifier
    ;

constructExpression
    : (LeftBrack NewLine* type (Comma NewLine* type)* NewLine* RightBrack)? typeIdentifier
        LeftParen NewLine* (expression (Comma NewLine* expression)*)? NewLine* RightParen
    ;

functionCallExpression
    : (LeftBrack NewLine* type (Comma NewLine* type)* NewLine* RightBrack)? variableIdentifier
              LeftParen NewLine* (expression (Comma NewLine* expression)*)? NewLine* RightParen
    ;

literalExpression
    : integerExpression
    | floatExpression
    | runeExpression
    | stringExpression
    | boolExpression
    ;

type
    : (LeftBrack NewLine* type (Comma NewLine* type)* NewLine* RightBrack)? typeIdentifier
    | functionType
    ;

functionType: Fn parameterList type;

typeIdentifier: UpperIdentifier;

variableIdentifier: LowerIdentifier;

stringExpression: StringLiteral;

runeExpression: RuneLiteral;

floatExpression: FloatLiteral;

boolExpression: True | False;

integerExpression: DecimalLiteral | BinaryLiteral | OctalLiteral | HexLiteral;

multiplicativeOperator: (Mul | Div | Mod) NewLine?;

additiveOperator: (Add | Sub)  NewLine?;

logicAndOperator: BitAnd NewLine?;

logicOrOperator: BitOr NewLine?;

compareOperator: (Less | Greater | LessEqual | GreaterEqual | EqualEqual | NotEqual) NewLine?;