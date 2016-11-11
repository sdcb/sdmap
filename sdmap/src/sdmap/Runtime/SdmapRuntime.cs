using Antlr4.Runtime;
using sdmap.Functional;
using sdmap.Macros;
using sdmap.Parser.G4;
using sdmap.Parser.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Runtime
{
    public class SdmapRuntime
    {
        private readonly SdmapContext _context = SdmapContext.CreateEmpty();

        public Result AddSourceCode(string sourceCode)
        {
            var inputStream = new AntlrInputStream(sourceCode);
            var lexer = new SdmapLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new SdmapParser(tokenStream);

            var visitor = SqlItemVisitor.Create(_context);
            return visitor.Visit(parser.root());
        }

        public void DropCleanEmiters()
        {
            _context.Emiters.Clear();
        }

        public Result AddMacro(string id, SdmapTypes[] arguments, MacroDelegate method)
        {
            return _context.MacroManager.Add(new Macro
            {
                Name = id, 
                Arguments = arguments, 
                Method = method
            });
        }

        public Result<string> TryEmit(string id, object query)
        {
            var fullName = _context.GetFullName(id);
            SqlEmiterBase emiter;
            if (_context.Emiters.TryGetValue(fullName, out emiter))
            {
                return emiter.TryEmit(query, _context);
            }
            else
            {
                return Result.Fail<string>($"Key: '{id}' not found.");
            }
        }

        public string Emit(string id, object query)
        {
            return TryEmit(id, query).Value;
        }

        public Result EnsureCompiled()
        {
            foreach (var kv in _context.Emiters)
            {
                var ok = kv.Value.EnsureCompiled(_context);
                if (ok.IsFailure) return ok;
            }

            return Result.Ok();
        }
    }
}
