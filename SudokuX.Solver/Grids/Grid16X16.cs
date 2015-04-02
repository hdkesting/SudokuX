namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 16x16 grid with square blocks.
    /// </summary>
    public class Grid16X16 : RectangularGrid
    {
        public Grid16X16()
            : base(4, 4)
        {

        }

        /*
         * 0 | 1 | 2 | 3
         * --+---+---+---- 
         * 4 | 5 | 6 | 7
         * --+---+---+----
         * 8 | 9 | A | B
         * --+---+---+---
         * C | D | E | F
         */

        public static Grid16X16 LoadFromString(string challenge)
        {
            var grid = new Grid16X16();
            grid.InitializeFromString(challenge);
            return grid;
        }
    }
}
