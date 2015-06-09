using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.GridPatterns
{
    /// <summary>
    /// Creates a pattern with rotational symmetry (2 x 180°).
    /// </summary>
    public class Rotational2Pattern : IGridPattern
    {
        /// <summary>
        /// Get a list of positions according to the pattern.
        /// </summary>
        /// <param name="start">The start position.</param>
        /// <param name="gridSize">Size of the grid.</param>
        /// <returns></returns>
        public IEnumerable<Position> GetSymmetricPositions(Position start, int gridSize)
        {
            var max = gridSize - 1;
            return new List<Position>
            {
                start,
                new Position(max - start.Row, max - start.Column)
            }.Distinct().ToList();

        }
    }
}
