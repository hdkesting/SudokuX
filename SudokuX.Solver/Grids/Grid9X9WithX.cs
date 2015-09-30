using System.Collections.Generic;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 9x9 grid with square blocks and diagonals.
    /// </summary>
    public class Grid9X9WithX : Grid9X9
    {
        private CellGroup _diagonal1, _diagonal2;

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid9X9WithX"/> class.
        /// </summary>
        public Grid9X9WithX()
        {
            // Grid9x9 constructor has executed by now
            CreateDiags();
        }

        /// <summary>
        /// Clones the board, preserving size and blocks.
        /// </summary>
        /// <returns></returns>
        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Grid9X9WithX();
            CopyChallenge(grid);

            return grid;
        }

        private void CreateDiags()
        {
            // add two "diagonal" groups
            _diagonal1 = new CellGroup(GridSize, 1) { Name = "Diagonal NW-SE", GroupType = GroupType.SpecialLine };
            _diagonal2 = new CellGroup(GridSize, 2) { Name = "Diagonal SW-NE", GroupType = GroupType.SpecialLine };

            // add the existing cells to these groups
            for (int p = 0; p < GridSize; p++)
            {
                var cell = GetCellByRowColumn(p, p);
                cell.AddToGroups(_diagonal1);
                cell.Name += "X";

                cell = GetCellByRowColumn(GridSize - 1 - p, p);
                cell.AddToGroups(_diagonal2);
                cell.Name += "X";
            }
        }

        /// <summary>
        /// Gets all defined groups of cells.
        /// </summary>
        public override IEnumerable<CellGroup> CellGroups
        {
            get
            {
                foreach (var cellGroup in base.CellGroups)
                {
                    yield return cellGroup;
                }

                yield return _diagonal1;
                yield return _diagonal2;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this grid has special groups.
        /// </summary>
        /// <value>
        /// <c>true</c> if this grid has special groups; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSpecialGroups
        {
            get { return true; }
        }
    }
}
