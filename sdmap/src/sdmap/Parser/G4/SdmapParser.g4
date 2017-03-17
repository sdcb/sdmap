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
    SYNTAX OpenBrace boolExpression? (Comma boolExpression)* CloseBrace #BoolFunc      |
	Bool                                                                #BoolLeteral   |
	OpenBrace boolExpression CloseBrace                                 #BoolBrace     |
	nsSyntax (Equal | NotEqual) Null                                    #BoolNull      |
	nsSyntax                                                            #BoolNsSyntax  |
	boolExpression OpAnd boolExpression                                 #BoolOpAnd     |
	boolExpression OpOr  boolExpression                                 #BoolOpOr      |
	OpNot boolExpression                                                #BoolOpNot;

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