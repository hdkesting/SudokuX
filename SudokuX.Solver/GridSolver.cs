using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Solve(ISudokuGrid grid)
        {
            var solver = new Strategies.Solver(grid, _solvers);

            var result = solver.ProcessSolvers();
            GridScore = result.Score;
            Validity = result.Validity;

            var emptycount = grid.AllCells().Count(c => !c.GivenValue.HasValue);
            if (emptycount > 0)
            {
                WeightedGridScore = ((double)GridScore) / emptycount;
            }
        }

        public double WeightedGridScore { get; private set; }

        public int GridScore { get; private set; }

        public Validity Validity { get; private set; }
    }
}
