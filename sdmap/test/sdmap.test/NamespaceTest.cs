using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.IntegratedTest
{
    public class NamespaceTest
    {
        [Fact]
        public void CanReferenceOtherInOneNamespace()
        {
            var code = 
                "namespace ns{sql v1{1#include<v2>} \r\n" + 
                "sql v2{2}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("ns.v1", new { A = true });
            Assert.Equal("12", result);
        }

        [Fact]
        public void CanCombineTwoNs()
        {
            var code = 
                "namespace ns1{sql v1{1#include<ns2.v2>}} \r\n" + 
                "namespace ns2{sql v2{2}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("ns1.v1", null);
            Assert.Equal("12", result);
        }

        [Fact]
        public void CanNestNamespace()
        {
            var code =
                "namespace ns1{namespace ns2{sql v1{Hello}}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("ns1.ns2.v1", null);
            Assert.Equal("Hello", result);
        }

        [Fact]
        public void CanNestNamespaceUsingDot()
        {
            var code =
                "namespace ns1.ns2{sql v1{Hello}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("ns1.ns2.v1", null);
            Assert.Equal("Hello", result);
        }

        [Fact]
        public void CanFindInParentScope()
        {
            var code =
                "namespace ns1{" + 
                " namespace ns2{sql v1{#include<test>}}" + 
                " sql test{Hello}" +
                "}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("ns1.ns2.v1", null);
            Assert.Equal("Hello", result);
        }

        [Fact]
        public void CanFindInParentScopeUsingDot()
        {
            var code =
                "namespace ns1.ns2{sql v1{#include<test>}}" +
                "namespace ns1{sql test{Hello}}";
            var rt = new SdmapCompiler();
            rt.AddSourceCode(code);
            var result = rt.Emit("ns1.ns2.v1", null);
            Assert.Equal("Hello", result);
        }
    }
}
