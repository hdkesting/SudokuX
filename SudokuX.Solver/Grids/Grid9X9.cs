namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 9x9 grid with square blocks.
    /// </summary>
    public class Grid9X9 : RectangularGrid
    {
        public Grid9X9()
            : base(3, 3)
        {

        }

        public static Grid9X9 LoadFromString(string challenge)
        {
            var grid = new Grid9X9();
            grid.InitializeFromString(challenge);
            return grid;
        }

        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Grid9X9();
            CopyChallenge(grid);

            return grid;
        }

    }
}
