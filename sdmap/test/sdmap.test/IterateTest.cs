using sdmap.Functional;
using sdmap.Macros;
using sdmap.Parser.Visitor;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.IntegratedTest
{
    public class IterateTest
    {
        [Fact]
        public void IterateAll()
        {
            var code = "sql v1{#iterate<',', sql{#prop<Id> #prop<Name>}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new List<object>
            {
                new {Id = 3, Name = "Hello" }, 
                new {Id = 4, Name = "Nice" }
            });
            Assert.Equal("3 Hello,4 Nice", result);
        }

        [Fact]
        public void EachAll()
        {
            var code = "sql v1{#each<A, ',', sql{#prop<Id> #prop<Name>}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("v1", new
            {
                A = new List<object>
                {
                    new {Id = 3, Name = "Hello" },
                    new {Id = 4, Name = "Nice" }
                }
            });
            Assert.Equal("3 Hello,4 Nice", result);
        }
    }
}
