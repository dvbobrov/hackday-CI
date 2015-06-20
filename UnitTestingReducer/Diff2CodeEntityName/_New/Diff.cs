namespace Diff2CodeEntityName._New
{
    using System.Collections.Generic;

    public class Diff
    {
        private readonly ISet<int> _leftLines;

        private readonly ISet<int> _rightLines;

        public Diff()
        {
            _leftLines = new HashSet<int>();
            _rightLines = new HashSet<int>();
        }

        public string Left { get; set; }

        public string Right { get; set; }

        public IReadOnlyCollection<int> LeftLines
        {
            get
            {
                return (IReadOnlyCollection<int>)_leftLines;
            }
        }

        public IReadOnlyCollection<int> RightLines
        {
            get
            {
                return (IReadOnlyCollection<int>)_rightLines;
            }
        }

        public void AddLeft(int line)
        {
            _leftLines.Add(line);
        }

        public void AddRight(int line)
        {
            _rightLines.Add(line);
        }

        public override string ToString() 
        {
            return "Left: " + string.Join(",", LeftLines) + " Right: " + string.Join(",", RightLines);
        }
    }
}