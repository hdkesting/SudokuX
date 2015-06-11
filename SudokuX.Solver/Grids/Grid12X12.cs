using SudokuX.Solver.Core;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 12x12 grid with rectangular blocks.
    /// </summary>
    public class Grid12X12 : RectangularGrid
    {
        public Grid12X12()
            : base(4, 3)
        {

        }

        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Grid12X12();
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
