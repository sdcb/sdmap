using sdmap.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;

namespace sdmap.ext
{
    /// <summary>
    /// An implementation of ISdmapEmiter that emits SQL code from embedded resources.
    /// </summary>
    public class EmbeddedResourceSqlEmiter : ISdmapEmiter
    {
        private readonly SdmapCompiler _compiler = new();

        /// <summary>
        /// Emit SQL code for a given statement ID using the provided parameters.
        /// </summary>
        /// <param name="statementId">The statement ID for which SQL is to be emitted.</param>
        /// <param name="parameters">The parameters used for SQL code emission.</param>
        /// <returns>The emitted SQL code as a string.</returns>
        public string Emit(string statementId, object parameters)
        {
            return _compiler.Emit(statementId, parameters);
        }

        /// <summary>
        /// Creates an instance of EmbeddedResourceSqlEmiter based on embedded resources in a given assembly.
        /// </summary>
        /// <param name="assembly">The assembly containing sdmap embedded resources.</param>
        /// <returns>An instance of EmbeddedResourceSqlEmiter.</returns>
        public static EmbeddedResourceSqlEmiter CreateFrom(Assembly assembly)
        {
            var emiter = new EmbeddedResourceSqlEmiter();

            foreach (var name in assembly.GetManifestResourceNames()
                .Where(x => x.EndsWith(".sdmap")))
            {
                using StreamReader reader = new(assembly.GetManifestResourceStream(name));
                emiter._compiler.AddSourceCode(reader.ReadToEnd());
            }

            return emiter;
        }
    }

    /// <summary>
    /// An implementation of ISdmapEmiter that emits SQL code from multiple assemblies' embedded resources.
    /// </summary>
    public class MultipleAssemblyEmbeddedResourceSqlEmiter : ISdmapEmiter
    {
        private readonly SdmapCompiler _compiler = new();

        /// <summary>
        /// Emit SQL code for a given statement ID using the provided parameters.
        /// </summary>
        /// <param name="statementId">The statement ID for which SQL is to be emitted.</param>
        /// <param name="parameters">The parameters used for SQL code emission.</param>
        /// <returns>The emitted SQL code as a string.</returns>
        public string Emit(string statementId, object parameters)
        {
            return _compiler.Emit(statementId, parameters);
        }

        /// <summary>
        /// Adds an assembly to the SQL emitter, allowing it to emit SQL code from the assembly's embedded resources.
        /// </summary>
        /// <param name="assembly">The assembly containing sdmap embedded resources to add.</param>
        public void AddAssembly(Assembly assembly)
        {
            foreach (var name in assembly.GetManifestResourceNames()
                    .Where(x => x.EndsWith(".sdmap")))
            {
                using StreamReader reader = new(assembly.GetManifestResourceStream(name));
                _compiler.AddSourceCode(reader.ReadToEnd());
            }
        }

        /// <summary>
        /// Creates an instance of MultipleAssemblyEmbeddedResourceSqlEmiter based on embedded resources in multiple assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies containing sdmap embedded resources.</param>
        /// <returns>An instance of MultipleAssemblyEmbeddedResourceSqlEmiter.</returns>
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
