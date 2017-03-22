using System;
using System.Collections.Generic;
using System.Text;

namespace sdmap.Extensions
{
    public interface ISqlEmiter
    {
        string EmitSql(string sqlId, object queryObject);
    }
}
