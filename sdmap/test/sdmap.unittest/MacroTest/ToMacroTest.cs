using sdmap.Macros;
using sdmap.unittest.MacroTest.ToMacroImpl;
using System.Linq;
using Xunit;
using static sdmap.Macros.Implements.MacroUtil;

namespace sdmap.unittest.MacroTest
{
    public class ToMacroTest
    {
        [Fact]
        public void NameCanChange()
        {
            var macro = GetTypeMacroMethods(typeof(NameCanChangeImpl))
                .Select(ToSdmapMacro)
                .FirstOrDefault();

            Assert.Equal("NiceName", macro.Name);
        }

        [Fact]
        public void NotNullTest()
        {
            var macro = GetTypeMacroMethods(typeof(NameCanChangeImpl))
                .Select(ToSdmapMacro)
                .FirstOrDefault();

            Assert.NotNull(macro.Arguments);
            Assert.Empty(macro.Arguments);
        }

        [Fact]
        public void DetectArgumentsTest()
        {
            var macro = GetTypeMacroMethods(typeof(DetectArgumentImpl))
                .Select(ToSdmapMacro)
                .FirstOrDefault();

            Assert.Equal(
            [
                SdmapTypes.Syntax, 
                SdmapTypes.Sql
            ], macro.Arguments);
        }
    }
}
