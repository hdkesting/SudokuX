using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.SolverStrategies
{
    /// <summary>
    /// Finds unfilled cells with exactly one possibility. That's then the cell's value.
    /// </summary>
    public class NakedSingle : ISolverStrategy
    {
        /// <summary>
        /// Processes the grid and returns any helpful conclusions.
        /// </summary>
        /// <param name="grid">The grid to process.</param>
        /// <returns></returns>
        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            var list = grid.AllCells().ToList()
                .Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Count() == 1)
                .Select(c => new Conclusion(Support.Enums.SolverType.NakedSingle, c, Complexity, c.AvailableValues.Single(), new[] { c }))
                .ToList();

            return list;
        }

        /// <summary>
        /// Gets the complexity-score of this solver (1).
        /// </summary>
        /// <value>
        /// The complexity.
        /// </value>
        public float Complexity
        {
            get { return 1.5f; }
        }
    }
}
