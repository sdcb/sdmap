lexer grammar SdmapLexer;

@lexer::members {
private string bracePrefix = "";
}

KSql: 
	'sql'{bracePrefix = "sql";};

KNamespace:
	'namespace'{bracePrefix = "namespace";};

OpenCurlyBrace: 
	'{'{if (bracePrefix == "sql") PushMode(SQL);};

CloseCurlyBrace:
	'}'{bracePrefix = "";};

STRING:
	'@"' (~'"' | '""')* '"' |
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

Bool: 
	'true' | 'false';

SYNTAX: 
	[a-zA-Z_] [0-9a-zA-Z_]*;

NSSyntax:
	SYNTAX ('.' SYNTAX)+;

WS: 
	WHITE +     -> skip;

BlockComment: 
	'/*' .*? '*/';

LineComment: 
	'//' ~[\r\n]*;

Comma:
	',';

OpenAngleBracket:
	'<';

CloseAngleBracket:
	'>' -> popMode;

mode SQL;
SQLText: 
	~('#' | '}')+;

CloseSql:
	'}' -> popMode;

Hash: 
	'#' -> pushMode(DEFAULT_MODE);