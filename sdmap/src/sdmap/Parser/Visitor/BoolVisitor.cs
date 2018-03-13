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
using sdmap.Macros.Implements;

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
            _il.Emit(OpCodes.Ldarg_0);                              // ctx
            _il.Emit(OpCodes.Call, OneCallContext.GetObj);     // self
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

        public override Result VisitBoolBool([NotNull] BoolBoolContext context)
        {
            var op = context.children[1].GetText();
            _il.Emit(OpCodes.Ldarg_0);                              // ctx
            _il.Emit(OpCodes.Call, OneCallContext.GetObj);     // self
            _il.Emit(OpCodes.Ldstr, context.children[0].GetText()); // self propName
            _il.Emit(OpCodes.Call, typeof(IfUtils).GetTypeInfo().GetMethod(
                nameof(IfUtils.LoadProp)));                         // obj

            var boolVal = bool.Parse(context.Bool().GetText());
            var boolOpcode = boolVal ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0;
            switch (op)
            {
                case "==":
                    _il.Emit(boolOpcode);
                    _il.Emit(OpCodes.Ceq);
                    return Result.Ok();
                case "!=":
                    _il.Emit(boolOpcode);
                    _il.Emit(OpCodes.Ceq);
                    _il.Emit(OpCodes.Ldc_I4_0);
                    _il.Emit(OpCodes.Ceq);
                    return Result.Ok();
            }
            return Result.Fail($"Operator '{op}' is not allowed in bool-bool expression.");
        }

        public override Result VisitBoolNsSyntax([NotNull] BoolNsSyntaxContext context)
        {
            _il.Emit(OpCodes.Ldarg_0);                              // ctx
            _il.Emit(OpCodes.Call, OneCallContext.GetObj);     // self
            _il.Emit(OpCodes.Ldstr, context.children[0].GetText()); // self propName
            _il.Emit(OpCodes.Call, typeof(IfUtils).GetTypeInfo().GetMethod(
                nameof(IfUtils.PropertyExistsAndEvalToTrue)));
            return Result.Ok();
        }

        public override Result VisitBoolLeteral([NotNull] BoolLeteralContext context)
        {
            if (bool.TryParse(context.GetText(), out bool b))
            {
                if (b)
                    _il.Emit(OpCodes.Ldc_I4_1);
                else
                    _il.Emit(OpCodes.Ldc_I4_0);
                return Result.Ok();
            }
            
            return Result.Fail($"Failed to parse '{context.GetText()}' as bool");
        }

        public override Result VisitBoolOpAnd([NotNull] BoolOpAndContext context)
        {
            var end = _il.DefineLabel();
            var gofalse = _il.DefineLabel();

            var exp1 = Visit(context.boolExpression()[0]);
            if (exp1.IsFailure) return exp1;
            _il.Emit(OpCodes.Brfalse_S, gofalse);

            var exp2 = Visit(context.boolExpression()[1]);
            if (exp2.IsFailure) return exp2;
            _il.Emit(OpCodes.Brfalse_S, gofalse);

            _il.Emit(OpCodes.Ldc_I4_1);
            _il.Emit(OpCodes.Br_S, end);

            _il.MarkLabel(gofalse);
            _il.Emit(OpCodes.Ldc_I4_0);

            _il.MarkLabel(end);
            return Result.Ok();
        }

        public override Result VisitBoolOpOr([NotNull] BoolOpOrContext context)
        {
            var end = _il.DefineLabel();
            var gotrue = _il.DefineLabel();

            var exp1 = Visit(context.boolExpression()[0]);
            if (exp1.IsFailure) return exp1;
            _il.Emit(OpCodes.Brtrue_S, gotrue);

            var exp2 = Visit(context.boolExpression()[1]);
            if (exp2.IsFailure) return exp2;
            _il.Emit(OpCodes.Brtrue_S, gotrue);

            _il.Emit(OpCodes.Ldc_I4_0);
            _il.Emit(OpCodes.Br_S, end);

            _il.MarkLabel(gotrue);
            _il.Emit(OpCodes.Ldc_I4_1);

            _il.MarkLabel(end);
            return Result.Ok();
        }

        public override Result VisitBoolOpNot([NotNull] BoolOpNotContext context)
        {
            var exp = Visit(context.boolExpression());
            if (exp.IsFailure) return exp;
            _il.Emit(OpCodes.Ldc_I4_0);
            _il.Emit(OpCodes.Ceq);
            return Result.Ok();
        }

        public override Result VisitBoolFunc([NotNull] BoolFuncContext context)
        {
            var syntax = context.GetToken(SYNTAX, 0).GetText();
            var exps = context.nsSyntax();

            foreach (var exp in exps)
            {
                _il.Emit(OpCodes.Ldarg_0);                              // ctx
                _il.Emit(OpCodes.Call, OneCallContext.GetObj);     // self
                _il.Emit(OpCodes.Ldstr, exp.GetText());                 // self prop
                _il.Emit(OpCodes.Call, typeof(DynamicRuntimeMacros).GetMethod(
                    nameof(DynamicRuntimeMacros.GetPropValue)));        // val
            }

            switch (syntax)
            {
                case "isEmpty":
                    if (exps.Length != 1) break;
                    _il.Emit(OpCodes.Call, typeof(IfUtils).GetTypeInfo()
                        .GetMethod(nameof(IfUtils.IsEmpty)));
                    return Result.Ok();
                case "isNotEmpty":
                    if (exps.Length != 1) break;
                    _il.Emit(OpCodes.Call, typeof(IfUtils).GetTypeInfo()
                        .GetMethod(nameof(IfUtils.IsEmpty)));
                    _il.Emit(OpCodes.Ldc_I4_0);
                    _il.Emit(OpCodes.Ceq);
                    return Result.Ok();
            }
            return Result.Fail(
                $"Function '{syntax}' with {exps.Length} arguments is not supported in bool expression.");
        }

        public override Result VisitBoolBrace([NotNull] BoolBraceContext context)
        {
            return Visit(context.boolExpression());
        }
    }
}
