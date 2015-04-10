using System.Collections.Generic;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Strategies
{
    /// <summary>
    /// Interface for executing the solving strategies.
    /// </summary>
    public interface ISolver
    {
        /// <summary>
        /// Processes the grid and returns any helpful conclusions.
        /// </summary>
        /// <param name="grid">The grid to process.</param>
        /// <returns></returns>
        IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid);
    }
}
