using System.Collections.Generic;
namespace Diff2CodeEntityName
{
    class CodeEntity
    {
        public enum CodeEntityType
        {
            Root,
            Namespace,
            Class,
            Method
        };

        public CodeEntity()
        {
            Scope = new Range<uint>();
            Childs = new List<CodeEntity>();
        }

        public CodeEntity(CodeEntityType type) : this()
        {
            Type = type;
        }

        public CodeEntityType Type { get; set; }

        public List<CodeEntity> Childs { get; set; }

        public Range<uint> Scope { get; set; }

        public string FullName { get; set; }
    }
}