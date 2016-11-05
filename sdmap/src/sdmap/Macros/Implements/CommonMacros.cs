using sdmap.Functional;
using sdmap.Macros.Attributes;
using sdmap.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace sdmap.Macros.Implements
{
    public static class CommonMacros
    {
        [Macro("include")]
        [MacroArguments(SdmapTypes.Syntax)]
        public static Result<string> Include(SdmapContext context, object self, object[] arguments)
        {
            var contextId = (string)arguments[0];
            return context.TryGetEmiter(contextId)
                .OnSuccess(emiter => emiter.Emit(self, context));
        }

        [Macro("val")]
        public static Result<string> ValueItSelf(SdmapContext context, object self, object[] arguments)
        {
            return Result.Ok(self?.ToString() ?? string.Empty);
        }

        [Macro("prop")]
        [MacroArguments(SdmapTypes.Syntax)]
        public static Result<string> Prop(SdmapContext context, object self, object[] arguments)
        {
            if (self == null)
                return Result.Fail<string>("Query object requires not null in macro 'prop'.");
            var syntax = (string)arguments[0];
            var prop = self.GetType().GetTypeInfo().GetProperty(syntax);
            if (prop == null)
                return Result.Fail<string>($"Query object requires property '{syntax}' in macro 'prop'.");
            return Result.Ok(prop.GetValue(self)?.ToString() ?? string.Empty);
        }
    }
}
