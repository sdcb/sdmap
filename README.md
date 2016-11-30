# sdmap
A template engine for writing dynamic sql.

# How to Compile
0. Install .NET Core SDK(https://www.microsoft.com/net/download/core)
1. Install JRE 1.6+(http://www.oracle.com/technetwork/java/javase/downloads/jre8-downloads-2133155.html)
2. Add ANTLR4.CodeGenerator package in project.json
3. Wait for installation complete and then remove ANTLR4.CodeGenerator
4. Run Parser/G4/build.bat to generate ANTLR related lexer and parser
5. Compile project and run test