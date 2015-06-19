namespace MappingTool
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal class Program
    {
        // Dictionary: < method_name, List<test_name> >
        private static Dictionary<string, List<string>> _mainMap;

        private static void Main(string[] args)
        {
            _mainMap = new Dictionary<string, List<string>>();

            DirectoryInfo directory = new DirectoryInfo(@"C:\HackDay\Input");
            foreach (FileInfo file in directory.EnumerateFiles())
            {
                //Console.WriteLine(file.FullName);

                ProcessCoverReport(file);

                Console.WriteLine("==========");
            }

            foreach (var key in _mainMap.Keys)
            {
                Console.WriteLine("Method: " + key);
                Console.WriteLine("Tests:");

                foreach (var method in _mainMap[key])
                {
                    Console.WriteLine("\t" + method);
                }

                Console.WriteLine();
            }

            Console.ReadLine();
        }

        private static void ProcessCoverReport(FileInfo inputFile)
        {
            using (var str = new StreamReader(inputFile.FullName))
            {
                System.Xml.Serialization.XmlSerializer xSerializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(coverage));

                var cov = (coverage)xSerializer.Deserialize(str);

                if (cov != null)
                {
                    string testName = inputFile.Name.TrimEnd(".xml".ToCharArray());

                    foreach (coverageModule module in cov.module)
                    {
                        foreach (coverageModuleClass @class in module.@class)
                        {
                            foreach (coverageModuleClassMethod method in @class.method)
                            {
                                if (method.instrumented == "true")
                                {
                                    string methodName = @class.name + "." + method.name;

                                    Console.WriteLine(methodName);

                                    if (_mainMap.ContainsKey(methodName))
                                    {
                                        if (!_mainMap[methodName].Contains(testName))
                                        {
                                            _mainMap[methodName].Add(testName);
                                        }
                                    }
                                    else
                                    {
                                        _mainMap[methodName] = new List<string> { testName };
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

//StreamWriter writer = new StreamWriter("newresult.xml");
//System.Xml.Serialization.XmlSerializer xSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Coverage));

//Coverage cov = new Coverage();
//cov.module = new List<Module>();
//cov.module.Add(new Module() { xmlClass = new List<Class>() { new Class() { method = new List<Method>() { new Method() } } } });

//xSerializer.Serialize(writer, cov);

//ResultSet res = (ResultSet)xSerializer.Deserialize(str);

//foreach (ResultSetResult r in res.Result)
//{
//    Console.WriteLine(r.Title);
//    Console.WriteLine(r.Summary);
//    Console.WriteLine();
//}

//writer.Close();