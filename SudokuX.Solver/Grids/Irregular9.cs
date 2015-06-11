using SudokuX.Solver.Core;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Creates a 9x9 grid with irregular blocks.
    /// </summary>
    public class Irregular9 : IrregularGrid
    {

        public Irregular9()
            : base(3, 3)
        {
        }

        private Irregular9(IrregularGrid source)
            : base(source)
        {
        }

        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Irregular9(this);
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
