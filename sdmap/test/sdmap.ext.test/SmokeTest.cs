using sdmap.ext.Dapper;
using Xunit;

namespace sdmap.ext.test
{
    public class SimpleTest
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
    }
}
