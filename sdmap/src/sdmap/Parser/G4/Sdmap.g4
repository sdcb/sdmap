grammar Sdmap;

options { tokenVocab=SdmapLexerBase; }

root:
	namespace | namedSql*;

namespace:
	'namespace' NSSyntax
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
	BeginMacro
		directiveParameters*
	EndMacro;

directiveParameters:
	NSSyntax | 
	value |
	unnamedSql;

value:
	STRING |
	NUMBER |
	DATE;

