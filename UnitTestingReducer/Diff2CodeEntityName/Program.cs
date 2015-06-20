namespace Diff2CodeEntityName
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Diff2CodeEntityName._New;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;

    internal class Program
    {
        private static void Main(string[] args) 
        {
            var outputMethods = new HashSet<string>();
            var parser = new FcStreamParser();
            using (var s = new StreamReader(Console.OpenStandardInput()))
            {
                foreach (Diff diff in parser.Process(s))
                {
                    ProcessFile(diff.LeftFileName, diff.LeftLines, outputMethods);
                    ProcessFile(diff.RightFileName, diff.RightLines, outputMethods);
                }
            }
        }

        private static void ProcessFile(string fileName, ISet<int> lines, HashSet<string> outputMethods)
        {
            SourceText source;

            using (Stream s = File.OpenRead(fileName))
            {
                source = SourceText.From(s);
            }

            SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
            SyntaxNode root = tree.GetRoot();

            List<Range<int>> lineRanges = BuildLineRanges(lines);

            foreach (MemberDeclarationSyntax member in root.DescendantNodes().OfType<MemberDeclarationSyntax>())
            {
                FileLinePositionSpan span = tree.GetLineSpan(member.Span);
                var range = new Range<int>
                {
                    Minimum = span.StartLinePosition.Line,
                    Maximum = span.EndLinePosition.Line
                };

                var method = member as MethodDeclarationSyntax;

                if (method != null)
                {
                    if (lineRanges.Any(lr => lr.Intersects(range)))
                    {
                        OutputMethodName(method, outputMethods);
                    }
                }

                var ctor = member as ConstructorDeclarationSyntax;
                if (ctor != null)
                {
                    if (lineRanges.Any(lr => lr.Intersects(range)))
                    {
                        OutputMemberFullName(".ctor", member.Ancestors(), outputMethods);
                    }
                }
            }
        }

        private static void OutputMethodName(MethodDeclarationSyntax method, HashSet<string> outputMethods)
        {
            OutputMemberFullName(method.Identifier.ToString(), method.Ancestors(), outputMethods);
        }

        private static void OutputMemberFullName(string selfName, IEnumerable<SyntaxNode> ancestors, HashSet<string> outputMethods)
        {
            Stack<string> fullName = new Stack<string>();
            fullName.Push(selfName);
            foreach (SyntaxNode ancestor in ancestors)
            {
                var cls = ancestor as TypeDeclarationSyntax;
                if (cls != null)
                {
                    fullName.Push(cls.Identifier.ValueText);
                }

                var ns = ancestor as NamespaceDeclarationSyntax;
                if (ns != null)
                {
                    fullName.Push(ns.Name.ToString());
                }
            }

            var builder = new StringBuilder();
            while (fullName.Count != 0)
            {
                builder.Append(fullName.Pop());
                if (fullName.Count != 0)
                {
                    builder.Append(".");
                }
            }

            string name = builder.ToString();
            if (outputMethods.Contains(name))
            {
                return;
            }

            Console.WriteLine(name);
            outputMethods.Add(name);
        }

        private static List<Range<int>> BuildLineRanges(ISet<int> lines)
        {
            var lineRanges = new List<Range<int>>();
            Range<int> curRange = null;

            foreach (int lineNum in lines.OrderBy(i => i))
            {
                if (curRange == null)
                {
                    curRange = new Range<int>
                                   {
                                       Minimum = lineNum,
                                       Maximum = lineNum
                                   };
                }
                else if (lineNum == curRange.Maximum + 1)
                {
                    curRange.Maximum++;
                }
                else
                {
                    lineRanges.Add(curRange);
                    curRange = new Range<int>
                                   {
                                       Minimum = lineNum,
                                       Maximum = lineNum
                                   };
                }
            }

            if (curRange != null)
            {
                lineRanges.Add(curRange);
            }
            return lineRanges;
        }
    }
}