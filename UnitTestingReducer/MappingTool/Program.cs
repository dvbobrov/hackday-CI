using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace EmcHack1
{
    class Program
    {
        // Dictionary: < method_name, List<test_name> >
        static Dictionary<string, List<string>> MainMap;

        static void Main(string[] args)
        {
            MainMap = new Dictionary<string, List<string>>();

            DirectoryInfo directory = new DirectoryInfo(@"C:\HackDay\Input");
            foreach (var file in directory.EnumerateFiles())
            {
                //Console.WriteLine(file.FullName);

                ProcessCoverReport(file);

                Console.WriteLine("==========");
            }

            foreach (var key in MainMap.Keys)
            {
                Console.WriteLine("Method: " + key);
                Console.WriteLine("Tests:");

                foreach (var method in MainMap[key])
                {
                    Console.WriteLine("\t" + method);
                }

                Console.WriteLine();
            }

            Console.ReadLine();
        }

        static void ProcessCoverReport(FileInfo inputFile)
        {
            StreamReader str = new StreamReader(inputFile.FullName);
            System.Xml.Serialization.XmlSerializer xSerializer = new System.Xml.Serialization.XmlSerializer(typeof(coverage));

            coverage cov = (coverage)xSerializer.Deserialize(str);

            if (cov != null)
            {
                string testName = inputFile.Name.TrimEnd(".xml".ToCharArray());

                foreach (var module in cov.module)
                {
                    foreach (var @class in module.@class)
                    {
                        foreach (var method in @class.method)
                        {
                            if (method.instrumented == "true")
                            {
                                string methodName = @class.name + "." + method.name;

                                Console.WriteLine(methodName);

                                if (MainMap.ContainsKey(methodName))
                                {
                                    if (!MainMap[methodName].Contains(testName))
                                    {
                                        MainMap[methodName].Add(testName);
                                    }
                                }
                                else
                                {
                                    MainMap[methodName] = new List<string> { testName };
                                }
                            }
                        }
                    }
                }
            }

            str.Close();
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
