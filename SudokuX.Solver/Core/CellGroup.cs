using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.Core
{
    /// <summary>
    /// A full group of cells: a row, column, block (rectangular or not)  or diagonal.
    /// </summary>
    public class CellGroup
    {
        private readonly List<Cell> _containedCells;

        /// <summary>
        /// Initializes a new instance of the <see cref="CellGroup"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="ordinal">The ordinal.</param>
        public CellGroup(int size, int ordinal)
        {
            Ordinal = ordinal;
            Size = size;
            _containedCells = new List<Cell>(size);
        }

        /// <summary>
        /// Gets the ordinal.
        /// </summary>
        /// <value>
        /// The ordinal.
        /// </value>
        public int Ordinal { get; private set; }

        /// <summary>
        /// Gets the size of this group.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public int Size { get; private set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the group.
        /// </summary>
        /// <value>
        /// The type of the group.
        /// </value>
        public GroupType GroupType { get; set; }

        /// <summary>
        /// Gets the cells in this group.
        /// </summary>
        /// <value>
        /// The cells.
        /// </value>
        public IList<Cell> Cells
        {
            get { return _containedCells; }
        }

        /// <summary>
        /// Erases the available value from all cells in this group.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool EraseAvailable(int value)
        {
            var done = false;
            foreach (var cell in _containedCells)
            {
                done |= cell.EraseAvailable(value);
            }
            return done;
        }

        /// <summary>
        /// Is the value already present as a given value in this group?
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool ValueWasGiven(int val)
        {
            return _containedCells.Any(c => c.GivenValue == val);
        }

        /// <summary>
        /// Checks whether the supplied value was already given or calculated.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public bool ValueWasGivenOrCalculated(int val)
        {
            return _containedCells.Any(c => c.GivenValue == val || c.CalculatedValue == val);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "CellGroup " + Ordinal + ": " + Name;
        }
    }
}
