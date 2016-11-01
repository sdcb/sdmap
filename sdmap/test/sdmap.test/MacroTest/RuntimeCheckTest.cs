using sdmap.Macros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static sdmap.Macros.MacroUtil;

namespace sdmap.test.MacroTest
{
    public class RuntimeCheckTest
    {
        [Fact]
        public void EmptyNullOk()
        {
            var mockedMacro = new SdmapMacro
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
            var mockedMacro = new SdmapMacro
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
            var mockedMacro = new SdmapMacro
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
            var mockedMacro = new SdmapMacro
            {
                Name = "Test",
                Arguments = new[] { SdmapTypes.Date }
            };
            var ok = RuntimeCheck(new object[] { 3 }, mockedMacro);

            Assert.False(ok.IsSuccess);
            Assert.Equal("Macro 'Test' argument 1 requires Date but provides Int32.", ok.Error);
        }

        [Fact]
        public void IsNumber()
        {
            var mockedMacro = new SdmapMacro
            {
                Name = "Test",
                Arguments = new[] 
                {
                    SdmapTypes.Number, SdmapTypes.Number,
                    SdmapTypes.Number, SdmapTypes.Number
                }
            };
            var ok = RuntimeCheck(new object[] { 3m, 3.14, 3L, 3f }, mockedMacro);

            Assert.True(ok.IsSuccess);
        }
    }
}
