namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 12x12 grid with rectangular blocks.
    /// </summary>
    public class Grid12X12 : RectangularGrid
    {
        public Grid12X12()
            : base(4, 3)
        {

        }

        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Grid12X12();
            CopyChallenge(grid);

            return grid;
        }
    }
}
