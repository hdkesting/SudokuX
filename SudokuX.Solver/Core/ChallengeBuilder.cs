﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SudokuX.Solver.GridPatterns;
using SudokuX.Solver.NextPositionStrategies;
using SudokuX.Solver.SolverStrategies;
using SudokuX.Solver.Support;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.Core
{
    /// <summary>
    /// Baseclass that tries to find an optimal set of next positions for a challenge.
    /// </summary>
    internal sealed class ChallengeBuilder
    {
        private readonly ISudokuGrid _grid;
        private readonly IGridPattern _pattern;
        private readonly Random _rng;
        private readonly Stack<SelectedValue> _stack = new Stack<SelectedValue>();
        private readonly Queue<Position> _nextQueue = new Queue<Position>();
        private readonly Solver _solver;

        public ChallengeBuilder(ISudokuGrid grid, IGridPattern pattern, IList<ISolverStrategy> solvers, Random rng)
        {
            _grid = grid;
            _pattern = pattern;
            _rng = rng;
            _solver = new Solver(_grid, solvers);

            _scoreCalculator = NextPositionStrategy.MaxSum;

            // _scoreCalculator =  positions => grid.GridSize - positions.Select(p => grid.GetCellByRowColumn(p.Row, p.Column).AvailableValues.Count).Average();
        }



        public event EventHandler<ProgressEventArgs> Progress;

        private void SolverProgress(object sender, ProgressEventArgs e)
        {
            var handler = Progress;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private void OnProgress()
        {
            var handler = Progress;
            if (handler != null)
            {
                var e = new ProgressEventArgs
                {
                    Calculated = _grid.AllCells().Count(c => c.HasGivenOrCalculatedValue),
                    Given = _grid.AllCells().Count(c => c.GivenValue.HasValue),
                    Total = _grid.GridSize * _grid.GridSize
                };
                handler(this, e);
            }

        }

        public int BackTracks { get; private set; }

        public int ValueSets { get; private set; }

        public int FullResets { get; private set; }


        private Func<ISudokuGrid, IEnumerable<Position>, int> _scoreCalculator;
        /// <summary>
        /// Gets the score calculator for a position list. Higher = better.
        /// </summary>
        /// <value>
        /// The score calculator.
        /// </value>
        private Func<ISudokuGrid, IEnumerable<Position>, int> CalculateScore { get { return _scoreCalculator; } }

        public void CreateGrid()
        {
            // process solvers
            // if error, backtrack
            //    top from stack 
            //    get next available, push rest back on stack
            //    no available, next from stack
            // if not done, get new position
            //    queue is empty: get a group, put in queue
            //    get first from queue
            // repeat until done

            var swProcess = new Stopwatch();
            var swBacktrack = new Stopwatch();
            var swSelect = new Stopwatch();
            var swCheck = new Stopwatch();

            while (true)
            {
                swProcess.Start();
                var result = _solver.ProcessSolvers();
                swProcess.Stop();

                switch (result.Validity)
                {
                    case Validity.Full:
                        CompleteSymmetry();
                        double total = 0.0;
                        foreach (var measureKvp in _solver.Measurements)
                        {
                            Debug.WriteLine("Solver {0} used {1} times, found {2} conclusions, took {3}, average {4} ms",
                                measureKvp.Key,
                                measureKvp.Value.Invocations,
                                measureKvp.Value.ResultCount,
                                measureKvp.Value.TimeSpent,
                                measureKvp.Value.TimeSpent.TotalMilliseconds / measureKvp.Value.Invocations);
                            total += measureKvp.Value.TimeSpent.TotalMilliseconds;
                        }

                        Debug.WriteLine("Total in solvers: {0:N} ms", total);

                        Debug.WriteLine("Timings: process {0:N} ms, backtrack {1:N} ms, select extra {2:N} ms, check {3:N} ms",
                            swProcess.ElapsedMilliseconds, swBacktrack.ElapsedMilliseconds, swSelect.ElapsedMilliseconds, swCheck.ElapsedMilliseconds);

                        return; // done!

                    case Validity.Invalid:
                        swBacktrack.Start();
                        PerformBackTrack();
                        swBacktrack.Stop();
                        break;

                    case Validity.Maybe:
                        if (_grid.GetPercentageDone() > 0.5)
                        {
                            _scoreCalculator = NextPositionStrategy.MinCount;
                        }
                        else
                        {
                            _scoreCalculator = NextPositionStrategy.MaxSum;
                        }
                        swSelect.Start();
                        SelectExtraGiven();
                        swSelect.Stop();
                        OnProgress();
                        break;
                }

            }
        }

        [Conditional("DEBUG")]
        private void TestGrid()
        {
            var testgrid = _grid.CloneBoardAsChallenge();
            var solver = new GridSolver(new ISolverStrategy[] { new NakedSingle() });
            solver.Solve(testgrid);
            Trace.WriteLine(solver.Validity);
        }

        /// <summary>
        /// Add extra givens to complete the symmetry, when the challenge is complete.
        /// </summary>
        private void CompleteSymmetry()
        {
            while (_nextQueue.Count > 0)
            {
                var pos = _nextQueue.Dequeue();

                var cell = _grid.GetCellByRowColumn(pos.Row, pos.Column);
                // ReSharper disable once PossibleInvalidOperationException
                cell.SetGivenValue(cell.CalculatedValue.Value);
            }
        }

        /// <summary>
        /// Select a new given value for the challenge.
        /// </summary>
        private void SelectExtraGiven()
        {
            // get a position
            var pos = GetNextPosition();
            if (pos == null)
            {
                return; // or throw?
            }

            var cell = _grid.GetCellByRowColumn(pos.Row, pos.Column);

            // and select a value, if possible (if not, then the grid is illegal)
            SelectValueForCell(cell);
        }

        private void SelectValueForCell(Cell cell)
        {
            if (cell.AvailableValues.Any())
            {
                // select a random available value
                var val = cell.AvailableValues[_rng.Next(cell.AvailableValues.Count)];
                // and remove it from the list
                cell.EraseAvailable(val);
                Debug.WriteLine(">> Setting {0} to value {1}", cell, val);
                cell.SetGivenValue(val);
                _stack.Push(new SelectedValue(cell, cell.AvailableValues));

                ResetGrid(_grid);  // earlier conclusions may not be valid anymore, so erase them
                ValueSets++;
            }
        }

        private void PerformBackTrack()
        {
            Rewind();
            Debug.WriteLine("Rewound, {0} givens left", CountGivens(_grid));
            BackTracks++;
            var s = _grid.ToString();
            if (BackTracks > 1000)
            {
                BackTracks = 0;
                FullResets++;
                Debug.WriteLine("Giving up on this track, resetting all (#{0})", FullResets);
                Cleargrid(_grid);
            }
        }

        private void Cleargrid(ISudokuGrid grid)
        {
            foreach (var cell in grid.AllCells().Where(c => c.GivenValue.HasValue))
            {
                cell.Reset(true);
            }
            ResetGrid(_grid);
            _nextQueue.Clear();
        }

        private int CountGivens(ISudokuGrid grid)
        {
            return grid.AllCells().Count(c => c.GivenValue.HasValue);
        }

        private Position GetNextPosition()
        {
            if (_nextQueue.Any())
            {
                return _nextQueue.Dequeue();
            }

            // get a number of random positions and their symmetrics
            var list = new List<PositionList>();
            for (int i = 0; i < _grid.GridSize / 2; i++)
            {
                var pos = _grid.GetRandomEmptyPosition(_rng);
                if (pos != null)
                {
                    var sublist = _pattern.GetSymmetricPositions(pos, _grid.GridSize);
                    list.Add(new PositionList(sublist));
                }
            }

            if (!list.Any())
                return null;

            // calculate their relative score
            foreach (var positionList in list)
            {
                positionList.SeverityScore = CalculateScore(_grid, positionList.Positions);
            }

            // get the highest scoring one
            var winner = list.OrderByDescending(x => x.SeverityScore).First();

            // push winning list of positions onto queue
            foreach (var pos in winner.Positions)
            {
                _nextQueue.Enqueue(pos);
            }

            // get one
            return _nextQueue.Dequeue();
        }

        private void Rewind()
        {
            Cell cell;
            while (true)
            {
                // if stack is empty, escape
                if (_stack.Count == 0)
                {
                    Cleargrid(_grid);
                    return;
                }

                // get the top one of the stack and reset it
                var top = _stack.Pop();
                top.Target.Reset(true);

                // is it a usefull value?
                if (top.Remaining.Any())
                {
                    // reset the "available" list
                    foreach (var oldval in top.Target.AvailableValues.Except(top.Remaining).ToList())
                    {
                        top.Target.EraseAvailable(oldval);
                    }

                    // and exit the "while"
                    cell = top.Target;
                    break;
                }
            }

            // select a new value
            SelectValueForCell(cell);
            ResetSymmetry();
        }

        private void ResetSymmetry()
        {
            _nextQueue.Clear();

            // find all used positions
            // add symmetrical positions to list
            var fields = new List<Position>();
            for (int r = 0; r < _grid.GridSize; r++)
            {
                for (int c = 0; c < _grid.GridSize; c++)
                {
                    var cell = _grid.GetCellByRowColumn(r, c);
                    if (cell.GivenValue.HasValue)
                    {
                        var group = _pattern.GetSymmetricPositions(new Position(r, c), _grid.GridSize);
                        fields.AddRange(group);
                    }
                }
            }

            // skip duplicates and cells that already have a value. Add rest (if any) to queue.
            foreach (var position in fields.Distinct())
            {
                var cell = _grid.GetCellByRowColumn(position.Row, position.Column);
                if (!cell.GivenValue.HasValue)
                {
                    _nextQueue.Enqueue(position);
                }
            }
        }

        private static void ResetGrid(ISudokuGrid grid)
        {
            // erase all conclusions
            foreach (var cell in grid.AllCells())
            {
                if (!cell.GivenValue.HasValue)
                {
                    cell.Reset(false);
                }
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