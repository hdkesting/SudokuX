namespace SudokuX.Solver.Core
{
    /// <summary>
    /// A grid with rectangular blocks.
    /// </summary>
    public interface IRegularSudokuGrid : ISudokuGrid
    {
        /// <summary>
        /// Gets the width of the block.
        /// </summary>
        /// <value>
        /// The width of the block.
        /// </value>
        int BlockWidth { get; }

        /// <summary>
        /// Gets the height of the block.
        /// </summary>
        /// <value>
        /// The height of the block.
        /// </value>
        int BlockHeight { get; }

        /// <summary>
        /// Creates a serialized version of the challenge.
        /// </summary>
        /// <returns></returns>
        string ToChallengeString();
    }
}
