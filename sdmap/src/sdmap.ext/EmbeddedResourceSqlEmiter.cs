using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace sdmap.ext
{
    public class EmbeddedResourceSqlEmiter : ISqlEmiter
    {
        private SdmapCompiler _compiler = new SdmapCompiler();

        public string EmitSql(string sqlMapName, object queryObject)
        {
            return _compiler.Emit(sqlMapName, queryObject);
        }

        public static EmbeddedResourceSqlEmiter CreateFrom(Assembly assembly)
        {
            var emiter = new EmbeddedResourceSqlEmiter();

            foreach (var name in assembly.GetManifestResourceNames()
                .Where(x => x.EndsWith(".sdmap")))
            {
                using (var reader = new StreamReader(assembly.GetManifestResourceStream(name)))
                {
                    emiter._compiler.AddSourceCode(reader.ReadToEnd());
                }
            }

            return emiter;
        }
    }
}
