lexer grammar SdmapLexer;

STRING:
	'"' (('\\' (["\\/bfnrt] | UNICODE)) | ~ ["\\])* '"' | 
	'\'' (('\\' (['\\/bfnrt] | UNICODE)) | ~ ['\\])* '\'';

fragment UNICODE: 
	'u' HEX HEX HEX HEX;

fragment HEX: 
	[0-9a-fA-F];

NUMBER: 
	'-'? INT '.' [0-9] + EXP? | '-'? INT EXP | '-'? INT;

fragment INT: 
	'0' | [1-9] [0-9]*;

fragment EXP: 
	[Ee] [+\-]? INT;

fragment WHITE:
	[ \t\n\r];

DATE:
	INT '-' INT '-' INT |
	INT '/' INT '/' INT;

SYNTAX: 
	[a-zA-Z_] [0-9a-zA-Z_]*;

NSSyntax:
	SYNTAX ('.' SYNTAX)*;

WS: 
	WHITE +     -> skip;

BlockComment: 
	'/*' .*? '*/' -> skip;

LineComment: 
	'//' ~[\r\n]* -> skip;

OpenNamedSql:
	'sql' WHITE+ SYNTAX WHITE* '{' -> pushMode(SQL);

OpenUnnamedSql:
	'sql' WHITE* '{' -> pushMode(SQL);

OpenNamespace:
	'namespace' WHITE+ NSSyntax WHITE* '{';

Close:
	'}';

Comma:
	',';

CloseMacro:
	'>' -> popMode;

mode SQL;
SQLText: 
	~('#' | '}')+;

CloseSql:
	'}' -> popMode;

OpenMacro:
	'#' SYNTAX WHITE* '<' -> pushMode(DEFAULT_MODE);