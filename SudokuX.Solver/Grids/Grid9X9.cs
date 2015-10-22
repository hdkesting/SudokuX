using SudokuX.Solver.Core;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 9x9 grid with square blocks.
    /// </summary>
    [System.Serializable]
    public class Grid9X9 : RectangularGrid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Grid9X9"/> class.
        /// </summary>
        public Grid9X9()
            : base(3, 3)
        {
        }

        /// <summary>
        /// Loads the grid from a string version.
        /// </summary>
        /// <param name="challenge">The challenge.</param>
        /// <returns></returns>
        public static Grid9X9 LoadFromString(string challenge)
        {
            var grid = new Grid9X9();
            grid.InitializeFromString(challenge);
            return grid;
        }

        /// <summary>
        /// Clones the board, preserving size and blocks.
        /// </summary>
        /// <returns></returns>
        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Grid9X9();
            CopyChallenge(grid);

            return grid;
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
