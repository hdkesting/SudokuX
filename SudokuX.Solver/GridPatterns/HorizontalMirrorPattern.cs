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
