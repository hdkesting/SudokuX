using System;
using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.GridPatterns;
using SudokuX.Solver.Strategies;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.NextPositionStrategies
{
    /// <summary>
    /// Selects (groups of) cells with the highest amount of available values
    /// </summary>
    public class HighestNumberAvailable : BaseNextPositionPattern
    {
        public HighestNumberAvailable(ISudokuGrid grid, IGridPattern pattern, IList<ISolver> solvers, Random rng)
            : base(grid, pattern, solvers, rng)
        {
        }

        protected override double CalculateScore(ISudokuGrid grid, IEnumerable<Position> positions)
        {
            return positions.Select(p => grid.GetCellByRowColumn(p.Row, p.Column).AvailableValues.Count).Average();
        }
    }
}
