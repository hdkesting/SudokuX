using SudokuX.Solver.Core;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 9x9 grid with irregular blocks.
    /// </summary>
    public class Irregular9 : IrregularGrid
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Irregular9"/> class.
        /// </summary>
        public Irregular9()
            : base(3, 3)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Irregular9"/> class, copying the block structure of the supplied grid.
        /// </summary>
        /// <param name="source">The source.</param>
        private Irregular9(IrregularGrid source)
            : base(source)
        {
        }

        /// <summary>
        /// Clones the board, preserving size and blocks.
        /// </summary>
        /// <returns></returns>
        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Irregular9(this);
            CopyChallenge(grid);

            return grid;
        }

        /// <summary>
        /// Gets a value indicating whether this grid has special groups.
        /// </summary>
        /// <value>
        /// <c>true</c> if this grid has special groups; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSpecialGroups
        {
            get { return false; }
        }
    }
}
