using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace sdmap.ext
{
    public class EmbeddedResourceSqlEmiter : ISdmapEmiter
    {
        private SdmapCompiler _compiler = new SdmapCompiler();

        public string Emit(string statementId, object parameters)
        {
            return _compiler.Emit(statementId, parameters);
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

    public class MultipleAssemblyEmbeddedResourceSqlEmiter : ISdmapEmiter
    {
        private SdmapCompiler _compiler = new SdmapCompiler();

        public string Emit(string statementId, object parameters)
        {
            return _compiler.Emit(statementId, parameters);
        }

        public void AddAssembly(Assembly assembly)
        {
            foreach (var name in assembly.GetManifestResourceNames()
                    .Where(x => x.EndsWith(".sdmap")))
            {
                using (var reader = new StreamReader(assembly.GetManifestResourceStream(name)))
                {
                    _compiler.AddSourceCode(reader.ReadToEnd());
                }
            }
        }

        public static MultipleAssemblyEmbeddedResourceSqlEmiter CreateFrom(params Assembly[] assemblies)
        {
            var emiter = new MultipleAssemblyEmbeddedResourceSqlEmiter();

            foreach (var assembly in assemblies)
            {
                emiter.AddAssembly(assembly);
            }

            return emiter;
        }
    }
}
