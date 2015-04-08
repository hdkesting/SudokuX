using System.Collections.Generic;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.UI.Common
{
    /// <summary>
    /// A row, column, block or diagonal that contains a set of cells.
    /// All groups have the same number of cells.
    /// </summary>
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
