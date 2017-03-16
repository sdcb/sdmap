using sdmap.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sdmap.Utils
{
    internal class NumberUtil
    {
        public static Result<double> Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Result.Fail<double>($"number literial must not be empty.");

            double n;
            if (double.TryParse(input, out n))
            {
                return Result.Ok(n);
            }
            else
            {
                return Result.Fail<double>($"Literial '{input}' is not valid number format.");
            }
        }
    }
}
