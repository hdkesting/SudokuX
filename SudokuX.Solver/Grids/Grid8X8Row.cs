using SudokuX.Solver.Core;

namespace SudokuX.Solver.Grids
{
    public class Grid8X8Row : RectangularGrid
    {
        public Grid8X8Row() 
            : base(4, 2)
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
            var grid = new Grid8X8Row();
            CopyChallenge(grid);

            return grid;
        }
    }
}
