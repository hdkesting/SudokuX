using SudokuX.Solver.Core;

namespace SudokuX.Solver.Grids
{
    public class Grid8X8Column : RectangularGrid
    {
        public Grid8X8Column() 
            : base(2, 4)
        {
        }

        public override bool HasSpecialGroups
        {
            get { return false; }
        }

        public override bool IsRegular
        {
            get { return true; }
        }

        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Grid8X8Column();
            CopyChallenge(grid);

            return grid;
        }
    }
}
