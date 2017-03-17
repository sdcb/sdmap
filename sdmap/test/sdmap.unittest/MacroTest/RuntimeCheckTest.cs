using sdmap.Macros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static sdmap.Macros.Implements.MacroUtil;

namespace sdmap.unittest.MacroTest
{
    public class RuntimeCheckTest
    {
        [Fact]
        public void EmptyNullOk()
        {
            var mockedMacro = new Macro
            {
                Name = "Test", 
                Arguments = new SdmapTypes[0]
            };
            var ok = RuntimeCheck(null, mockedMacro);

            Assert.True(ok.IsSuccess);
        }

        [Fact]
        public void IsDate()
        {
            var mockedMacro = new Macro
            {
                Name = "Test",
                Arguments = new[] {SdmapTypes.Date}
            };
            var ok = RuntimeCheck(new object[] { DateTime.Now }, mockedMacro);

            Assert.True(ok.IsSuccess);
        }

        [Fact]
        public void NullableTest()
        {
            var mockedMacro = new Macro
            {
                Name = "Test",
                Arguments = new[] { SdmapTypes.Date }
            };
            var ok = RuntimeCheck(new object[] { new DateTime?(DateTime.Now) }, mockedMacro);

            Assert.True(ok.IsSuccess);
        }

        [Fact]
        public void NotDate()
        {
            var mockedMacro = new Macro
            {
                Name = "Test",
                Arguments = new[] { SdmapTypes.Date }
            };
            var ok = RuntimeCheck(new object[] { 3 }, mockedMacro);

            Assert.False(ok.IsSuccess);
            Assert.Equal("Macro 'Test' argument 1 requires Date but provides Int32.", ok.Error);
        }

        [Fact]
        public void IsSyntax()
        {
            var mockedMacro = new Macro
            {
                Name = "Test",
                Arguments = new[]
                {
                    SdmapTypes.Syntax
                }
            };
            var ok = RuntimeCheck(new object[] { "ZipCode" }, mockedMacro);

            Assert.True(ok.IsSuccess);
        }
    }
}
