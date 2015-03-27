using System.Collections.Generic;
using SudokuX.Solver.Support;

namespace SudokuX.UI.Common
{
    public class Group
    {
        private readonly List<Cell> _containedCells = new List<Cell>();

        public List<Cell> ContainedCells { get { return _containedCells; } }

        public GroupType GroupType { get; private set; }

        public int Ordinal { get; private set; }

        public Group(GroupType groupType, int ordinal)
        {
            GroupType = groupType;
            Ordinal = ordinal;
        }
    }
}
