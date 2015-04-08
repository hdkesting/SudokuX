﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Strategies
{
    /// <summary>
    /// Try and solve a grid as far as possible.
    /// </summary>
    public class Solver
    {
        private readonly ISudokuGrid _grid;
        private readonly IList<ISolver> _solvers;
        private readonly Dictionary<Type, PerformanceMeasurement> _measurements = new Dictionary<Type, PerformanceMeasurement>();

        public Solver(ISudokuGrid grid, IList<ISolver> solvers)
        {
            _grid = grid;
            _solvers = solvers;
        }

        /// <summary>
        /// Occurs when there is progress to report.
        /// </summary>
        public event EventHandler<ProgressEventArgs> Progress;

        public Dictionary<Type, PerformanceMeasurement> Measurements { get { return _measurements; } }

        readonly Stopwatch _swConclusion = new Stopwatch();
        private int _conclusionSets;

        private IList<Conclusion> ProcessGrid(ISolver solver)
        {
            // mark start
            var sw = Stopwatch.StartNew();
            var result = solver.ProcessGrid(_grid).ToList();
            // mark stop
            sw.Stop();

            // update timing measurements
            if (_measurements.ContainsKey(solver.GetType()))
            {
                var measure = _measurements[solver.GetType()];
                measure.Invocations++;
                measure.ResultCount += result.Count;
                measure.TimeSpent += sw.Elapsed;
            }
            else
            {
                var measure = new PerformanceMeasurement
                {
                    Invocations = 1,
                    ResultCount = result.Count,
                    TimeSpent = sw.Elapsed
                };
                _measurements.Add(solver.GetType(), measure);
            }

            Debug.WriteLine("Spent time processing {1:N0} conclusion sets: {0:N} ms (avg {2:N} ms)",
                _swConclusion.ElapsedMilliseconds, _conclusionSets, (double)_swConclusion.ElapsedMilliseconds / _conclusionSets);

            return result;
        }

        /// <summary>
        /// Process all solvers for as long as they return results. The grid result may be done, undecided or false.
        /// </summary>
        public void ProcessSolvers()
        {
            bool foundone = true;
            bool keepgoing = true;

            while (foundone && keepgoing) // keep looping while there are results
            {
                foundone = false;
                foreach (var solver in _solvers) // process in supplied order (of complexity)
                {
                    foreach (var conclusion in ProcessGrid(solver))
                    {
                        _swConclusion.Start();
                        _conclusionSets++;
                        if (conclusion.ExactValue.HasValue)
                        {
                            foundone = true;
                            //Debug.WriteLine("Found value {1} for cell {0}", conclusion.TargetCell, conclusion.ExactValue.Value);
                            conclusion.TargetCell.SetCalculatedValue(conclusion.ExactValue.Value);
                            conclusion.TargetCell.UsedComplexityLevel += conclusion.ComplexityLevel;
                        }
                        else
                        {
                            foreach (var value in conclusion.ExcludedValues)
                            {
                                //Debug.WriteLine("Excluding value {1} from cell {0}", conclusion.TargetCell, value);
                                foundone = conclusion.TargetCell.EraseAvailable(value) | foundone; // always erase
                            }
                            conclusion.TargetCell.UsedComplexityLevel += conclusion.ComplexityLevel;

                            if (!conclusion.TargetCell.AvailableValues.Any())
                            {
                                Debug.WriteLine("No availables left in targetcell {0}!", conclusion.TargetCell);
                                DumpGrid(_grid);
                                keepgoing = false;
                            }
                        }
                        _swConclusion.Stop();
                    }

                    if (foundone)
                    {
                        // skip other, more difficult, solvers for now; start again at the simple ones
                        break; // foreach solver
                    }
                }
            }

        }

        private void OnProgress()
        {
            var handler = Progress;
            if (handler != null)
            {
                var args = new ProgressEventArgs
                {
                    Total = _grid.GridSize * _grid.GridSize,
                    Given = _grid.AllCells().Count(c => c.GivenValue.HasValue),
                    Calculated = _grid.AllCells().Count(c => c.CalculatedValue.HasValue && !c.GivenValue.HasValue) // the last givens already had a calculated value, if needed to complete a symmetry
                };

                handler(this, args);
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
