using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver
{
    public class GridSolver
    {
        private readonly IList<ISolverStrategy> _solvers;

        public GridSolver(IList<ISolverStrategy> solvers)
        {
            _solvers = solvers;
            Validity = Validity.Maybe;
        }

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

        public double WeightedGridScore { get; private set; }

        public int GridScore { get; private set; }

        public Validity Validity { get; private set; }
    }
}
