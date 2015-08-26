using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.Support
{
    /// <summary>
    /// The result after processing.
    /// </summary>
    public struct ProcessResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessResult"/> struct.
        /// </summary>
        /// <param name="score">The score.</param>
        /// <param name="validity">The validity.</param>
        public ProcessResult(int score, Validity validity)
            : this()
        {
            Score = score;
            Validity = validity;
        }

        /// <summary>
        /// Gets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public int Score { get; private set; }

        /// <summary>
        /// Gets the validity.
        /// </summary>
        /// <value>
        /// The validity.
        /// </value>
        public Validity Validity { get; private set; }
    }
}
