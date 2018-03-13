using sdmap.Macros.Implements;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.unittest.MacroImplTest  
{
    public class ValTest
    {
        [Fact]
        public void HelloWorld()
        {
            var val = "Hello World";
            var actual = DynamicRuntimeMacros.Val(OneCallContext.CreateEmpty(), "", val, null);
            Assert.True(actual.IsSuccess);
            Assert.Equal(val, actual.Value);
        }

        [Fact]
        public void InputNullWillBeEmpty()
        {
            string val = null;
            var actual = DynamicRuntimeMacros.Val(OneCallContext.CreateEmpty(), "", val, null);
            Assert.True(actual.IsSuccess);
            Assert.Equal(string.Empty, actual.Value);
        }
    }
}
