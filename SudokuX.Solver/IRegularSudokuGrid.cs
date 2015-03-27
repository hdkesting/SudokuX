namespace SudokuX.Solver
{
    public interface IRegularSudokuGrid : ISudokuGrid
    {
        int BlockWidth { get; }
        int BlockHeight { get; }
        string ToChallengeString();
    }
}
