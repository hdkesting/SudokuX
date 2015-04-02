using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.GridPatterns
{
    /// <summary>
    /// Creates a "pattern" that is random.
    /// </summary>
    public class RandomPattern : IGridPattern
    {
        /// <summary>
        /// Get a list of positions according to the pattern.
        /// </summary>
        /// <param name="start">The start position.</param>
        /// <param name="gridSize">Size of the grid.</param>
        /// <returns></returns>
        public IEnumerable<Position> GetSymmetricPositions(Position start, int gridSize)
        {
            return Enumerable.Repeat(start, 1);
        }
    }
}
