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
            var result = func(new OneCallContext(ctx, new { A = propValue }));
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ScalarTest(bool expected)
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression("A", ctx);
            var actual = func(new OneCallContext(ctx, new { A = expected }));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BraceTest()
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression("(A)", ctx);
            var actual = func(new OneCallContext(ctx, new { A = true }));
            Assert.Equal(true, actual);
        }

        [Fact]
        public void NsSyntax2Test()
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression("A.B", ctx);
            var actual = func(new OneCallContext(ctx, new { A = new { B = true } }));
            Assert.Equal(true, actual);
        }

        [Fact]
        public void NsSyntax4Test()
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression("A.B.C.D", ctx);
            var actual = func(new OneCallContext(ctx, new
            {
                A = new { B = new { C = new { D = false } } }
            }));
            Assert.Equal(false, actual);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void LiteralTest(string input, bool expected)
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression(input, ctx);
            var actual = func(new OneCallContext(ctx, null));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("true && true", true)]
        [InlineData("true && false", false)]
        [InlineData("false && true", false)]
        [InlineData("false && false", false)]
        public void AndTest(string code, bool expected)
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression(code, ctx);
            var actual = func(new OneCallContext(ctx, null));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("true || true", true)]
        [InlineData("true || false", true)]
        [InlineData("false || true", true)]
        [InlineData("false || false", false)]
        public void OrTest(string code, bool expected)
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression(code, ctx);
            var actual = func(new OneCallContext(ctx, null));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AndShortCircuit()
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression("A.Value && B.Value", ctx);
            var obj = new
            {
                A = new BoolWithAccessCount(false),
                B = new BoolWithAccessCount(true)
            };
            var actual = func(new OneCallContext(ctx, obj));
            Assert.False(actual);
            Assert.Equal(0, obj.B.AccessCount);
            Assert.Equal(1, obj.A.AccessCount);
        }

        [Fact]
        public void OrShortCircuit()
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression("A.Value || B.Value", ctx);
            var obj = new
            {
                A = new BoolWithAccessCount(true),
                B = new BoolWithAccessCount(false)
            };
            var actual = func(new OneCallContext(ctx, obj));
            Assert.True(actual);
            Assert.Equal(0, obj.B.AccessCount);
            Assert.Equal(1, obj.A.AccessCount);
        }

        [Theory]
        [InlineData("!true", false)]
        [InlineData("!false", true)]
        public void NotTest(string code, bool expected)
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression(code, ctx);
            var actual = func(new OneCallContext(ctx, null));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("isEmpty(A)", "", true)]
        [InlineData("isEmpty(A)", null, true)]
        public void IsEmptyString(string code, string value, bool expected)
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression(code, ctx);
            var actual = func(new OneCallContext(ctx, new { A = value }));
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("isEmpty(A)", true, true)]
        [InlineData("isEmpty(A)", false, false)]
        public void IsEmptyArray(string code, bool emptyArray, bool expected)
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression(code, ctx);
            var actual = func(new OneCallContext(ctx, new
            {
                A = emptyArray ? new int[] { } : new[] { 1 }
            }));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsNotEmptyTest()
        {
            var ctx = SdmapCompilerContext.CreateEmpty();
            var func = CompileExpression("isNotEmpty(A)", ctx);
            var actual = func(new OneCallContext(ctx, new { A = new[] { 1 } }));
            Assert.Equal(true, actual);
        }

        private BoolVisitorDelegate CompileExpression(string code, SdmapCompilerContext ctx)
        {
            var dm = new DynamicMethod(
                "test",
                typeof(bool),
                new[] { typeof(OneCallContext) });
            var il = dm.GetILGenerator();

            var visitOk = new BoolVisitor(il).Visit(GetParser(code).boolExpression());
            Assert.True(visitOk.IsSuccess);

            il.Emit(OpCodes.Ret);
            return (BoolVisitorDelegate)dm.CreateDelegate(typeof(BoolVisitorDelegate));
        }

        public delegate bool BoolVisitorDelegate(OneCallContext ctx);

        private class BoolWithAccessCount
        {
            private readonly bool _v;

            public BoolWithAccessCount(bool v)
            {
                _v = v;
            }

            public int AccessCount { get; private set; }

            public bool Value
            {
                get
                {
                    ++AccessCount;
                    return _v;
                }
            }
        }
    }
}
