grammar Sdmap;

options { tokenVocab=SdmapToken; }

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
	SqlText;

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

