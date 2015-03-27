namespace SudokuX.Solver.Grids
{
    public class Grid9X9 : RectangularGrid
    {
        public Grid9X9()
            : base(3, 3)
        {

        }

        //protected override int GetBlockOrdinalByRowColumn(int row, int column)
        //{
        //    /*
        //     * NW | NM | NE
        //     * ---+----+--- 
        //     * MW | MM | ME
        //     * ---+----+---
        //     * SW | SM | SE
        //     */

        //    if (row < 3)
        //    {
        //        if (column < 3)
        //        {
        //            return 0; // NW
        //        }
        //        if (column < 6)
        //        {
        //            return 1; // NM
        //        }
        //        return 2; // NE
        //    }

        //    if (row < 6)
        //    {
        //        if (column < 3)
        //        {
        //            return 3; // MW
        //        }
        //        if (column < 6)
        //        {
        //            return 4; // MM
        //        }
        //        return 5; // ME
        //    }

        //    if (column < 3)
        //    {
        //        return 6; // SW
        //    }
        //    if (column < 6)
        //    {
        //        return 7; // SM
        //    }
        //    return 8; // SE
        //}

        public static Grid9X9 LoadFromString(string challenge)
        {
            var grid = new Grid9X9();
            grid.InitializeFromString(challenge);
            return grid;
        }

    }
}
