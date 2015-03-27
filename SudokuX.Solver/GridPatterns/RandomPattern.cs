using System;
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
        public IEnumerable<Position> GetSymmetricPositions(Position start, int gridSize)
        {
            return Enumerable.Repeat(start, 1);
        }
    }
}
