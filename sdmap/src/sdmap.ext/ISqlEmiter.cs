using System;
using System.Collections.Generic;
using System.Text;

namespace sdmap.ext
{
    public interface ISqlEmiter
    {
        string EmitSql(string sqlMapName, object queryObject);
    }
}
