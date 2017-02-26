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
                Thread.Sleep(100);
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
