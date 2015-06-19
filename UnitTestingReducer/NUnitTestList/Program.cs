namespace NUnitTestList
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using NUnit.Framework;

    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("usage: NUnitTestList DLL1 DLL2 ...");
                return;
            }

            foreach (var dll in args)
            {
                var assembly = Assembly.LoadFile(Path.GetFullPath(dll));
                var testMethods = assembly.GetTypes().SelectMany(a => a.GetMethods()).Where(HasTestAttribute);
                foreach (var method in testMethods)
                {
                    Console.WriteLine(method.DeclaringType.FullName + "." + method.Name);
                }
            }
        }

        private static bool HasTestAttribute(MethodInfo m)
        {
            var attributes = m.GetCustomAttributes(typeof(TestAttribute));
            return attributes != null && attributes.Any();
        }
    }
}