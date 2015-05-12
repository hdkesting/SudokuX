using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.GridPatterns
{
    /// <summary>
    /// Creates a pattern that's mirrored both horizontally and vertically.
    /// </summary>
    public class DoubleMirroredPattern : IGridPattern
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
                new Position(max-start.Row, start.Column),
                new Position(start.Row, max-start.Column),
                new Position(max-start.Row, max-start.Column)
            }.Distinct().ToList();
        }
    }
}
