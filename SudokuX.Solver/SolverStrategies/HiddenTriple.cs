using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.SolverStrategies
{
    /// <summary>
    /// Find a set of three numbers that are only in three cells within the group. The other numbers in those cells are invalid.
    /// Note that not all three values need to be in all three cells.
    /// </summary>
    public class HiddenTriple : ISolverStrategy
    {
        /// <summary>
        /// Processes the grid and returns any helpful conclusions.
        /// </summary>
        /// <param name="grid">The grid to process.</param>
        /// <returns></returns>
        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            var result = new List<Conclusion>();
            foreach (var group in grid.CellGroups)
            {
                var list = FindHiddenTriples(group, grid.MinValue, grid.MaxValue).ToList();
                result.AddRange(list);
            }

            return result;
        }

        /// <summary>
        /// Gets the complexity-score of this solver (6).
        /// </summary>
        /// <value>
        /// The complexity.
        /// </value>
        public int Complexity
        {
            get { return 6; }
        }

        private IEnumerable<Conclusion> FindHiddenTriples(CellGroup cellGroup, int minValue, int maxValue)
        {
            // find triplet values to ignore
            var knownvals =
                cellGroup.Cells.Where(c => c.GivenOrCalculatedValue.HasValue).Select(c => c.GivenOrCalculatedValue.Value).ToList();

            if (knownvals.Count >= maxValue - minValue - 2)
            {
                // 3 open cells or less? never mind.
                //return Enumerable.Empty<Conclusion>();
                yield break;
            }

            foreach (var triplet in GetTriplets(minValue, maxValue, knownvals))
            {
                var cells =
                    cellGroup.Cells.Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Any(v => triplet.Contains(v))).ToList();
                if (cells.Count == 3)
                {
                    // exactly 3 cells with any of the three triplet values - this is a triplet, possibly hidden
                    var result = new List<Conclusion>();
                    var reason = cellGroup.Cells.Except(cells).ToList();
                    for (int i = 0; i < 3; i++)
                    {
                        var extravalues = cells[i].AvailableValues.Where(v => !triplet.Contains(v)).ToList();
                        if (extravalues.Any())
                        {
                            result.Add(new Conclusion(Support.Enums.SolverType.HiddenTriple, cells[i], Complexity, extravalues, reason));
                        };
                    }

                    foreach (var res in result)
                        yield return res;
                }
            }
        }

        /// <summary>
        /// Get triplets that do not contain any of the known values.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="knowVals"></param>
        /// <returns></returns>
        private IEnumerable<IList<int>> GetTriplets(int minValue, int maxValue, IList<int> knowVals)
        {
            //for (int i1 = minValue; i1 <= maxValue - 2; i1++)
            //{
            //    if (!knowVals.Contains(i1))
            //    {
            //        for (int i2 = i1 + 1; i2 <= maxValue - 1; i2++)
            //        {
            //            if (!knowVals.Contains(i2))
            //            {
            //                for (int i3 = i2 + 1; i3 <= maxValue; i3++)
            //                {
            //                    if (!knowVals.Contains(i3))
            //                    {
            //                        yield return new[] { i1, i2, i3 };
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            return EnumerableExtensions.GetSubsets(minValue, maxValue, 3)
                .Where(l => l.All(i => !knowVals.Contains(i)));
        }
    }
}
