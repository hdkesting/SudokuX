namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 9x9 grid with irregular blocks.
    /// </summary>
    public class Irregular9 : IrregularGrid
    {

        public Irregular9()
            : base(3, 3)
        {
        }

        private Irregular9(IrregularGrid source)
            : base(source)
        {
        }

        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Irregular9(this);
            CopyChallenge(grid);

            return grid;
        }

    }
}
