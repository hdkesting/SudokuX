using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.NextPositionStrategies
{
    internal static class NextPositionStrategy
    {
        public static int MaxSum(ISudokuGrid grid, IEnumerable<Position> positions)
        {
            return positions.Select(p => grid.GetCellByRowColumn(p.Row, p.Column).AvailableValues.Count).Sum();
        }

        public static int MinCount(ISudokuGrid grid, IEnumerable<Position> positions)
        {
            return positions.Select(p => grid.GetCellByRowColumn(p.Row, p.Column).AvailableValues.Count).Min();
        }
    }
}
