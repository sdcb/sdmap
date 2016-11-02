using sdmap.Macros.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.SelfTest
{
    public class GetCustomAttributeTest
    {
        [Fact]
        public void GetCustomAttributeCanBeNull()
        {
            var attr = typeof(GetCustomAttributeTest)
                .GetMethod(nameof(GetCustomAttributeCanBeNull))
                .GetCustomAttribute<MacroNameAttribute>();
            Assert.Null(attr);
        }
    }
}
