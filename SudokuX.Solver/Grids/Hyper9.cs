using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
