namespace Diff2CodeEntityName._New
{
    using System.Collections.Generic;

    public class Diff
    {
        private readonly ISet<uint> _leftLines;

        private readonly ISet<uint> _rightLines;

        public Diff()
        {
            _leftLines = new HashSet<uint>();
            _rightLines = new HashSet<uint>();
        }

        public string LeftFileName { get; set; }

        public string RightFileName { get; set; }

        public IReadOnlyCollection<uint> LeftLines
        {
            get
            {
                return (IReadOnlyCollection<uint>)_leftLines;
            }
        }

        public IReadOnlyCollection<uint> RightLines
        {
            get
            {
                return (IReadOnlyCollection<uint>)_rightLines;
            }
        }

        public void AddLeft(uint line)
        {
            _leftLines.Add(line);
        }

        public void AddRight(uint line)
        {
            _rightLines.Add(line);
        }

        public override string ToString() 
        {
            return "Left: " + string.Join(",", LeftLines) + " Right: " + string.Join(",", RightLines);
        }
    }
}