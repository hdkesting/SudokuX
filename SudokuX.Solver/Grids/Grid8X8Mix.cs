using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuX.Solver.Core;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// An 8x8 grid with a mix of rows and columns
    /// </summary>
    [System.Serializable]
    public class Grid8X8Mix : IrregularGrid
    {
        public Grid8X8Mix()
            : base(2,4, false)
        {
            SetupBlocks();
        }

        private Grid8X8Mix(IrregularGrid source)
            : base(source)
        {
        }

        public override bool HasSpecialGroups
        {
            get { return false; }
        }

        public override ISudokuGrid CloneBoardAsChallenge()
        {
            var grid = new Grid8X8Mix(this);
            CopyChallenge(grid);

            return grid;
        }

        private void SetupBlocks()
        {
            /*
                AAAA CC DD
                AAAA CC DD
                BBBB CC DD
                BBBB CC DD
                EE FF GGGG
                EE FF GGGG
                EE FF HHHH
                EE FF HHHH
            */
            AttachBlock(0, 0, 0, 4, 2); // A
            AttachBlock(1, 2, 0, 4, 2); // B
            AttachBlock(2, 0, 4, 2, 4); // C
            AttachBlock(3, 0, 6, 2, 4); // D

            AttachBlock(4, 4, 0, 2, 4); // E
            AttachBlock(5, 4, 2, 2, 4); // F
            AttachBlock(6, 4, 4, 4, 2); // G
            AttachBlock(7, 6, 4, 4, 2); // H
        }

        private void AttachBlock(int blockOrdinal, int startRow, int startColumn, int width, int height)
        {
            var block = _blocks.Single(b => b.Ordinal == blockOrdinal);
            for(int r=0; r<height; r++)
            {
                for(int c=0; c<width; c++)
                {
                    var cell = GetCellByRowColumn(r + startRow, c + startColumn);
                    cell.AddToGroups(block);
                }
            }
        }
    }
}
