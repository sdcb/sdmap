using sdmap.Extensions;
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
        private class SimpleSqlEmiter : ISqlEmiter
        {
            public string EmitSql(string sqlId, object queryObject)
            {
                return "Simple";
            }
        }

        [Fact]
        public void SqlEmiterTest()
        {
            SdmapExtensions.SetSqlEmiter(new SimpleSqlEmiter());
            var actual = SdmapExtensions.EmitSql("test", null);
            Assert.Equal("Simple", actual);
        }

        [Fact]
        public void WatchSmoke()
        {
            Directory.CreateDirectory("sqls");
            var tempFile = @"sqls\test.sdmap";
            File.WriteAllText(tempFile, "sql Hello{Hello}");
            SdmapExtensions.SetSqlDirectoryAndWatch(@".\sqls");

            try
            {
                File.WriteAllText(tempFile, "sql Hello{Hello2}");
                Thread.Sleep(30);
                var text = SdmapExtensions.EmitSql("Hello", null);
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
