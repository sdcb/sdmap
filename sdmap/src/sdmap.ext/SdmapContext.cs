using System;
using System.Collections.Generic;
using System.Text;

namespace sdmap.ext
{
    public class SdmapContext
    {
        private readonly ISdmapEmiter _sqlEmiter;

        public SdmapContext(ISdmapEmiter sqlEmiter)
        {
            _sqlEmiter = sqlEmiter;
        }

        public string Emit(string statementId, object parameters)
        {
            return _sqlEmiter.Emit(statementId, parameters);
        }
    }
}
