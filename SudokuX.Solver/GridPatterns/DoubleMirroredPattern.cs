using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.GridPatterns
{
    /// <summary>
    /// Creates a pattern that's mirrored both horizontaly and verticaly.
    /// </summary>
    public class DoubleMirroredPattern : IGridPattern
    {
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
