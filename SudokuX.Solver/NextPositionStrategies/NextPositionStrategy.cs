using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.NextPositionStrategies
{
    /// <summary>
    /// How should I select the next position where to place a given value? 
    /// </summary>
    internal static class NextPositionStrategy
    {
        /// <summary>
        /// Counts the total number of available values in the list.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="positions">The positions.</param>
        /// <returns></returns>
        public static int MaxSum(ISudokuGrid grid, IEnumerable<Position> positions)
        {
            return positions.Select(p => grid.GetCellByRowColumn(p.Row, p.Column).AvailableValues.Count).Sum();
        }

        /// <summary>
        /// Selects the minimum count of available values from the group.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="positions">The positions.</param>
        /// <returns></returns>
        public static int MinCount(ISudokuGrid grid, IEnumerable<Position> positions)
        {
            return positions.Select(p => grid.GetCellByRowColumn(p.Row, p.Column).AvailableValues.Count).Min();
        }

        /// <summary>
        /// Selects the maximum count of available values from the group.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="positions">The positions.</param>
        /// <returns></returns>
        public static int MaxCount(ISudokuGrid grid, IEnumerable<Position> positions)
        {
            return positions.Select(p => grid.GetCellByRowColumn(p.Row, p.Column).AvailableValues.Count).Max();
        }

        /// <summary>
        /// Just selects 1, any group is just as good as any other.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="positions">The positions.</param>
        /// <returns></returns>
        public static int Any(ISudokuGrid grid, IEnumerable<Position> positions)
        {
            return 1;
        }
    }
}
