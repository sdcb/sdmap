using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Parser.Context
{
    using ContextType = SortedDictionary<string, SqlEmiter>;

    public class SdmapManager
    {
        private readonly ContextType _context = new ContextType();

        public void AddSourceCode(string code)
        {
            throw new NotImplementedException();
        }

        public Result<string> TryEmit(string key, object v)
        {
            SqlEmiter emiter;
            if (_context.TryGetValue(key, out emiter))
            {
                return emiter.TryEmit(v, _context);
            }
            else
            {
                return Result.Fail<string>($"Key: '${key}' not found.");
            }
        }

        public string Emit(string key, object v)
        {
            return TryEmit(key, v).Value;
        }

        public Result EnsureCompiled()
        {
            foreach (var kv in _context)
            {
                var ok = kv.Value.EnsureCompiled(_context);
                if (ok.IsFailure) return ok;
            }

            return Result.Ok();
        }
    }
}
