using SudokuX.Solver.Core;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 6x6 grid with irregular blocks.
    /// </summary>
    public class Irregular6 : IrregularGrid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Irregular6"/> class.
        /// </summary>
        public Irregular6()
            : base(3, 2)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Irregular6"/> class, copying the block structure of the supplied grid.
        /// </summary>
        /// <param name="source">The source.</param>
        private Irregular6(IrregularGrid source)
            : base(source)
        {
        }

        /// <summary>
        /// Clones the board, preserving size and blocks.
        /// </summary>
        /// <returns></returns>
        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Irregular6(this);
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
