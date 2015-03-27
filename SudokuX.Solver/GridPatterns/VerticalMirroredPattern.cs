using System.Collections.Generic;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.GridPatterns
{
    /// <summary>
    /// Creates a pattern that's mirrored vertically.
    /// </summary>
    public class VerticalMirroredPattern : IGridPattern
    {
        public IEnumerable<Position> GetSymmetricPositions(Position start, int gridSize)
        {
            return new List<Position>
            {
                start,
                new Position(start.Row, gridSize - 1 - start.Column)
            };
        }
    }
}
