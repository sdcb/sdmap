using sdmap.Compiler;
using sdmap.Functional;
using sdmap.Macros.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.unittest.MacroImplTest
{
    public class GreaterLessTest
    {
        [Theory]
        [InlineData(5, "<=", 5)]
        [InlineData(5, "<" , 6)]
        [InlineData(6, ">" , 5)]
        [InlineData(6, ">=", 6)]
        public void Predicate(int v1, string op, int v2)
        {
            var result = CallCompare(new
            {
                A = v1
            }, op, "A", v2, "OK");
            Assert.True(result.IsSuccess);
            Assert.Equal("OK", result.Value);
        }

        [Theory]
        [InlineData(6, "<=", 5)]
        [InlineData(5, "<", 5)]
        [InlineData(5, ">", 5)]
        [InlineData(4, ">=", 5)]
        public void PredicateFalse(int v1, string op, int v2)
        {
            var result = CallCompare(new
            {
                A = v1
            }, op, "A", v2, "OK");
            Assert.True(result.IsSuccess);
            Assert.Equal("", result.Value);
        }

        [Theory]
        [InlineData("<=")]
        [InlineData("<")]
        [InlineData(">=")]
        [InlineData(">")]
        public void RequirePropNotNull(string op)
        {
            var result = CallCompare(null, op, "A", 3, "OK");
            Assert.False(result.IsSuccess);
        }

        [Theory]
        [InlineData("<=")]
        [InlineData("<")]
        [InlineData(">=")]
        [InlineData(">")]
        public void RequirePropExist(string op)
        {
            var result = CallCompare(new { B = 3 }, op, "A", 3, "OK");
            Assert.False(result.IsSuccess);
        }

        private Result<string> CallCompare(object self, string op, string prop, int val, string result)
        {
            switch (op)
            {
                case ">":
                    return DynamicRuntimeMacros.IsGreaterThan(OneCallContext.CreateEmpty(), 
                        "", self, new object[] { prop, val, result });
                case "<":
                    return DynamicRuntimeMacros.IsLessThan(OneCallContext.CreateEmpty(),
                        "", self, new object[] { prop, val, result });
                case ">=":
                    return DynamicRuntimeMacros.IsGreaterEqual(OneCallContext.CreateEmpty(),
                        "", self, new object[] { prop, val, result });
                case "<=":
                    return DynamicRuntimeMacros.IsLessEqual(OneCallContext.CreateEmpty(),
                        "", self, new object[] { prop, val, result });
                default:
                    throw new ArgumentOutOfRangeException(nameof(op));
            }
        }
    }
}
