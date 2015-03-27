namespace SudokuX.Solver.Grids
{
    public class Grid16X16 : RectangularGrid
    {
        public Grid16X16()
            : base(4, 4)
        {

        }

        //protected override int GetBlockOrdinalByRowColumn(int row, int column)
        //{
        //    /*
        //     * 00 | 01 | 01 | 03
        //     * ---+----+----+---- 
        //     * 10 | 11 | 12 | 13
        //     * ---+----+----+----
        //     * 20 | 21 | 22 | 23
        //     * ---+----+----+----
        //     * 30 | 31 | 32 | 33
        //     */
        //    /*
        //     * 0 | 1 | 2 | 3
        //     * --+---+---+---- 
        //     * 4 | 5 | 6 | 7
        //     * --+---+---+----
        //     * 8 | 9 | A | B
        //     * --+---+---+---
        //     * C | D | E | F
        //     */

        //    if (row < 4)
        //    {
        //        if (column < 4)
        //            return 0;

        //        if (column < 8)
        //            return 1;

        //        if (column < 12)
        //            return 2;

        //        return 3;
        //    }

        //    if (row < 8)
        //    {
        //        if (column < 4)
        //            return 4;

        //        if (column < 8)
        //            return 5;


        //        if (column < 12)
        //            return 6;

        //        return 7;
        //    }

        //    if (row < 12)
        //    {
        //        if (column < 4)
        //            return 8;

        //        if (column < 8)
        //            return 9;


        //        if (column < 12)
        //            return 10;

        //        return 11;
        //    }

        //    if (column < 4)
        //        return 12;

        //    if (column < 8)
        //        return 13;

        //    if (column < 12)
        //        return 14;

        //    return 15;

        //}

        public static Grid16X16 LoadFromString(string challenge)
        {
            var grid = new Grid16X16();
            grid.InitializeFromString(challenge);
            return grid;
        }
    }
}
