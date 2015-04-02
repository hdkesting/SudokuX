namespace SudokuX.Solver
{
    /// <summary>
    /// A grid with rectangular blocks.
    /// </summary>
    public interface IRegularSudokuGrid : ISudokuGrid
    {
        int BlockWidth { get; }
        int BlockHeight { get; }
        string ToChallengeString();
    }
}
