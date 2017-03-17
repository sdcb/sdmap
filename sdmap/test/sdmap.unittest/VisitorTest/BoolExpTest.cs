using sdmap.Compiler;
using sdmap.Parser.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.unittest.VisitorTest
{
    public class BoolExpTest : VisitorTestBase
    {
        [Theory]
        [InlineData(null, "== null", true)]
        [InlineData(null, "!= null", false)]
        [InlineData("33", "== null", false)]
        [InlineData("33", "!= null", true)]
        public void IsNullTest(string propValue, string op, bool expected)
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression($"A {op}", ctx);
            var result = func(ctx, new { A = propValue });
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ScalarTest(bool expected)
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression("A", ctx);
            var actual = func(ctx, new { A = expected });
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BraceTest()
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression("(A)", ctx);
            var actual = func(ctx, new { A = true });
            Assert.Equal(true, actual);
        }

        private BoolVisitorDelegate CompileExpression(string code, SdmapCompilerContext ctx)
        {
            var dm = new DynamicMethod(
                "test",
                typeof(bool),
                new[] { typeof(SdmapCompilerContext), typeof(object) });
            var il = dm.GetILGenerator();

            var visitOk = new BoolVisitor(il).Visit(GetParser(code).boolExpression());
            Assert.True(visitOk.IsSuccess);

            il.Emit(OpCodes.Ret);
            return (BoolVisitorDelegate)dm.CreateDelegate(typeof(BoolVisitorDelegate));
        }

        public delegate bool BoolVisitorDelegate(SdmapCompilerContext ctx, object obj);
    }
}
