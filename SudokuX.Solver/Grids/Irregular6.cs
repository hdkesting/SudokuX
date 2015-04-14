namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 6x6 grid with irregular blocks.
    /// </summary>
    public class Irregular6 : IrregularGrid
    {
        public Irregular6()
            : base(3, 2)
        {
        }

        private Irregular6(IrregularGrid source)
            : base(source)
        {
        }

        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Irregular6(this);
            CopyChallenge(grid);

            return grid;
        }

    }
}
