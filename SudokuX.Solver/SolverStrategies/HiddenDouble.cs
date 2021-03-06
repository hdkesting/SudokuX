﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.SolverStrategies
{
    /// <summary>
    /// Find a set of two numbers that are only in two cells: the other numbers in those two cells are not valid
    /// </summary>
    public class HiddenDouble : ISolverStrategy
    {
        /// <summary>
        /// Processes the grid and returns any helpful conclusions.
        /// </summary>
        /// <param name="grid">The grid to process.</param>
        /// <returns></returns>
        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            //Debug.WriteLine("Invoking HiddenDouble");

            var result = new List<Conclusion>();
            foreach (var group in grid.CellGroups)
            {
                var list = FindHiddenDoubles(group, grid.MinValue, grid.MaxValue).ToList();
                result.AddRange(list);
            }

            return result;
        }

        /// <summary>
        /// Gets the complexity-score of this solver (4).
        /// </summary>
        /// <value>
        /// The complexity.
        /// </value>
        public float Complexity
        {
            get { return 4f; }
        }

        private IEnumerable<Conclusion> FindHiddenDoubles(CellGroup group, int min, int max)
        {
            // find all given or calculated values in this group
            var knownvalues = group.Cells
                                .Where(c => c.GivenOrCalculatedValue.HasValue)
                                .Select(c => c.GivenOrCalculatedValue)
                                .ToList();

            // loop through all possible pairs of values
            // first value just short of the maximum value
            for (int v1 = min; v1 < max; v1++)
            {
                if (knownvalues.Contains(v1))
                    continue; // already known - forget about it

                // second value is always higher than the first - so no double checks
                for (int v2 = v1 + 1; v2 <= max; v2++)
                {
                    if (knownvalues.Contains(v2))
                        continue; // already known - forget about it

                    // v1 < v2
                    int[] potentialDouble = { v1, v2 };

                    // count the number of cells that contain at least one of the values of the pair
                    int count = group.Cells.Count(cell => !cell.GivenOrCalculatedValue.HasValue && cell.AvailableValues.Intersect(potentialDouble).Any());

                    if (count == 2)
                    {
                        // exactly two cells found with this pair of numbers is a possibility - that's a double!
                        // but is it still hidden between other possibilities?
                        var pair = group.Cells
                                        .Where(cell => !cell.GivenOrCalculatedValue.HasValue && cell.AvailableValues.Intersect(potentialDouble).Any())
                                        .ToArray();

                        var reasons = group.Cells.Where(c => !c.GivenOrCalculatedValue.HasValue && c != pair[0] && c != pair[1]).ToList();
                        var concl1 = new Conclusion(Support.Enums.SolverType.HiddenDouble, pair[0], Complexity, pair[0].AvailableValues.Except(potentialDouble), reasons);
                        var concl2 = new Conclusion(Support.Enums.SolverType.HiddenDouble, pair[1], Complexity, pair[1].AvailableValues.Except(potentialDouble), reasons);

                        if (concl1.ExcludedValues.Any() || concl2.ExcludedValues.Any())
                        {
                            //Debug.WriteLine("Hidden double found: {0} and {1} in group {2}", potentialDouble[0], potentialDouble[1], group);

                            if (concl1.ExcludedValues.Any())
                                yield return concl1;
                            if (concl2.ExcludedValues.Any())
                                yield return concl2;
                        }
                    }
                }
            }
        }
    }
}
