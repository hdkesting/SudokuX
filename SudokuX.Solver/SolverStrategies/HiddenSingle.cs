using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.SolverStrategies
{
    /// <summary>
    /// Finds cells that are the only one within a group where some particular value can be.
    /// That is then the value this cell <b>has</b> to be.
    /// </summary>
    public class HiddenSingle : ISolverStrategy
    {
        /// <summary>
        /// Processes the grid and returns any helpful conclusions.
        /// </summary>
        /// <param name="grid">The grid to process.</param>
        /// <returns></returns>
        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            //Debug.WriteLine("Invoking HiddenSingle");
            var list1 = grid.CellGroups
                .SelectMany(g => HiddenSinglesInGroup(g, grid.MinValue, grid.MaxValue))
                .Distinct()
                .ToList();
            // "distinct" because it may be found in both a row and a column

            return list1;
        }

        /// <summary>
        /// Gets the complexity-score of this solver (2).
        /// </summary>
        /// <value>
        /// The complexity.
        /// </value>
        public int Complexity
        {
            get { return 2; }
        }

        private IEnumerable<Conclusion> HiddenSinglesInGroup(CellGroup group, int min, int max)
        {
            for (int val = min; val <= max; val++)
            {
                int found = 0;
                Cell latest = null;
                foreach (var cell in group.Cells)
                {
                    if (cell.CalculatedValue == val || cell.GivenValue == val)
                    {
                        // already have given this value, it can't be a hidden single
                        latest = null;
                        break; // foreach
                    }

                    if (!cell.GivenOrCalculatedValue.HasValue && cell.AvailableValues.Contains(val))
                    {
                        // found this value as available - but it might be elsewhere also
                        latest = cell;
                        found++;
                    }
                }

                if (latest != null && found == 1)
                {
                    // just one possibility found!
                    //Debug.WriteLine("Found hidden single {0} in cell {1} in group {2}", val, latest, group);
                    yield return new Conclusion(latest, Complexity) { ExactValue = val };
                }
            }
        }
    }
}
