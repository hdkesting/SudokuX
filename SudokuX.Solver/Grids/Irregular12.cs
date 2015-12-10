using SudokuX.Solver.Core;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Tries to create an irregular 12x12 grid. DOES NOT WORK YET (can't get a solution).
    /// </summary>
    [System.Serializable]
    public class Irregular12 : IrregularGrid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Irregular12"/> class.
        /// </summary>
        /// <param name="generateBlocks">if set to <c>true</c> [generate blocks].</param>
        public Irregular12(bool generateBlocks): base(4,3,generateBlocks)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Irregular12"/> class, copying the block structure of the supplied grid.
        /// </summary>
        /// <param name="source">The source.</param>
        public Irregular12(IrregularGrid source): base(source)
        {
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

        /// <summary>
        /// Clones the board, preserving size and blocks.
        /// </summary>
        /// <returns></returns>
        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Irregular12(this);
            CopyChallenge(grid);

            return grid;
        }
    }
}
