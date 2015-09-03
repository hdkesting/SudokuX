﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using SudokuX.Solver.Core;
using SudokuX.Solver.GridPatterns;
using SudokuX.Solver.SolverStrategies;
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
        private readonly ISudokuGrid _grid;

        private IGridPattern _pattern;
        private List<ISolverStrategy> _solvers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChallengeCreator"/> class.
        /// </summary>
        /// <param name="boardSize">Size of the board.</param>
        public ChallengeCreator(BoardSize boardSize)
        {
            _boardSize = boardSize;
            _grid = GridCreator.Create(_boardSize);

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
        /// Sets up the symmetry pattern and the solvers used (and thus the complexity level).
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Unsupported board size.</exception>
        private void Setup()
        {
            switch (_boardSize)
            {
                case BoardSize.Board4:
                    _pattern = new RandomPattern();
                    _solvers = new List<ISolverStrategy>
                        {
                            new NakedSingle(), 
                            new HiddenSingle()
                        };
                    break;

                case BoardSize.Board6:
                    _pattern = new RandomPattern();
                    _solvers = new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new NakedDouble(),
                            new HiddenDouble()
                        };
                    break;

                case BoardSize.Irregular6:
                    _pattern = new RandomPattern();
                    _solvers = new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle()
                        };
                    break;

                case BoardSize.Board9:
                    _pattern = new Rotational2Pattern();
                    _solvers = new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new NakedTriple(),
                            new XWing(),
                            new SolveWithColors()
                        };
                    break;

                case BoardSize.Board9X:
                    _pattern = new DoubleMirroredPattern();
                    _solvers = new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new HiddenTriple(),
                            new SolveWithColors()
                        };
                    break;

                case BoardSize.Irregular9:
                    _pattern = new RandomPattern();
                    _solvers = new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new HiddenTriple()
                        };
                    break;

                case BoardSize.Hyper9:
                    _pattern = new RandomPattern();
                    _solvers = new List<ISolverStrategy>
                    {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new HiddenTriple()                        
                    };
                    break;

                case BoardSize.Board12:
                    _pattern = new Rotational4Pattern();
                    _solvers = new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            //new LockedCandidates(),
                            new NakedDouble(),
                            new HiddenDouble(),
                            new NakedTriple(),
                            new SolveWithColors()
                        };
                    break;

                case BoardSize.Board16:
                    _pattern = new Rotational4Pattern();
                    _solvers = new List<ISolverStrategy>
                        {
                            new NakedSingle(),
                            new HiddenSingle(),
                            new LockedCandidates(),
                            new NakedDouble(),
                            //new HiddenDouble(), 
                            //new NakedTriple(),
                            new SolveWithColors()
                        };
                    break;

                default:
                    throw new InvalidOperationException("Unsupported board size: " + _boardSize);
            }

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

            var strategy = new ChallengeBuilder(grid, pattern, solvers, rng);
            strategy.Progress += strategy_Progress;
            var success = strategy.CreateGrid();

            sw.Stop();
            strategy.Progress -= strategy_Progress;

            Debug.WriteLine("Created a grid in {0} ms with {1} backtracks and {2} values set ({3} full resets):", sw.ElapsedMilliseconds, strategy.BackTracks, strategy.ValueSets, strategy.FullResets);
            DumpGrid(grid);

            return success;
        }

        private void strategy_Progress(object sender, ProgressEventArgs e)
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
