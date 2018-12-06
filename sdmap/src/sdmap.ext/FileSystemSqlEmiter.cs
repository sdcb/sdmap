using sdmap.Compiler;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace sdmap.ext
{
    public class FileSystemSqlEmiter : ISdmapEmiter
    {
        private SdmapCompiler _compiler = null;

        protected FileSystemSqlEmiter(SdmapCompiler compiler)
        {
            _compiler = compiler;
        }

        public string Emit(string statementId, object parameters)
        {
            return _compiler.Emit(statementId, parameters);
        }

        private static SdmapCompiler CreateCompilerFromSqlDirectory(
            string sqlDirectory,
            IFileSystem fileSystem,
            bool ensureCompiled)
        {
            var compiler = new SdmapCompiler();

            foreach (var file in fileSystem.Directory.EnumerateFiles(sqlDirectory, "*.sdmap", SearchOption.AllDirectories))
            {
                var code = fileSystem.File.ReadAllText(file);
                compiler.AddSourceCode(code);
            }

            if (ensureCompiled)
            {
                var compileResult = compiler.EnsureCompiled();
                if (compileResult.IsFailure)
                {
                    throw new InvalidProgramException(compileResult.Error);
                };
            }

            return compiler;
        }

        public static FileSystemSqlEmiter FromSqlDirectory(
            string sqlDirectory,
            bool ensureCompiled = false) => FromSqlDirectory(sqlDirectory, new FileSystem(), ensureCompiled);

        public static FileSystemSqlEmiter FromSqlDirectory(
            string sqlDirectory,
            IFileSystem fileSystem,
            bool ensureCompiled = false)
        {
            return new FileSystemSqlEmiter(CreateCompilerFromSqlDirectory(
                sqlDirectory,
                fileSystem,
                ensureCompiled));
        }

        public static FileSystemSqlEmiter FromSqlDirectoryAndWatch(
            string sqlDirectory,
            bool ensureCompiled = false) => FromSqlDirectoryAndWatch(sqlDirectory, new FileSystem(), ensureCompiled);

        public static FileSystemSqlEmiter FromSqlDirectoryAndWatch(
            string sqlDirectory,
            IFileSystem fileSystem,
            bool ensureCompiled = false)
        {
            var compiler = CreateCompilerFromSqlDirectory(
                sqlDirectory,
                fileSystem,
                ensureCompiled);

            var result = new FileSystemSqlEmiter(compiler);

            var watcher = fileSystem.FileSystemWatcher.FromPath(sqlDirectory);
            watcher.Path = sqlDirectory;
            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += async (o, e) =>
            {
                await Task.Delay(1);
                result._compiler = CreateCompilerFromSqlDirectory(
                    sqlDirectory,
                    fileSystem,
                    ensureCompiled);
                GC.KeepAlive(watcher);
            };

            return result;
        }
    }
}
