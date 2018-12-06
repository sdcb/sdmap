using sdmap.ext.Dapper;
using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.ext.test
{
    public class WatchTest
    {
        [Fact]
        public async Task WatchSmokeAsync()
        {
            var fs = new TestFileSystem();
            fs.FileSystemWatcher = new TestFSWatcherFactory();

            fs.Directory.CreateDirectory("sqls");
            fs.File.WriteAllText(@"sqls\test.sdmap", "sql Hello{Hello}");
            DbConnectionExtensions.SetSqlDirectoryAndWatch(@"sqls", fs);

            fs.File.WriteAllText(@"sqls\test.sdmap", "sql Hello{Hello2}");
            fs.FileSystemWatcher.TheOnlyWatcher.FireChange(WatcherChangeTypes.Changed, "sqls", "test.sdmap");
            await Task.Delay(30);
            var text = DbConnectionExtensions.EmitSql("Hello", null);
            Assert.Equal("Hello2", text);
        }

        private class TestFileSystem : MockFileSystem
        {
            public new TestFSWatcherFactory FileSystemWatcher
            {
                get => (TestFSWatcherFactory)base.FileSystemWatcher;
                set => base.FileSystemWatcher = value;
            }

            public TestFileSystem()
            {
                FileSystemWatcher = new TestFSWatcherFactory();
            }
        }

        private class TestFSWatcherFactory : IFileSystemWatcherFactory
        {
            public MockFsWatcher TheOnlyWatcher { get; } = new MockFsWatcher();

            public FileSystemWatcherBase CreateNew()
            {
                throw new NotImplementedException();
            }

            public FileSystemWatcherBase FromPath(string path) => TheOnlyWatcher;
        }

        private class MockFsWatcher : FileSystemWatcherBase
        {
            public override bool IncludeSubdirectories { get; set; }
            public override bool EnableRaisingEvents { get; set; }
            public override string Filter { get; set; }
            public override int InternalBufferSize { get; set; }
            public override NotifyFilters NotifyFilter { get; set; }
            public override string Path { get; set; }

            public override WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
                => throw new NotImplementedException();

            public override WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
                => throw new NotImplementedException();

            public void FireChange(WatcherChangeTypes changeType, string directory, string name)
                => OnChanged(this, new FileSystemEventArgs(WatcherChangeTypes.All, directory, name));
        }
    }
}
