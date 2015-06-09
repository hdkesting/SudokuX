using System.Collections.Generic;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Core
{
    /// <summary>
    /// Supplies positions for clues in a specific pattern.
    /// </summary>
    public interface IGridPattern
    {
        /// <summary>
        /// Get a list of positions according to the pattern.
        /// </summary>
        /// <param name="start">The start position.</param>
        /// <param name="gridSize">Size of the grid.</param>
        /// <returns></returns>
        IEnumerable<Position> GetSymmetricPositions(Position start, int gridSize);

    }
}
