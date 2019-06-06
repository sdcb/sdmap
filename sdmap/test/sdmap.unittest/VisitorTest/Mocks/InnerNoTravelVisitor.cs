using sdmap.Functional;
using sdmap.Parser.G4;
using Antlr4.Runtime.Misc;

namespace sdmap.unittest.VisitorTest.Mocks
{
    public class InnerNoTravelVisitor : SdmapParserBaseVisitor<Result>
    {
        public bool VisitedNamedSql { get; set; } = false;

        public override Result VisitNamespace([NotNull] SdmapParser.NamespaceContext context)
        {
            return Result.Ok();
        }

        public override Result VisitNamedSql([NotNull] SdmapParser.NamedSqlContext context)
        {
            VisitedNamedSql = true;
            return Result.Ok();
        }
    }
}
