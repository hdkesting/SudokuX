using System;

namespace SudokuX.Solver.Support.Enums
{
    /// <summary>
    /// Extensions for various enums.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the size of the grid.
        /// </summary>
        /// <param name="boardSize">Size of the board.</param>
        /// <returns></returns>
        public static int GridSize(this BoardSize boardSize)
        {
            switch (boardSize)
            {
                case BoardSize.Board4:
                    return 4;
                case BoardSize.Board6:
                case BoardSize.Irregular6:
                    return 6;
                case BoardSize.Board8Column:
                case BoardSize.Board8Row:
                case BoardSize.Board8Mix:
                    return 8;
                case BoardSize.Board9:
                case BoardSize.Irregular9:
                case BoardSize.Board9X:
                case BoardSize.Hyper9:
                    return 9;
                case BoardSize.Board12:
                case BoardSize.Irregular12:
                    return 12;
                case BoardSize.Board16:
                    return 16;
            }

            throw new ArgumentException("Invalid boardsize for GridSize: " + boardSize, "boardSize");
        }

        /// <summary>
        /// Determines whether this board is irregular.
        /// </summary>
        /// <param name="boardSize">Size of the board.</param>
        /// <returns></returns>
        public static bool IsIrregular(this BoardSize boardSize)
        {
            switch (boardSize)
            {
                case BoardSize.Irregular6:
                case BoardSize.Irregular9:
                case BoardSize.Irregular12:
                case BoardSize.Board8Mix:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether this board has diagonals.
        /// </summary>
        /// <param name="boardSize">Size of the board.</param>
        /// <returns></returns>
        public static bool HasDiagonals(this BoardSize boardSize)
        {
            return boardSize == BoardSize.Board9X;
        }

        /// <summary>
        /// Gets the vertical size of a block. 
        /// </summary>
        /// <remarks>Also used for pencilmarks, therefore applicable to irregular grids</remarks>
        /// <param name="boardSize"></param>
        /// <returns></returns>
        public static int BlockWidth(this BoardSize boardSize)
        {
            switch (boardSize)
            {
                case BoardSize.Board4:
                    return 2;

                case BoardSize.Irregular6:
                case BoardSize.Board6:
                case BoardSize.Board9:
                case BoardSize.Irregular9:
                case BoardSize.Board9X:
                case BoardSize.Hyper9:
                    return 3;

                case BoardSize.Board8Column:
                case BoardSize.Board8Row:
                case BoardSize.Board8Mix:
                case BoardSize.Board12:
                case BoardSize.Board16:
                case BoardSize.Irregular12:
                    return 4;
            }

            throw new ArgumentException("Invalid boardsize for BlockWidth: " + boardSize, "boardSize");

        }

        /// <summary>
        /// Gets the horizontal size of a block. 
        /// </summary>
        /// <remarks>Also used for pencilmarks, therefore applicable to irregular grids</remarks>
        /// <param name="boardSize"></param>
        /// <returns></returns>
        public static int BlockHeight(this BoardSize boardSize)
        {
            switch (boardSize)
            {
                case BoardSize.Board4:
                case BoardSize.Board6:
                case BoardSize.Irregular6:
                case BoardSize.Board8Row:
                case BoardSize.Board8Mix:
                case BoardSize.Board8Column:
                    return 2;

                case BoardSize.Board9:
                case BoardSize.Irregular9:
                case BoardSize.Board9X:
                case BoardSize.Hyper9:
                case BoardSize.Board12:
                case BoardSize.Irregular12:
                    return 3;

                case BoardSize.Board16:
                    return 4;
            }

            throw new ArgumentException("Invalid boardsize for BlockHeight: " + boardSize, "boardSize");
        }
    }
}
