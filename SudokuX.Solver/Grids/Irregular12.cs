using SudokuX.Solver.Core;

namespace SudokuX.Solver.Grids
{
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

        public override bool HasSpecialGroups
        {
            get { return false; }
        }

        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Irregular12(this);
            CopyChallenge(grid);

            return grid;
        }
    }
}
