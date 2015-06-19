namespace MapAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    internal class Program
    {
        private static List<string> _names = null;

        private static Dictionary<string, List<string>> _namesToTests = null;

        private static string _outputPath = ".\tests.txt";

        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Out.WriteLine(
                    "\nProgram requires 2 parameters:\n\t/namesList:<path_to_list>\n\t/map:<path_to_map>\n\nOptional parameter:\n\t/output:<path_to_output_txt>");
                return;
            }

            if (!ParseParameters(args))
            {
                return;
            }

            CreateListOfTests();
        }

        private static bool ParseParameters(string[] args)
        {
            foreach (string arg in args)
            {
                string paramName = arg.Substring(0, arg.IndexOf(':') + 1);
                string paramVal = arg.Remove(0, arg.IndexOf(':') + 1);
                switch (paramName.ToLowerInvariant())
                {
                    case "/nameslist:":
                        _names = ExtractNames(paramVal);
                        break;
                    case "/map:":
                        _namesToTests = ExtractMap(paramVal);
                        break;
                    case "/output:":
                        _outputPath = paramVal;
                        break;
                    default:
                        break;
                }
            }

            if (_names == null ||
                _namesToTests == null)
            {
                return false;
            }

            return true;
        }

        private static void CreateListOfTests()
        {
            HashSet<string> tests = new HashSet<string>();

            foreach (string name in _names)
            {
                if (_namesToTests.ContainsKey(name))
                {
                    tests.UnionWith(_namesToTests[name]);
                }
            }

            StreamWriter writer = new StreamWriter(_outputPath);

            foreach (string test in tests)
            {
                writer.WriteLine(test);
            }

            writer.Close();
            Console.Out.WriteLine("{0} successfully created");
        }

        /// <summary>
        /// Extracts method/class names from file
        /// </summary>
        /// <param name="pathToList">Path to file that contains list of method/class names</param>
        /// <returns>Returns list of names. Returns null if file not exists.</returns>
        private static List<string> ExtractNames(string pathToList)
        {
            if (!File.Exists(pathToList))
            {
                Console.Error.WriteLine("List with names file does not exists");
                return null;
            }

            HashSet<string> ret = new HashSet<string>();

            StreamReader reader = new StreamReader(pathToList);
            while (!reader.EndOfStream)
            {
                ret.Add(reader.ReadLine());
            }
            reader.Close();

            return ret.ToList();
        }

        /// <summary>
        /// Extracts map (method/class name to list of test) from file
        /// </summary>
        /// <param name="pathToMap">Path to file that contains list of method/class names</param>
        /// <returns>Returns map (name to list of tests). Returns null if file not exists or if file is not XML.</returns>
        private static Dictionary<string, List<string>> ExtractMap(string pathToMap)
        {
            if (!File.Exists(pathToMap))
            {
                Console.Error.WriteLine("Map file does not exists");
                return null;
            }

            if (!pathToMap.ToLowerInvariant().EndsWith(".xml"))
            {
                Console.Error.WriteLine("Map file is not XML file");
                return null;
            }

            Dictionary<string, List<string>> ret = new Dictionary<string, List<string>>();

            try
            {
                XDocument xDoc = XDocument.Load(pathToMap);

                foreach (XElement name in xDoc.Root.Elements())
                {
                    HashSet<string> testNames = new HashSet<string>();

                    foreach (XElement testName in name.Elements())
                    {
                        testNames.Add(testName.Attribute("Name").Value);
                    }

                    ret.Add(name.Attribute("Name").Value, testNames.ToList());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexcepted error while parsing {0} - {1}", pathToMap, e.Message);
                return null;
            }

            return ret;
        }
    }
}