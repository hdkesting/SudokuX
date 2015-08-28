using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using SudokuX.Solver.SolverStrategies;
using SudokuX.Solver.Support;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.Core
{
    /// <summary>
    /// Try and solve a grid as far as possible.
    /// </summary>
    public class Solver
    {
        private readonly ISudokuGrid _grid;
        private readonly IList<ISolverStrategy> _solvers;
        private readonly Dictionary<Type, PerformanceMeasurement> _measurements = new Dictionary<Type, PerformanceMeasurement>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Solver"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="solvers">The solvers.</param>
        /// <exception cref="System.ArgumentNullException">
        /// grid
        /// or
        /// solvers
        /// </exception>
        /// <exception cref="System.ArgumentException">List of solvers cannot be empty.;solvers</exception>
        /// <exception cref="System.InvalidOperationException">Do not add BasicRule as a solver, it is built-in.</exception>
        public Solver([NotNull] ISudokuGrid grid, [NotNull] IList<ISolverStrategy> solvers)
        {
            if (grid == null) throw new ArgumentNullException("grid");
            if (solvers == null) throw new ArgumentNullException("solvers");
            if (!solvers.Any()) throw new ArgumentException("List of solvers cannot be empty.", "solvers");
            if (solvers.Any(s => s is BasicRule)) throw new InvalidOperationException("Do not add BasicRule as a solver, it is built-in.");

            _grid = grid;
            _solvers = solvers;
        }

        /// <summary>
        /// Occurs when there is progress to report.
        /// </summary>
        public event EventHandler<ProgressEventArgs> Progress;

        /// <summary>
        /// Gets the performance measurements.
        /// </summary>
        /// <value>
        /// The measurements.
        /// </value>
        public Dictionary<Type, PerformanceMeasurement> Measurements { get { return _measurements; } }

        readonly Stopwatch _swConclusion = new Stopwatch();
        private int _conclusionSets;

        private IList<Conclusion> ProcessGrid(ISolverStrategy solver)
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
        public ProcessResult ProcessSolvers()
        {
            bool foundone = true;
            bool keepgoing = true;

            int score = 0;
            Validity val;
            int max = 0;

            ISolverStrategy basic = new BasicRule();

            while (foundone && keepgoing) // keep looping while there are results
            {
                // use the BasicRule, to make sure the validity check makes sense
                var conclusions = ProcessGrid(basic).ToList();
                ProcessConclusions(conclusions, ref score, ref keepgoing);

                val = _grid.IsChallengeDone();
                if (val == Validity.Invalid)
                {
                    return new ProcessResult(0, Validity.Invalid);
                }

                foundone = false;
                foreach (var solver in _solvers) // process in supplied order (of complexity)
                {
                    conclusions = ProcessGrid(solver).ToList();
                    foundone = ProcessConclusions(conclusions, ref score, ref keepgoing);

                    if (foundone)
                    {
                        max = Math.Max(solver.Complexity, max);
                        // skip other, more difficult, solvers for now; start again at the simple ones
                        break; // foreach solver
                    }
                }
            }

            val = _grid.IsChallengeDone();
            Trace.WriteLine(String.Format("Solvers processed, max={0}, result={1}, score={2}", max, val, score));
            return new ProcessResult(score, val);
        }

        private bool ProcessConclusions(List<Conclusion> conclusions, ref int score, ref bool keepgoing)
        {
            bool foundone = false;

            foreach (var conclusion in conclusions)
            {
                _swConclusion.Start();
                _conclusionSets++;
                if (conclusion.ExactValue.HasValue)
                {
                    if (!conclusion.TargetCell.HasGivenOrCalculatedValue)
                    {
                        foundone = true;
                        //Debug.WriteLine("Found value {1} for cell {0}", conclusion.TargetCell, conclusion.ExactValue.Value);
                        conclusion.TargetCell.SetCalculatedValue(conclusion.ExactValue.Value);
                        conclusion.TargetCell.UsedComplexityLevel += conclusion.ComplexityLevel;
                        score += conclusion.ComplexityLevel;
                    }
                }
                else
                {
                    foreach (var value in conclusion.ExcludedValues)
                    {
                        //Debug.WriteLine("Excluding value {1} from cell {0}", conclusion.TargetCell, value);
                        foundone = conclusion.TargetCell.EraseAvailable(value) | foundone; // always erase
                        score += conclusion.ComplexityLevel;
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
            return foundone;
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
