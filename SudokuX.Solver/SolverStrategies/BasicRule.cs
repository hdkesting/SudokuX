using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.SolverStrategies
{
    /// <summary>
    /// Process the One Rule: have just one of each value in each group. 
    /// So remove availables when there is a given or calculated value.
    /// </summary>
    public class BasicRule : ISolverStrategy
    {
        /// <summary>
        /// Processes the grid and returns any helpful conclusions.
        /// </summary>
        /// <param name="grid">The grid to process.</param>
        /// <returns></returns>
        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            // Debug.WriteLine("Invoking BasicRule");

            var result = new List<Conclusion>();

            // check all cells with a value
            // is that still an "available" value in sibling cells? If so, remove that.
            foreach (var cell in grid.AllCells().Where(c => c.GivenOrCalculatedValue.HasValue))
            {
                // ReSharper disable once PossibleInvalidOperationException
                int value = cell.CalculatedValue ?? cell.GivenValue.Value;

                result.AddRange(cell.ContainingGroups
                    .SelectMany(g => g.Cells)
                    .Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Contains(value))
                    .Select(sibling => new Conclusion(Support.Enums.SolverType.Basic, sibling, Complexity, new[] { value }, new[] { cell })));
            }

            return result.Distinct();
        }

        /// <summary>
        /// Gets the complexity-score of this solver (0).
        /// </summary>
        /// <value>
        /// The complexity.
        /// </value>
        public float Complexity
        {
            get { return 0f; }
        }
    }
}
