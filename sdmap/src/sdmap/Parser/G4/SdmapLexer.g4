lexer grammar SdmapLexer;

@lexer::members {
}

KSql: 
	'sql'{braceStack.Push("sql");};

KIf: 
	'if'{braceStack.Push("if");};

Null: 
	'null';

KNamespace:
	'namespace'{braceStack.Push("namespace");};

OpAnd:
	'&&';

OpOr:
	'||';

OpNot:
	'!';

OpenCurlyBrace: 
	'{'{if (bracePrefix == "sql" || bracePrefix == "if") PushMode(SQL);};

CloseCurlyBrace:
	'}'{
	if (braceStack.Count > 0) 
		braceStack.Pop();
	else
		Skip();
};

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

Dot:
	'.';

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

OpenBrace: 
	'(';

CloseBrace: 
	')';

Equal: 
	'==';

NotEqual: 
	'!=';





mode SQL;
SQLText: 
	(~('#' | '}') | '\\#' | '\\}')+;

CloseSql:
	'}'{
	if (bracePrefix == "if")
	{
		PopMode();
	}
	braceStack.Pop();
	PopMode();
};

Hash: 
	'#' -> pushMode(DEFAULT_MODE);