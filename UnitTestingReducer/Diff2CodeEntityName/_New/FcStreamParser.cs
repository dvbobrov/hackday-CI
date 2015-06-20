using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Diff2CodeEntityName._New
{
    class FcStreamParser
    {
        readonly List<Diff> _allFilesDiff = new List<Diff>();

        public IEnumerable<Diff> Process(StreamReader reader)
        {
            ParseStream(reader);

            return _allFilesDiff;
        }

        private void ParseStream(StreamReader reader)
        {
            Diff fileDiff = null;
            string blockOfDiffSeparator = "*****";

            bool leftFileIsCurrent = false;

            do
            {
                string plainTextLine = reader.ReadLine();

                if (string.IsNullOrEmpty(plainTextLine))
                {
                    continue;
                }

                if (plainTextLine.StartsWith("Comparing files", true, CultureInfo.InvariantCulture) || reader.EndOfStream)
                {
                    if (fileDiff != null)
                    {
                        _allFilesDiff.Add(fileDiff);
                    }

                    if (!reader.EndOfStream)
                    {
                        fileDiff = new Diff();
                    }
                }
                else if (plainTextLine.StartsWith(blockOfDiffSeparator))
                {
                    string fileName = plainTextLine.Substring(blockOfDiffSeparator.Length).Trim();

                    if (fileName.IsNullOrEmpty())
                    {
                        continue;
                    }
                    else if (fileDiff.LeftFileName.IsNullOrEmpty())
                    {
                        fileDiff.LeftFileName = fileName;
                        leftFileIsCurrent = true;
                    }
                    else if (fileDiff.RightFileName.IsNullOrEmpty())
                    {
                        fileDiff.RightFileName = fileName;
                        leftFileIsCurrent = false;
                    }
                    else if (string.Compare(fileDiff.LeftFileName, fileName, true) == 0)
                    {
                        leftFileIsCurrent = true;
                    }
                    else
                    {
                        leftFileIsCurrent = false;
                    }

                }
                else if (Regex.IsMatch(plainTextLine, @"\s*\d+:")) // e.g. " 13: "
                {
                    try
                    {
                        int lineNumber = Int32.Parse(Regex.Match(plainTextLine, @"\d+:").Value.Replace(":", ""));
                        // e.g. "13:" one digit at least and one colon

                        if (leftFileIsCurrent)
                        {
                            fileDiff.AddLeft(lineNumber);
                        }
                        else
                        {
                            fileDiff.AddRight(lineNumber);
                        }

                    }
                    catch (FormatException)
                    {
                        //do nothing
                    }
                }

            } while (!reader.EndOfStream);
        }
    }
}
