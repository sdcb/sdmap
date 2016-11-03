using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
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

        [Fact]
        public void CanMoveReferenceTypeToStack()
        {
            var method = new DynamicMethod("Hello", typeof(string), new[] { typeof(StringBuilder) });
            var il = method.GetILGenerator();

            var str = "Hello World";
            var sb = new StringBuilder();
            sb.Append(str);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Callvirt, 
                typeof(StringBuilder).GetMethod(nameof(StringBuilder.ToString), Type.EmptyTypes));
            il.Emit(OpCodes.Ret);

            var action = (Func<StringBuilder, string>)method.CreateDelegate(typeof(Func<StringBuilder, string>));

            Assert.Equal(str, action(sb));
        }
    }
}
