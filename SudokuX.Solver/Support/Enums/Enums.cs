namespace SudokuX.Solver.Support.Enums
{
    /// <summary>
    /// The size of the board.
    /// </summary>
    public enum BoardSize
    {
        /// <summary>
        /// A 4x4 board of 2x2 blocks
        /// </summary>
        Board4,

        /// <summary>
        /// A 6x6 board of 3x2 blocks
        /// </summary>
        Board6,

        /// <summary>
        /// A 9x9 board of 3x3 blocks
        /// </summary>
        Board9,

        /// <summary>
        /// A 9x9 board of 3x3 blocks with 2 diagonals
        /// </summary>
        Board9X,

        /// <summary>
        /// A 12x12 board of 4x3 blocks
        /// </summary>
        Board12,

        /// <summary>
        /// A 16x16 board of 4x4 blocks
        /// </summary>
        Board16,

        /// <summary>
        /// An irregular 9x9 board
        /// </summary>
        Board9Irregular,

        /// <summary>
        /// An irregular 6x6 board
        /// </summary>
        Board6Irregular
    }

    /// <summary>
    /// The validity of a calculated sudoku challenge.
    /// </summary>
    public enum Validity
    {
        /// <summary>
        /// The challenge is invalid.
        /// </summary>
        Invalid,

        /// <summary>
        /// The challenge may be valid, but needs extra clues.
        /// </summary>
        Maybe,

        /// <summary>
        /// The challenge is fully valid. This can be solved.
        /// </summary>
        Full
    }

    /// <summary>
    /// The type of cell-group.
    /// </summary>
    public enum GroupType
    {
        Other,
        Row,
        Column,
        Block,
        Diagonal
    }

}
