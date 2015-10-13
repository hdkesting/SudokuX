using System;
using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support.Enums;
using SudokuX.Solver.Support;

namespace SudokuX.Solver
{
    /// <summary>
    /// Try and solve a supplied grid, using a supplied list of solvers.
    /// </summary>
    public class GridSolver
    {
        private readonly IList<ISolverStrategy> _solvers;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridSolver"/> class.
        /// </summary>
        /// <param name="solvers">The solvers.</param>
        public GridSolver(IList<ISolverStrategy> solvers)
        {
            _solvers = solvers.OrderBy(s => s.Complexity).ToList();
            Validity = Validity.Maybe;
        }

        /// <summary>
        /// Tries to solve the specified grid.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public void Solve(ISudokuGrid grid)
        {
            var solver = new Core.Solver(grid, _solvers);

            var result = solver.ProcessSolvers();
            GridScore = result.Score;
            Validity = result.Validity;

            var emptycount = grid.AllCells().Count(c => !c.GivenValue.HasValue);
            if (emptycount > 0)
            {
                WeightedGridScore = ((double)GridScore) / emptycount;
            }
        }

        /// <summary>
        /// Gets the weighted grid score (=<see cref="GridScore"/> / number of empty cells in challenge).
        /// </summary>
        /// <value>
        /// The weighted grid score.
        /// </value>
        public double WeightedGridScore { get; private set; }

        /// <summary>
        /// Gets the absolute score of the solved grid.
        /// </summary>
        /// <value>
        /// The grid score.
        /// </value>
        public int GridScore { get; private set; }

        /// <summary>
        /// Gets the validity of the solved grid.
        /// </summary>
        /// <value>
        /// The validity.
        /// </value>
        public Validity Validity { get; private set; }

        public Conclusion SolveUntilFirstValue(ISudokuGrid grid)
        {
            var solver = new Core.Solver(grid, _solvers);

            Conclusion result = solver.ProcessSolversUntilFirstValue();
            return result;
        }
    }
}
