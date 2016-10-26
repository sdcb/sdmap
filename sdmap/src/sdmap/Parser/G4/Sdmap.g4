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
	(macro | plainText)+;

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

macro:
	BeginMacro
		macroParameter? (',' macroParameter)*
	EndMacro;

macroParameter:
	NSSyntax | 
	value |
	unnamedSql;

value:
	STRING |
	NUMBER |
	DATE;

