namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 4x4 grid square blocks.
    /// </summary>
    public class Grid4X4 : RectangularGrid
    {
        public Grid4X4()
            : base(2, 2)
        {
        }

        public static Grid4X4 LoadFromString(string challenge)
        {
            var grid = new Grid4X4();
            grid.InitializeFromString(challenge);
            return grid;
        }

    }
}
