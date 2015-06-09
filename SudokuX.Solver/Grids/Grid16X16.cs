using SudokuX.Solver.Core;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 16x16 grid with square blocks.
    /// </summary>
    public class Grid16X16 : RectangularGrid
    {
        public Grid16X16()
            : base(4, 4)
        {

        }

        /*
         * 0 | 1 | 2 | 3
         * --+---+---+---- 
         * 4 | 5 | 6 | 7
         * --+---+---+----
         * 8 | 9 | A | B
         * --+---+---+---
         * C | D | E | F
         */

        public static Grid16X16 LoadFromString(string challenge)
        {
            var grid = new Grid16X16();
            grid.InitializeFromString(challenge);
            return grid;
        }

        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Grid16X16();
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
