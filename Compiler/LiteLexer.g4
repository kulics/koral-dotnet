lexer grammar LiteLexer;

Pow:    '*^';
Root:   '/^';
Log:    '\\^';

Add_Add: '++';
Sub_Sub: '--';

Add_Equal:         '+=';
Sub_Equal:         '-=';
Mul_Equal:         '*=';
Div_Equal:         '/=';
Mod_Equal:        '\\=';

Equal_Equal:        '==';
Less_Equal:         '<=';
Greater_Equal:      '>=';
Not_Equal:          '><';
Combine_Equal:      '<>';

Dot_Dot_Dot: '...';
Dot_Dot: '..';
Dot: '.';

Comma: ',';

Colon_Arrow: ':>';
Equal_Arrow: '=>';

Right_Arrow: '->';
Left_Arrow: '<-';

Right_Flow: '->>';
Left_Flow: '<<-';

Equal: '=';

Less_Less: '<<';
Greater_Greater: '>>';

Less: '<';
Greater: '>';

Semi: ';';

Left_Paren:             '(';
Right_Paren:             ')';
Left_Brace:             '{';
Right_Brace:             '}';
Left_Brack:             '[';
Right_Brack:             ']';

Colon: ':';

Question: '?';

At: '@';

Bang: '!';

Coin: '$';
Cent: '%';

Wave: '~';

Add:    '+';
Sub:    '-';
Mul:    '*';
Div:    '/';
Mod:   '\\';

And:    '&';
Or:     '|';
Xor:    '^';

Grave:  '`';

TypeI8: 'i8';
TypeU8: 'u8';
TypeI16: 'i16';
TypeU16: 'u16';
TypeI32: 'i32';
TypeU32: 'u32';
TypeI64: 'i64';
TypeU64: 'u64';
TypeF32: 'f32';
TypeF64: 'f64';
TypeChr: 'chr';
TypeStr: 'str';
TypeBool: 'bool';
TypeInt: 'int';
TypeNum: 'num';
TypeByte: 'byte';
TypeAny: 'any';
NilLiteral: 'nil';
TrueLiteral: 'true';
FalseLiteral: 'false';
UndefinedLiteral: 'undef';

NumberLiteral: DIGIT+ ; // 整数
fragment DIGIT: [0-9] ;   // 单个数字
TextLiteral: '"' ('\\' [btnfr"\\] | .)*? '"'; // 文本
CharLiteral: '\'' ('\\\'' | '\\' [btnfr\\] | .)*? '\''; // 单字符
IDPrivate: '_' [a-zA-Z0-9_]+; // 私有标识符
IDPublic: [a-zA-Z] [a-zA-Z0-9_]*; // 公有标识符
Discard: '_'; // 匿名变量

Comment_Block: '#=' .*? '=#' -> skip; // 块注释
Comment_Line: '#' ~[\r\n]* -> skip; // 行注释

New_Line: '\r'? '\n'; 
//WS: (' ' |'\t' |'\n' |'\r' )+ -> skip ;

WS: [ \t]+ -> skip; // 空白， 后面的->skip表示antlr4在分析语言的文本时，符合这个规则的词法将被无视

