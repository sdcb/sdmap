parser grammar SdmapParser;

options { tokenVocab=SdmapLexer; }

root:
	(namespace | namedSql)*;

namespace:
	KNamespace nsSyntax OpenCurlyBrace
		(namespace | namedSql)*
	CloseCurlyBrace;

coreSql:
	(if | macro | plainText)+;

plainText:
	SQLText;

namedSql:
	KSql SYNTAX OpenCurlyBrace
		coreSql?
	CloseSql;

unnamedSql:
	KSql OpenCurlyBrace
		coreSql?
	CloseSql;

if:
	Hash KIf OpenBrace boolExpression CloseBrace
	OpenCurlyBrace
		coreSql?
	CloseSql;

boolExpression: 
    SYNTAX OpenBrace boolExpression? (Comma boolExpression)* CloseBrace |
	SYNTAX                                                              |
	Bool                                                                |
	OpenBrace boolExpression CloseBrace                                 |
	SYNTAX (Equal | NotEqual) Null                                      |
	boolExpression OpAnd boolExpression                                 |
	boolExpression OpOr  boolExpression;

macro:
	Hash SYNTAX OpenAngleBracket
		macroParameter? (Comma macroParameter)*
	CloseAngleBracket;

nsSyntax:
	SYNTAX (Dot SYNTAX)*;

macroParameter:
	Bool |
	nsSyntax |
	STRING |
	NUMBER |
	DATE |
	unnamedSql;