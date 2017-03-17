using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using sdmap.Functional;
using sdmap.Macros;
using sdmap.Parser.G4;
using sdmap.Utils;
using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static sdmap.Parser.G4.SdmapParser;

namespace sdmap.Parser.Visitor
{
    internal class BoolVisitor : SdmapParserBaseVisitor<Result>
    {
        private readonly ILGenerator _il;

        public BoolVisitor(ILGenerator il)
        {
            _il = il;
        }

        public override Result VisitBoolNull([NotNull] BoolNullContext context)
        {
            var op = context.children[1].GetText();
            _il.Emit(OpCodes.Ldarg_1);                              // self
            _il.Emit(OpCodes.Ldstr, context.children[0].GetText()); // self propName
            _il.Emit(OpCodes.Call, typeof(IfUtils).GetTypeInfo().GetMethod(
                nameof(IfUtils.LoadProp)));                         // obj
            switch (op)
            {
                case "==":
                    _il.Emit(OpCodes.Ldnull);
                    _il.Emit(OpCodes.Ceq);
                    return Result.Ok();
                case "!=":
                    _il.Emit(OpCodes.Ldnull);
                    _il.Emit(OpCodes.Ceq);
                    _il.Emit(OpCodes.Ldc_I4_0);
                    _il.Emit(OpCodes.Ceq);
                    return Result.Ok();
            }
            return Result.Fail($"Operator '{op}' is not allowed in bool-null expression.");
        }

        public override Result VisitBoolNsSyntax([NotNull] BoolNsSyntaxContext context)
        {
            _il.Emit(OpCodes.Ldarg_1);                              // stack: self
            _il.Emit(OpCodes.Ldstr, context.children[0].GetText()); // stack: self propName
            _il.Emit(OpCodes.Call, typeof(IfUtils).GetTypeInfo().GetMethod(
                nameof(IfUtils.PropertyExistsAndEvalToTrue)));
            return Result.Ok();
        }

        public override Result VisitBoolBrace([NotNull] BoolBraceContext context)
        {
            return Visit(context.boolExpression());
        }
    }
}
