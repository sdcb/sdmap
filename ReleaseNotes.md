## 0.10.0
- [core] support #if(){} syntax.
- [core] switch a large amount of class modifier from public to internal.
- [core] optimized performance for nested sql.

## 0.9.1:
- [core] fix a issue that EnsureCompiled will fail when using sub-sql.
- [ext] bump version.

## 0.9.0:
- [core] add support for bool type
- [core] add support for any type
- [core] allow skip argument runtime check
- [core] enable any type in ifEquals/ifNotEquals
- [core] all property syntax in macro support Nested Objects
- [vstool] bump version
- [ext] bump version

## 0.8.2:
- [core] Support Verbatium String(@"\/")
- [vstool] update to support verbatium string

## 0.8.1:
- [core] Reduce .NET version request to 4.5.1
- [ext] Reduce .NET version request to 4.5.1

## 0.8.0:
- [core] Breaking change: rename ifNotEmpty->isNotEmpty, ifEmpty->isEmpty
- [core] Breaking change: rename SdmapRuntime->SdmapCompiler, SdmapContext->SdmapCompilerContext
- [core] Add hasProp, isNull, isEqual, isNotEqual, isLike, isNotLike
- [core] Add iterate/each support

## 0.7.1: 
- [core] fix a bug causing unnamed sql cannot be found.

## 0.7.0: 
- [core] drop state in runtime.

## 0.6.0: 
- [core]: Separate keywords from lexer
- [ext]: (breaking change) Support ResetSqlDirectory
- [vstool]: Highlight real keywords instead of full OpenXXX syntax.

## 0.5.0: 
- [core]: Support nested namespace
- [vstool]: Support comment highlight in visual studio.

## 0.4.0: 
- [core]: Support StringOrSql type
- [core]: Support multiple namespace in single file.
