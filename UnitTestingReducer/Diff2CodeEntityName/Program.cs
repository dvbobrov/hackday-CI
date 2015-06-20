namespace Diff2CodeEntityName
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    using Diff2CodeEntityName._New;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var parser = new FcStreamParser();
            using (var s = File.OpenText("diff.txt"))
            {
                foreach (Diff diff in parser.Process(s)) 
                {
                }
            }
        }

        //private static void Parse(string plainTextLine, ref FileDiff targetFile)
        //{
        //    if (string.IsNullOrEmpty(plainTextLine))
        //    {
        //        return;
        //    }

        //    if (plainTextLine.StartsWith("Comparing files", true, CultureInfo.InvariantCulture))
        //    {
        //        string[] separators = { " and " };
        //        string[] parts = plainTextLine.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        //        targetFile.FileName = parts[0].Replace("Comparing files ", ""); // CHECK INDEX BEFORE
        //    }
        //    else if (Regex.IsMatch(plainTextLine, @"\s*\d+:")) // e.g. " 13: "
        //    {
        //        try
        //        {
        //            uint lineNumber = UInt32.Parse(Regex.Match(plainTextLine, @"\d+:").Value.Replace(":", ""));
        //            // e.g. "13:" one digit at least and one colon

        //            if (!targetFile.Lines.Contains(lineNumber))
        //            {
        //                targetFile.Lines.Add(lineNumber);
        //            }
        //        }
        //        catch (FormatException)
        //        {
        //            //do nothing
        //        }
        //    }
        //}
    }
}