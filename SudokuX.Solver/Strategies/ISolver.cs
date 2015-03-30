using System.Collections.Generic;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Strategies
{
    /// <summary>
    /// Interface for executing the solving strategies.
    /// </summary>
    public interface ISolver
    {
        IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid);
    }
}
