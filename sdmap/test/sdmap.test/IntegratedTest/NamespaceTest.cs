using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.IntegratedTest
{
    public class NamespaceTest
    {
        [Fact]
        public void CanReferenceOtherInOneNamespace()
        {
            var code = "namespace ns{sql v1{1#include<v2>} sql v2{2}}";
            var rt = new SdmapRuntime();
            rt.AddSourceCode(code);
            var result = rt.Emit("ns.v1", new { A = true });
            Assert.Equal("12", result);
        }

        [Fact]
        public void CanCombineTwoNs()
        {
            var code = 
                "namespace ns1{sql sql{1#include<ns2.sql>}} \r\n" + 
                "namespace ns2{sql sql{2}}";
            var rt = new SdmapRuntime();
            rt.AddSourceCode(code);
            var result = rt.Emit("ns1.sql", null);
            Assert.Equal("12", result);
        }
    }
}
