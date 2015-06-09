using SudokuX.Solver.Core;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 6x6 grid with rectangular blocks.
    /// </summary>
    public class Grid6X6 : RectangularGrid
    {
        public Grid6X6()
            : base(3, 2)
        {

        }

        public static Grid6X6 LoadFromString(string challenge)
        {
            var grid = new Grid6X6();
            grid.InitializeFromString(challenge);
            return grid;
        }

        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Grid6X6();
            CopyChallenge(grid);

            return grid;
        }

    }
}
