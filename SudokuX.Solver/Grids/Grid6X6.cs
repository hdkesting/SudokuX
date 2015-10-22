using SudokuX.Solver.Core;
using System;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 6x6 grid with rectangular blocks.
    /// </summary>
    [Serializable]
    public class Grid6X6 : RectangularGrid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Grid6X6"/> class.
        /// </summary>
        public Grid6X6()
            : base(3, 2)
        {
        }

        /// <summary>
        /// Loads the grid from a serialized version.
        /// </summary>
        /// <param name="challenge">The challenge.</param>
        /// <returns></returns>
        public static Grid6X6 LoadFromString(string challenge)
        {
            var grid = new Grid6X6();
            grid.InitializeFromString(challenge);
            return grid;
        }

        /// <summary>
        /// Clones the board, preserving size and blocks.
        /// </summary>
        /// <returns></returns>
        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Grid6X6();
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
