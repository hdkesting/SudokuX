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

        // 0 1 2
        // 3 4 5
        // 6 7 8
        // 9 A B
    }
}
