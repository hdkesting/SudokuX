using System.Collections.Generic;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Grids
{
    public class Grid9X9WithX : Grid9X9
    {
        private CellGroup _diagonal1, _diagonal2;

        public Grid9X9WithX()
        {
            // Grid9x9 constructor has executed by now
            CreateDiags();
        }

        private void CreateDiags()
        {
            _diagonal1 = new CellGroup(GridSize, 1) { Name = "Diagonal NW-SE", GroupType = GroupType.Diagonal };
            _diagonal2 = new CellGroup(GridSize, 2) { Name = "Diagonal SW-NE", GroupType = GroupType.Diagonal };

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
    }
}
