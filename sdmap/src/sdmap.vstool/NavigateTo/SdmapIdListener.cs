using sdmap.Parser.G4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Microsoft.VisualStudio.Language.NavigateTo.Interfaces;

namespace sdmap.Vstool.NavigateTo
{
    internal class SdmapIdListener : SdmapParserBaseListener
    {
        private readonly string _sqlIdToFind;
        public readonly List<NavigateToMatch> _matches = new List<NavigateToMatch>();

        public SdmapIdListener(string sqlIdToFind)
        {
            _sqlIdToFind = sqlIdToFind;
        }

        public override void ExitNamespace([NotNull] SdmapParser.NamespaceContext context)
        {
            var match = Match(IdKind.Namespace, t => t.nsSyntax().GetText(), context);
            if (match.success) _matches.Add(match.match);

            base.ExitNamespace(context);
        }

        public override void ExitNamedSql([NotNull] SdmapParser.NamedSqlContext context)
        {
            var match = Match(IdKind.NamedSql, x => x.SYNTAX().GetText(), context);
            if (match.success) _matches.Add(match.match);

            base.ExitNamedSql(context);
        }

        private (bool success, NavigateToMatch match) Match<T>(
            IdKind idKind,
            Func<T, string> syntaxGetter, 
            T context)
            where T : ParserRuleContext
        {
            var syntax = syntaxGetter(context);
            var toFind = _sqlIdToFind;
            var syntaxI = syntax.ToUpperInvariant();
            var toFindI = syntax.ToUpperInvariant();

            if (syntax == toFind)
            {
                return (true, NavigateToMatch.Create(idKind, 
                    syntax, context, MatchKind.Exact, isCaseSensitive: true));
            }
            else if (syntaxI == toFindI)
            {
                return (true, NavigateToMatch.Create(idKind,
                    syntax, context, MatchKind.Exact, isCaseSensitive: false));
            }
            else if (syntaxI.StartsWith(toFindI))
            {
                return (true, NavigateToMatch.Create(idKind,
                    syntax, context, MatchKind.Prefix, isCaseSensitive: false));
            }
            else if (syntaxI.Contains(toFindI))
            {
                return (true, NavigateToMatch.Create(idKind,
                    syntax, context, MatchKind.Substring, isCaseSensitive: false));
            }

            return (false, null);
        }

        public static List<NavigateToMatch> FindMatches(string code, string searchValue)
        {
            var lexer = new SdmapLexer(new AntlrInputStream(code));
            var parser = new SdmapParser(new CommonTokenStream(lexer));
            var listener = new SdmapIdListener(searchValue);
            parser.AddParseListener(listener);
            parser.root();
            return listener._matches;
        }
    }

    internal enum IdKind
    {
        Namespace, 
        NamedSql, 
    }

    internal class NavigateToMatch
    {
        public string MatchedText { get; set; }

        public MatchKind MatchKind { get; set; }

        public IdKind IdKind { get; set; }

        public LineColumn Start { get; set; }

        public LineColumn Stop { get; set; }

        public bool IsCaseSensitive { get; set; }

        public static NavigateToMatch Create(
            IdKind kind, 
            string syntax, 
            ParserRuleContext ctx, 
            MatchKind matchKind, 
            bool isCaseSensitive)
        {
            return new NavigateToMatch
            {
                IdKind = kind, 
                MatchKind = matchKind,
                MatchedText = syntax,
                Start = new LineColumn(ctx.Start.Line, ctx.start.Column),
                Stop = new LineColumn(ctx.Stop.Line, ctx.stop.Column), 
                IsCaseSensitive = isCaseSensitive, 
            };
        }

        public NavigateToItem ToNavigateToItem(INavigateToItemDisplayFactory displayFactory)
        {
            return new NavigateToItem(
                name: MatchedText, 
                kind: IdKind.ToString(), 
                language: "sdmap", 
                secondarySort: null, 
                tag: this, 
                matchKind: MatchKind, 
                isCaseSensitive: IsCaseSensitive, 
                displayFactory: displayFactory);
        }
    }

    internal struct LineColumn
    {
        public int Line;
        public int Column;

        public LineColumn(int line, int column)
        {
            Line = line;
            Column = column;
        }
    }
}
