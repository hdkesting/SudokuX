using System;
using System.Collections.Generic;
using SudokuX.Solver.Strategies;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver
{
    public class GridSolver
    {
        private readonly IList<ISolver> _solvers;

        public GridSolver(IList<ISolver> solvers)
        {
            _solvers = solvers;
            Validity = Validity.Maybe;
        }

        public Validity Solve(ISudokuGrid grid)
        {
            var solver = new Strategies.Solver(grid, _solvers);

            GridScore = solver.ProcessSolvers();

            return Validity = grid.IsChallengeDone();
        }

        public int GridScore { get; private set; }

        public Validity Validity { get; private set; }
    }
}
