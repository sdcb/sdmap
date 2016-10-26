grammar Sdmap;

options { tokenVocab=SdmapLexerBase; }

root:
	namespace | namedSql*;

namespace:
	'namespace' key 
	'{'
		namedSql* 
	'}';

coreSql:
	(directive | plainText)+;

plainText:
	SQLText;

namedSql:
	BeginNamedSql
		coreSql
	EndSql;

unnamedSql:
	BeginUnnamedSql
		coreSql
	EndSql;

directive:
	EnterDirectiveMode
		directiveParameters*
	ExitDirectiveMode;

directiveParameters:
	key | 
	value |
	unnamedSql;

value:
	STRING |
	NUMBER |
	DATE;

key:
	SYNTAX ('.' SYNTAX)*;

