using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.Support
{
    public struct ProcessResult
    {
        public ProcessResult(int score, Validity validity)
            : this()
        {
            Score = score;
            Validity = validity;
        }

        public int Score { get; private set; }
        public Validity Validity { get; private set; }
    }
}
