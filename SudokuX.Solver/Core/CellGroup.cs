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

        public CellGroup(int size, int ordinal)
        {
            Ordinal = ordinal;
            Size = size;
            _containedCells = new List<Cell>(size);
        }

        public int Ordinal { get; private set; }

        public int Size { get; private set; }

        public string Name { get; set; }

        public GroupType GroupType { get; set; }

        public IList<Cell> Cells
        {
            get { return _containedCells; }
        }

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

        public bool ValueWasGivenOrCalculated(int val)
        {
            return _containedCells.Any(c => c.GivenValue == val || c.CalculatedValue == val);
        }

        public override string ToString()
        {
            return "CellGroup " + Ordinal + ": " + Name;
        }
    }
}
