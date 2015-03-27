using System;

namespace SudokuX.Solver.Grids
{
    public class Grid12X12 : RectangularGrid
    {
        public Grid12X12()
            : base(4, 3)
        {

        }

        //protected override int GetBlockOrdinalByRowColumn(int row, int column)
        //{
        //    // 0 1 2
        //    // 3 4 5
        //    // 6 7 8
        //    // 9 A B
        //    if (row < 3)
        //    {
        //        if (column < 4)
        //            return 0;
        //        if (column < 8)
        //            return 1;
        //        return 2;
        //    }

        //    if (row < 6)
        //    {
        //        if (column < 4)
        //            return 3;
        //        if (column < 8)
        //            return 4;
        //        return 5;

        //    }

        //    if (row < 9)
        //    {
        //        if (column < 4)
        //            return 6;
        //        if (column < 8)
        //            return 7;
        //        return 8;
        //    }

        //    if (column < 4)
        //        return 9;
        //    if (column < 8)
        //        return 10;
        //    return 11;
        //}
    }
}
