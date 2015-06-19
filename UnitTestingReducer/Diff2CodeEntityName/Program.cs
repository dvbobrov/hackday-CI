﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Diff2CodeEntityName
{
    class Program
    {
        static void Main(string[] args)
        {
            FileDiff targetFile = new FileDiff();

            string path1 = @"C:\Users\ermakd\Desktop\Hackday\hackday-CI\UnitTestingReducer\Diff2CodeEntityName\Program1.cs";
            string path2 = @"C:\Users\ermakd\Desktop\Hackday\hackday-CI\UnitTestingReducer\Diff2CodeEntityName\Program2.cs";

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = @"C:\Windows\System32\fc.exe";
            p.StartInfo.Arguments = @"/N /T /W " + path1 + " " + path2;

            p.OutputDataReceived += (s, e) =>
                {
                    Parse(e.Data, ref targetFile);
                    Console.WriteLine(e.Data);
                };

            p.Start();
            p.BeginOutputReadLine();

            p.WaitForExit();

            // continue
        }

        static void Parse(string plainTextLine, ref FileDiff targetFile)
        {
            if (plainTextLine == null || plainTextLine == string.Empty)
            {
                return;
            }

            if (plainTextLine.StartsWith("comparing files", true, System.Globalization.CultureInfo.InvariantCulture))
            {
                string[] separators = { "comparing files ", " and " };
                string[] parts = plainTextLine.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                targetFile.FileName = parts[0]; // CHECK INDEX BEFORE

            }
            else if (Regex.IsMatch(plainTextLine, @"\s{0,}\d{1,}:")) // e.g. " 13: "
            {
                try
                {
                    uint lineNumber = UInt32.Parse(Regex.Match(plainTextLine, @"\d{1,}:").Value.Replace(":", "")); // e.g. "13:"

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

        static string GetChangedEntryName(FileDiff targetFile)
        {

            return "";
        }
    }

    class FileDiff
    {
        public FileDiff()
        {
            Lines = new List<uint>();
        }

        public string FileName { get; set; }

        public List<uint> Lines { get; set; }
    }
}