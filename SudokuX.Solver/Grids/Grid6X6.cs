namespace SudokuX.Solver.Grids
{
    public class Grid6X6 : RectangularGrid
    {
        public Grid6X6()
            : base(3, 2)
        {

        }

        //protected override int GetBlockOrdinalByRowColumn(int row, int column)
        //{
        //    /*
        //     * NW | NE
        //     * ---+---
        //     * MW | ME
        //     * ---+---
        //     * SW | SE
        //     */

        //    if (row < 2)
        //    {
        //        if (column < 3)
        //        {
        //            return 0; // NW
        //        }
        //        return 1; // NE
        //    }

        //    if (row < 4)
        //    {
        //        if (column < 3)
        //        {
        //            return 2; // MW
        //        }
        //        return 3; // ME
        //    }

        //    if (column < 3)
        //    {
        //        return 4; // SW
        //    }
        //    return 5; // SE

        //}

        public static Grid6X6 LoadFromString(string challenge)
        {
            var grid = new Grid6X6();
            grid.InitializeFromString(challenge);
            return grid;
        }

    }
}
