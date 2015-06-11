using System.Collections.Generic;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.Grids
{
    public class Hyper9 : Grid9X9
    {
        private CellGroup _blockNw, _blockNe, _blockSe, _blockSw;

        public Hyper9()
        {
            CreateHyperBlocks();
        }

        private void CreateHyperBlocks()
        {
            _blockNw = new CellGroup(GridSize, 1) { Name = "Hyper NW", GroupType = GroupType.Special };
            _blockNe = new CellGroup(GridSize, 2) { Name = "Hyper NE", GroupType = GroupType.Special };
            _blockSe = new CellGroup(GridSize, 3) { Name = "Hyper SE", GroupType = GroupType.Special };
            _blockSw = new CellGroup(GridSize, 4) { Name = "Hyper SW", GroupType = GroupType.Special };

            AddCells(_blockNw, 1, 1);
            AddCells(_blockNe, 5, 1);
            AddCells(_blockSe, 5, 5);
            AddCells(_blockSw, 1, 5);
        }

        private void AddCells(CellGroup cellGroup, int startrow, int startcol)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    var cell = GetCellByRowColumn(row + startrow, col + startcol);
                    cell.AddToGroups(cellGroup);
                }
            }
        }

        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Hyper9();
            CopyChallenge(grid);

            return grid;
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

                yield return _blockNw;
                yield return _blockNe;
                yield return _blockSw;
                yield return _blockSe;
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
