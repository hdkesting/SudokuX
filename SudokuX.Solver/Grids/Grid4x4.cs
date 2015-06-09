using SudokuX.Solver.Core;

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

        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Grid4X4();
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
