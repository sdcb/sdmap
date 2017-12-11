using sdmap.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace sdmap.unittest.EmiterTests.CSharpTests
{
    public class MacroTest : CSharpBaseUnitTest
    {
        [Fact]
        public void MacroWillEmit()
        {
            var source = "sql id{#test<>}";
            var expected = TransformRuntimeProvider(@"{
    var result = MacroProvider.test();
    if (result.IsSuccess)
    {
        sb.Append(result.Value);
    }
    else
    {
        return result;
    }
}
");
            var text = GetEmiterText(source, p => p.root().namedSql()[0].coreSql());
            Assert.True(text.IsSuccess);
            Assert.Equal(expected, text.Value);
        }

        [Fact]
        public void MacroArguments()
        {
            var source = "sql id{#test<1, Test, \"test\", 2017/1/1, 3.14>}";
            var expected = TransformRuntimeProvider(@"{
    var result = MacroProvider.test(1, @""Test"", @""test"", new DateTime(2017, 1, 1), 3.14);
    if (result.IsSuccess)
    {
        sb.Append(result.Value);
    }
    else
    {
        return result;
    }
}
");
            var text = GetEmiterText(source, p => p.root().namedSql()[0].coreSql());
            Assert.True(text.IsSuccess);
            Assert.Equal(expected, text.Value);
        }

        [Fact]
        public void UnnamedSqlTest()
        {
            var source = "sql id{#test<sql {Hello}>}";
            var hash = HashUtil.Base64SHA256("sql{Hello}");
            var expected = TransformRuntimeProvider($@"{{
    var result = MacroProvider.test(Unnamed{hash}());
    if (result.IsSuccess)
    {{
        sb.Append(result.Value);
    }}
    else
    {{
        return result;
    }}
}}
");
            var text = GetEmiterText(source, p => p.root().namedSql()[0].coreSql());
            Assert.True(text.IsSuccess);
            Assert.Equal(expected, text.Value);
        }
    }
}
