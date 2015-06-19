using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diff2CodeEntityName
{
    class Program
    {
        static void Main(string[] args)
        {
        	int a = 0;
        	int b = 3;
        	a += b;
        }

        static void Foo()
        {
        	await Foo();
        }
    }
}
