using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Functional
{
    public class ResultFailedException : Exception
    {
        public ResultFailedException() : base()
        {
        }

        public ResultFailedException(string message) : base(message)
        {
        }
    }
}
