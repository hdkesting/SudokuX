using System.Collections.Generic;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.UI.Common
{
    /// <summary>
    /// A row, column, block or special group that contains a set of cells.
    /// All groups have the same number of cells.
    /// </summary>
    public class Group
    {
        private readonly List<Cell> _containedCells = new List<Cell>();

        /// <summary>
        /// Gets the list of cells, contained in this group.
        /// </summary>
        /// <value>
        /// The contained cells.
        /// </value>
        public List<Cell> ContainedCells { get { return _containedCells; } }

        /// <summary>
        /// Gets the type of the group.
        /// </summary>
        /// <value>
        /// The type of the group.
        /// </value>
        public GroupType GroupType { get; private set; }

        /// <summary>
        /// Gets the ordinal number of this group.
        /// </summary>
        /// <value>
        /// The ordinal.
        /// </value>
        public int Ordinal { get; private set; }

        public Group(GroupType groupType, int ordinal)
        {
            GroupType = groupType;
            Ordinal = ordinal;
        }
    }
}
