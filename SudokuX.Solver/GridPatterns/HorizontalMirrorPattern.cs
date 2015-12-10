using SudokuX.Solver.Core;
using System.Collections.Generic;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.GridPatterns
{
    /// <summary>
    /// Creates a pattern that's mirrored horizontally.
    /// </summary>
    public class HorizontalMirrorPattern : IGridPattern
    {
        /// <summary>
        /// Get a list of positions according to the pattern.
        /// </summary>
        /// <param name="start">The start position.</param>
        /// <param name="gridSize">Size of the grid.</param>
        /// <returns></returns>
        public IEnumerable<Position> GetSymmetricPositions(Position start, int gridSize)
        {
            return new List<Position>
            {
                start,
                new Position(gridSize - 1 - start.Row, start.Column)
            };
        }
    }
}
