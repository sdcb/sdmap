using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace sdmap.test.SelfTest
{
    public class DictionaryTest
    {
        [Fact]
        public void WillThrowWhenDuplicateKey()
        {
            var dic = new Dictionary<string, int>();
            dic.Add("test", 3);
            Assert.Throws<ArgumentException>(() =>
            {
                dic.Add("test", 4);
            });
        }
    }
}
