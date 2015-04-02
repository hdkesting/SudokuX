namespace SudokuX.Solver.Support
{
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
}
