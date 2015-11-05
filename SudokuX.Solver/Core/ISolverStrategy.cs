using System.Collections.Generic;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Core
{
    /// <summary>
    /// Interface for executing the solving strategies.
    /// </summary>
    public interface ISolverStrategy
    {
        /// <summary>
        /// Processes the grid and returns any helpful conclusions.
        /// </summary>
        /// <param name="grid">The grid to process.</param>
        /// <returns></returns>
        IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid);

        /// <summary>
        /// Gets the complexity-score of this solver.
        /// </summary>
        /// <value>
        /// The complexity score.
        /// </value>
        float Complexity { get; }
    }
}
