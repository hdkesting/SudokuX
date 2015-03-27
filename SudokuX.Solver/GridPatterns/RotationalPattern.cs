using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.GridPatterns
{
    /// <summary>
    /// Creates a pattern with rotational symmetry (4 x 90°).
    /// </summary>
    public class RotationalPattern : IGridPattern
    {
        public IEnumerable<Position> GetSymmetricPositions(Position start, int gridSize)
        {
            var max = gridSize - 1;
            return new List<Position>
            {
                start,
                new Position(start.Column, max - start.Row),
                new Position(max - start.Row, max - start.Column),
                new Position(max - start.Column, start.Row)
            }.Distinct().ToList();

        }
    }
}
