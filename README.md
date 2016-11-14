# sdmap
A template engine for writing dynamic sql.

# How to Compile
0. Install .net core SDK 1.0.0-preview2.1-003155 (lower version is fine, you can downgrade version in global.json)
1. Install JRE
2. Add ANTLR4.CodeGenerator package in project.json
3. Wait for installation complete and then remove ANTLR4.CodeGenerator
4. Run Parser/G4/build.bat to generate ANTLR related lexer and parser
5. Compile project and run test