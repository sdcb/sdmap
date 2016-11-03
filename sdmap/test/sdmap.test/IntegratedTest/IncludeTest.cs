using sdmap.Parser.Visitor;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.IntegratedTest
{
    public class IncludeTest
    {
        [Fact]
        public void Include()
        {
            var code = "sql v1{#include<v2>} sql v2{Life is good}";
            var rt = new SdmapRuntime();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", null);
            Assert.Equal("Life is good", result);
        }

        [Fact]
        public void MixIncludeWithPlain()
        {
            var code = 
                "sql v1{Hello World #include<v2>}" + 
                "sql v2{Life is good}";
            var rt = new SdmapRuntime();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", null);
            Assert.Equal("Hello World Life is good", result);
        }

        [Fact]
        public void Include2()
        {
            var code =
                "sql v1{#include<v2> #include<v3>}" +
                "sql v2{Life is good}" + 
                "sql v3{Nice!}";
            var rt = new SdmapRuntime();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", null);
            Assert.Equal("Life is good Nice!", result);
        }

        [Fact]
        public void IncludeNested()
        {
            var code =
                "sql v1{#include<v2>#include<v3>}" +
                "sql v2{2#include<v3>}" +
                "sql v3{3}";
            var rt = new SdmapRuntime();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", null);
            Assert.Equal("233", result);
        }
    }
}
