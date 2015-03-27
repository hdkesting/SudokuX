using System;
using System.Collections.Generic;
using SudokuX.Solver.Strategies;

namespace SudokuX.Solver.Grids
{
    public class Grid4X4 : RectangularGrid
    {

        private const int Size = 4;

        public Grid4X4()
            : base(2, 2)
        {
        }

        //protected override int GetBlockOrdinalByRowColumn(int row, int column)
        //{
        //    /*
        //     * NW | NE
        //     * ---+---
        //     * SW | SE
        //     */

        //    if (row < 2)
        //    {
        //        if (column < 2)
        //            return 0; // NW
        //        return 1; // NE
        //    }

        //    if (column < 2)
        //        return 2; // SW

        //    return 3; // SE
        //}

        public static Grid4X4 LoadFromString(string challenge)
        {
            var grid = new Grid4X4();
            grid.InitializeFromString(challenge);
            return grid;
        }

    }
}
