using sdmap.Functional;
using System;

namespace sdmap.Utils
{
    internal class DateUtil
    {
        public static Result<DateTime> Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Result.Fail<DateTime>($"Date literial must not be empty.");

            DateTime d;
            if (DateTime.TryParse(input, out d))
            {
                return Result.Ok(d);
            }
            else
            {
                return Result.Fail<DateTime>($"Literial '{input}' is not valid DateTime format.");
            }
        }
    }
}
