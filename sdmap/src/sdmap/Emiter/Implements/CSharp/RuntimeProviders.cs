using sdmap.Macros.Implements;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdmap.Emiter.Implements.CSharp
{
    public static class RuntimeProviders
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

        public static RuntimeMacros RuntimeMacros { get; set; } = new RuntimeMacros();
    }
}
