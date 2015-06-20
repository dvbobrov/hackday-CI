namespace Diff2CodeEntityName
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Diff2CodeEntityName._New;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;

    internal class Program
    {
        private static void Main(string[] args) 
        {
            var parser = new FcStreamParser();
//            using (var s = File.OpenRead("diff.txt")) 
//            {
//                foreach (Diff diff in parser.Process(s)) 
//                {
//                    ProcessFile(diff.Left, diff.LeftLines);
//                }
//            }

            var diff = new Diff
                           {
                               Left = "Foo.cs",
                               Right = "Foo1.cs"
                           };

            diff.AddLeft(16);
            diff.AddLeft(17);
            diff.AddLeft(18);
            diff.AddLeft(25);
            diff.AddLeft(26);
            diff.AddLeft(30);
            diff.AddLeft(31);
            diff.AddLeft(32);

            diff.AddRight(16);
            diff.AddRight(17);
            diff.AddRight(18);
            diff.AddRight(19);
            diff.AddRight(26);
            diff.AddRight(27);
            diff.AddRight(28);
            diff.AddRight(32);
            diff.AddRight(33);
            diff.AddRight(34);

            var outputMethods = new HashSet<string>();
            ProcessFile(diff.Left, diff.LeftLines, outputMethods);
            ProcessFile(diff.Right, diff.RightLines, outputMethods);
        }


            //            FileDiff targetFile = new FileDiff();
            //
            //            string path1 =
            //                @"C:\Users\ermakd\Desktop\Hackday\hackday-CI\UnitTestingReducer\Diff2CodeEntityName\Program1.cs";
            //            string path2 =
            //                @"C:\Users\ermakd\Desktop\Hackday\hackday-CI\UnitTestingReducer\Diff2CodeEntityName\Program2.cs";
            //
            //            Process p = new Process();
            //            p.StartInfo.UseShellExecute = false;
            //            p.StartInfo.RedirectStandardOutput = true;
            //            p.StartInfo.FileName = @"C:\Windows\System32\fc.exe";
            //            p.StartInfo.Arguments = @"/N /T /W " + path1 + " " + path2;
            //
            //            p.OutputDataReceived += (s, e) =>
            //                {
            //                    Parse(e.Data, ref targetFile);
            //                    Console.WriteLine(e.Data);
            //                };
            //
            //            p.Start();
            //            p.BeginOutputReadLine();
            //
            //            p.WaitForExit();
            //
            //            string[] changed = GetChangedEntryNames(targetFile);
        //}

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

        private static void Parse(string plainTextLine, ref FileDiff targetFile)
        {
            if (string.IsNullOrEmpty(plainTextLine))
            {
                return;
            }

            if (plainTextLine.StartsWith("Comparing files", true, CultureInfo.InvariantCulture))
            {
                string[] separators = { " and " };
                string[] parts = plainTextLine.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                targetFile.FileName = parts[0].Replace("Comparing files ", ""); // CHECK INDEX BEFORE
            }
            else if (Regex.IsMatch(plainTextLine, @"\s*\d+:")) // e.g. " 13: "
            {
                try
                {
                    uint lineNumber = UInt32.Parse(Regex.Match(plainTextLine, @"\d+:").Value.Replace(":", ""));
                    // e.g. "13:" one digit at least and one colon

                    if (!targetFile.Lines.Contains(lineNumber))
                    {
                        targetFile.Lines.Add(lineNumber);
                    }
                }
                catch (FormatException)
                {
                    //do nothing
                }
            }
        }

        private static string[] GetChangedEntryNames(FileDiff targetFile)
        {
            using (StreamReader reader = new StreamReader(targetFile.FileName))
            {
                uint bracesCounter = 0;

                CodeEntity root = new CodeEntity(CodeEntity.CodeEntityType.Root);
                CodeEntity.CodeEntityType currentLevel = CodeEntity.CodeEntityType.Root;

                do
                {
                    string line = reader.ReadLine().Trim();

                    if (line.LooksLikeNamespaceDefinition())
                    {
                        int a = 1;
                    }
                    else if (line.LooksLikeClassDefinition())
                    {
                        int a = 1;
                    }
                    else if (line.LooksLikeMethodDefinition())
                    {
                        int a = 1;
                    }
                    else if (line.Contains("{"))
                    {
                        bracesCounter++;
                    }
                    else if (line.Contains("}"))
                    {
                        bracesCounter--;
                    }
                }
                while (!reader.EndOfStream);
            }

            return new[] { "", "" };
        }
    }

    internal class FileDiff
    {
        public FileDiff()
        {
            Lines = new List<uint>();
        }

        public string FileName { get; set; }

        public List<uint> Lines { get; set; }
    }
}