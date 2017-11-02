using System;
using System.Collections.Generic;
using System.Text;

namespace sdmap.Emiter.Implements.CSharp
{
    public static class EmiterProvider
    {
        public static Func<Type, ISdmapEmiter> GetEmiterImplement = DefaultGetService;

        private static ISdmapEmiter DefaultGetService(Type type)
        {
            return (ISdmapEmiter)Activator.CreateInstance(type);
        }

        public static ISdmapEmiter GetEmiter<T>()
        {
            return GetEmiterImplement(typeof(T));
        }
    }
}
