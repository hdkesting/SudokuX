using SudokuX.Solver.Core;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// An 8x8 grid, organized in rows.
    /// </summary>
    [System.Serializable]
    public class Grid8X8Row : RectangularGrid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Grid8X8Row"/> class.
        /// </summary>
        public Grid8X8Row() 
            : base(4, 2)
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
        /// Gets a value indicating whether this grid is regular.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this grid is regular; otherwise, <c>false</c>.
        /// </value>
        public override bool IsRegular
        {
            get { return true; }
        }

        /// <summary>
        /// Clones the board, preserving size and blocks.
        /// </summary>
        /// <returns></returns>
        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Grid8X8Row();
            CopyChallenge(grid);

            return grid;
        }
    }
}
