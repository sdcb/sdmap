using sdmap.Compiler;
using sdmap.Functional;
using sdmap.Macros.Implements;
using Xunit;

namespace sdmap.unittest.MacroImplTest
{
    public class IsLikeTest
    {
        [Fact]
        public void IsLike()
        {
            var actual = CallIsLike(new { OrderBy = "A.B" }, "OrderBy", @"^\w\.\w$", "Ok");
            Assert.True(actual.IsSuccess);
            Assert.Equal("Ok", actual.Value);
        }

        [Fact]
        public void IsLikeFail()
        {
            var actual = CallIsLike(new { OrderBy = "A.B" }, "OrderBy", @"^\d\.\w$", "Ok");
            Assert.True(actual.IsSuccess);
            Assert.Equal("", actual.Value);
        }

        [Fact]
        public void IsNotLike()
        {
            var actual = CallIsNotLike(new { OrderBy = "A.B" }, "OrderBy", @"^\d\.\w$", "Ok");
            Assert.True(actual.IsSuccess);
            Assert.Equal("Ok", actual.Value);
        }

        [Fact]
        public void IsNotLikeFail()
        {
            var actual = CallIsNotLike(new { OrderBy = "A.B" }, "OrderBy", @"^\w\.\w$", "Ok");
            Assert.True(actual.IsSuccess);
            Assert.Equal("", actual.Value);
        }

        private Result<string> CallIsLike(object self,
            string prop, string regex, string result)
        {
            return DynamicRuntimeMacros.IsLike(OneCallContext.CreateEmpty(),
                "", self, new object[] { prop, regex, result });
        }

        private Result<string> CallIsNotLike(object self,
            string prop, string regex, string result)
        {
            return DynamicRuntimeMacros.IsNotLike(OneCallContext.CreateEmpty(),
                "", self, new object[] { prop, regex, result });
        }
    }
}
