namespace Diff2CodeEntityName._New
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    internal class FcStreamParser
    {
        public IEnumerable<Diff> Process(Stream stream)
        {
            var lexer = new FcOutputLexer(new AntlrInputStream(stream));
            var tokens = new CommonTokenStream(lexer);
            var parser = new FcOutputParser(tokens);

            FcOutputParser.FcOutputContext context = parser.fcOutput();
            return context.Accept(new SingleDiffVisitor());
        }
    }

    class SingleDiffVisitor : FcOutputBaseVisitor<IEnumerable<Diff>>
    {
        public override IEnumerable<Diff> VisitSingleDiff(FcOutputParser.SingleDiffContext context)
        {
            if (context.children.Count > 3 && context.children[3] is FcOutputParser.SingleDiffItemContext)
            {
                var diff = new Diff();
                SetNames(context.children[3], diff);

                foreach (var item in context.children.Skip(3).Cast<FcOutputParser.SingleDiffItemContext>())
                {
                    var leftDiffList = item.GetChild<FcOutputParser.DiffLineListContext>(0);
                    var rightDiffList = item.GetChild<FcOutputParser.DiffLineListContext>(1);

                    HandleDiffList(leftDiffList, diff.AddLeft);
                    HandleDiffList(rightDiffList, diff.AddRight);
                }
                
                yield return diff;
            }
        }

        private static void SetNames(IParseTree item, Diff diff)
        {
            IParseTree firstNameToken = item.GetChild(1);
            string firstName = firstNameToken.GetText();
            diff.Left = firstName;

            IParseTree secondNameToken = item.GetChild(5);
            string secondName = secondNameToken.GetText();
            diff.Right = secondName;
        }

        private static void HandleDiffList(FcOutputParser.DiffLineListContext diffList, Action<int> handler)
        {
            foreach (var diffLine in diffList.children.Cast<FcOutputParser.DiffLineContext>())
            {
                handler(int.Parse(diffLine.GetChild(0).GetText()));
            }
        }

        protected override IEnumerable<Diff> AggregateResult(IEnumerable<Diff> aggregate, IEnumerable<Diff> nextResult)
        {
            if (aggregate != null)
            {
                foreach (Diff fileDiff in aggregate)
                {
                    yield return fileDiff;
                }
            }

            if (nextResult != null)
            {
                foreach (Diff fileDiff in nextResult)
                {
                    yield return fileDiff;
                }
            }
        }
    }
}