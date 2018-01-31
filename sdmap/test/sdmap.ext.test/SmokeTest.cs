using sdmap.ext.Dapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.ext.test
{
    public class SmokeTest
    {
        private class SimpleSqlEmiter : ISdmapEmiter
        {
            public string Emit(string sqlId, object queryObject)
            {
                return sqlId;
            }
        }

        [Fact]
        public void SqlEmiterTest()
        {
            DbConnectionExtensions.SetSqlEmiter(new SimpleSqlEmiter());
            var actual = DbConnectionExtensions.EmitSql("test", null);
            Assert.Equal("test", actual);
        }

        [Fact]
        public void WatchSmoke()
        {
            Directory.CreateDirectory("sqls");
            var tempFile = @"sqls\test.sdmap";
            File.WriteAllText(tempFile, "sql Hello{Hello}");
            DbConnectionExtensions.SetSqlDirectoryAndWatch(@".\sqls");

            try
            {
                File.WriteAllText(tempFile, "sql Hello{Hello2}");
                Thread.Sleep(30);
                var text = DbConnectionExtensions.EmitSql("Hello", null);
                Assert.Equal("Hello2", text);
            }
            finally
            {
                File.Delete(tempFile);
                Directory.Delete("sqls");
            }
        }
    }
}
