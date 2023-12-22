using sdmap.Macros;
using System;
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
                Arguments = [SdmapTypes.Date]
            };
            var ok = RuntimeCheck([DateTime.Now], mockedMacro);

            Assert.True(ok.IsSuccess);
        }

        [Fact]
        public void NullableTest()
        {
            var mockedMacro = new Macro
            {
                Name = "Test",
                Arguments = [SdmapTypes.Date]
            };
            var ok = RuntimeCheck([new DateTime?(DateTime.Now)], mockedMacro);

            Assert.True(ok.IsSuccess);
        }

        [Fact]
        public void NotDate()
        {
            var mockedMacro = new Macro
            {
                Name = "Test",
                Arguments = [SdmapTypes.Date]
            };
            var ok = RuntimeCheck([3], mockedMacro);

            Assert.False(ok.IsSuccess);
            Assert.Equal("Macro 'Test' argument 1 requires Date but provides Int32.", ok.Error);
        }

        [Fact]
        public void IsSyntax()
        {
            var mockedMacro = new Macro
            {
                Name = "Test",
                Arguments =
                [
                    SdmapTypes.Syntax
                ]
            };
            var ok = RuntimeCheck(["ZipCode"], mockedMacro);

            Assert.True(ok.IsSuccess);
        }
    }
}
