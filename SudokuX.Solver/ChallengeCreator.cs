using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver
{
    /// <summary>
    /// Create a sudoku challenge.
    /// </summary>
    public class ChallengeCreator
    {
        private readonly BoardSize _boardSize;
        private readonly Difficulty _difficulty;
        private readonly ISudokuGrid _grid;

        private IGridPattern _pattern;
        private IList<ISolverStrategy> _solvers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChallengeCreator" /> class.
        /// </summary>
        /// <param name="boardSize">Size of the board.</param>
        /// <param name="difficulty">The difficulty of the board.</param>
        public ChallengeCreator(BoardSize boardSize, Difficulty difficulty)
        {
            _boardSize = boardSize;
            _difficulty = difficulty;
            _grid = GridConfigurator.Create(_boardSize);

            Setup();
        }

        /// <summary>
        /// Gets the grid containing the challenge (when done).
        /// </summary>
        /// <value>
        /// The grid.
        /// </value>
        public ISudokuGrid Grid { get { return _grid; } }

        /// <summary>
        /// Gets the solvers to use in creating the challenge.
        /// </summary>
        /// <value>
        /// The solvers.
        /// </value>
        public IList<ISolverStrategy> Solvers
        {
            get { return new ReadOnlyCollection<ISolverStrategy>(_solvers); }
        }

        /// <summary>
        /// Gets the solvers really used in this challenge.
        /// </summary>
        /// <value>
        /// The used solvers.
        /// </value>
        public IList<SolverType> UsedSolvers { get; private set; }

        /// <summary>
        /// Sets up the symmetry pattern and the solvers used (and thus the complexity level).
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Unsupported board size.</exception>
        private void Setup()
        {
            _pattern = GridConfigurator.GetGridPattern(_boardSize, _difficulty);
            _solvers = GridConfigurator.GetGridSolvers(_boardSize, _difficulty);
        }

        /// <summary>
        /// Occurs when new progress can be reported.
        /// </summary>
        public event EventHandler<ProgressEventArgs> Progress;

        /// <summary>
        /// Creates the challenge.
        /// </summary>
        /// <param name="rng">The RNG.</param>
        public bool CreateChallenge(Random rng = null)
        {
            if (rng == null)
            {
                var time = (int)DateTime.Now.Ticks;
                Debug.WriteLine("############ RNG seed {0} ###############", time);
                rng = new Random(time);
            }

            return CreateGrid(_grid, _pattern, _solvers, rng);
        }

        /// <summary>
        /// Create a complete grid
        /// </summary>
        /// <param name="grid">IN: empty grid, OUT: full challenge+solution.</param>
        /// <param name="pattern">A symmetry pattern (if any) for the givens.</param>
        /// <param name="solvers">A list of strategies to solve a grid.</param>
        /// <param name="rng">A random number generator</param>
        private bool CreateGrid(ISudokuGrid grid, IGridPattern pattern, IList<ISolverStrategy> solvers, Random rng)
        {
            var sw = Stopwatch.StartNew();

            var builder = new ChallengeBuilder(grid, pattern, solvers, rng);
            builder.Progress += builder_Progress;
            var success = builder.CreateGrid();

            sw.Stop();
            builder.Progress -= builder_Progress;

            Debug.WriteLine("Created a grid in {0} ms with {1} backtracks and {2} values set ({3} full resets):", sw.ElapsedMilliseconds, builder.BackTracks, builder.ValueSets, builder.FullResets);
            UsedSolvers = builder.UsedSolvers;
            DumpGrid(grid);

            return success;
        }

        private void builder_Progress(object sender, ProgressEventArgs e)
        {
            var progress = Progress;

            if (progress != null)
            {
                progress(sender, e);
            }
        }

        [Conditional("DEBUG")]
        private static void DumpGrid(ISudokuGrid grid)
        {
            string s = grid.ToString();
            using (var sr = new StringReader(s))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Debug.WriteLine(line);
                }
            }
        }


    }
}
