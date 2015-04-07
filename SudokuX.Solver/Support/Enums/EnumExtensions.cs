using System.ComponentModel;

namespace SudokuX.Solver.Support.Enums
{
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
                case BoardSize.Board6Irregular:
                    return 6;
                case BoardSize.Board9:
                case BoardSize.Board9Irregular:
                case BoardSize.Board9X:
                    return 9;
                case BoardSize.Board12:
                    return 12;
                case BoardSize.Board16:
                    return 16;
            }

            throw new InvalidEnumArgumentException("boardSize", (int)boardSize, typeof(BoardSize));
        }

        public static bool IsIrregular(this BoardSize boardSize)
        {
            switch (boardSize)
            {
                case BoardSize.Board6Irregular:
                case BoardSize.Board9Irregular:
                    return true;
            }

            return false;
        }

        public static bool HasDiagonals(this BoardSize boardSize)
        {
            return boardSize == BoardSize.Board9X;
        }
    }
}
