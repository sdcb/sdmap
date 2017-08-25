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
using System.Diagnostics.Contracts;
using System.Diagnostics;
using System.IO;
using EnvDTE;

namespace sdmap.Vstool.NavigateTo
{
    internal class SdmapIdListener : SdmapParserBaseListener
    {
        private readonly string _toFind;
        private readonly string _toFindI;
        private readonly Stack<string> _nsStack = new Stack<string>();
        private readonly List<NavigateToMatch> _matches = new List<NavigateToMatch>();
        private bool _isNamespace;

        public SdmapIdListener(string sqlIdToFind)
        {
            _toFind = sqlIdToFind;
            _toFindI = sqlIdToFind.ToUpperInvariant();
        }

        public override void EnterNamespace([NotNull] SdmapParser.NamespaceContext context)
        {
            _isNamespace = true;
            base.EnterNamespace(context);
        }

        public override void ExitNsSyntax([NotNull] SdmapParser.NsSyntaxContext context)
        {
            if (_isNamespace)
            {
                _isNamespace = false;
                var text = context.GetText();

                _nsStack.Push(text);

                var match = Match(IdKind.Namespace, text, context);
                if (match.success) _matches.Add(match.match);
            }

            base.ExitNsSyntax(context);
        }

        public override void ExitNamespace([NotNull] SdmapParser.NamespaceContext context)
        {
            Contract.Assert(_nsStack.Count > 0);
            if (_nsStack.Count > 0)
            {
                _nsStack.Pop();
            }

            base.ExitNamespace(context);
        }

        public override void ExitNamedSql([NotNull] SdmapParser.NamedSqlContext context)
        {
            (bool success, NavigateToMatch match) match;

            var syntax = context.SYNTAX().GetText();
            //match = Match(IdKind.NamedSql, syntax, context);
            //if (match.success) _matches.Add(match.match);

            var prefix = string.Join(".", _nsStack.Reverse());
            var nsSyntax = prefix != "" ?
                prefix + "." + syntax :
                syntax;
            match = Match(IdKind.SQL, nsSyntax, context);
            if (match.success) _matches.Add(match.match);

            base.ExitNamedSql(context);
        }

        private (bool success, NavigateToMatch match) Match<T>(
            IdKind idKind,
            string syntax,
            T context)
            where T : ParserRuleContext
        {
            var syntaxI = syntax.ToUpperInvariant();

            if (syntax == _toFind)
            {
                return (true, NavigateToMatch.Create(idKind,
                    syntax, context, MatchKind.Exact, isCaseSensitive: true));
            }
            else if (syntaxI == _toFindI)
            {
                return (true, NavigateToMatch.Create(idKind,
                    syntax, context, MatchKind.Exact, isCaseSensitive: false));
            }
            else if (syntaxI.StartsWith(_toFindI))
            {
                return (true, NavigateToMatch.Create(idKind,
                    syntax, context, MatchKind.Prefix, isCaseSensitive: false));
            }
            else if (syntaxI.Contains(_toFindI))
            {
                return (true, NavigateToMatch.Create(idKind,
                    syntax, context, MatchKind.Substring, isCaseSensitive: false));
            }

            return (false, null);
        }

        public static List<NavigateToMatch> FindMatches(
            ProjectItem projectItem, 
            string searchValue)
        {
            var code = File.ReadAllText(projectItem.FileNames[0]);
            var lexer = new SdmapLexer(new AntlrInputStream(code));
            var parser = new SdmapParser(new CommonTokenStream(lexer));
            var listener = new SdmapIdListener(searchValue);
            parser.AddParseListener(listener);

            try
            {
                parser.root();
            }
            catch (InvalidOperationException e)
                when (e.HResult == -2146233079) // stack empty
            {
            }
            
            foreach (var item in listener._matches)
            {
                item.ProjectItem = projectItem;
            }
            return listener._matches;
        }
    }

    internal enum IdKind
    {
        Namespace,
        SQL,
    }

    internal class NavigateToMatch
    {
        public string MatchedText { get; set; }

        public MatchKind MatchKind { get; set; }

        public IdKind IdKind { get; set; }

        public LineColumn Start { get; set; }

        public LineColumn Stop { get; set; }

        public bool IsCaseSensitive { get; set; }

        public ProjectItem ProjectItem { get; set; }

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

        public override string ToString()
        {
            return $"({Line}:{Column})";
        }
    }
}
