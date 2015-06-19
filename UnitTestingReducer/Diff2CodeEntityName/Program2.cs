using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

Namespace Diff2CodeEntityName
{
    namespace Some
    {
    	class Program
    	{
	        static void Main(string[] args)
	        {
	        	int a = 0;
	        	int b = 12;
	        	a += b;
	        }

	        static void Foo()
	        {
	        	await Foo();
	        }
    	}
    }
    
}
