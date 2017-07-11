@ECHO OFF

SET CLASSPATH=%SystemDrive%%HomePath%\.nuget\packages\Antlr4.CodeGenerator\4.6.2\tools\antlr4-csharp-4.6.2-complete.jar
SET Namespace=sdmap.Parser.G4
SET  G4Folder=%~dp0
SET   Options=-Dlanguage=CSharp_v4_5 -package %Namespace% -visitor -listener -encoding UTF8

SET    G4File=SdmapLexer
DEL /Q ^
	%G4Folder%%G4File%.tokens		  ^
	%G4Folder%%G4File%.cs
SET FullPath=%G4Folder%%G4File%.g4
JAVA org.antlr.v4.Tool %FullPath% %Options%

SET    G4File=SdmapParser
DEL /Q ^
	%G4Folder%%G4File%.tokens		  ^
	%G4Folder%%G4File%.cs		      ^
	%G4Folder%%G4File%Listener.cs	  ^
	%G4Folder%%G4File%BaseListener.cs ^
	%G4Folder%%G4File%Visitor.cs	  ^
	%G4Folder%%G4File%BaseVisitor.cs
SET FullPath=%G4Folder%%G4File%.g4
JAVA org.antlr.v4.Tool %FullPath% %Options%