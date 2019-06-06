using sdmap.Functional;
using sdmap.Parser.G4;
using Antlr4.Runtime.Misc;

namespace sdmap.unittest.VisitorTest.Mocks
{
    public class InnerTravelVisitor : SdmapParserBaseVisitor<Result>
    {
        public bool VisitedNamedSql { get; set; } = false;

        public override Result VisitNamespace([NotNull] SdmapParser.NamespaceContext context)
        {
            return base.VisitNamespace(context);
        }

        public override Result VisitNamedSql([NotNull] SdmapParser.NamedSqlContext context)
        {
            VisitedNamedSql = true;
            return Result.Ok();
        }
    }
}
