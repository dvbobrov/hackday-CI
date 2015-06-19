using System.Collections.Generic;
namespace Diff2CodeEntityName
{
    abstract class CodeEntity
    {
        public CodeEntity()
        {
            Scope = new Range<uint>();
        }

        public virtual Range<uint> Scope { get; set; }

        public virtual string FullName { get; set; }
    }


    class Namespace
    {
        public List<Class> Classes { get; set; }
    }

    class Class
    {
        public List<Method> Methods { get; set; }
    }

    class Method : CodeEntity
    {

    }
}