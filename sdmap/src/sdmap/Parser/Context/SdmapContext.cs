using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Parser.Context
{
    public class SdmapContext
    {
        private SortedDictionary<string, SqlEmiter> _syntaxEmiter;

        public void AddSourceCode(string code)
        {
            throw new NotImplementedException();
        }

        public Result<string> TryEmit(string key, object v)
        {
            SqlEmiter emiter;
            if (_syntaxEmiter.TryGetValue(key, out emiter))
            {
                return emiter.TryEmit(v, this);
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
            foreach (var kv in _syntaxEmiter)
            {
                var ok = kv.Value.EnsureCompiled(this);
                if (ok.IsFailure) return ok;
            }

            return Result.Ok();
        }
    }
}
