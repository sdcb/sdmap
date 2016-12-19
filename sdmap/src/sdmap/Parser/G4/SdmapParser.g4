parser grammar SdmapParser;

options { tokenVocab=SdmapLexer; }

root:
	(namespace | namedSql)*;

namespace:
	KNamespace (SYNTAX|NSSyntax) OpenCurlyBrace
		(namespace | namedSql)*
	CloseCurlyBrace;

coreSql:
	(macro | plainText)+;

plainText:
	SQLText;

namedSql:
	KSql SYNTAX OpenCurlyBrace
		coreSql
	CloseCurlyBrace;

unnamedSql:
	KSql OpenCurlyBrace
		coreSql
	CloseCurlyBrace;

macro:
	Hash SYNTAX OpenAngleBracket
		macroParameter? (Comma macroParameter)*
	CloseAngleBracket;

macroParameter:
	SYNTAX |
	NSSyntax | 
	STRING |
	NUMBER |
	DATE |
	unnamedSql;

