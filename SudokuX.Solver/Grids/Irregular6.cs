using SudokuX.Solver.Core;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 6x6 grid with irregular blocks.
    /// </summary>
    public class Irregular6 : IrregularGrid
    {
        public Irregular6()
            : base(3, 2)
        {
        }

        private Irregular6(IrregularGrid source)
            : base(source)
        {
        }

        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Irregular6(this);
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
            get { return false; }
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
