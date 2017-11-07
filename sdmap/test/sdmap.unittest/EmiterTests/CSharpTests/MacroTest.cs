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
    }
}
