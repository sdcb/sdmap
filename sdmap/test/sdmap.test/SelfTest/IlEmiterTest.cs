using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.SelfTest
{
    public class IlEmiterTest
    {
        [Fact]
        public void HelloWorld()
        {
            var method = new DynamicMethod("Hello", typeof(string), Type.EmptyTypes);
            var il = method.GetILGenerator();

            var str = "Hello World";
            il.Emit(OpCodes.Ldstr, str);
            il.Emit(OpCodes.Ret);

            var action = (Func<string>)method.CreateDelegate(typeof(Func<string>));

            Assert.Equal(str, action());
        }
    }
}
