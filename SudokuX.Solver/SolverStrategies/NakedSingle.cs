﻿using System.Collections.Generic;
using System.Diagnostics;
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
            Debug.WriteLine("Invoking NakedSingle");

            var list = grid.AllCells().ToList()
                .Where(c => !c.HasGivenOrCalculatedValue && c.AvailableValues.Count() == 1)
                .Select(c =>
                {
                    Debug.WriteLine("Found naked single {0} in cell {1}", c.AvailableValues.Single(), c);
                    return new Conclusion(c, Complexity) { ExactValue = c.AvailableValues.Single() };
                })
                .ToList();

            return list;
        }

        /// <summary>
        /// Gets the complexity-score of this solver (1).
        /// </summary>
        /// <value>
        /// The complexity.
        /// </value>
        public int Complexity
        {
            get { return 1; }
        }
    }
}
